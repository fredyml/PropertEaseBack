namespace PropertEase.Application.Dtos
{

    public class PropertyDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string CodeInternal { get; set; }
        public int Year { get; set; }
        public OwnerDto Owner { get; set; }
        public IEnumerable<ImageDto> Images { get; set; }
        public IEnumerable<TraceDto> Traces { get; set; }
    }
}

