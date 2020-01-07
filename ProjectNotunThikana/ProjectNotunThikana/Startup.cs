using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectNotunThikana.Startup))]
namespace ProjectNotunThikana
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
