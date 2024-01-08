﻿using ProArtist.Presentation.Views;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Presentation.Wpf;

namespace ProArtist.Presentation.ViewModels
{
    [Export]
    public class ManageViewModel : ViewModel<IManageView>
    {
        [ImportingConstructor]
        public ManageViewModel(IManageView view) : base(view)
        {
        }
    }
}
