using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Presentation.Wpf.Models;
using XNet.Presentation.Wpf.Views;

namespace XNet.Presentation.Wpf.ViewModels
{
    [Export]
    public class MainViewModel : ViewModel<IMainView>
    {
        [ImportingConstructor]
        public MainViewModel(IMainView view, 
            [ImportMany]IEnumerable<IEntryExtension> entryExtensions,
            [ImportMany]IEnumerable<IMenuitemExtension> menuitemExtensions,
            [Import(AllowDefault =true)] IEntrySorter entrySorter) : base(view)
        {
            if(entrySorter==null)
            {
                EntryModels = new ObservableCollection<EntryModel>(entryExtensions.Select(d => new EntryModel(d)));
            }
            else
            {
                EntryModels = new ObservableCollection<EntryModel>(entryExtensions.OrderBy(o => entrySorter.Index(o)).Select(d => new EntryModel(d)));
            }
                

            MenuitemModels = new ObservableCollection<MenuitemModel>(menuitemExtensions
                .Select(o => new MenuitemModel(o)));
        }

        public ObservableCollection<EntryModel> EntryModels { get; }

        public ObservableCollection<MenuitemModel> MenuitemModels { get; }
    }
}
