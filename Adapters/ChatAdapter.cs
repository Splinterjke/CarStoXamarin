// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Content.Res;
using Android.Support.V4.Graphics.Drawable;

namespace CarSto.Adapters
{
    public class ChatAdapter : RecyclerView.Adapter
    {
        public List<Dictionary<string, object>> messagesList;
        public ChatAdapter(List<Dictionary<string, object>> messagesList)
        {
            this.messagesList = messagesList;
        }

        public override int ItemCount => messagesList.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ChatViewHolder vh = holder as ChatViewHolder;
            vh.messageText.Text = messagesList[position]["Comment"].ToString();
            vh.messageTime.Text = $"{((DateTime)messagesList[position]["Time"]).ToShortDateString()} {((DateTime)messagesList[position]["Time"]).ToShortTimeString()}";
            var avaparam = vh.messageAva.LayoutParameters as RelativeLayout.LayoutParams;
            var bubleparam = vh.messageBubble.LayoutParameters as RelativeLayout.LayoutParams;
            var messageparam = vh.messageTime.LayoutParameters as RelativeLayout.LayoutParams;
            if ((string)messagesList[position]["Initiator"] == "Станция")
            {
                avaparam.AddRule(LayoutRules.AlignParentLeft, vh.messageLayout.Id);
                bubleparam.AddRule(LayoutRules.RightOf, vh.messageAva.Id);
                messageparam.AddRule(LayoutRules.RightOf, vh.messageAva.Id);
                vh.messageBubble.Background = ContextCompat.GetDrawable(vh.ItemView.Context, Resource.Drawable.IncomingMessage);
            }
            else
            {
                avaparam.AddRule(LayoutRules.AlignParentRight, vh.messageLayout.Id);
                bubleparam.AddRule(LayoutRules.LeftOf, vh.messageAva.Id);
                messageparam.AddRule(LayoutRules.LeftOf, vh.messageAva.Id);
                vh.messageBubble.Background = ContextCompat.GetDrawable(vh.ItemView.Context, Resource.Drawable.OutgoingMessage);
            }
            vh.messageAva.LayoutParameters = avaparam;
            vh.messageBubble.LayoutParameters = bubleparam;
            vh.messageTime.LayoutParameters = messageparam;
            //vh.messageAva.SetImageDrawable(roundDrawable);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.view_chatmessage, parent, false);
            ChatViewHolder vh = new ChatViewHolder(itemView);
            return vh;
        }

        public class ChatViewHolder : RecyclerView.ViewHolder
        {
            public RelativeLayout messageLayout;
            public FrameLayout messageBubble;
            public TextView messageText, messageTime;
            public ImageView messageAva;

            public ChatViewHolder(View itemView) : base(itemView)
            {
                messageLayout = itemView.FindViewById<RelativeLayout>(Resource.Id.view_chatmessage_layout);
                messageBubble = itemView.FindViewById<FrameLayout>(Resource.Id.view_chatmessage_bubble);
                messageText = itemView.FindViewById<TextView>(Resource.Id.view_chatmessage_text);
                messageTime = itemView.FindViewById<TextView>(Resource.Id.view_chatmessage_time);
                messageAva = itemView.FindViewById<ImageView>(Resource.Id.view_chatmessage_ava);
            }
        }
    }
}