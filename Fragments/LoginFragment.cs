using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using static CarSto.Services.ViewInjectorHelper;
using CarSto.Presenters.Login;
using CarSto.Presenters;
using CarSto.Activities;

namespace CarSto.Fragments
{
    public class LoginFragment : Fragment, IStyleView
    {
        #region Widgets
        [InjectView(Resource.Id.login_signin_btn)]
        private Button signinBtn;

        [InjectView(Resource.Id.login_createaccount_btn)]
        private TextView createAccountBtn;

        [InjectView(Resource.Id.login_phone_edittext)]
        public EditText phoneText;

        [InjectView(Resource.Id.login_password_edittext)]
        public EditText passwordText;
        #endregion

        private LoginPresenter presenter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_login, container, false);
            presenter = (this.Activity as TestUILoginActivity).presenter;
            ViewInjector.Inject(this, view);
            StyleView();
            return view;
        }
        
        [InjectOnTextChanged(Resource.Id.login_phone_edittext)]
        private void PhoneText_Changed(object obj, Android.Text.TextChangedEventArgs args)
        {
            presenter.Login = args.Text.ToString();
        }

        [InjectOnTextChanged(Resource.Id.login_password_edittext)]
        private void PasswordText_Changed(object obj, Android.Text.TextChangedEventArgs args)
        {
            presenter.Password = args.Text.ToString();
        }

        [InjectOnClick(Resource.Id.login_signin_btn)]
        private void Login_Clicked(object obj, EventArgs args)
        {
            presenter.OnLoginClicked();
        }

        [InjectOnClick(Resource.Id.login_createaccount_btn)]
        private void Register_Clicked(object obj, EventArgs args)
        {
            presenter.OnRegisterClicked();
        }

        public void StyleView()
        {
            createAccountBtn.PaintFlags = createAccountBtn.PaintFlags | Android.Graphics.PaintFlags.UnderlineText;
        }
    }
}