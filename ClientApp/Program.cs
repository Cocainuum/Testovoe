using System;
using System.Configuration;
using ClientApp.Configuration;
using ClientApp.ServiceClient;
using ClientApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ClientApp
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<App>();
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<MedicineClient>();
                    
                    var address = ConfigurationManager.AppSettings[MedicineClientOptions.AddressField];
                    services.AddHttpClient(MedicineClientOptions.ClientName, cfg => cfg.BaseAddress = new Uri(address));
                })
                .Build();
            var app = host.Services.GetService<App>();
            app?.Run();
        }
    }
}