using System;
using System.Collections.Generic;

using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Support.V4.Graphics.Drawable;
using CarSto.Services;
using CarSto.Activities;
using Android.Support.V4.App;
using static CarSto.Services.ViewInjectorHelper;
using CarSto.Presenters.UserProfile;
using FFImageLoading;
using FFImageLoading.Views;
using FFImageLoading.Transformations;

namespace CarSto.Fragments
{
    public class EditUserProfileFragment : Fragment, IUserProfileView
    {
        #region Widgets
        [InjectView(Resource.Id.edituserprofile_avatar)]
        private ImageViewAsync userAvatar;

        [InjectView(Resource.Id.edituserprofile_gender_radiogroup)]
        private RadioGroup genderGroup;

        [InjectView(Resource.Id.edituserprofile_gender_male)]
        private RadioButton genderMale;

        [InjectView(Resource.Id.edituserprofile_gender_female)]
        private RadioButton genderFemale;

        [InjectView(Resource.Id.edituserprofile_firstname_text)]
        private TextView firstName;

        [InjectView(Resource.Id.edituserprofile_middlename_text)]
        private TextView middleName;

        [InjectView(Resource.Id.edituserprofile_lastname_text)]
        private TextView lastName;

        [InjectView(Resource.Id.edituserprofile_birthdate)]
        private TextView userBirthDate;

        [InjectView(Resource.Id.edituserprofile_cancel_btn)]
        private ImageButton cancelBtn;

        [InjectView(Resource.Id.edituserprofile_ok_btn)]
        private ImageButton acceptBtn;
        #endregion

        private UserProfilePresenter presenter;
        #region Base Methods
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            presenter = new UserProfilePresenter() { View = this };
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_edituserprofile, container, false);
            ViewInjector.Inject(this, view);
            presenter.LoadUserData();
            return view;
        }
        #endregion

        #region Methods
        [InjectOnCheckedChange(Resource.Id.edituserprofile_gender_radiogroup)]
        private void Gender_Changed(object o, RadioGroup.CheckedChangeEventArgs e)
        {
            switch (e.CheckedId)
            {
                case Resource.Id.userprofile_gender_male:
                    presenter.Gender = 0;
                    break;
                case Resource.Id.userprofile_gender_female:
                    presenter.Gender = 1;
                    break;
            }
        }

        [InjectOnClick(Resource.Id.edituserprofile_birthdate)]
        private void BirthDayTextView_Clicked(object o, EventArgs e)
        {
            var datepickerdialog = new Android.App.DatePickerDialog(this.Context, Datepickerdialog_DateSet, presenter.BirthDate.Year, presenter.BirthDate.Month, presenter.BirthDate.Day);
            datepickerdialog.SetTitle("Выберите дату рождения");
            datepickerdialog.Show();
        }

        private void Datepickerdialog_DateSet(object o, Android.App.DatePickerDialog.DateSetEventArgs e)
        {
            presenter.BirthDate = e.Date;
            userBirthDate.Text = e.Date.ToShortDateString();
        }

        public void ShowSavingError(string errorMessage)
        {
            Toast.MakeText(this.Activity, errorMessage, ToastLength.Short).Show();
        }

        [InjectOnClick(Resource.Id.edituserprofile_ok_btn)]
        private void Accept_Clicked(object o, EventArgs e)
        {
            presenter.SaveProfile();
        }

        [InjectOnClick(Resource.Id.edituserprofile_cancel_btn)]
        private void Cancel_Clicked(object o, EventArgs e)
        {
            this.Activity.SupportFragmentManager.PopBackStack();
        }

        [InjectOnTextChanged(Resource.Id.edituserprofile_firstname_text)]
        private void FirstNameText_Changed(object obj, Android.Text.TextChangedEventArgs args)
        {
            presenter.FirstName = args.Text.ToString();
        }

        [InjectOnTextChanged(Resource.Id.edituserprofile_middlename_text)]
        private void MiddleNameText_Changed(object obj, Android.Text.TextChangedEventArgs args)
        {
            presenter.MiddleName = args.Text.ToString();
        }

        [InjectOnTextChanged(Resource.Id.edituserprofile_lastname_text)]
        private void LastNameText_Changed(object obj, Android.Text.TextChangedEventArgs args)
        {
            presenter.LastName = args.Text.ToString();
        }

        public void CloseEditUserProfileForm()
        {
            if (this.ParentFragment is UserProfileFragment)
                Cancel_Clicked(this, null);
            else NavigateToMainScreen();
        }

        public void ShowUserData()
        {
            if (presenter.Gender == 0)
                genderGroup.Check(Resource.Id.userprofile_gender_male);
            else genderGroup.Check(Resource.Id.userprofile_gender_female);
            userBirthDate.Text = presenter.BirthDate.ToShortDateString();
            firstName.Text = presenter.FirstName;
            middleName.Text = presenter.MiddleName;
            lastName.Text = presenter.LastName;
            ImageService.Instance.LoadCompiledResource(presenter.AvatarPath).DownSample(width: 180).Transform(new CircleTransformation(12, "#FFFFFF")).Into(userAvatar);
        }

        public void NavigateToMainScreen()
        {
            this.Activity.StartActivity(typeof(MenuActivity));
        }
        #endregion
    }
}