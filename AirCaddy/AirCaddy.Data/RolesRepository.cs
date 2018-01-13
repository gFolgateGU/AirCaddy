using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AirCaddy.Data
{
    public interface IRolesRepository
    {
        Task<List<AspNetRole>> GetRoles();
    }

    public class RolesRepository : IRolesRepository
    {
        private readonly AirCaddy_DataEntities _dataEntities;

        public RolesRepository()
        {
            _dataEntities = new AirCaddy_DataEntities();
        }

        public async Task<List<AspNetRole>> GetRoles()
        {
            var allRoles = await _dataEntities.AspNetRoles.ToListAsync();
            return allRoles;
        }

        public void Test()
        {
            var identityRole = new IdentityRole();
            identityRole.Name = "User";
        }

    }
}
