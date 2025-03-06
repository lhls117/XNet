using MaterialDesignThemes.Wpf;
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

namespace XNet.SCADA.Views
{
    /// <summary>
    /// AboutView.xaml 的交互逻辑
    /// </summary>
    [Export(typeof(IAboutView))]
    public partial class AboutView : IAboutView
    {
        public AboutView()
        {
            InitializeComponent();
        }

        #region implemention of IAboutView
       

        public async Task ShowDialogAsync(object dialogHost)
        {
            await DialogHost.Show(this, (dialogHost as DialogHost)?.Identifier);
        }

        #endregion
    }
}
