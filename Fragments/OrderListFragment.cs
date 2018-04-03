// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using CarSto.Adapters;
using CarSto.CustomControls;
using CarSto.Services;
using Android.App;
using Newtonsoft.Json;
using static CarSto.Adapters.OrderListAdapter;
using System.Linq;
using System;

namespace CarSto.Fragments
{
    public class OrderListFragment : Android.Support.V4.App.Fragment
    {
        private OrdersCategoryFragment parent;
        private List<Dictionary<string, object>> orders;
        #region Widgets
        private RecyclerView orderList;
        #endregion

        #region Base Methods
        public OrderListFragment(List<Dictionary<string, object>> orders)
        {
            this.orders = orders;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_orderlist, container, false);
            GetElements(view);
            InitData();
            SetEventHandlers();
            return view;
        }

        private void SetEventHandlers()
        {
            if (orderList.GetAdapter() is OrderListAdapter adapter)
                adapter.ItemClick += Adapter_ItemClick;
        }
        #endregion

        #region Methods
        private void InitData()
        {
            if (orders == null) return;
            OrderListAdapter adapter = new OrderListAdapter(orders);
            orderList.SetAdapter(adapter);
        }

        private async void Adapter_ItemClick(object sender, int e)
        {
            //try
            //{
            //    var response = await ClientAPI.GetAsync($"PreOrder/{(orderList.GetAdapter() as OrderListAdapter).orders[e]["Id"]}");
            //    if (response.Item1)
            //        return;
            //    var orderViewModel = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Item2);
            //    this.Activity.SupportFragmentManager.BeginTransaction().AddToBackStack("PreOrderFragment").Replace(Resource.Id.detail_content, new PreOrderFragment(orderViewModel)).Commit();
            //}
            //catch (JsonSerializationException ex)
            //{
            //    AlertDialogs.SimpleAlertDialog(ex.ToString(), this.Context).Show();
            //    return;
            //}
        }

        private void GetElements(View view)
        {
            orderList = view.FindViewById<RecyclerView>(Resource.Id.orderlist_recycle_list);
            orderList.AddItemDecoration(new DividerItemDecoration(view.Context, DividerItemDecoration.Vertical));
            orderList.SetLayoutManager(new LinearLayoutManager(view.Context));
            //orderList.NestedScrollingEnabled = false;
        }
        #endregion
    }
}