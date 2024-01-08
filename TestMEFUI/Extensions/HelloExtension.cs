using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMEFUI.ViewModels;
using XNet.Presentation.Wpf;

namespace TestMEFUI.Extensions
{
    [Export(typeof(IEntryExtension))]
    public class HelloExtension : EntryExtension<HellowViewModel>
    {
        [ImportingConstructor]
        public HelloExtension(ExportFactory<HellowViewModel> viewModelFactory) : base(viewModelFactory)
        {
            Name = "Hello";
        }
    }
}
