namespace ErpConnector.Models
{
    public class Product
    {
        public int ApiId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public decimal Price { get; set; }
        public string FullDescription {get;set;}
        public int ProductTypeId {get;set;}
        public Product()
        {
            if(Name == null)
            {
                Name = "";
            }

            if(Sku == null)
            {
                Sku = "";
            }

            if(FullDescription == null)
            {
                FullDescription = "";
            }
        }
    }
}
