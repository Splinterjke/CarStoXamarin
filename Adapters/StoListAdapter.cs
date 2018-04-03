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
using Android.Graphics;

namespace CarSto.Adapters
{
    public class StoListAdapter : ArrayAdapter<Dictionary<string,string>>
    {
        private Context context;
        private Dictionary<string, string> values;

        public StoListAdapter(Context context, int textViewResourceId, Dictionary<string, string> objects) : base(context, textViewResourceId)
        {
            this.context = context;
            this.values = objects;
        }

        public override int Count => values.Count;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            TextView label = new TextView(context);
            label.SetPadding(10, 15, 10, 15);
            label.SetTextColor(Color.Black);
            label.SetText(values.ElementAt(position).Value, TextView.BufferType.Normal);
            label.TextSize = 16;
            label.Tag = values.ElementAt(position).Key;
            
            return label;
        }
    }
}