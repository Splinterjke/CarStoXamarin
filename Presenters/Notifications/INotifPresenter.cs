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

namespace CarSto.Presenters.Notifications
{
    public interface INotifPresenter
    {
        INotifView View { get; set; }
        void LoadNotifications();
        void OnLoadSuccessful(string response);
        void OnLoadFailed(string error);
        void OnNotifClicked(int index);
        void OnNotifRemoved(int index);
        void OnAllNotifRemoved();
    }
}