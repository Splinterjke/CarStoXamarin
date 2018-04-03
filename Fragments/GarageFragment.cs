// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;

using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Support.V4.View;
using CarSto.Adapters;
using CarSto.CustomControls;
using CarSto.Services;
using Newtonsoft.Json;
using static CarSto.Services.ViewInjectorHelper;
using CarSto.Presenters.Garage;
using CarSto.Activities;
using Android.Support.Design.Widget;
using Android.Widget;
using CarSto.Presenters;

namespace CarSto.Fragments
{
    public class GarageFragment : Fragment, IGarageView, IStyleView
    {
        public GaragePresenter presenter;
        private TabsAdapter userCarListAdapter;

        #region Widgets
        [InjectView(Resource.Id.garageViewPager)]
        public ViewPager garageViewPager;

        [InjectView(Resource.Id.indicator_default)]
        private CircleIndicator ci;

        [InjectView(Resource.Id.garage_fab)]
        private FloatingActionMenuButton fab;

        private List<Fragment> fragments;
        #endregion

        #region Base Methods
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_garage, null);
            presenter = new GaragePresenter() { View = this };
            ViewInjector.Inject(this, view);
            StyleView();
            ToolbarHelper.SetToolbarStyle("Гараж", false);
            (this.Activity as MenuActivity).HideToolbar();            
            garageViewPager.PageSelected += GarageViewPager_PageSelected;
            return view;
        }

        private void GarageViewPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            if (e.Position == garageViewPager.Adapter.Count - 1)
                fab.Visibility = ViewStates.Gone;
            else fab.Visibility = ViewStates.Visible;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            presenter.LoadGarage();
        }

        #endregion

        #region Methods
        public void UpdateGarage()
        {
            if (fragments != null && fragments.Count > 0)
                fragments.Clear();
            else fragments = new List<Fragment>();
            for (int i = 0; i < presenter.userCars.Count; i++)
            {
                fragments.Add(new UserCarFragment(i));
            }
            fragments.Add(new AddCarFragment());
            if (fragments.Count == 1)
                fab.Visibility = ViewStates.Gone;
            else fab.Visibility = ViewStates.Visible;
            userCarListAdapter = new TabsAdapter(this.ChildFragmentManager, fragments);
            garageViewPager.Adapter = userCarListAdapter;
            garageViewPager.OffscreenPageLimit = garageViewPager.Adapter.Count; // garageViewPager.Adapter.Count;
            ci.SetViewPager(garageViewPager);            
            //GarageViewPager_PageSelected(null, new ViewPager.PageSelectedEventArgs(0));
        }

        public void ShowError(string errorMessage)
        {
            var toast = Toast.MakeText(this.Activity.ApplicationContext, errorMessage, ToastLength.Short);
            toast.Show();
        }

        public void ShowProcessing()
        {
            (this.Activity as MenuActivity).ShowProcessing();
        }

        public void HideProcessing()
        {
            (this.Activity as MenuActivity).HideProcessing();
        }

        public void StyleView()
        {
            fab.SetMainButton(Resource.Color.Accent, Resource.Drawable.ic_garage_menu, null, true);
            fab.SetAnimation(FloatingActionMenuButton.AnimationType.Explosion);
            fab.AddAction(Resource.Color.Accent, Resource.Drawable.ic_car_repair, () =>
            {
                var fragment = new MechanicalCategoryFragment(presenter.userCars[garageViewPager.CurrentItem]["Id"].ToString(), vehRecId: presenter.userCars[garageViewPager.CurrentItem]["VehicleRecordId"].ToString());
                //this.FragmentManager.BeginTransaction().AddToBackStack(null).Replace(Resource.Id.detail_content, fragment).Commit();

                this.FragmentManager.BeginTransaction().AddToBackStack(null).Add(Resource.Id.detail_content, fragment).Commit();
                //this.Activity.SupportFragmentManager.BeginTransaction().AddToBackStack(null).Add(fragment, "MechanicalFragment").Commit();
            });
            fab.AddAction(Resource.Color.Accent, Resource.Drawable.ic_fuel, () =>
            {
                Toast.MakeText(this.Context, "2nd option is clicked", ToastLength.Short).Show();
            });
            fab.AddAction(Resource.Color.Accent, Resource.Drawable.ic_messaging, () =>
            {
                Toast.MakeText(this.Context, "3rd option is clicked", ToastLength.Short).Show();
            });
        }
        #endregion
    }
}