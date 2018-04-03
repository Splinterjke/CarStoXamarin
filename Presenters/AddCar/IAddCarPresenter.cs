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

namespace CarSto.Presenters.AddCar
{
    public interface IAddCarPresenter
    {
        IAddCarView View { get; set; }
        void VinValidating();
        void SelectionStep();
        void OnCarAddClicked();
        void OnShowCarClicked();
        void OnVinTextValidating();
        void OnVinValidationSuccessful(string response);
        void OnVinValidationFailed(string error);
    }
}