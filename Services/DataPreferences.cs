// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
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
using Newtonsoft.Json;

namespace CarSto.Services
{
    public class DataPreferences
    {
        private static DataPreferences instance;
        private DataPreferences() { }
        public static DataPreferences Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataPreferences();
                }
                return instance;
            }
        }

        public List<string> selectedLabors = new List<string>();
        public List<string> selectedParts = new List<string>();

        /// <summary>
        /// UserToken
        /// </summary>
        private string token;
        public string Token
        {
            get
            {
                return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiNzk4MTE3NjgwNjgiLCJpc3MiOiJDYXJQb3J0In0.1XPEH1G86BL5kcHGLvvj7N1RYIyPZvESBQuld4C_6XU";
                if (token == null)
                    token = LoadToken();
                return token;
            }
            set
            {
                if (value == null)
                    return;
                token = value;
                SaveToken(value);
            }
        }

        /// <summary>
        /// UserGarage
        /// </summary>
        public string UserGarage
        {
            get
            {
                var userGarage = LoadUserGarage();
                return userGarage;
            }
            set
            {
                if (value == null)
                    return;
                SaveUserGarage(value);
            }
        }

        /// <summary>
        /// UserData
        /// </summary>
        public string UserData
        {
            get
            {
                var userData = LoadUserData();
                return userData;
            }
            set
            {
                if (value == null)
                    return;
                SaveUserData(value);
            }
        }

        /// <summary>
        /// Notifications
        /// </summary>
        public string Notifications
        {
            get
            {
                var notifications = LoadNotifications();
                return notifications;
            }
            set
            {
                if (value == null)
                    return;
                SaveNotifications(value);
            }
        }


        /// <summary>
        /// Saving notifications to SharedPreferences
        /// </summary>
        /// <param name="value"></param>
        private void SaveNotifications(string notifications)
        {
            var prefs = Application.Context.GetSharedPreferences("RunningAssistant.preferences", FileCreationMode.Private);
            var editor = prefs.Edit();
            editor.PutString("notifications", notifications);
            editor.Apply();
        }

        /// <summary>
        /// Access user token from SharedPreferences
        /// </summary>
        /// <returns>Notiifications JSON string</returns>
        private string LoadNotifications()
        {
            var prefs = Application.Context.GetSharedPreferences("RunningAssistant.preferences", FileCreationMode.Private);
            if (prefs.Contains("notifications"))
                return prefs.GetString("notifications", "");
            return "";
        }        

        /// <summary>
        /// Saving user auth token to SharedPreferences
        /// </summary>
        /// <param name="token">User's token JSON string</param>
        private void SaveToken(string token)
        {
            var prefs = Application.Context.GetSharedPreferences("RunningAssistant.preferences", FileCreationMode.Private);
            var editor = prefs.Edit();
            editor.PutString("userToken", token);
            editor.Apply();
        }

        /// <summary>
        /// Access user token from SharedPreferences
        /// </summary>
        /// <returns>User's token JSON string</returns>
        public string LoadToken()
        {
            var prefs = Application.Context.GetSharedPreferences("RunningAssistant.preferences", FileCreationMode.Private);
            if (prefs.Contains("userToken"))
                return prefs.GetString("userToken", "");
            return "";
        }

        /// <summary>
        /// Saving user garage to SharedPreferences
        /// </summary>
        /// <param name="usergarage">User's garage JSON string</param>
        public void SaveUserGarage(string usergarage)
        {
            var prefs = Application.Context.GetSharedPreferences("RunningAssistant.preferences", FileCreationMode.Private);
            var editor = prefs.Edit();
            editor.PutString("userGarage", usergarage);
            editor.Apply();
        }

        /// <summary>
        /// Access user garage from SharedPreferences
        /// </summary>
        /// <returns>User's garage JSON string</returns>
        public string LoadUserGarage()
        {
            var prefs = Application.Context.GetSharedPreferences("RunningAssistant.preferences", FileCreationMode.Private);
            if (prefs.Contains("userGarage"))
                return prefs.GetString("userGarage", "");
            return "";
        }

        /// <summary>
        /// Saving user data to SharedPreferences
        /// </summary>
        /// <param name="userData"></param>
        public void SaveUserData(string userData)
        {
            var prefs = Application.Context.GetSharedPreferences("RunningAssistant.preferences", FileCreationMode.Private);
            var editor = prefs.Edit();
            editor.PutString("userData", userData);
            editor.Apply();
        }

        /// <summary>
        /// Access user data from SharedPreferences
        /// </summary>
        /// <returns>User's data JSON string</returns>
        public string LoadUserData()
        {
            var prefs = Application.Context.GetSharedPreferences("RunningAssistant.preferences", FileCreationMode.Private);
            if (prefs.Contains("userData"))
                return prefs.GetString("userData", "");
            return "";
        }

        /// <summary>
        /// Clears keys and values from SharedPrefernces
        /// </summary>
        public void ClearPreferences()
        {
            Token = string.Empty;
        }
    }
}