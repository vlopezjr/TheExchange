using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TheExchange.MVC.Startup))]
namespace TheExchange.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
