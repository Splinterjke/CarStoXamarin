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
using Android.Graphics.Drawables;
using Android.Text;
using Android.Util;
using Android.Support.V4.Content;

namespace CarSto.CustomControls
{
    public class TreeSearchEditText : EditText
    {
        private Drawable findBtn;
        public event EventHandler SearchClick;

        public TreeSearchEditText(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        private void Init()
        {
            findBtn = ContextCompat.GetDrawable(this.Context, Resource.Drawable.ic_search);
            findBtn.SetBounds(0, 0, findBtn.IntrinsicWidth, findBtn.IntrinsicHeight);
            this.SetSingleLine(true);
            this.CompoundDrawablePadding = 20;
            this.SetPadding(this.PaddingStart, this.PaddingTop, 20, this.PaddingBottom);
            this.SetCompoundDrawables(this.GetCompoundDrawables()[0], this.GetCompoundDrawables()[1], findBtn, this.GetCompoundDrawables()[3]);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            //if (this.GetCompoundDrawables()[2] == null)
            //{
            //    if (!HasFocus)
            //        RequestFocus();
            //    return base.OnTouchEvent(e);
            //}

            //if (e.Action != MotionEventActions.Up)
            //{
            //    if (!HasFocus)
            //        RequestFocus();
            //    return base.OnTouchEvent(e);
            //}

            if (e.GetX() > this.Width - this.PaddingRight - findBtn.IntrinsicWidth)
            {
                SearchClick?.Invoke(this, new EventArgs());
            }
            return base.OnTouchEvent(e);
        }      
    }
}