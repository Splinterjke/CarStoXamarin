// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Collections.Generic;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Text.RegularExpressions;
using CarSto.Services;
using CarSto.Activities;
using Android.Support.V4.App;
using System;
using static CarSto.Services.ViewInjectorHelper;
using CarSto.Presenters.Login;

namespace CarSto.Fragments
{
    public class RegisterFragment : Fragment
    {
        #region Widgets
        [InjectView(Resource.Id.register_phone_text)]
        private EditText phoneText;

        [InjectView(Resource.Id.register_password_text)]
        private EditText passwordText;

        [InjectView(Resource.Id.register_repeatpassword_text)]
        private EditText repeatPasswordText;

        [InjectView(Resource.Id.register_cancel_btn)]
        private ImageButton cancelButton;

        [InjectView(Resource.Id.register_ok_btn)]
        private ImageButton okButton;
        #endregion

        private LoginPresenter presenter;

        #region Base Methods
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_register, null, false);
            presenter = (this.Activity as TestUILoginActivity).presenter;
            ViewInjector.Inject(this, view);
            return view;
        }
        #endregion

        #region Methods
        [InjectOnClick(Resource.Id.register_ok_btn)]
        private void Accept_Clicked(object o, EventArgs args)
        {
            presenter.OnRegisterCompleteClicked();
        }

        [InjectOnClick(Resource.Id.register_cancel_btn)]
        private void Cancel_Clicked(object o, EventArgs args)
        {
            presenter.RepeatPassword = null;
            this.Activity.SupportFragmentManager.PopBackStack();            
        }

        [InjectOnTextChanged(Resource.Id.register_phone_text)]
        private void PhoneText_Changed(object obj, Android.Text.TextChangedEventArgs args)
        {
            presenter.Login = args.Text.ToString();
        }

        [InjectOnTextChanged(Resource.Id.register_password_text)]
        private void PasswordText_Changed(object obj, Android.Text.TextChangedEventArgs args)
        {
            presenter.Password = args.Text.ToString();
        }

        [InjectOnTextChanged(Resource.Id.register_repeatpassword_text)]
        private void RepeatPasswordText_Changed(object obj, Android.Text.TextChangedEventArgs args)
        {
            presenter.RepeatPassword = args.Text.ToString();
        }
        #endregion
    }
}