using System;
using System.Text.Json.Serialization;
using WebService.Interfaces;

namespace WebService.Models
{
    public class User : IEquatable<User>, IAggregateRoot<string>
    {
        public string Username { get; }
        public int Id { get; }
        
        [JsonIgnore]
        public string Password { get; set; }
        public int ContributionPoints { get; set; }

        public User(string username, string password, int contributionPoints)
        {
            Username = username;
            Password = password;
            ContributionPoints = contributionPoints;
        }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public User(string username)
        {
            Username = username;
            Password = null;
        }

        public void AddContributionPoints(int contributionModifier)
        {
            ContributionPoints += contributionModifier;
        }

        public bool Equals(User other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Username == other.Username;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User) obj);
        }

        public override int GetHashCode()
        {
            return Username.GetHashCode();
        }

        public bool Save()
        {
            // TODO Should store data to the database.
            throw new NotImplementedException();
        }

        public string GetIdentifier()
        {
            return Username;
        }
    }
}