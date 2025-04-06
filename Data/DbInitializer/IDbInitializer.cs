namespace ErpConnector.Services
{
    public interface IDbInitializer
    {
        Task InitializeProductTableAsync();
    }
}