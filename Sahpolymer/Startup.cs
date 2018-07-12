using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WorkWellPipe.Startup))]
namespace WorkWellPipe
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
