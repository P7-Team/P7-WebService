namespace WebService.Interfaces
{
    public interface ITokenStore
    {
        void Store(string token, string username);
        string Fetch(string token);
    }
}