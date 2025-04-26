namespace ErpConnector.DTOs
{
    public class ProductFromApiDto
    {
        public int Id {get;set;}
        public string Title {get;set;}
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public ProductFromApiDto()
        {
            if(Title == null)
            {
                Title = "";
            }
            
            if(Description == null)
            {
                Description = "";
            }

            if(Category == null)
            {
                Category = "";
            }
        }

    }
}
