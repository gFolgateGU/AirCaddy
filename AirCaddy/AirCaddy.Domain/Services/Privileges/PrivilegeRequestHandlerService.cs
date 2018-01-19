using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirCaddy.Domain.ViewModels.Privileges;

namespace AirCaddy.Domain.Services.Privileges
{
    public class PrivilegeRequestHandlerService
    {
        public PrivilegeRequestHandlerService() { }

        public bool IsDuplicateEntry(PrivilegeRequestViewModel pendingRequest)
        {

            return false;
        }
    }
}
