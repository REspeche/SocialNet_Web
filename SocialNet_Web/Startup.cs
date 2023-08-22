using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SocialNet_Web.Startup))]
namespace SocialNet_Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
