using System;

namespace WebService
{
    public class User : IEquatable<User>
    {
        private string _username;
        private readonly int _id;
        private string _password;
        private int _contributionPoints = 0;

        public User(string username, int ID, string password)
        {
            _username = username;
            _id = ID;
            _password = password;
        }

        public string GetUsername()
        {
            return _username;
        }

        public void SetPassword(string passwordHash)
        {
            _password = passwordHash;
        }

        public int GetId()
        {
            return _id;
        }

        public int GetContributionPoints()
        {
            return _contributionPoints;
        }

        public void SetContributionPoints(int contributionModifier)
        {
            _contributionPoints += contributionModifier;
        }

        public bool Equals(User other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _id == other._id;
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
            return _id;
        }

        public bool Save()
        {
            // TODO Should store data to the database.
            throw new NotImplementedException();
        }
    }
}