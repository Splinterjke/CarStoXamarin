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

namespace CarSto.Presenters.Login
{
    public interface ILoginPresenter
    {
        ILoginView View { get; set; }
        bool TryLoginWithToken();
        void OnLoginClicked();
        void OnRegisterClicked();
        void OnRegisterCompleteClicked();
        void OnSecureCodeCompleteClicked();
        void OnLoginFailed(string errorMessage);
        void OnLoginSuccessful();
    }
}