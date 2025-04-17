namespace ErpConnector.DTOs
{
    public class CategroryFromApiDto
    {
        public string Title;

        public CategroryFromApiDto()
        {
            if(Title == null)
            {
                Title = "";
            }
        }
    }
}