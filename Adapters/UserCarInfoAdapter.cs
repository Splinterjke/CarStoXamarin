// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
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
using CarSto.Adapters.ViewHolders;
using CarSto.Models;
using XamDroid.ExpandableRecyclerView;
using Android.Support.V7.Widget;
using static CarSto.Services.ViewInjectorHelper;

namespace CarSto.Adapters
{
	public class UserCarInfoAdapter : RecyclerView.Adapter
    {
        public Dictionary<string, string> Items;

        public UserCarInfoAdapter(Dictionary<string, string> items)
        {
            this.Items = items;
        }

        public override int ItemCount => Items != null ? Items.Count : 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var vh = holder as UserCarInfoViewHolder;
            vh.usercarTitle.Text = Items.ElementAt(position).Key;
            vh.usercarDescription.Text = Items.ElementAt(position).Value;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.view_usercarinfo, parent, false);
            UserCarInfoViewHolder vh = new UserCarInfoViewHolder(view);
            return vh;
        }

        public class UserCarInfoViewHolder : RecyclerView.ViewHolder
        {
            [InjectView(Resource.Id.view_usercarinfo_title)]
            public TextView usercarTitle;

            [InjectView(Resource.Id.view_usercarinfo_discription)]
            public TextView usercarDescription;

            public UserCarInfoViewHolder(View itemView) : base(itemView)
            {
                ViewInjector.Inject(this, itemView);
            }
        }
    }
}