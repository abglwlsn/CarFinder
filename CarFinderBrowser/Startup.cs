using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarFinderBrowser.Startup))]
namespace CarFinderBrowser
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
