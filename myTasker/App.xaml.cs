using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace myTasker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Configure le conteneur de services
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            Services = serviceCollection.BuildServiceProvider();

            // Ouvre la fenêtre principale
            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Ajouter DbContext
            services.AddDbContext<AppDbContext>();

            // Ajouter les repositories et services
            services.AddScoped(typeof(GenericRepository<>));
            services.AddScoped<ProjectService>();
            services.AddScoped<TaskItemService>();
            services.AddScoped<MemberService>();

            // Ajouter la fenêtre principale
            services.AddTransient<MainWindow>();
        }
    }
}
