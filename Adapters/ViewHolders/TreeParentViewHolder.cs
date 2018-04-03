using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarSto;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using XamDroid.ExpandableRecyclerView;

namespace CarSto.Adapters.ViewHolders
{
    public class TreeParentViewHolder : ParentViewHolder
    {
        public TextView textView;
        public TreeParentViewHolder(View itemView) : base(itemView)
        {
            textView = itemView.FindViewById<TextView>(CarSto.Resource.Id.tree_title);
        }
    }
}