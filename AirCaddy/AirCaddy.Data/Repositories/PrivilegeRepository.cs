using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirCaddy.Data.Repositories
{
    public interface IPrivilegeRepository
    {
        Task<bool> ExistsCourseRequestAsync(string courseName, string courseAddress);

        Task AddCourseRequestAsync(PrivilegeRequest privilegeRequest);
    }

    public class PrivilegeRepository : BaseRepository, IPrivilegeRepository
    {
        public PrivilegeRepository () : base() { }

        public async Task<bool> ExistsCourseRequestAsync(string courseName, string courseAddress)
        {
            var courseRequest = await _dataEntities.PrivilegeRequests
                                                .FirstOrDefaultAsync
                                                (cr => cr.GolfCourseName.Contains(courseName) && cr.GolfCourseAddress.Contains(courseAddress));
            return courseRequest != null;
        }

        public async Task AddCourseRequestAsync(PrivilegeRequest privilegeRequest)
        {
            _dataEntities.PrivilegeRequests.Add(privilegeRequest);
            await _dataEntities.SaveChangesAsync();
        }
 
    }
}
