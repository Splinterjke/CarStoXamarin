using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using XamDroid.ExpandableRecyclerView;
using CarSto.Adapters.ViewHolders;
using Android.Support.V7.Widget;

namespace CarSto.Adapters
{
    public class PreOrderAdapter : RecyclerView.Adapter
    {
        public List<Dictionary<string, object>> items;
        public event EventHandler<int> DeleteItemClick;
        string listType;

        public PreOrderAdapter(List<Dictionary<string, object>> items, string listType)
        {
            this.items = items;
            this.listType = listType;
        }

        public override int ItemCount => items == null ? 0 : items.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            PreOrderViewHolder vh = holder as PreOrderViewHolder;
            if (listType == "Part")
                vh.ItemDescription.Text = $"{items[position]["Oem"]}\n{items[position]["Text"]}";
            else vh.ItemDescription.Text = $"{items[position]["Text"]}";
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View preOrderView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.view_preorderitem, parent, false);
            PreOrderViewHolder preOrderViewHolder = new PreOrderViewHolder(preOrderView, OnDeleteClick);
            return preOrderViewHolder;
        }

        private void OnDeleteClick(int position)
        {
            DeleteItemClick?.Invoke(this, position);
        }

        public class PreOrderViewHolder : RecyclerView.ViewHolder
        {
            public TextView ItemDescription;
            public ImageButton DeleteButton;

            public PreOrderViewHolder(View itemView, Action<int> listener) : base(itemView)
            {
                ItemDescription = itemView.FindViewById<TextView>(Resource.Id.preorderitem_description);
                DeleteButton = itemView.FindViewById<ImageButton>(Resource.Id.preorderitem_remove_button);
                DeleteButton.Click += (s, e) =>
                {
                    listener(AdapterPosition);
                };
            }
        }
    }
}