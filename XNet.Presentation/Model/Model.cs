using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace XNet.Presentation
{

    /// <summary>
    /// 抽象的Model
    /// </summary>
    [DataContract]
    public abstract class Model : INotifyPropertyChanged
    {

        #region impletment of INotifyPropertyChanged

        /// <summary>
        /// 属性变更通知事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion




        #region Functions

        /// <summary>
        /// 设置属性值，如果属性值改变，则触发PropertyChanged事件，并返回true
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) { return false; }

            field = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        #endregion
    }
}
