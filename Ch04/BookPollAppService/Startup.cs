using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BookPollAppService.Startup))]

namespace BookPollAppService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}