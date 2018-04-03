using System.Collections.Generic;

using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using XamDroid.ExpandableRecyclerView;
using CarSto.Adapters;
using static CarSto.Services.ViewInjectorHelper;
using CarSto.Presenters;
using System;
using Android.Support.V4.Content;
using CarSto.Services;
using Newtonsoft.Json;
using Com.Nex3z.Notificationbadge;

namespace CarSto.Fragments
{
    public class PreOrderListFragment : Fragment, IStyleView
    {
        [InjectView(Resource.Id.preorder_preOrderPartList)]
        public RecyclerView preOrderPartList;

        [InjectView(Resource.Id.preorder_preOrderLaborList)]
        public RecyclerView preOrderLaborList;

        [InjectView(Resource.Id.preorder_acceptButton)]
        private Button acceptButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_preorderlist, container, false);
            ViewInjector.Inject(this, view);
            StyleView();

            var partAdapter = new PreOrderAdapter(new List<Dictionary<string, object>>(), "Part");
            partAdapter.DeleteItemClick += (s, e) =>
            {
                partAdapter.items.RemoveAt(e);
                partAdapter.NotifyItemRemoved(e);
                ((this.ParentFragment as MechanicalCategoryFragment).notifBadge as NotificationBadge).SetNumber(preOrderPartList.GetAdapter().ItemCount + preOrderLaborList.GetAdapter().ItemCount);
            };
            preOrderPartList.SetAdapter(partAdapter);

            var laborAdapter = new PreOrderAdapter(new List<Dictionary<string, object>>(), "Labor");
            laborAdapter.DeleteItemClick += (s, e) =>
            {
                laborAdapter.items.RemoveAt(e);
                laborAdapter.NotifyItemRemoved(e);
                ((this.ParentFragment as MechanicalCategoryFragment).notifBadge as NotificationBadge).SetNumber(preOrderPartList.GetAdapter().ItemCount + preOrderLaborList.GetAdapter().ItemCount);
            };
            preOrderLaborList.SetAdapter(laborAdapter);

            return view;
        }

        [InjectOnClick(Resource.Id.preorder_acceptButton)]
        private async void AcceptButton_Click(object sender, EventArgs e)
        {
            var model = new Dictionary<string, object> { { "ucId", (ParentFragment as MechanicalCategoryFragment).carId }, { "reasonsAndComms", "Слесарные-работы" } };
            var response = await ClientAPI.PostAsync("Preorder/Create", model);
            if (response.Item1)
                return;
            var preOrderModel = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Item2);
            if (preOrderPartList.GetAdapter().ItemCount > 0)
            {
                var adapter = preOrderPartList.GetAdapter() as PreOrderAdapter;
                var partCount = adapter.ItemCount;
                for (int i = 0; i < partCount; i++)
                {
                    response = await ClientAPI.PostAsync($"PreOrder/{preOrderModel["Id"]}/Part?ucid={(ParentFragment as MechanicalCategoryFragment).carId}", adapter.items[i]);
                }
            }
            if (preOrderLaborList.GetAdapter().ItemCount > 0)
            {
                var adapter = preOrderLaborList.GetAdapter() as PreOrderAdapter;
                var laborCount = adapter.ItemCount;
                for (int i = 0; i < laborCount; i++)
                {
                    response = await ClientAPI.PostAsync($"PreOrder/{preOrderModel["Id"]}/Labor?ucid={(ParentFragment as MechanicalCategoryFragment).carId}", adapter.items[i]);
                }
            }

            ParentFragment.FragmentManager.PopBackStack();
        }

        public void StyleView()
        {
            preOrderPartList.SetLayoutManager(new LinearLayoutManager(this.Context));
            var dividerItemDecoration = new DividerItemDecoration(this.Context, DividerItemDecoration.Vertical);
            dividerItemDecoration.SetDrawable(ContextCompat.GetDrawable(Context, Resource.Drawable.line_devider));
            preOrderPartList.AddItemDecoration(dividerItemDecoration);

            preOrderLaborList.SetLayoutManager(new LinearLayoutManager(this.Context));
            preOrderLaborList.AddItemDecoration(dividerItemDecoration);
        }
    }
}