using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AirCaddy_Web.Startup))]
namespace AirCaddy_Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
