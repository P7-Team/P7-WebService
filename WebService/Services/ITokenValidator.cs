namespace WebService.Services
{
    public interface ITokenValidator
    {
        bool Validate(string token);
        void Invalidate(string token);
    }
}