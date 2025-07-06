namespace ErpConnector.DTOs
{
    public class CategoryFromApiDto
    {
        public string Name {get;set;}
        public string Slug {get;set;}

        public CategoryFromApiDto()
        {
            if(Name == null)
            {
                Name = "";
            }

            if(Slug == null)
            {
                Slug = "";
            }
        }
    }
}