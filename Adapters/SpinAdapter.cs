// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Content;
using Android.App;
using System.Collections.Generic;

namespace CarSto.Adapters
{
    public class SpinAdapter : BaseAdapter<string>
    {
        private Activity context;
        private List<string> values;
        public List<string> ssd;

        public SpinAdapter(Activity context, List<string> values, List<string> ssd = null) : base()
        {
            this.context = context;
            this.values = values;
            this.ssd = ssd;
        }

        public override int Count => values.Count;

        public override string this[int position] => values[position];

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            var textView = view.FindViewById<TextView>(Android.Resource.Id.Text1);
            textView.Text = values[position];
            textView.SetPadding(10, 5, 10, 5);
            textView.TextSize = 18;
            view.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            return view;
        }

        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
            if (position == 0)
            {
                view.SetOnClickListener(null);
            }
            var textView = view.FindViewById<TextView>(Android.Resource.Id.Text1);
            textView.Text = values[position];
            textView.SetPadding(10, 5, 10, 5);
            textView.TextSize = 18;
            view.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
            return view;
        }

        public override long GetItemId(int position)
        {
            return position;
        }
    }
}