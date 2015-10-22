using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TE_System.Startup))]
namespace TE_System
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
