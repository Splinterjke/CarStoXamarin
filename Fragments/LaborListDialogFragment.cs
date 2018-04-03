using System;
using System.Collections.Generic;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using CarSto.Activities;
using CarSto.Adapters;
using CarSto.Services;
using Newtonsoft.Json;
using Android.Support.V7.App;
using Android.Support.V4.Content;
using Com.Nex3z.Notificationbadge;
using Android.Graphics.Drawables;
using System.Drawing;

namespace CarSto.Fragments
{
    public class LaborListDialogFragment : AppCompatDialogFragment
    {
        RecyclerView laborList;
        Button acceptDialogButton;
        string partOem;
        string title;

        public LaborListDialogFragment(string partOem, string title)
        {
            this.partOem = partOem;
            this.title = title;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_labor, container, false);
            var title = view.FindViewById<TextView>(Resource.Id.labordialog_laborTitle);
            title.Text = this.title;
            laborList = view.FindViewById<RecyclerView>(Resource.Id.labordialog_laborList);
            laborList.SetLayoutManager(new LinearLayoutManager(this.Context));
            var dividerItemDecoration = new DividerItemDecoration(laborList.Context, DividerItemDecoration.Vertical);
            dividerItemDecoration.SetDrawable(ContextCompat.GetDrawable(this.Context, Resource.Drawable.line_devider));
            laborList.AddItemDecoration(dividerItemDecoration);
            acceptDialogButton = view.FindViewById<Button>(Resource.Id.labordialog_acceptDialogButton);
            acceptDialogButton.Click += AcceptDialogButton_Click;
            this.Dialog.Window.SetDimAmount(0);
            return view;
        }

        private void AcceptDialogButton_Click(object sender, EventArgs e)
        {
            this.Dismiss();
        }

        public override async void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            var response = await ClientAPI.PutAsync($"Vehicle/{(this.ParentFragment as MechanicalCategoryFragment).vehRecId}/Labors?oem={partOem}", null);
            if (response.Item1)
                return;
            var laborList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(response.Item2);
            var adapter = new LaborListAdapter(laborList);
            adapter.ItemClick += Adapter_ItemClick;
            this.laborList.SetAdapter(adapter);
            this.laborList.GetAdapter().NotifyItemRangeChanged(0, laborList.Count);
        }

        private void Adapter_ItemClick(object sender, int e)
        {
            var adapter = laborList.GetAdapter() as LaborListAdapter;
            var preOrderFragment = ((this.ParentFragment as MechanicalCategoryFragment).viewPager.Adapter as TabsAdapter).GetItem(2) as PreOrderListFragment;
            var preOrderLaborAdapter = preOrderFragment.preOrderLaborList.GetAdapter() as PreOrderAdapter;
            preOrderLaborAdapter.items.Add(adapter.Items[e]);
            preOrderLaborAdapter.NotifyItemRangeChanged(0, preOrderLaborAdapter.ItemCount);
            var preOrderPartAdapter = preOrderFragment.preOrderPartList.GetAdapter() as PreOrderAdapter;
            ((this.ParentFragment as MechanicalCategoryFragment).notifBadge as NotificationBadge).SetNumber(preOrderPartAdapter.ItemCount + preOrderLaborAdapter.ItemCount);
        }
    }
}