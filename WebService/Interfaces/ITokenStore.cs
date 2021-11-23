namespace WebService.Interfaces
{
    public interface ITokenStore
    {
        /// <summary>
        /// Stores a username under the token used to during authentication
        /// </summary>
        /// <param name="token">The token</param>
        /// <param name="username">The username of the user owning the token</param>
        /// <param name="overrideExisting">Used to indicate whether the existing value for a token should be overriden (Default=true)</param>
        void Store(string token, string username, bool overrideExisting = true);
        /// <summary>
        /// Fetch the username stored with a given token
        /// </summary>
        /// <param name="token">The token used to index the username</param>
        /// <returns>The username associated with the token if it exists, else an empty string</returns>
        string Fetch(string token);
    }
}