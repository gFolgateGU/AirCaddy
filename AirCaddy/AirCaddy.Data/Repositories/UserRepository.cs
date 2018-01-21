using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirCaddy.Data.Repositories
{
    public interface IUserRepository
    {
        string GetUserIdFromUsername(string username);
    }

    public class UserRepository : BaseRepository, IUserRepository
    { 
        public UserRepository() : base() { }

        public string GetUserIdFromUsername(string username)
        {
            var user = _dataEntities.AspNetUsers.FirstOrDefault(u => u.UserName.Contains(username));

            return user.Id;
        }
    }
}
