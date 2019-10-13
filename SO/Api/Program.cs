using Logic.Utils;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseIIS()
                //.UseKestrel()
                //.UseIISIntegration()
                .UseUrls(Consts.ApiUrl)
                .UseStartup<Startup>();
        }
    }
}
