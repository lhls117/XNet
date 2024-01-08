using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Presentation.Wpf.Views;

namespace XNet.Presentation.Wpf.ViewModels
{
    [Export]
    public class MainViewModel : ViewModel<IMainView>
    {
        [ImportingConstructor]
        public MainViewModel(IMainView view, [ImportMany]IEnumerable<IEntryExtension> entryExtensions, [Import(AllowDefault =true)] IEntrySorter entrySorter) : base(view)
        {
            EntryModels = new ObservableCollection<EntryModel>(entryExtensions.OrderBy(o=>entrySorter.Index(o)).Select(d => new EntryModel(d)));
        }

        public ObservableCollection<EntryModel> EntryModels { get; }
    }
}
