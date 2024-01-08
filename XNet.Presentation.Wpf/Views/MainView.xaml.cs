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

namespace XNet.Presentation.Wpf.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IMainView))]
    public partial class MainView : IMainView
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void EntryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
