using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace ClientWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var services = new ServiceCollection();
            services.AddAppServices(configuration);
            services.AddBusinessLayer();

            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<ChoiseWin>();
            mainWindow.Show();
        }
    }


}
