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
using CarSto.Services;

namespace CarSto.Adapters
{
    public class LaborListAdapter : RecyclerView.Adapter
    {
        public List<Dictionary<string, object>> Items;

        public event EventHandler<int> ItemClick;

        public LaborListAdapter(List<Dictionary<string, object>> items)
        {
            this.Items = items;
        }

        public override int ItemCount => Items != null ? Items.Count : 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            LaborViewHolder vh = holder as LaborViewHolder;
            vh.LaborCheckBox.Checked = DataPreferences.Instance.selectedLabors != null && DataPreferences.Instance.selectedLabors.Contains(Items[position]["Id"].ToString());
            vh.LaborDescription.Text = $"{Items[position]["Text"]}\nДействие: {Items[position]["Note"]}\nВремя: {Items[position]["Time"]} ч.";
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View makeLaborView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.view_laboritem, parent, false);
            LaborViewHolder makeOrderLaborViewHolder = new LaborViewHolder(makeLaborView, OnClick);
            return makeOrderLaborViewHolder;
        }

        private void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }

        public class LaborViewHolder : RecyclerView.ViewHolder
        {
            public TextView LaborDescription;
            public AppCompatCheckBox LaborCheckBox;

            public LaborViewHolder(View itemView, Action<int> listener) : base(itemView)
            {
                LaborDescription = itemView.FindViewById<TextView>(Resource.Id.labor_description);
                LaborCheckBox = itemView.FindViewById<AppCompatCheckBox>(Resource.Id.labor_checkbox);

                LaborCheckBox.Click += (s, e) =>
                {
                    listener(AdapterPosition);
                };

                itemView.Click += (s, e) =>
                {
                    LaborCheckBox.Checked = !LaborCheckBox.Checked;
                    listener(AdapterPosition);
                };
            }
        }
    }
}