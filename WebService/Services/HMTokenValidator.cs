namespace WebService.Services
{
    public class HMTokenValidator : ITokenValidator
    {
        public bool Validate(string token)
        {
            return true;
        }

        public void Invalidate(string token)
        {
            // Intentionally left empty...
        }
    }
}