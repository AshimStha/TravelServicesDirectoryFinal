using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TravelServicesDirectoryFinal.Startup))]
namespace TravelServicesDirectoryFinal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
