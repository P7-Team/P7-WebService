using System;
using System.Collections.Generic;
using WebService.Interfaces;

namespace WebService.Services
{
    public class TokenStore : ITokenStore
    {
        private static readonly Dictionary<string, string> Tokens = new Dictionary<string, string>();

        public void Store(string token, string username, bool overrideExisting = true)
        {
            bool exists = !Tokens.TryAdd(token, username);
            if (exists && overrideExisting)
            {
                Tokens[token] = username;
            }
        }
        
        public string Fetch(string token)
        {
            return Tokens.GetValueOrDefault(token, String.Empty);
        }
    }
}