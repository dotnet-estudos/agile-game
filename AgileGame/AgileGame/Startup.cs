using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AgileGame.Startup))]
namespace AgileGame
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
