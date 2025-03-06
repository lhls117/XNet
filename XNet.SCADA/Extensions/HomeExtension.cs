using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Presentation.Wpf;
using XNet.SCADA.ViewModels;

namespace XNet.SCADA.Extensions
{
    [Export(typeof(IEntryExtension))]
    public class HomeExtension : EntryExtension<HomeViewModel>
    {
        [ImportingConstructor]
        public HomeExtension(ExportFactory<HomeViewModel> viewModelFactory) : base(viewModelFactory)
        {
            Name = "主界面";
            ImageKey = "HomeAnalytics";
        }
    }
}
