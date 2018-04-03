// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
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
    public class OrderLaborsFragment : Fragment
    {
        private PreOrderFragment parent;

        #region Widgets
        private RecyclerView orderLabors;
        private Button addLaborsBtn;
        #endregion

        #region Base Methods
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_orderlabors, container, false);
            GetElements(view);
            InitData();
            SetEventHandlers();
            return view;
        }
        #endregion

        #region Methods
        private void GetElements(View view)
        {
            orderLabors = view.FindViewById<RecyclerView>(Resource.Id.orderlabors_recycle_list);
            orderLabors.AddItemDecoration(new DividerItemDecoration(view.Context, DividerItemDecoration.Vertical));
            orderLabors.SetLayoutManager(new LinearLayoutManager(view.Context));
            addLaborsBtn = view.FindViewById<Button>(Resource.Id.orderlabors_addlabor_button);
        }

        private void InitData()
        {
            parent = ParentFragment as PreOrderFragment;
            var adapter = new PartLaborListAdapter(parent.laborList, OrderItemType.Labor, true);
            orderLabors.SetAdapter(adapter);
        }

        private void SetEventHandlers()
        {
            //addLaborsBtn.Click += delegate
            //{
            //    this.Activity.SupportFragmentManager.BeginTransaction().AddToBackStack(null).Replace(Resource.Id.detail_content, new MechanicalCategoryFragment(parent.userCarData["Id"].ToString())).Commit();
            //};
            //if (orderLabors.GetAdapter() is PartLaborListAdapter adapter)
            //{
            //    adapter.DeleteClick += async (s, e) =>
            //    {
            //        var response = await ClientAPI.DeleteAsync($"Order/{parent.orderId}/Labor/{adapter.Items[e]["Id"]}");
            //        if (response == null)
            //            return;
            //        adapter.Items.RemoveAt(e);
            //        adapter.NotifyItemRemoved(e);
            //        adapter.NotifyItemRangeChanged(e, adapter.ItemCount);
            //    };
            //    adapter.ChangeRealizClick += (s, e) =>
            //    {

            //    };
            //}
        }
        #endregion
    }
}