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
    public interface ILoginView
    {
        void ShowLoginForm();
        void ShowSecureCode();
        void ShowRegistration();
        void NavigateToMainScreen();
        void ShowEditUserProfile();
        void ShowLoginFailed(string errorMessage);
        void ShowLoginValidationError();
        void ShowPasswordValidationError();
        void ShowSecureCodeValidationError();
    }
}