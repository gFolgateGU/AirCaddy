﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCaddy.Data.CustomDataModels;

namespace AirCaddy.Data.Repositories
{
    public interface IPrivilegeRepository
    {
        Task<bool> ExistsCourseRequestAsync(string courseName, string courseAddress);

        Task AddCourseRequestAsync(PrivilegeRequest privilegeRequest);

        Task<IEnumerable<UserPrivilegeRequest>> GetAllPendingRequestsAsync();

        Task<IEnumerable<PrivilegeRequest>> GetAllPendingRequestsForUserAsync(string userId);

        Task DeleteRequestAsync(int id);
        
        Task AcceptRequestAsync(int id);

        Task<PrivilegeRequest> GetPrivilegeRequest(int id);

        Task<PrivilegeRequest> CheckIfPrivilegeRequestIsOwnedByUser(int id, string userId);

        Task<bool> EditPrivilegeRequest(PrivilegeRequest updatedPrivilegeRequest);

        Task<bool> DeletePrivilegeRequest(int id);
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

        public async Task<IEnumerable<UserPrivilegeRequest>> GetAllPendingRequestsAsync()
        {
            const string query =
                @"SELECT AspNetUsers.UserName, PrivilegeRequests.Id, PrivilegeRequests.GolfCourseName, PrivilegeRequests.GolfCourseAddress, PrivilegeRequests.CoursePhoneNumber,
	                     PrivilegeRequests.GolfCourseType, PrivilegeRequests.Reason, PrivilegeRequests.Verified
	                     FROM AspNetUsers
	                     INNER JOIN PrivilegeRequests on AspNetUsers.Id = PrivilegeRequests.UserId
	                     WHERE PrivilegeRequests.Verified = 0
	                     GROUP BY AspNetUsers.Id, AspNetUsers.UserName, PrivilegeRequests.Id, PrivilegeRequests.GolfCourseName, PrivilegeRequests.GolfCourseAddress, PrivilegeRequests.CoursePhoneNumber,
	                     PrivilegeRequests.GolfCourseType, PrivilegeRequests.Reason, PrivilegeRequests.Verified";

            var pendingRequests = await _dataEntities.Database.SqlQuery<UserPrivilegeRequest>(query).ToListAsync();
            return pendingRequests;
        }

        public async Task<IEnumerable<PrivilegeRequest>> GetAllPendingRequestsForUserAsync(string userId)
        {
            var userPendingRequests =
                await _dataEntities.PrivilegeRequests.Where(pr => pr.UserId.Contains(userId) && pr.Verified == false).ToListAsync();
            return userPendingRequests;
        }

        public async Task DeleteRequestAsync(int id)
        {
            var privRequest = await _dataEntities.PrivilegeRequests.Where(gc => gc.Id.Equals(id)).FirstOrDefaultAsync();
            _dataEntities.PrivilegeRequests.Remove(privRequest);
            await _dataEntities.SaveChangesAsync();
        }
        
        public async Task AcceptRequestAsync(int id)
        {
            var privRequest = await _dataEntities.PrivilegeRequests.Where(gc => gc.Id.Equals(id)).FirstOrDefaultAsync();
            _dataEntities.PrivilegeRequests.Remove(privRequest);
            await _dataEntities.SaveChangesAsync();
        }

        public async Task<PrivilegeRequest> GetPrivilegeRequest(int id)
        {
            var privRequest = await _dataEntities.PrivilegeRequests.Where(gc => gc.Id.Equals(id)).FirstOrDefaultAsync();
            return privRequest;
        }

        public async Task<PrivilegeRequest> CheckIfPrivilegeRequestIsOwnedByUser(int id, string userId)
        {
            try
            {
                var privRequest = await _dataEntities.PrivilegeRequests.Where(pr => pr.Id.Equals(id)).FirstOrDefaultAsync();
                if (privRequest.UserId == userId)
                {
                    return privRequest;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception e)
            {
                e.GetBaseException();
                return null;
            }
        }

        public async Task<bool> EditPrivilegeRequest(PrivilegeRequest updatedPrivilegeRequest)
        {
            try
            {
                var privRequest = await _dataEntities.PrivilegeRequests.Where(gc => gc.Id.Equals(updatedPrivilegeRequest.Id)).FirstOrDefaultAsync();
                if (privRequest != null)
                {
                    privRequest = updatedPrivilegeRequest;
                    
                    await _dataEntities.SaveChangesAsync();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                e.GetBaseException();
                return false;
            }
            return true;
        }

        public async Task<bool> DeletePrivilegeRequest(int id)
        {
            try
            {
                var privRequestToBeDeleted = await _dataEntities.PrivilegeRequests.Where(pr => pr.Id.Equals(id)).FirstOrDefaultAsync();

                if (privRequestToBeDeleted != null)
                {
                    _dataEntities.PrivilegeRequests.Remove(privRequestToBeDeleted);

                    await _dataEntities.SaveChangesAsync();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                e.GetBaseException();
                return false;
            }

            return true; ;
        }
    }
}
