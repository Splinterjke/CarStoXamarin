using System;
using System.Collections.Generic;
using CarSto.Services;
using Newtonsoft.Json;

namespace CarSto.Presenters.Garage
{
    public class GaragePresenter : IGaragePresenter
    {
        public IGarageView View { get; set; }
        public List<Dictionary<string, object>> userCars;

        public async void LoadGarage()
        {
            if (!string.IsNullOrEmpty(DataPreferences.Instance.UserGarage))
                OnLoadSuccessful(DataPreferences.Instance.UserGarage);
            View.ShowProcessing();
            var response = await ClientAPI.GetAsync("Garage");
            View.HideProcessing();
            if (response.Item1)
            {
                OnLoadFailed(response.Item2);
                return;
            }
            OnLoadSuccessful(response.Item2);
        }

        public async void OnCarRemoved(int index)
        {
            try
            {
                var id = userCars[index]["Id"];
                userCars.RemoveAt(index);
                if (userCars.Count == 0)
                {
                    DataPreferences.Instance.UserGarage = string.Empty;
                    View.ShowError("Список пользовательских автомобилей пуст");
                }
                else DataPreferences.Instance.Notifications = JsonConvert.SerializeObject(userCars);
                var response = await ClientAPI.DeleteAsync($"Garage/{id}");
            }
            catch (JsonSerializationException)
            {
                userCars = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(DataPreferences.Instance.UserGarage);
            }
        }

        public void OnLoadFailed(string errorMessage)
        {
            View.ShowError(errorMessage);
        }

        public void OnLoadSuccessful(string response)
        {
            try
            {
                DataPreferences.Instance.UserGarage = response;
                userCars = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(response);
                //if (userCars == null || userCars.Count == 0)
                //{
                //    View.ShowError("Список пользовательских автомобилей пуст");
                //    return;
                //}
                View.UpdateGarage();
            }
            catch (Exception ex)
            {
                View.ShowError(ex.Message);
            }
        }

        public void OnMileageEdited()
        {

        }
    }
}