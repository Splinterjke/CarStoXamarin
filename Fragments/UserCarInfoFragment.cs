using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Support.V7.Widget;
using static CarSto.Services.ViewInjectorHelper;
using CarSto.Presenters;
using System;
using Android.Support.V4.Content;
using CarSto.Adapters;
using System.Collections.Generic;
using CarSto.Presenters.Garage;

namespace CarSto.Fragments
{
    public class UserCarInfoFragment : Fragment, IStyleView
    {
        public UserCarInfoAdapter adapter;
        #region Widgets
        [InjectView(Resource.Id.usercarinfo_info_list)]
        private RecyclerView infoList;
        #endregion

        int carIndex;

        public UserCarInfoFragment(int carIndex)
        {
            this.carIndex = carIndex;
        }

        #region Base Methods
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_usercarinfo, container, false);
            ViewInjector.Inject(this, view);
            StyleView();
            var parent = (this.ParentFragment as UserCarFragment).ParentFragment as GarageFragment;
            var userCarModel = (parent.presenter as GaragePresenter).userCars[carIndex];
            var userCarInfo = new Dictionary<string, string> { { "ГОД", userCarModel["Year"].ToString() }, { "ДВИГАТЕЛЬ", userCarModel["Engine"].ToString() }, { "КПП", userCarModel["GearBox"].ToString() }, { "ЦВЕТ", "Серый" }, { "VIN", userCarModel["Vin"] == null ? "-" : userCarModel["Vin"].ToString() } };
            adapter = new UserCarInfoAdapter(userCarInfo);
            infoList.SetAdapter(adapter);
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            
        }
        #endregion

        #region Methods
        public void StyleView()
        {
            infoList.SetLayoutManager(new LinearLayoutManager(this.Context));
            var dividerItemDecoration = new DividerItemDecoration(infoList.Context, DividerItemDecoration.Vertical);
            dividerItemDecoration.SetDrawable(ContextCompat.GetDrawable(this.Context, Resource.Drawable.line_devider));
            infoList.AddItemDecoration(dividerItemDecoration);
        }
        #endregion
    }
}