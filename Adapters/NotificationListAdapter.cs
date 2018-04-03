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
    public class NotificationListAdapter : RecyclerView.Adapter
    {
        public List<Dictionary<string, object>> notifications;
        public event EventHandler<int> ItemClick;
        public event EventHandler<int> DeleteItemClick;

        public NotificationListAdapter(List<Dictionary<string, object>> notifications)
        {
            this.notifications = notifications;
        }

        public override int ItemCount => (notifications != null) ? notifications.Count : 0;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var vh = holder as NotificationViewHolder;
            vh.notifDescription.Text = $"{notifications[position]["SenderName"]}\n{notifications[position]["Message"]}\n{((DateTime)notifications[position]["Time"]).ToShortTimeString()}";
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.view_notification, parent, false);
            NotificationViewHolder vh = new NotificationViewHolder(itemView, OnItemClick, OnDeleteClick);
            return vh;
        }

        private void OnItemClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }

        private void OnDeleteClick(int position)
        {
            DeleteItemClick?.Invoke(this, position);
        }

        public class NotificationViewHolder : RecyclerView.ViewHolder
        {
            public TextView notifDescription;
            public ImageButton deleteBtn;

            public NotificationViewHolder(View itemView, Action<int> itemListener, Action<int> deleteListener) : base(itemView)
            {
                notifDescription = itemView.FindViewById<TextView>(Resource.Id.notification_description);
                deleteBtn = itemView.FindViewById<ImageButton>(Resource.Id.notification_delete_button);
                itemView.Click += (s, e) => { itemListener(AdapterPosition); };
                deleteBtn.Click += (s, e) => { deleteListener(AdapterPosition); };
            }
        }
    }
}