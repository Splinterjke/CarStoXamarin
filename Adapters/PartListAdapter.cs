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
using Newtonsoft.Json;
using CarSto.Services;

namespace CarSto.Adapters
{
    public class PartListAdapter : RecyclerView.Adapter
    {
        public List<Dictionary<string, object>> Items;

        public event EventHandler<int> ItemClick;
        public event EventHandler<int> LaborClick;

        public PartListAdapter(List<Dictionary<string, object>> items)
        {
            this.Items = items;
        }

        public override int ItemCount => Items != null ? Items.Count : 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MakeOrderPartLaborViewHolder vh = holder as MakeOrderPartLaborViewHolder;
            vh.PartLaborCheckBox.Checked = (DataPreferences.Instance.selectedParts != null && DataPreferences.Instance.selectedParts.Contains(Items[position]["Id"].ToString()));
            vh.LaborButton.Visibility = Items[position].ContainsKey("HasLabors") && Items[position]["HasLabors"] != null && (bool)Items[position]["HasLabors"] ? ViewStates.Visible : ViewStates.Gone;
            vh.PartLaborDescription.Text = $"{Items[position]["Text"]}\nOEM: {Items[position]["Oem"]}\nКол.-во: {Items[position]["Amount"]}";
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View makeOrderPartLaborView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.view_partitem, parent, false);
            MakeOrderPartLaborViewHolder makeOrderPartLaborViewHolder = new MakeOrderPartLaborViewHolder(makeOrderPartLaborView, OnClick, OnLaborClick);
            return makeOrderPartLaborViewHolder;
        }

        private void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }

        private void OnLaborClick(int position)
        {
            LaborClick?.Invoke(this, position);
        }

        public class MakeOrderPartLaborViewHolder : RecyclerView.ViewHolder
        {
            public TextView PartLaborDescription;
            public AppCompatCheckBox PartLaborCheckBox;
            public ImageButton LaborButton;

            public MakeOrderPartLaborViewHolder(View itemView, Action<int> listener, Action<int> laborListener) : base(itemView)
            {
                PartLaborDescription = itemView.FindViewById<TextView>(Resource.Id.part_description);
                PartLaborCheckBox = itemView.FindViewById<AppCompatCheckBox>(Resource.Id.part_checkbox);
                LaborButton = itemView.FindViewById<ImageButton>(Resource.Id.part_laborbutton);

                PartLaborCheckBox.Click += (s, e) =>
                {
                    listener(AdapterPosition);
                };

                itemView.Click += (s, e) =>
                {
                    PartLaborCheckBox.Checked = !PartLaborCheckBox.Checked;
                    listener(AdapterPosition);
                };

                LaborButton.Click += (s, e) =>
                {
                    laborListener(AdapterPosition);
                };
            }
        }
    }
}