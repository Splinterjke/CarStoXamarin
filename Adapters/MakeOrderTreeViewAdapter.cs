using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using CarSto.Adapters.ViewHolders;
using CarSto.Models;
using XamDroid.ExpandableRecyclerView;

namespace CarSto.Adapters
{
    public class MakeOrderTreeViewAdapter : ExpandableRecyclerAdapter<TreeParentViewHolder, TreeChildViewHolder>
    {
        LayoutInflater _inflater;
        public event EventHandler<Dictionary<string, object>> ItemClick;

        public MakeOrderTreeViewAdapter(Context context, List<IParentObject> itemList) : base(context, itemList)
        {
            _inflater = LayoutInflater.From(context);
        }

        public override void OnBindChildViewHolder(TreeChildViewHolder childViewHolder, int position, object childObject)
        {
            if (childViewHolder == null)
                return;
            childViewHolder.textView.Text = (childObject as Dictionary<string, object>)["Title"].ToString();
            childViewHolder.data = childObject as Dictionary<string, object>;
        }

        public override void OnBindParentViewHolder(TreeParentViewHolder parentViewHolder, int position, object parentObject)
        {
            if (parentViewHolder == null)
                return;
            parentViewHolder.textView.Text = $"> {(parentObject as TreeNodeViewModel).Title}";
        }

        public override TreeChildViewHolder OnCreateChildViewHolder(ViewGroup childViewGroup)
        {
            var view = _inflater.Inflate(Resource.Layout.view_treesubitem, childViewGroup, false);
            return new TreeChildViewHolder(view, OnClick);
        }

        public override TreeParentViewHolder OnCreateParentViewHolder(ViewGroup parentViewGroup)
        {
            var view = _inflater.Inflate(Resource.Layout.view_treeitem, parentViewGroup, false);
            return new TreeParentViewHolder(view);
        }

        private void OnClick(Dictionary<string, object> model)
        {
            ItemClick?.Invoke(this, model);
        }
    }
}