// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using CarSto.Adapters;
using CarSto.CustomControls;
using Android.Widget;
using Newtonsoft.Json;
using CarSto.Services;

namespace CarSto.Fragments
{
    public class OrderPartsFragment : Fragment
    {
        #region Variables
        private PreOrderFragment parent;
        public string orderId;
        #endregion

        #region Widgets
        private RecyclerView orderParts;
        private Button addPartsBtn;
        #endregion

        #region Base Methods
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_orderparts, container, false);
            GetElements(view);
            InitData();
            SetEventHandlers();
            return view;
        }
        #endregion

        #region Methods
        private void GetElements(View view)
        {
            orderParts = view.FindViewById<RecyclerView>(Resource.Id.orderparts_recycle_list);
            orderParts.AddItemDecoration(new DividerItemDecoration(view.Context, DividerItemDecoration.Vertical));
            orderParts.SetLayoutManager(new LinearLayoutManager(view.Context));
            //orderParts.NestedScrollingEnabled = false;
            addPartsBtn = view.FindViewById<Button>(Resource.Id.orderparts_addpart_button);
        }

        private void InitData()
        {
            parent = ParentFragment as PreOrderFragment;
            orderId = parent.orderId;
            var adapter = new PartLaborListAdapter(parent.partList, OrderItemType.Part, true);            
            orderParts.SetAdapter(adapter);
        }

        private void SetEventHandlers()
        {
            //addPartsBtn.Click += delegate
            //{
            //    this.Activity.SupportFragmentManager.BeginTransaction().AddToBackStack("MechanicalCategoryFragment").Replace(Resource.Id.detail_content, new MechanicalCategoryFragment(parent.userCarData["Id"].ToString(), true, orderId)).Commit();
            //};
            //if (orderParts.GetAdapter() is PartLaborListAdapter adapter)
            //{
            //    adapter.DeleteClick += async (s, e) =>
            //    {
            //        var response = await ClientAPI.DeleteAsync($"Order/{orderId}/Part/{adapter.Items[e]["Id"]}");
            //        if (response == null)
            //            return;
            //        adapter.Items.RemoveAt(e);
            //        adapter.NotifyItemRemoved(e);
            //        adapter.NotifyItemRangeChanged(e, adapter.ItemCount);
            //    };
            //    adapter.ChangeRealizClick += async (s, e) =>
            //    {
            //        var response = await ClientAPI.GetAsync($"Trader/Offers/Download/{adapter.Items[e]["OEM"]}?format=rub&analog=1");
            //        if (response == null)
            //            return;
            //        var phisicalPartList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(response.Item2);
            //        if (phisicalPartList == null)
            //            return;
            //        this.Activity.SupportFragmentManager.BeginTransaction().AddToBackStack(null).Replace(Resource.Id.detail_content, new PhisPartSelectionFragment(phisicalPartList, adapter.Items[e]["Id"].ToString(), orderId)).Commit();
            //    };
            //}
        }
        #endregion
    }
}