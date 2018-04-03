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

namespace CarSto.Presenters.AddCar
{
    public class AddCarPresenter : IAddCarPresenter
    {
        public IAddCarView View { get; set; }

        public string VIN { get; set; }

        public string SSD { get; set; }

        public Dictionary<string, object> SelectedCar { get; set; }

        public List<Dictionary<string, object>> carList { get; set; }

        public async void OnShowCarClicked()
        {
            var response = await ClientAPI.GetAsync($"Catalog/GetVehiclesBySsd?vin={VIN}&ssd={SSD}");
            if (response.Item1)
            {
                View.ShowError(response.Item2);
                return;
            }
            carList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(response.Item2);
            List<string> cars = new List<string>(carList.Count);
            for (int i = 0; i < carList.Count; i++)
            {
                var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(carList[i]["Data"].ToString());
                cars.Add($"Марка: {carList[i]["Make"]}\nМодель: {carList[i]["Model"]}\nДвигатель: {data["Двигатель"]}\nКор.передач: {data["КП"]}");
            }
            View.ShowAvailableCars(cars);
            View.ShowAddToGarageButton();
        }

        public void OnVinTextValidating()
        {
            if (VIN.Length == 17)
                View.EnabledCheckVinButton();
            else View.DisableCheckVinButton();
        }

        public void OnVinValidationFailed(string error)
        {
            SSD = "null";
            SelectionStep();
        }

        public void OnVinValidationSuccessful(string response)
        {
            //if (error.Contains("[]"))
            //{
            //    SSD = "null";
            //    SelectionStep();
            //}
            //else View.ShowError(error);
        }

        public async void SelectionStep()
        {
            View.ClearFields();
            var response = await ClientAPI.GetAsync($"Catalog/SelectionStep?vin={VIN}&ssd={SSD}");
            if (response.Item1)
            {
                View.ShowError(response.Item2);
                return;
            }
            var data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(response.Item2);
            var prevSelected = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(data[0]["PrevSelected"].ToString());
            for (int i = 0; i < prevSelected.Count; i++)
            {
                //var values = JsonConvert.DeserializeObject<List<string>>();
                View.AddPrevFields(prevSelected[i]["Name"].ToString(), prevSelected[i]["Value"].ToString());
            }

            var currentFieldOptions = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(data[0]["CurrentFieldOptions"].ToString());
            for (int i = 0; i < currentFieldOptions.Count; i++)
            {
                var ssd = JsonConvert.DeserializeObject<List<string>>(currentFieldOptions[i]["Ssd"].ToString());
                var values = JsonConvert.DeserializeObject<List<string>>(currentFieldOptions[i]["Value"].ToString());
                values.Insert(0, "Не выбрано");
                View.AddCurrentFields(currentFieldOptions[i]["Name"].ToString(), values, ssd);
            }
            View.ShowFindCarsButton();
        }

        public async void VinValidating()
        {
            var response = await ClientAPI.PostAsync("Garage/Vin/", VIN);
            if (response.Item1)
            {
                OnVinValidationFailed(response.Item2);
                //View.ShowError(response.Item2);
                return;
            }
            carList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(response.Item2);
            if (carList == null || carList.Count == 0)
            {
                OnVinValidationFailed(response.Item2);
                //View.ShowError(response.Item2);
                return;
            }
            else
            {
                var cars = new List<string>(carList.Count);
                for (int i = 0; i < carList.Count; i++)
                {
                    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(carList[i]["Data"].ToString());
                    cars.Add($"Марка: {carList[i]["Make"]}\nМодель: {carList[i]["Model"]}\nДвигатель: {data["Двигатель"]}\nКор.передач: {data["КП"]}");
                }
                View.ShowAvailableCars(cars);
                View.ShowAddToGarageButton();
            }
                
        }

        public async void OnCarAddClicked()
        {
            if (SelectedCar == null)
                return;
            var response = await ClientAPI.PostAsync("Garage/DecodedVin", SelectedCar);
            if (response.Item1)
            {
                OnVinValidationFailed(response.Item2);
                //View.ShowError(response.Item2);
                return;
            }
            View.UpdateGarage();
        }
    }
}