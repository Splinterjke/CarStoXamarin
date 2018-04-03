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

namespace CarSto.Adapters.ViewHolders
{
    public class TreeChildViewHolder : ChildViewHolder
    {
        public TextView textView;
        public Dictionary<string, object> data;

        public TreeChildViewHolder(View itemView, Action<Dictionary<string,object>> listener) : base(itemView)
        {
            textView = itemView.FindViewById<TextView>(Resource.Id.subtree_title);
            itemView.Click += (s, e) => { listener(data); };
        }
    }
}