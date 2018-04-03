// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace CarSto.Adapters
{
    public class OrderListAdapter : RecyclerView.Adapter
    {
        public List<Dictionary<string, object>> orders;
        public event EventHandler<int> ItemClick;

        public OrderListAdapter(List<Dictionary<string, object>> orders)
        {
            this.orders = orders;
        }

        public override int ItemCount => (orders != null) ? orders.Count : 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            OrderViewHolder vh = holder as OrderViewHolder;
            vh.orderId.Text = $"#{orders[position]["Id"]}";
            var parts = JsonConvert.DeserializeObject<List<object>>(orders[position]["listParts"].ToString());
            var labors = JsonConvert.DeserializeObject<List<object>>(orders[position]["listLabors"].ToString());
            vh.orderDescription.Text = $"Дата создания: {((DateTime)orders[position]["CreationTime"]).ToShortDateString()}\nДеталей: {parts.Count}\nРабот: {labors.Count}";
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.view_orderinfo, parent, false);
            OrderViewHolder vh = new OrderViewHolder(itemView, OnClick);
            return vh;
        }

        private void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }

        public class OrderViewHolder : RecyclerView.ViewHolder
        {
            public TextView orderId, orderDescription;

            public OrderViewHolder(View itemView, Action<int> listener) : base(itemView)
            {
                orderId = itemView.FindViewById<TextView>(Resource.Id.orderinfo_id);
                orderDescription = itemView.FindViewById<TextView>(Resource.Id.orderinfo_description);
                itemView.Click += (s, e) => { listener(AdapterPosition); };
            }
        }
    }
}