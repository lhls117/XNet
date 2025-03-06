using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNet.Presentation.Wpf
{
    /// <summary>
    /// 菜单栏扩展
    /// </summary>
    public interface IMenuitemExtension : IExtension
    {
        /// <summary>
        ///     图标。
        /// </summary>
        string ImageKey { get; }

        /// <summary>
        ///     名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     扩展使用操作。
        /// </summary>
        string Operation { get; }

        /// <summary>
        ///     执行命令。
        /// </summary>
        void Execute();

    }


    /// <summary>
    ///     菜单扩展。
    /// </summary>
    public abstract class MenuitemExtension : IMenuitemExtension
    {
        #region Implementation of IMenuitemExtension

        public string ImageKey { get; protected set; }

        public virtual string Name { get; protected set; }


        public string Operation { get; protected set; }


        public abstract void Execute();
        #endregion
    }

    /// <summary>
    ///     菜单视图模型扩展。
    /// </summary>
    public abstract class MenuitemExtension<TViewModel> : MenuitemExtension, IModelExtension
        where TViewModel : ViewModel
    {
        private readonly ExportFactory<TViewModel> _viewModelFactory;

        protected MenuitemExtension(ExportFactory<TViewModel> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public TViewModel ViewModel
        {
            get
            {
                using (var export = _viewModelFactory.CreateExport())
                {
                    return export.Value;
                }
            }
        }

        #region Implementation of IModelExtension

        ViewModel IModelExtension.ViewModel => ViewModel;

        #endregion
    }


}
