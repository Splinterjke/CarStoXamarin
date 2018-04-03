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
using Android.Support.V4.View;
using Android.Support.Design.Widget;
using CarSto.Adapters;
using Android.Animation;
using CarSto.CustomControls;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CarSto.Services;
using CarSto.Activities;

namespace CarSto.Fragments
{
    public class OrdersCategoryFragment : Android.Support.V4.App.Fragment
    {
        #region Widgets
        private ViewPager ordersViewPager;
        private List<Fragment> fragments;
        private CircleIndicator ci;
        private TabsAdapter adapter;
        #endregion

        #region Base Methods
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_orderscategory, container, false);
            ToolbarHelper.SetToolbarStyle($"Заказ-наряды", false);
            (this.Activity as MenuActivity).ShowToolbar();
            GetElements(view);
            InitData();
            return view;
        }
        #endregion

        #region Methods
        private void GetElements(View view)
        {
            ordersViewPager = view.FindViewById<ViewPager>(Resource.Id.ordersViewPager);
            ci = view.FindViewById<CircleIndicator>(Resource.Id.indicator_default);
            ordersViewPager.PageSelected += (s, e) =>
            {
                switch (e.Position)
                {
                    case 0:
                        ToolbarHelper.SetToolbarStyle($"Активные заказы", false);
                        break;
                    case 1:
                        ToolbarHelper.SetToolbarStyle($"Предзаказы", false);
                        break;
                    case 2:
                        ToolbarHelper.SetToolbarStyle($"Черновики", false);
                        break;
                }
            };
        }

        private async void InitData()
        {
            try
            {

                //if (fragments != null && adapter != null)
                //{                    
                //    ordersViewPager.Adapter = adapter;
                //    ci.SetViewPager(ordersViewPager);
                //    ordersViewPager.Adapter.NotifyDataSetChanged();
                //    return;
                //}
                var orders = new List<string>();
                fragments = new List<Fragment>();

                //var taskList = new Task<string>[] { ClientAPI.GetAsync("Order"), ClientAPI.GetAsync("Order/Recommendation"), ClientAPI.GetAsync("Order/Raw"), ClientAPI.GetAsync("Order/Archive"), };
                //var result = await Task.WhenAll(taskList);

                var orderList = await ClientAPI.GetAsync("Order");
                orders.Add(orderList.Item2);
                orderList = await ClientAPI.GetAsync("PreOrder/All");
                orders.Add(orderList.Item2);
                orderList = await ClientAPI.GetAsync("Order/Raw");
                orders.Add(orderList.Item2);

                //var taskList = new List<Task<string>>(4) { ClientAPI.GetAsync("Order"), ClientAPI.GetAsync("Order/Recommendation"), ClientAPI.GetAsync("Order/Raw"), ClientAPI.GetAsync("Order/Archive") };
                //var response = await Task.WhenAll(taskList);

                for (int i = 0; i < orders.Count; i++)
                {
                    if (orders[i] == null)
                        continue;
                    var orderData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(orders[i]);
                    var orderListFragment = new OrderListFragment(orderData);
                    fragments.Add(orderListFragment);
                }
                adapter = new TabsAdapter(this.FragmentManager, fragments);
                ordersViewPager.Adapter = adapter;
                ci.SetViewPager(ordersViewPager);
            }
            catch (JsonSerializationException ex)
            {
                AlertDialogs.SimpleAlertDialog(ex.ToString(), this.Context).Show();
                return;
            }
        }
        #endregion
    }
}