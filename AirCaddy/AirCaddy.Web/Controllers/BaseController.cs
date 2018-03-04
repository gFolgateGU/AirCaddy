using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirCaddy.Controllers
{
    public abstract class BaseController : Controller
    {
        public virtual bool SessionTimeOutVerification(string sessionUsername)
        {
            return sessionUsername != string.Empty;
        }
    }
}