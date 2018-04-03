using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CarSto.Adapters
{
    public class ScheduleAdapter : ArrayAdapter
    {
        private Context context;
        private int lineCount;

        public ScheduleAdapter(Context context, int resource, int lineCount) : base(context, resource)
        {
            this.context = context;
            this.lineCount = lineCount;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.view_schedulebackground, parent, false);
            return view;
        }
    }
}