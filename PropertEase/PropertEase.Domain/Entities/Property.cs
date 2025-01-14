namespace PropertEase.Domain.Entities
{
    public class Property
    {
        public string Id { get; set; }
        public string IdOwner { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
    }
}
