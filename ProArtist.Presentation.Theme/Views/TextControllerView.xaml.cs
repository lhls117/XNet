using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProArtist.Presentation.Theme.Views
{
    /// <summary>
    /// TextControllerView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(ITextControllerView))]
    public partial class TextControllerView : ITextControllerView
    {
        public TextControllerView()
        {
            InitializeComponent();
        }

      

    
    }
}
