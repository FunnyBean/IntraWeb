using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Angular2Introduction.Startup))]
namespace Angular2Introduction
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
