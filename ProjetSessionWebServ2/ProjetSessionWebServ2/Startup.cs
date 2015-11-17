using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjetSessionWebServ2.Startup))]
namespace ProjetSessionWebServ2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
