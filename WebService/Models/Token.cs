namespace WebService.Models
{
    public class Token
    {
        public string Tkn { get; set; }

        public Token(string token)
        {
            Tkn = token;
        }
    }
}