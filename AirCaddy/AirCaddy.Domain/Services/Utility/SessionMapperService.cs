using AirCaddy.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirCaddy.Domain.Services
{
    public interface ISessionMapperService
    {
        string MapUserIdFromSessionUsername(string username);
    }

    public class SessionMapperService : ISessionMapperService
    {
        private readonly IUserRepository _userRepository;

        public SessionMapperService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string MapUserIdFromSessionUsername(string username)
        {
            var userId = _userRepository.GetUserIdFromUsername(username);
            return userId;
        }
    }
}
