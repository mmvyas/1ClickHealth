using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OneClickHealth.Startup))]
namespace OneClickHealth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
