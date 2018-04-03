using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using CarSto.Services;
using Newtonsoft.Json;

namespace CarSto.Adapters
{
    public enum OrderItemType
    {
        Part = 0,
        Labor = 1,
        PhisicalPart = 2
    }

    public class PartLaborListAdapter : RecyclerView.Adapter
    {
        public List<Dictionary<string, object>> Items;
        
        public event EventHandler<int> DeleteClick;
        public event EventHandler<int> ChangeRealizClick;
        public event EventHandler<int> ItemClick;
        private bool readyOrder;
        private OrderItemType orderItemType;
        private int lastCheckedPosition;

        public PartLaborListAdapter(List<Dictionary<string, object>> items, OrderItemType orderItemType, bool readyOrder = false)
        {
            this.Items = items;
            this.readyOrder = readyOrder;
            this.orderItemType = orderItemType;
        }

        public override int ItemCount => Items != null ? Items.Count : 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            if(orderItemType == OrderItemType.PhisicalPart)
            {
                MakeOrderPartLaborViewHolder vh = holder as MakeOrderPartLaborViewHolder;
                vh.PartLaborDescription.Text = $"{Items[position]["OEM"]}\n{Items[position]["Count"]} шт.\n{Items[position]["Price"]} руб.";
                vh.PartLaborCheckBox.Checked = (position == lastCheckedPosition);
                return;
            }
            if (readyOrder)
            {
                OrderPartLaborViewHolder vh = holder as OrderPartLaborViewHolder;
                vh.PartLaborDescription.Text = orderItemType == OrderItemType.Part ? $"OEM: {Items[position]["OEM"]}\nВремя доставки: {Items[position]["DeliveryTime"]}\nСтоимость: {Items[position]["Price"]}" : $"Название: {Items[position]["Text"]}\nВремя: {Items[position]["Time"]}\nДействие: {Items[position]["Action"]}";
                vh.PartLaborChangeRealizeBtn.Visibility = orderItemType == OrderItemType.Part ? ViewStates.Visible : ViewStates.Gone;
            }
            else
            {
                MakeOrderPartLaborViewHolder vh = holder as MakeOrderPartLaborViewHolder;
                vh.PartLaborCheckBox.Checked = (orderItemType == OrderItemType.Part && DataPreferences.Instance.selectedParts != null && DataPreferences.Instance.selectedParts.Contains(Items[position]["Id"])) || (orderItemType == OrderItemType.Labor && DataPreferences.Instance.selectedLabors != null && DataPreferences.Instance.selectedLabors.Contains(Items[position]["Id"]));
                var isLaborsExist = Items[position].ContainsKey("LaborIds") && Items[position]["LaborIds"] != null && JsonConvert.DeserializeObject<int[]>(Items[position]["LaborIds"].ToString()).Length > 0 ? "ЕСТЬ" : "НЕТ";
                vh.PartLaborDescription.Text = orderItemType == OrderItemType.Part ? $"Работы: {isLaborsExist}\nНазвание: {Items[position]["Text"]}\nOEM: {Items[position]["OEM"]}\nКол.-во: {(Items[position] as Dictionary<string, object>)["RequiredCount"]}" : $"Название: {Items[position]["Text"]}\nВремя: {Items[position]["Time"]}\nNote: {Items[position]["Note"]}";
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {            
            if (readyOrder)
            {
                var orderPartLaborView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.view_orderpartlabor, parent, false);
                var orderPartLaborViewHolder = new OrderPartLaborViewHolder(orderPartLaborView, OnDelete, OnChange);
                return orderPartLaborViewHolder;
            }
            View makeOrderPartLaborView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.view_orderpartlabor, parent, false);
            MakeOrderPartLaborViewHolder makeOrderPartLaborViewHolder = new MakeOrderPartLaborViewHolder(makeOrderPartLaborView, OnClick);
            return makeOrderPartLaborViewHolder;
        }

        private void OnClick(int position)
        {
            lastCheckedPosition = position;
            ItemClick?.Invoke(this, position);
        }

        private void OnDelete(int position)
        {
            DeleteClick?.Invoke(this, position);
        }

        private void OnChange(int position)
        {
            ChangeRealizClick?.Invoke(this, position);
        }

        public class MakeOrderPartLaborViewHolder : RecyclerView.ViewHolder
        {
            public TextView PartLaborDescription { get; private set; }
            public CheckBox PartLaborCheckBox { get; private set; }

            public MakeOrderPartLaborViewHolder(View itemView, Action<int> listener) : base(itemView)
            {
                //PartLaborDescription = itemView.FindViewById<TextView>(Resource.Id.makeorder_partlabor_description);
                //PartLaborCheckBox = itemView.FindViewById<CheckBox>(Resource.Id.makeorder_partlabor_checkbox);
                //PartLaborCheckBox.Click += (s, e) =>
                //{
                //    listener(AdapterPosition);
                //};
                //itemView.Click += (s, e) =>
                //{
                //    PartLaborCheckBox.Checked = !PartLaborCheckBox.Checked;
                //    listener(AdapterPosition);
                //};
            }
        }

        public class OrderPartLaborViewHolder : RecyclerView.ViewHolder
        {
            public TextView PartLaborDescription;
            public ImageButton PartLaborDeleteBtn, PartLaborChangeRealizeBtn;

            public OrderPartLaborViewHolder(View itemView, Action<int> deleteListener, Action<int> changeRealizeListener) : base(itemView)
            {
                PartLaborDescription = itemView.FindViewById<TextView>(Resource.Id.order_partlabor_description);
                PartLaborDeleteBtn = itemView.FindViewById<ImageButton>(Resource.Id.order_partlabor_delete_btn);
                PartLaborDeleteBtn.Click += delegate
                {
                    deleteListener(AdapterPosition);
                };
                PartLaborChangeRealizeBtn = itemView.FindViewById<ImageButton>(Resource.Id.order_partlabor_change_realisation_btn);
                PartLaborChangeRealizeBtn.Click += delegate
                {
                    changeRealizeListener(AdapterPosition);
                };
            }
        }
    }
}