// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using CarSto.Adapters;
using CarSto.Services;
using Newtonsoft.Json;

namespace CarSto.Fragments
{
    public class OrderChatFragment : Fragment
    {
        private PreOrderFragment parent;
        #region Widgets
        private EditText messageEdit;
        private ImageButton sendButton;
        private RecyclerView messageList;
        #endregion

        #region Base Methods
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_orderchat, container, false);
            GetElements(view);
            InitData();
            SetEventHandlers();
            return view;
        }

        private void SetEventHandlers()
        {
            sendButton.Click += async delegate
            {
                var response = await ClientAPI.PutAsync($"Order/{(this.ParentFragment as PreOrderFragment).orderData["Id"]}/Comment", messageEdit.Text);
                if (response == null)
                    return;
                messageEdit.Text = string.Empty;
                var orderData = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Item2);
                var adapter = messageList.GetAdapter() as ChatAdapter;
                adapter.messagesList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(orderData["Comments"].ToString());
                adapter.NotifyItemRangeChanged(0, adapter.ItemCount);
                messageList.SmoothScrollToPosition(adapter.ItemCount);
            };
        }
        #endregion

        #region Methods
        private void GetElements(View view)
        {
            messageEdit = view.FindViewById<EditText>(Resource.Id.orderchat_messageEdit);
            sendButton = view.FindViewById<ImageButton>(Resource.Id.orderchat_send_btn);
            messageList = view.FindViewById<RecyclerView>(Resource.Id.orderchat_messages_container);
            messageList.SetLayoutManager(new LinearLayoutManager(view.Context));
        }

        private void InitData()
        {
            parent = ParentFragment as PreOrderFragment;
            var adapter = new ChatAdapter(parent.commentList);
            messageList.SetAdapter(adapter);
            messageList.SmoothScrollToPosition(adapter.ItemCount);
        }
        #endregion
    }
}