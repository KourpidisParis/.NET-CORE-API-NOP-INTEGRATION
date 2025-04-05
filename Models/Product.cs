namespace ErpConnector.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public decimal Price { get; set; }

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
        }
    }
}
