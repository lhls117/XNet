using ProArtist.Domain;
using ProArtist.Presentation.Theme.Models;
using ProArtist.Presentation.Theme.Views;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Presentation.Wpf;

namespace ProArtist.Presentation.Theme.ViewModels
{
    [Export]
    public class TextControllerViewModel : ViewModel<ITextControllerView>
    {
        [ImportingConstructor]
        public TextControllerViewModel(ITextControllerView view) : base(view)
        {
           
        }

        private TextModel controller;
        public TextModel Controller
        {
            get => controller;
            set=>SetProperty(ref controller, value);
        }
    }
}
