using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNet.Presentation.Wpf
{

    /// <summary>
    /// 视图模型扩展。
    /// </summary>
    public abstract class ModelExtension<TViewModel> : IModelExtension where TViewModel : ViewModel
    {
        private readonly ExportFactory<TViewModel> _viewModelFactory;

        protected ModelExtension(ExportFactory<TViewModel> viewModelFactory)
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
    /// <summary>
    ///     入口视图模型扩展。
    /// </summary>
    public abstract class EntryExtension<TViewModel> : ModelExtension<TViewModel>, IEntryExtension
        where TViewModel : ViewModel
    {
        protected EntryExtension(ExportFactory<TViewModel> viewModelFactory) : base(viewModelFactory)
        {
        }

        #region Implementation of IEntryExtension

        public string ImageKey { get; protected set; }

        public virtual string Name { get; protected set; }

       
        public string Operation { get; protected set; }

        #endregion
    }
}
