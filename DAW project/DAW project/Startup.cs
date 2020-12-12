using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DAW_project.Startup))]
namespace DAW_project
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
