using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XNet.Presentation.Wpf.Views;

namespace TestMEFUI
{
    public class Container
    {
        private Container()
        {
            ConfigureComposition();
            ConfigureServices();
        }
        public static Container Default { get; } = new Container();

        public ContainerConfiguration Composition { get; set; }

        public ServiceProvider Services { get; set; }

        private void ConfigureComposition()
        {
            Composition = new ContainerConfiguration()
                .WithAssembly(typeof(App).GetTypeInfo().Assembly)
            .WithAssembly(typeof(IMainView).GetTypeInfo().Assembly); ;

        }
        private void ConfigureServices()
        {
            var service = new ServiceCollection();
           
            Services = service.BuildServiceProvider();
        }

    }
}
