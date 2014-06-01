using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VDolgah.Startup))]
namespace VDolgah
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
