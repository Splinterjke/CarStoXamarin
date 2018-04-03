// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using Android.Widget;
using System;
using Android.Content;
using CarSto.Services;
using Calligraphy;
using CarSto.Fragments;
using static CarSto.Services.ViewInjectorHelper;
using CarSto.Presenters;
using CarSto.Presenters.Login;
using Android.Support.V7.App;
using Android.App;
using Android.OS;

namespace CarSto.Activities
{
    [Activity(MainLauncher = true, Label = "TestUILoginActivity", Theme = "@style/DarkStatusBar")]
    public class TestUILoginActivity : AppCompatActivity, ILoginView
    {
        public LoginPresenter presenter;
        #region Base Methods
        protected override void OnCreate(Bundle savedInstanceState)
        {           
            base.OnCreate(savedInstanceState);
            presenter = new LoginPresenter() { View = this };
            var authResult = presenter.TryLoginWithToken();
            if (authResult)
                return;
            SetContentView(Resource.Layout.activity_testui_login);                   
            ViewInjector.Inject(this);
            ShowLoginForm();            
        }

        protected override void AttachBaseContext(Context context)
        {
            base.AttachBaseContext(CalligraphyContextWrapper.Wrap(context));
        }
        #endregion

        #region Methods
        public void ShowLoginForm()
        {
            SupportFragmentManager.BeginTransaction().AddToBackStack(null).Replace(Resource.Id.login_content, new LoginFragment()).Commit();
        }

        public void ShowRegistration()
        {
            SupportFragmentManager.BeginTransaction().AddToBackStack(null).Replace(Resource.Id.login_content, new RegisterFragment()).Commit();
        }

        public void ShowLoginFailed(string errorMessage)
        {
            Toast.MakeText(this, errorMessage, ToastLength.Short).Show();
        }

        public void ShowLoginValidationError()
        {
            Toast.MakeText(this, "Некорретный логин", ToastLength.Short).Show();
        }

        public void ShowPasswordValidationError()
        {
            Toast.MakeText(this, "Некорретный пароль", ToastLength.Short).Show();
        }

        public void ShowSecureCodeValidationError()
        {
            Toast.MakeText(this, "Некорретный проверочный код", ToastLength.Short).Show();
        }

        public void ShowSecureCode()
        {
            SupportFragmentManager.BeginTransaction().AddToBackStack(null).Replace(Resource.Id.login_content, new SecurityCodeFragment()).Commit();
        }

        public void NavigateToMainScreen()
        {
            StartActivity(typeof(MenuActivity));
        }

        public void ShowEditUserProfile()
        {
            SupportFragmentManager.BeginTransaction().AddToBackStack(null).Replace(Resource.Id.login_content, new EditUserProfileFragment()).Commit();
        }        
        #endregion
    }
}