using Android.OS;
using Android.Views;
using Android.Support.V4.App;
using System;
using Android.Widget;
using Android.Support.V7.Widget;
using CarSto.Adapters;
using CarSto.Services;
using System.Linq;
using System.Collections.Generic;

namespace CarSto.Fragments
{
    public class PhisPartSelectionFragment : Fragment
    {
        private string orderId;
        private List<Dictionary<string, object>> phisPartList;
        private object tradeOfferId;
        private string partPositionId;
        #region Widgets
        private Button completeBtn;
        private RecyclerView partList;
        #endregion

        public PhisPartSelectionFragment(List<Dictionary<string, object>> phisPartList, string ppId, string orderId)
        {
            this.phisPartList = phisPartList;
            this.partPositionId = ppId;
            this.orderId = orderId;
        }

        #region Base Methods
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_phispartselection, container, false);
            GetElements(view);
            InitData();
            SetEventHandlers();
            return view;
        }
        #endregion

        #region Methdos
        private void SetEventHandlers()
        {
            if (partList.GetAdapter() is PartLaborListAdapter adapter)
                adapter.ItemClick += Adapter_ItemClick;
            completeBtn.Click += async delegate
            {
                var response = await ClientAPI.PutAsync($"Order/{orderId}/Part/{partPositionId}", tradeOfferId);
                if (response == null)
                    return;
                this.Activity.SupportFragmentManager.PopBackStack();
            };
        }

        private void Adapter_ItemClick(object sender, int e)
        {
            var adapter = partList.GetAdapter() as PartLaborListAdapter;
            tradeOfferId = adapter.Items[e]["Id"];
        }

        private void InitData()
        {
            if (phisPartList == null) return;
            var adapter = new PartLaborListAdapter(phisPartList, OrderItemType.PhisicalPart);
            partList.SetAdapter(adapter);
        }

        private void GetElements(View view)
        {
            partList = view.FindViewById<RecyclerView>(Resource.Id.phispartselection_part_list);
            partList.SetLayoutManager(new LinearLayoutManager(this.Context));
            completeBtn = view.FindViewById<Button>(Resource.Id.phispartselection_complete_button);
        }
        #endregion
    }
}