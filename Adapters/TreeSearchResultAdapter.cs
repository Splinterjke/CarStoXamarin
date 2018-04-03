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
using Android.Support.V7.Widget;

namespace CarSto.Adapters
{
    public class TreeSearchResultAdapter : RecyclerView.Adapter
    {
        public List<Dictionary<string, object>> Items;
        public event EventHandler<int> ItemClick;

        public TreeSearchResultAdapter(List<Dictionary<string, object>> items)
        {
            this.Items = items;
        }

        public override int ItemCount => Items != null ? Items.Count : 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            TreeSearchResultViewHolder vh = holder as TreeSearchResultViewHolder;
            vh.PartDescription.Text = $"{Items[position]["Text"]}\nOEM: {Items[position]["Oem"]}\nКол.-во: {Items[position]["Amount"]}";
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View treeSearchPartView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.view_partitem, parent, false);
            TreeSearchResultViewHolder treeSearchPartViewHolder = new TreeSearchResultViewHolder(treeSearchPartView, OnClick);
            return treeSearchPartViewHolder;
        }

        private void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }

        public class TreeSearchResultViewHolder : RecyclerView.ViewHolder
        {
            public TextView PartDescription;

            public TreeSearchResultViewHolder(View itemView, Action<int> listener) : base(itemView)
            {
                PartDescription = itemView.FindViewById<TextView>(Resource.Id.part_description);
                itemView.Click += (s, e) =>
                {
                    listener(AdapterPosition);
                };
            }
        }
    }
}