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

namespace TestMEFUI.Views
{
    /// <summary>
    /// HellowView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IHelloView))]
    public partial class HellowView : IHelloView
    {
        public HellowView()
        {
            InitializeComponent();
        }
    }
}
