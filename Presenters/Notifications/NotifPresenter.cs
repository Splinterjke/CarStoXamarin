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

namespace CarSto.Presenters.Notifications
{
    public class NotifPresenter : INotifPresenter
    {
        public INotifView View { get; set; }
        public List<Dictionary<string, object>> notifications;

        public async void LoadNotifications()
        {
            View.ShowProcessing();
            if (!string.IsNullOrEmpty(DataPreferences.Instance.Notifications))
                OnLoadSuccessful(DataPreferences.Instance.Notifications);
            
            var response = await ClientAPI.GetAsync("Notif");
            View.HideProcessing();
            if (response.Item1)
            {
                OnLoadFailed(response.Item2);
                return;
            }
            OnLoadSuccessful(response.Item2);
        }

        public void OnLoadFailed(string errorMessage)
        {
            View.ShowError(errorMessage);
        }

        public void OnLoadSuccessful(string response)
        {
            try
            {
                DataPreferences.Instance.Notifications = response;
                notifications = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(response);
                if (notifications == null || notifications.Count == 0)
                {
                    View.ShowError("Список уведомлений пуст");
                    return;
                }
                View.UpdateNotificationList();
            }
            catch (Exception ex)
            {
                View.ShowError(ex.Message);
            }
        }

        public void OnNotifClicked(int index)
        {
            //var response = await ClientAPI.PutAsync($"Notif/{notifications[index]["Id"]}");
        }

        public async void OnNotifRemoved(int index)
        {
            try
            {
                var id = notifications[index]["Id"];
                notifications.RemoveAt(index);
                if(notifications.Count == 0)
                {
                    DataPreferences.Instance.Notifications = string.Empty;
                    View.ShowError("Список уведомлений пуст");
                }
                else DataPreferences.Instance.Notifications = JsonConvert.SerializeObject(notifications);
                await ClientAPI.DeleteAsync($"Notif/{id}");
            }
            catch (JsonSerializationException)
            {
                notifications = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(DataPreferences.Instance.Notifications);
            }
        }

        public void OnAllNotifRemoved()
        {

        }
    }
}