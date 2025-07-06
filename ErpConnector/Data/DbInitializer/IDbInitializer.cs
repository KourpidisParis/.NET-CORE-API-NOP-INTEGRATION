namespace ErpConnector.Data
{
    public interface IDbInitializer
    {
        Task InitializeProductTableAsync();
        Task InitializeCategoryTableAsync();
    }
}