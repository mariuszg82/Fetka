using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Fetka.Startup))]
namespace Fetka
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
