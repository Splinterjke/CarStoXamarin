// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.OS;
using Toolbar = Android.Support.V7.Widget.Toolbar;
using Android.Widget;
using CarSto.Services;
using System.Collections.Generic;
using Newtonsoft.Json;
using CarSto.Presenters.MainPresenter;
using static CarSto.Services.ViewInjectorHelper;
using Android.App;
using Android.Views;
using Android.Support.Graphics.Drawable;
using Com.Ittianyu.Bottomnavigationviewex;
using CarSto.Presenters;
using System;

namespace CarSto.Activities
{
    [Activity(Label = "Menu")]
    public class MenuActivity : AppCompatActivity, IMainView, IStyleView
    {
        #region Variables

        #endregion

        #region Widgets
        [InjectView(Resource.Id.bottom_navigation)]
        private BottomNavigationViewEx bottomNavigationView;

        [InjectView(Resource.Id.tool_bar)]
        private Toolbar toolbar;

        [InjectView(Resource.Id.toolbar_title)]
        private TextView toolbarTitle;

        [InjectView(Resource.Id.loading_bar)]
        private View loadingBar;

        private Java.Lang.Runnable action;
        private AnimatedVectorDrawableCompat avdProgress;

        private MainPresenter presenter;
        #endregion

        #region Base Methods
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //align current activity to fullscreen < --makes statusbar and navbar transparent
            //if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
            //{
            //    Window.AddFlags(WindowManagerFlags.LayoutNoLimits);
            //    Window.AddFlags(WindowManagerFlags.TranslucentNavigation);
            //    Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
            //}
            presenter = new MainPresenter() { View = this };
            SetContentView(Resource.Layout.activity_menu);
            ViewInjector.Inject(this);
            StyleView();
            //setting custom toolbar
            ToolbarHelper.Init(this, toolbar, toolbarTitle);
            ToolbarHelper.SetToolbar();
            //setting StartPageFragment as first displayed fragment
            if (savedInstanceState == null)
            {
                BottomNavigationView_NavigationItemSelected(this, new BottomNavigationView.NavigationItemSelectedEventArgs(false, bottomNavigationView.Menu.FindItem(Resource.Id.nav_garagepage)));
            }
            //Firebase.FirebaseApp.InitializeApp(this);
            var IsPlayServicesAvailable = GoogleServiceHelper.Instance.IsPlayServicesAvailable(this);
            if (!IsPlayServicesAvailable.Item1)
                AlertDialogs.SimpleAlertDialog(IsPlayServicesAvailable.Item2, this).Show();
        }
        #endregion

        #region Methods
        [InjectOnNavigationItemSelected(Resource.Id.bottom_navigation)]
        private void BottomNavigationView_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            DataPreferences.Instance.selectedParts?.Clear();
            DataPreferences.Instance.selectedLabors?.Clear();

            //creating fragment by ItemId
            var backStackIdentifier = string.Empty;
            Android.Support.V4.App.Fragment detailContent = null;
            switch (e.Item.ItemId)
            {
                case Resource.Id.nav_garagepage:
                    detailContent = new Fragments.GarageFragment();
                    backStackIdentifier = "GarageFragment";
                    break;
                case Resource.Id.nav_orderspage:
                    detailContent = new Fragments.OrdersCategoryFragment();
                    backStackIdentifier = "OrdersCategoryFragment";
                    break;
                case Resource.Id.nav_notifications:
                    detailContent = new Fragments.NotificationsFragment();
                    backStackIdentifier = "NotificationsFragment";
                    break;
            }

            //switching active fragment
            if (detailContent != null)
                SupportFragmentManager.BeginTransaction().AddToBackStack(backStackIdentifier).Replace(Resource.Id.detail_content, detailContent).Commit();
        }

        public void ShowGarage()
        {
            BottomNavigationView_NavigationItemSelected(this, new BottomNavigationView.NavigationItemSelectedEventArgs(false, bottomNavigationView.Menu.FindItem(Resource.Id.nav_garagepage)));
        }

        public void ShowOrders()
        {
            //BottomNavigationView_NavigationItemSelected(this, new BottomNavigationView.NavigationItemSelectedEventArgs(false, bottomNavigationView.Menu.FindItem(Resource.Id.nav_orderspage)));
        }

        public void ShowNotifications()
        {
            BottomNavigationView_NavigationItemSelected(this, new BottomNavigationView.NavigationItemSelectedEventArgs(false, bottomNavigationView.Menu.FindItem(Resource.Id.nav_notifications)));
        }

        public void ShowUserProfile()
        {
            //BottomNavigationView_NavigationItemSelected(this, new BottomNavigationView.NavigationItemSelectedEventArgs(false, bottomNavigationView.Menu.FindItem(Resource.Id.nav_userprofile)));
        }

        public void ShowProcessing()
        {
            action = new Java.Lang.Runnable(() => RepeatAnimation());
            //if (avdProgress == null)
            //{
            avdProgress = Android.Support.Graphics.Drawable.AnimatedVectorDrawableCompat.Create(this, Resource.Drawable.avd_line);
            loadingBar.Background = avdProgress;
            //}                
            loadingBar.Visibility = ViewStates.Visible;
            RepeatAnimation();
        }

        private void RepeatAnimation()
        {
            avdProgress.Start();
            loadingBar.PostDelayed(action, 300);
        }

        public void HideProcessing()
        {
            loadingBar.Visibility = ViewStates.Gone;
            loadingBar.RemoveCallbacks(action);
            avdProgress.Stop();
        }

        public void StyleView()
        {
            //bottomNavigationView.ItemHeight = 102;
            bottomNavigationView.EnableShiftingMode(false);
            bottomNavigationView.EnableItemShiftingMode(true);
            bottomNavigationView.SetTextSize(9);
        }

        public void HideToolbar()
        {
            if (toolbar.Visibility == ViewStates.Visible)
                toolbar.Visibility = ViewStates.Gone;
        }

        public void ShowToolbar()
        {
            if (toolbar.Visibility == ViewStates.Gone)
                toolbar.Visibility = ViewStates.Visible;
        }
        #endregion
    }
}