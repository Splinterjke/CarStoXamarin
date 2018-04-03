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

namespace CarSto.Presenters.Login
{
    public class LoginPresenter : ILoginPresenter
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string RepeatPassword { get; set; }

        public string SecureCode { get; set; }

        public ILoginView View { get; set; }

        public bool TryLoginWithToken()
        {
            if (string.IsNullOrEmpty(DataPreferences.Instance.Token))
                return false;
            View.NavigateToMainScreen();
            return true;
            if (!string.IsNullOrEmpty(DataPreferences.Instance.UserData))
            {
                var userData = JsonConvert.DeserializeObject<Dictionary<string, object>>(DataPreferences.Instance.UserData);
                if (string.IsNullOrEmpty(userData["FirstName"]?.ToString()) || string.IsNullOrEmpty(userData["FirstName"]?.ToString()))
                {
                    View.ShowEditUserProfile();
                    return true;
                }
                View.NavigateToMainScreen();
                return true;
            }
            return false;
        }

        public void OnLoginFailed(string errorMessage)
        {
            View.ShowLoginFailed(errorMessage);
        }

        public void OnLoginSuccessful()
        {
            View.ShowSecureCode();
        }

        public async void OnLoginClicked()
        {
            if (Login.Length < 10)
            {
                View.ShowLoginValidationError();
                return;
            }
            if (Password.Length < 6)
            {
                View.ShowPasswordValidationError();
                return;
            }
            var model = new Dictionary<string, string>
                {
                    { "phone", Login },
                    { "password", Password }
                };
            var response = await ClientAPI.PutAsync("Account/Login", model);
            if (response.Item1 && !response.Item2.Contains("Code already exist"))
            {
                OnLoginFailed(response.Item2);
                return;
            }
            OnLoginSuccessful();
        }

        public void OnRegisterClicked()
        {
            View.ShowRegistration();
        }

        public async void OnRegisterCompleteClicked()
        {
            var model = new Dictionary<string, string>
                {
                    { "phone", Login },
                    { "password", Password },
                    { "confirmPassword", RepeatPassword }
                };
            var response = await ClientAPI.PostAsync("Account/User", model);
            if (response.Item1)
            {
                OnLoginFailed(response.Item2);
                return;
            }
            OnLoginSuccessful();
        }

        public async void OnSecureCodeCompleteClicked()
        {
            var model = new Dictionary<string, string>
                {
                    { "phone", Login },
                    { "password", Password },
                    { "code", SecureCode }
                };
            var url = string.IsNullOrEmpty(RepeatPassword) ? "LoginConfirm" : "UserRegCode";
            var response = await ClientAPI.PutAsync($"Account/{url}", model);
            if (response.Item1)
            {
                OnLoginFailed(response.Item2);
                return;
            }
            var token = response.Item2.Split(',')[0].Split(':')[1].Trim(new char[] { '"' });
            DataPreferences.Instance.Token = token;
            var jsonUserData = await ClientAPI.GetAsync("User");
            if (response.Item1)
            {
                OnLoginFailed(response.Item2);
                return;
            }
            DataPreferences.Instance.UserData = jsonUserData.Item2;
            TryLoginWithToken();
        }
    }
}