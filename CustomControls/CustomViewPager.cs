using Android.Content;
using Android.Views;
using Android.Support.V4.View;
using Android.Util;

namespace CarSto.CustomControls
{
    public class CustomViewPager : ViewPager
    {
        public bool PagingEnabled { get; set; }

        public CustomViewPager(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            this.PagingEnabled = true;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (this.PagingEnabled)
            {
                return base.OnTouchEvent(e);
            }
            return false;
        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            if (this.PagingEnabled)
            {
                return base.OnInterceptTouchEvent(ev);
            }
            return false;
        }
    }
}