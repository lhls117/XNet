using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNet.Presentation;

namespace XNet.SCADA.Views
{
  public  interface IAboutView:IView
    {
        /// <summary>
        ///     显示视图对话框。
        /// </summary>
        /// <returns></returns>
        Task ShowDialogAsync(object dialogHost);
    }
}
