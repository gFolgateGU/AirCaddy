using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AirCaddy.Startup))]
namespace AirCaddy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
