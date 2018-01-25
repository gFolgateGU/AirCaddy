using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace AirCaddy.Controllers
{
    public abstract class IdentityBaseController : Controller
    {
        protected IdentityBaseController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        protected ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}