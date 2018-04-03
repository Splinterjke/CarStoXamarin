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

namespace CarSto.Presenters.Garage
{
    public interface IGarageView
    {
        void UpdateGarage();
        void ShowError(string errorMessage);
        void ShowProcessing();
        void HideProcessing();
    }
}