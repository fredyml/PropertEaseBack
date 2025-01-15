using AutoMapper;
using Moq;
using PropertEase.Application.Dtos;
using PropertEase.Application.Interfaces;
using PropertEase.Application.Mapping;
using PropertEase.Application.Services;
using PropertEase.Domain.Entities;
using PropertEase.Domain.Exceptions;

namespace PropertEase.Tests
{
    public class PropertyServiceTests
    {
        private readonly Mock<IPropertyRepository> _propertyRepositoryMock;
        private readonly IMapper _mapper;
        private readonly IPropertyService _propertyService;

        public PropertyServiceTests()
        {
            // Crear un mock del repositorio
            _propertyRepositoryMock = new Mock<IPropertyRepository>();

            // Configurar AutoMapper
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PropertyProfile()); // Asegúrate de tener un perfil de mapeo como PropertyProfile
            });
            _mapper = mapperConfig.CreateMapper();

            // Crear la instancia del servicio con las dependencias necesarias
            _propertyService = new PropertyService(_propertyRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetFilteredPropertiesAsync_ShouldReturnProperties_WhenPropertiesExist()
        {
            // Arrange
            var properties = new List<Property>
            {
                new Property { Name = "Casa Grande", Address = "Calle 123", Price = 200000 },
                new Property { Name = "Apartamento Pequeño", Address = "Avenida 456", Price = 150000 }
            };

            // Configurar el mock para que devuelva una lista de propiedades
            _propertyRepositoryMock
                .Setup(repo => repo.GetFilteredPropertiesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(properties);

            // Act
            var result = await _propertyService.GetFilteredPropertiesAsync(null, null, null, null, 1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, ((List<PropertyDto>)result).Count);
        }

        [Fact]
        public async Task GetFilteredPropertiesAsync_ShouldThrowPropertyNotFoundException_WhenNoPropertiesExist()
        {
            // Arrange
            _propertyRepositoryMock
                .Setup(repo => repo.GetFilteredPropertiesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal?>(), It.IsAny<decimal?>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new List<Property>());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<PropertyNotFoundException>(() =>
                _propertyService.GetFilteredPropertiesAsync(null, null, null, null, 1, 10));

            Assert.Equal("No properties were found matching the given filters.", exception.Message);
        }

        [Fact]
        public async Task GetFilteredPropertiesAsync_ShouldApplyFiltersCorrectly()
        {
            // Arrange
            var properties = new List<Property>
            {
                new Property { Name = "Casa Grande", Address = "Calle 123", Price = 200000 },
                new Property { Name = "Apartamento Pequeño", Address = "Avenida 456", Price = 150000 }
            };

            // Configurar el mock para que devuelva propiedades filtradas por nombre
            _propertyRepositoryMock
                .Setup(repo => repo.GetFilteredPropertiesAsync("Casa", null, null, null, 1, 10))
                .ReturnsAsync(properties.FindAll(p => p.Name.Contains("Casa")));

            // Act
            var result = await _propertyService.GetFilteredPropertiesAsync("Casa", null, null, null, 1, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Casa Grande", ((List<PropertyDto>)result)[0].Name);
        }
    }
}
