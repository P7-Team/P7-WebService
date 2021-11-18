namespace WebService.Models
{
    public class Token
    {
        public string Key { get; set; }

        public Token(string tokenKey)
        {
            Key = tokenKey;
        }
    }
}