// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Collections.Generic;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using CarSto.Adapters;
using CarSto.CustomControls;
using CarSto.Services;
using Newtonsoft.Json;
using System;

namespace CarSto.Fragments
{
	public class PreOrderFragment : Fragment
	{
        public string orderId;
        public Dictionary<string, object> orderData;
        public List<Dictionary<string, object>> partList, laborList, commentList;

        #region Widgets
        private ViewPager orderViewPager;
        private CircleIndicator ci;
        public List<Fragment> fragments;
        #endregion

        public PreOrderFragment(Dictionary<string, object> orderData)
        {
            this.orderId = orderData["Id"].ToString();
            this.orderData = orderData;            
        }

        #region Base Methods
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			var view = inflater.Inflate(Resource.Layout.fragment_order, container, false);
            ToolbarHelper.SetToolbarStyle($"Предзказ №{orderId}", false);
            GetElements(view);
            GetSubModels();
            InitData();
            return view;
		}
		#endregion

		#region Methods
		private void GetElements(View view)
		{
			orderViewPager = view.FindViewById<ViewPager>(Resource.Id.orderViewPager);
            ci = view.FindViewById<CircleIndicator>(Resource.Id.indicator_default);            
		}

        private void InitData()
        {   
            fragments = new List<Fragment> { new OrderInfoFragment("Preorder"), new OrderPartsFragment(), new OrderLaborsFragment(), new OrderChatFragment() };
            TabsAdapter adapter = new TabsAdapter(this.ChildFragmentManager, fragments);
            orderViewPager.Adapter = adapter;
            ci.SetViewPager(orderViewPager);
        }

        private void GetSubModels()
        {
            var userCarModel = JsonConvert.DeserializeObject<Dictionary<string, object>>(orderData["UserCarModel"].ToString());
            partList = orderData["PartList"] != null ? JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(orderData["listParts"].ToString()) : null;
            laborList = orderData["LaborList"] != null ? JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(orderData["listLabors"].ToString()) : null;            
        }

        private void SetEventHandlers()
        {
           
        }

        public void OrderDataUpdated(string updatedModel)
        {
            orderData = JsonConvert.DeserializeObject<Dictionary<string, object>>(updatedModel);
            GetSubModels();
            orderViewPager.Adapter.NotifyDataSetChanged();
        }
		#endregion
	}
}