// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using System;
using Android.Widget;
using Android.Support.V4.Graphics.Drawable;
using Android.Graphics;
using CarSto.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using static Android.App.DatePickerDialog;

namespace CarSto.Fragments
{
    public class UserProfileFragment : Fragment
    {
        private DateTime choosenBirthDate;
        private int choosenGender;
        private Dictionary<string, object> userData;

        #region Widgets
        private ImageView userAvatar;
        private RadioGroup genderGroup;
        private RadioButton genderMale, genderFemale;
        private TextView userName, userBirthDate;
        private Button exitButton, saveProfileBtn;
        #endregion

        public UserProfileFragment(Dictionary<string, object> userData)
        {
            this.userData = userData;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_userprofile, container, false);
            ToolbarHelper.SetToolbarStyle("Профиль", false);
            GetElements(view);
            InitData();
            SetEventHandlers();
            return view;
        }

        private void SetEventHandlers()
        {
            userBirthDate.Click += delegate
            {
                var today = DateTime.Today;
                var datepickerdialog = new Android.App.DatePickerDialog(this.Context, Datepickerdialog_DateSet, today.Year, today.Month, today.Day);
                datepickerdialog.SetTitle("Выберите дату рождения");
                datepickerdialog.CancelEvent += delegate
                {
                    choosenBirthDate = (DateTime)userData["BirthDate"];
                    userBirthDate.Text = ((DateTime)userData["BirthDate"]).ToShortDateString();
                };
                datepickerdialog.Show();
            };
            saveProfileBtn.Click += async delegate
            {
                var FML = userName.Text.Split(' ');
                var userSendData = new Dictionary<string, object> { { "FirstName", FML[0] }, { "MiddleName", FML[1] }, { "LastName", FML[2] }, { "Sex", choosenGender }, { "BirthDate", choosenBirthDate }, { "PrioritySto", 0 } };
                var response = await ClientAPI.PutAsync("User", userSendData);
                if (response == null) 
                    return;
                DataPreferences.Instance.SaveUserData(response.Item2);
                DataPreferences.Instance.LoadUserData();

            };
            genderGroup.CheckedChange += (s, e) =>
            {
                switch (e.CheckedId)
                {
                    case Resource.Id.userprofile_gender_male:
                        choosenGender = 0;
                        break;
                    case Resource.Id.userprofile_gender_female:
                        choosenGender = 1;
                        break;
                }
            };
        }

        private void Datepickerdialog_DateSet(object sender, DateSetEventArgs e)
        {
            choosenBirthDate = e.Date;
            userBirthDate.Text = e.Date.ToShortDateString();
        }

        private void GetElements(View view)
        {
            userAvatar = view.FindViewById<ImageView>(Resource.Id.userprofile_avatar);
            userName = view.FindViewById<TextView>(Resource.Id.userprofile_name);
            userBirthDate = view.FindViewById<TextView>(Resource.Id.userprofile_birthdate);
            genderGroup = view.FindViewById<RadioGroup>(Resource.Id.userprofile_gender_radiogroup);
            genderMale = view.FindViewById<RadioButton>(Resource.Id.userprofile_gender_male);
            genderFemale = view.FindViewById<RadioButton>(Resource.Id.userprofile_gender_female);
            saveProfileBtn = view.FindViewById<Button>(Resource.Id.userprofile_save_profile_btn);
            exitButton = view.FindViewById<Button>(Resource.Id.userprofile_exit);
        }

        private void InitData()
        {
            Bitmap avatar = BitmapFactory.DecodeResource(Resources, Resource.Drawable.avacar);
            RoundedBitmapDrawable roundDrawable = RoundedBitmapDrawableFactory.Create(Resources, avatar);
            roundDrawable.Circular = true;
            roundDrawable.SetAntiAlias(true);
            userAvatar.SetImageDrawable(roundDrawable);
            if (userData == null)
                return;
            userName.Text = $"{userData["FirstName"]} {userData["MiddleName"]} {userData["LastName"]}";
            genderGroup.Check(Resource.Id.userprofile_gender_male);
            userBirthDate.Text = ((DateTime)userData["BirthDate"]).ToShortDateString();
            choosenBirthDate = (DateTime)userData["BirthDate"];
        }
    }
}