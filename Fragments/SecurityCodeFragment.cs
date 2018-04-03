using Android.OS;
using Android.Views;
using Android.Support.V4.App;
using Android.Widget;
using Android.Support.V7.App;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using CarSto.Services;
using CarSto.Activities;
using Android.Graphics.Drawables;
using Android.Graphics;
using CarSto.Presenters;
using System;
using static CarSto.Services.ViewInjectorHelper;
using CarSto.Presenters.Login;

namespace CarSto.Fragments
{
    public class SecurityCodeFragment : Fragment
    {
        #region Widgets
        [InjectView(Resource.Id.security_code_text)]
        private EditText codeText;

        [InjectView(Resource.Id.security_cancel_btn)]
        private ImageButton cancelBtn;

        [InjectView(Resource.Id.security_ok_btn)]
        private ImageButton signinBtn;
        #endregion

        private LoginPresenter presenter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_securitycode, null, false);
            presenter = (this.Activity as TestUILoginActivity).presenter;
            ViewInjector.Inject(this, view);
            return view;
        }       

        #region Methods
        [InjectOnClick(Resource.Id.security_ok_btn)]
        private void Accept_Clicked(object obj, EventArgs args)
        {
            presenter.OnSecureCodeCompleteClicked();
        }

        [InjectOnClick(Resource.Id.security_cancel_btn)]
        private void Cancel_Clicked(object obj, EventArgs args)
        {
            this.Activity.SupportFragmentManager.PopBackStack();
        }

        [InjectOnTextChanged(Resource.Id.security_code_text)]
        private void PhoneText_Changed(object obj, Android.Text.TextChangedEventArgs args)
        {
            presenter.SecureCode = args.Text.ToString();
        }
        #endregion
    }
}