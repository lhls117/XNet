using ProArtist.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Presentation.Wpf;

namespace ProArtist.Presentation.Extensions
{
    [Export(typeof(IEntryExtension))]
    public class HomeExtension : EntryExtension<HomeViewModel>
    {
        [ImportingConstructor]
        public HomeExtension(ExportFactory<HomeViewModel> viewModelFactory) : base(viewModelFactory)
        {
            Name = "主页";
            
        }
    }
}
