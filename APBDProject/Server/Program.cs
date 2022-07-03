using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace APBDProject.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjU4MjAzQDMyMzAyZTMxMmUzMEoyQVVnbFpHQkV5RFFmMWR5L3Fxak5sTkhXdzJDOTZrYnRvSHdkc1JTTDQ9");
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
