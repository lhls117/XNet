using System;
using System.Collections.Generic;
using System.Text;

namespace XNet.Presentation
{
    public interface IView
    {
        /// <summary>
        /// Gets or sets the data context of the view.
        /// </summary>
        object DataContext { get; set; }
    }
}
