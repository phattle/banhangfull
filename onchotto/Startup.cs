using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OnChotto.Startup))]
namespace OnChotto
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
