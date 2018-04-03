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
    public interface IAddCarView
    {
        void ClearFields();
        void UpdateGarage();
        void AddPrevFields(string titles, string value);
        void AddCurrentFields(string title, List<string> values, List<string> ssd);
        void ShowFindCarsButton();
        void ShowAddToGarageButton();
        void EnabledCheckVinButton();
        void DisableCheckVinButton();
        void ShowAvailableCars(List<string> cars);
        void ShowError(string errorMessage);
        void ShowProcessing();
        void HideProcessing();
    }
}