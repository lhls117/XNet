using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNet.Presentation.Wpf
{
    public class EntryModel : Model
    {
        private readonly IEntryExtension _extension;
        private bool _isChecked;

        public EntryModel(IEntryExtension extension)
        {
            _extension = extension;
        }

        /// <summary>
        ///     权限控制标识<see cref="Operation" />。
        /// </summary>
        public string Operation => _extension.Operation;

        /// <summary>
        ///     菜单图片。
        /// </summary>
        public string ImageKey => _extension.ImageKey;

        /// <summary>
        ///     关联的视图。
        /// </summary>
        public IView View => _extension.ViewModel.View;

        /// <summary>
        ///     菜单项标题。
        /// </summary>
        public string Name => _extension.Name;

        /// <summary>
        ///     是否选择。
        /// </summary>
        public bool IsChecked
        {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }
    }
}
