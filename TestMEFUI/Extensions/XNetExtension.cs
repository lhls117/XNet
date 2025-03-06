
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
    public class XNetExtension : EntryExtension<XNetViewModel>
    {
        [ImportingConstructor]
        public XNetExtension(ExportFactory<XNetViewModel> viewModelFactory) : base(viewModelFactory)
        {
            Name = "XNet";
            ImageKey = "ShieldAccount";
        }
    }
}
