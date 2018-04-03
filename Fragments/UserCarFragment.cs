// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using CarSto.Adapters;
using CarSto.Models;
using Android.Graphics;
using Android.Support.V4.Graphics.Drawable;
using XamDroid.ExpandableRecyclerView;
using CarSto.Services;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using System.Text.RegularExpressions;
using CarSto.CustomControls;
using CarSto.Presenters;
using static CarSto.Services.ViewInjectorHelper;
using CarSto.Presenters.Garage;

namespace CarSto.Fragments
{
    public class UserCarFragment : Fragment, IStyleView
    {
        public GaragePresenter presenter;

        #region Widgets
        [InjectView(Resource.Id.userCarInfoViewPager)]
        private ViewPager userCarInfoViewPager;        

        [InjectView(Resource.Id.garage_usercar_image)]
        private ImageView carImage;

        [InjectView(Resource.Id.garage_ctl)]
        private CollapsingToolbarLayout garageCollapsingToolbar;

        [InjectView(Resource.Id.garage_usercar_short_info)]
        private TextView carShortInfo;

        [InjectView(Resource.Id.garage_tablayout)]
        private TabLayout garageTabLayout;

        int carIndex;
        #endregion

        public UserCarFragment(int carIndex)
        {
            this.carIndex = carIndex;
        }

        #region Base methods
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_usercar, container, false);
            presenter = (this.ParentFragment as GarageFragment).presenter;
            ViewInjector.Inject(this, view);
            StyleView();
            garageCollapsingToolbar.Title = $"{presenter.userCars[carIndex]["Make"]} {presenter.userCars[carIndex]["Model"].ToString().ToUpper()}";
            var userCarTabLayoutAdapter = new TabsAdapter(this.ChildFragmentManager, new List<Fragment> { new UserCarStatisticFragment(), new UserCarInfoFragment(carIndex) }, new List<Java.Lang.String> { new Java.Lang.String("—“¿“»—“» ¿"), new Java.Lang.String("»Õ‘Œ") });
            userCarInfoViewPager.Adapter = userCarTabLayoutAdapter;
            garageTabLayout.SetupWithViewPager(userCarInfoViewPager);
            return view;
        }
        #endregion

        #region Methods
        public void StyleView()
        {
            
        }
        #endregion
    }
}