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
using FFImageLoading;

namespace CarSto.Presenters.UserProfile
{
    public class UserProfilePresenter : IUserProfilePresenter
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string AvatarPath { get; set; }

        public IUserProfileView View { get; set; }

        public void LoadUserData()
        {
            var userData = DataPreferences.Instance.UserData;
            AvatarPath = "avacar.jpg";
            //BirthDate = ((DateTime)userData["BirthDate"]).Year == 0001 ? DateTime.Today : ((DateTime)userData["BirthDate"]);
            //FirstName = userData["FirstName"]?.ToString() ?? string.Empty;
            //MiddleName = userData["MiddleName"]?.ToString() ?? string.Empty;
            //LastName = userData["LastName"]?.ToString() ?? string.Empty;
            //Gender = Convert.ToInt32(userData["Sex"]);
            View.ShowUserData();
        }

        public void OnSavingFailed(string errorMessage)
        {
            View.ShowSavingError(errorMessage);
        }

        public void OnSavingSuccessful(string response)
        {
            DataPreferences.Instance.SaveUserData(response);
            DataPreferences.Instance.LoadUserData();
            View.CloseEditUserProfileForm();
        }

        public async void SaveProfile()
        {
            var userSendData = new Dictionary<string, object> { { "FirstName", FirstName }, { "MiddleName", MiddleName }, { "LastName", LastName }, { "Sex", Gender }, { "BirthDate", BirthDate }, { "PrioritySto", 0 } };
            var response = await ClientAPI.PutAsync("User", userSendData);
            if(response.Item1)
            {
                OnSavingFailed(response.Item2);
                return;
            }
            OnSavingSuccessful(response.Item2);
        }
    }
}