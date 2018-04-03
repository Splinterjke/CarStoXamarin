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
using CarSto.Services;
using Newtonsoft.Json;

namespace CarSto.Presenters.MainPresenter
{
    public class MainPresenter : IMainPresenter
    {
        public IMainView View { get; set; }

        public void InitStartPage()
        {
            View.ShowNotifications();
        }
    }
}