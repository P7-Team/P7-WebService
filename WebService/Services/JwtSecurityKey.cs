using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace WebService.Services
{
    public static class JwtSecurityKey  
    {  
        public static SymmetricSecurityKey Create(string secret)  
        {  
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));  
        }  
    }  
}