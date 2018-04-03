using System;
using Android.OS;
using Android.Views;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using CarSto.Adapters;
using Android.Support.V4.Content;
using CarSto.Services;
using Android.Support.V4.App;
using Android.Support.V4.View;
using static CarSto.Services.ViewInjectorHelper;
using CarSto.Presenters.Notifications;
using Android.Widget;
using Com.Wang.Avi;
using Android.Support.Graphics.Drawable;
using CarSto.Activities;
using CarSto.Presenters;

namespace CarSto.Fragments
{
    public class NotificationsFragment : Fragment, INotifView, IStyleView
    {
        private List<Dictionary<string, object>> notifications;
        private NotifPresenter presenter;
        private NotificationListAdapter adapter;

        #region Widgets
        [InjectView(Resource.Id.notifications_list)]
        private RecyclerView notificationList;

        [InjectView(Resource.Id.notifications_errorText)]
        private TextView errorText;
        #endregion

        #region Base Methods
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_notifications, container, false);
            presenter = new NotifPresenter() { View = this };
            ViewInjector.Inject(this, view);
            StyleView();
            ToolbarHelper.SetToolbarStyle("Уведомления", false);
            (this.Activity as MenuActivity).ShowToolbar();
            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            presenter.LoadNotifications();
        }
        #endregion

        public void UpdateNotificationList()
        {
            errorText.Visibility = ViewStates.Gone;
            if (adapter == null)
            {
                adapter = new NotificationListAdapter(presenter.notifications);
                notificationList.SetAdapter(adapter);
                adapter.ItemClick += (s, e) =>
                {
                    presenter.OnNotifClicked(e);
                };
                adapter.DeleteItemClick += (s, e) =>
                {
                    adapter.NotifyItemRemoved(e);                    
                    presenter.OnNotifRemoved(e);                    
                    //adapter.NotifyItemRangeChanged(e, adapter.ItemCount);
                };
                return;
            }
            adapter.notifications = presenter.notifications;
            adapter.NotifyItemRangeChanged(0, adapter.ItemCount);
        }

        public void ShowError(string errorMessage)
        {
            errorText.Visibility = ViewStates.Visible;
            errorText.Text = errorMessage;
        }

        public void StyleView()
        {
            notificationList.SetLayoutManager(new LinearLayoutManager(this.Context));
        }

        public void ShowProcessing()
        {
            (this.Activity as MenuActivity).ShowProcessing();
        }

        public void HideProcessing()
        {
            (this.Activity as MenuActivity).HideProcessing();
        }
    }
}