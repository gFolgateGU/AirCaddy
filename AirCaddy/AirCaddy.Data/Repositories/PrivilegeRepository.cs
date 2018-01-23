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

        Task<IEnumerable<PrivilegeRequest>> GetAllPendingRequests();
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

        public async Task<IEnumerable<PrivilegeRequest>> GetAllPendingRequests()
        {
            const string query = "SELECT * FROM PrivilegeRequests WHERE Verified = false";
            const string query2 =
                @"SELECT AspNetUsers.UserName, PrivilegeRequests.GolfCourseName, PrivilegeRequests.GolfCourseAddress, PrivilegeRequests.CoursePhoneNumber,
	                     PrivilegeRequests.GolfCourseType, PrivilegeRequests.Reason, PrivilegeRequests.Verified
	                     FROM AspNetUsers
	                     INNER JOIN PrivilegeRequests on AspNetUsers.Id = PrivilegeRequests.UserId
	                     WHERE PrivilegeRequests.Verified = 0
	                     GROUP BY AspNetUsers.Id, AspNetUsers.UserName, PrivilegeRequests.GolfCourseName, PrivilegeRequests.GolfCourseAddress, PrivilegeRequests.CoursePhoneNumber,
	                     PrivilegeRequests.GolfCourseType, PrivilegeRequests.Reason, PrivilegeRequests.Verified";
            var pendingRequests = await _dataEntities.Database.SqlQuery<PrivilegeRequest>(query).ToListAsync();
            return pendingRequests;
        }
 
    }
}
