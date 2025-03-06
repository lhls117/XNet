using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNet.Presentation.Wpf.Models
{
    /// <summary>
    ///     菜单模块视图模型。
    /// </summary>
    public class MenuitemModel : Model
    {
        private readonly IMenuitemExtension _extension;

        public MenuitemModel(IMenuitemExtension extension)
        {
            _extension = extension;
            Command = new DelegateCommand(ExecuteCommand);
        }

        public DelegateCommand Command { get; }

        /// <summary>
        ///     菜单图片。
        /// </summary>
        public string ImageKey => _extension.ImageKey;

        /// <summary>
        ///     菜单项标题。
        /// </summary>
        public string Name => _extension.Name;

        /// <summary>
        ///     权限控制标识<see cref="Operation" />。
        /// </summary>
        public string Operation => _extension.Operation;

        /// <summary>
        ///     执行。
        /// </summary>
        public void ExecuteCommand()
        {
            _extension.Execute();
        }
    }
}
