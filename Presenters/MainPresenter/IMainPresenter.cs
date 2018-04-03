using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CarSto.Presenters.MainPresenter
{
    public interface IMainPresenter
    {
        IMainView View { get; set; }
        void InitStartPage();
    }
}