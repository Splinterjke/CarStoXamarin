// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Animation;
using Android.Annotation;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Database;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;

namespace CarSto.CustomControls
{
    public class CircleIndicator : LinearLayout, ViewPager.IOnPageChangeListener, ITimeInterpolator
    {
        private static int DEFAULT_INDICATOR_WIDTH = 12;
        private ViewPager mViewpager;
        private int mIndicatorMargin = -1;
        private int mIndicatorWidth = -1;
        private int mIndicatorHeight = -1;
        private int mAnimatorResId = Resource.Animation.scale_with_alpha;
        private int mAnimatorReverseResId = 0;
        private int mIndicatorBackgroundResId = Resource.Drawable.circle_indicator_selected;
        private int mIndicatorUnselectedBackgroundResId = Resource.Drawable.circle_indicator_unselected;
        private Animator mAnimatorOut;
        private Animator mAnimatorIn;
        private Animator mImmediateAnimatorOut;
        private Animator mImmediateAnimatorIn;
        View currentIndicator;

        private int mLastPosition = -1;

        public CircleIndicator(Context context) : base(context)
        {
            Init(context, null);
        }

        public CircleIndicator(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context, attrs);
        }

        public CircleIndicator(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(context, attrs);
        }

        [TargetApi(Value = 21)]
        public CircleIndicator(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Init(context, attrs);
        }

        private void Init(Context context, IAttributeSet attrs)
        {
            HandleTypedArray(context, attrs);
            CheckIndicatorConfig(context);
        }

        private void HandleTypedArray(Context context, IAttributeSet attrs)
        {
            if (attrs == null)
            {
                return;
            }

            TypedArray typedArray = context.ObtainStyledAttributes(attrs, Resource.Styleable.CircleIndicator);
            mIndicatorWidth = typedArray.GetDimensionPixelSize(Resource.Styleable.CircleIndicator_ci_width, -1);
            mIndicatorHeight = typedArray.GetDimensionPixelSize(Resource.Styleable.CircleIndicator_ci_height, -1);
            mIndicatorMargin = typedArray.GetDimensionPixelSize(Resource.Styleable.CircleIndicator_ci_margin, -1);

            mAnimatorResId = typedArray.GetResourceId(Resource.Styleable.CircleIndicator_ci_animator, Resource.Animation.scale_with_alpha);
            mAnimatorReverseResId = typedArray.GetResourceId(Resource.Styleable.CircleIndicator_ci_animator_reverse, 0);
            mIndicatorBackgroundResId = typedArray.GetResourceId(Resource.Styleable.CircleIndicator_ci_drawable, Resource.Drawable.circle_indicator_selected);
            mIndicatorUnselectedBackgroundResId = typedArray.GetResourceId(Resource.Styleable.CircleIndicator_ci_drawable_unselected, Resource.Drawable.circle_indicator_unselected);

            int orientation = typedArray.GetInt(Resource.Styleable.CircleIndicator_ci_orientation, -1);
            Orientation = (Android.Widget.Orientation)orientation == Android.Widget.Orientation.Vertical ? Android.Widget.Orientation.Vertical : Android.Widget.Orientation.Horizontal;

            int gravity = typedArray.GetInt(Resource.Styleable.CircleIndicator_ci_gravity, -1);
            SetGravity(gravity >= 0 ? (GravityFlags)gravity : GravityFlags.Center);

            typedArray.Recycle();
        }



        public void ConfigureIndicator(int indicatorWidth, int indicatorHeight, int indicatorMargin, int animatorId, int animatorReverseId, int indicatorBackgroundId, int indicatorUnselectedBackgroundId)
        {
            //configureIndicator(indicatorWidth, indicatorHeight, indicatorMargin, Resource.Animation.scale_with_alpha, 0, R.drawable.white_radius, R.drawable.white_radius);
            mIndicatorWidth = indicatorWidth;
            mIndicatorHeight = indicatorHeight;
            mIndicatorMargin = indicatorMargin;

            mAnimatorResId = animatorId;
            mAnimatorReverseResId = animatorReverseId;
            mIndicatorBackgroundResId = indicatorBackgroundId;
            mIndicatorUnselectedBackgroundResId = indicatorUnselectedBackgroundId;

            CheckIndicatorConfig(this.Context);
        }

        private void CheckIndicatorConfig(Context context)
        {

            mIndicatorWidth = (mIndicatorWidth < 0) ? Dip2px(DEFAULT_INDICATOR_WIDTH) : mIndicatorWidth;
            mIndicatorHeight = (mIndicatorHeight < 0) ? Dip2px(DEFAULT_INDICATOR_WIDTH) : mIndicatorHeight;
            mIndicatorMargin = (mIndicatorMargin < 0) ? Dip2px(DEFAULT_INDICATOR_WIDTH) : mIndicatorMargin;

            mAnimatorResId = (mAnimatorResId == 0) ? Resource.Animation.scale_with_alpha : mAnimatorResId;

            mAnimatorOut = CreateAnimatorOut(context);
            mImmediateAnimatorOut = CreateAnimatorOut(context);
            mImmediateAnimatorOut.SetDuration(0);

            mAnimatorIn = CreateAnimatorIn(context);
            mImmediateAnimatorIn = CreateAnimatorIn(context);
            mImmediateAnimatorIn.SetDuration(0);

            mIndicatorBackgroundResId = (mIndicatorBackgroundResId == 0) ? mIndicatorUnselectedBackgroundResId : mIndicatorBackgroundResId;
            mIndicatorUnselectedBackgroundResId = (mIndicatorUnselectedBackgroundResId == 0) ? mIndicatorBackgroundResId : mIndicatorUnselectedBackgroundResId;
        }

        private Animator CreateAnimatorOut(Context context)
        {
            return AnimatorInflater.LoadAnimator(context, mAnimatorResId);
        }

        private Animator CreateAnimatorIn(Context context)
        {
            Animator animatorIn;
            if (mAnimatorReverseResId == 0)
            {
                animatorIn = AnimatorInflater.LoadAnimator(context, mAnimatorResId);
                animatorIn.SetInterpolator(this);
            }
            else
            {
                animatorIn = AnimatorInflater.LoadAnimator(context, mAnimatorReverseResId);
            }
            return animatorIn;
        }

        public void SetViewPager(ViewPager viewPager)
        {
            mViewpager = viewPager;
            if (mViewpager != null && mViewpager.Adapter != null)
            {
                mLastPosition = -1;
                CreateIndicators();
                mViewpager.RemoveOnPageChangeListener(this);
                mViewpager.AddOnPageChangeListener(this);
                OnPageSelected(mViewpager.CurrentItem);
            }
        }

        public void OnPageScrollStateChanged(int state)
        {
        }

        public void OnPageScrolled(int position, float positionOffset, int positionOffsetPixels)
        {
        }

        public void OnPageSelected(int position)
        {   
            if (mViewpager.Adapter == null || mViewpager.Adapter.Count <= 0)
            {
                return;
            }

            if (mAnimatorIn.IsRunning)
            {                
                mAnimatorIn.End();
                mAnimatorIn.Cancel();
            }

            if (mAnimatorOut.IsRunning)
            {                
                mAnimatorOut.End();
                mAnimatorOut.Cancel();
            }
            
            if (mLastPosition >= 0 && (currentIndicator = GetChildAt(mLastPosition)) != null)
            {                
                currentIndicator.SetBackgroundResource(mIndicatorUnselectedBackgroundResId);
                mAnimatorIn.SetTarget(currentIndicator);
                mAnimatorIn.Start();
            }

            View selectedIndicator = GetChildAt(position);
            if (selectedIndicator != null)
            {                
                selectedIndicator.SetBackgroundResource(mIndicatorBackgroundResId);
                mAnimatorOut.SetTarget(selectedIndicator);
                mAnimatorOut.Start();
            }
            mLastPosition = position;
        }

        public DataSetObserver GetDataSetObserver()
        {
            var dataSetObserver = new Observer();
            dataSetObserver.OnChangedEvent += DataSetObserver_OnChangedEvent;
            return dataSetObserver;
        }

        private void DataSetObserver_OnChangedEvent(object sender, EventArgs e)
        {
            if (mViewpager == null)
            {
                return;
            }

            int newCount = mViewpager.Adapter.Count;
            int currentCount = mViewpager.ChildCount;

            if (newCount == currentCount)
            {
                return;
            }
            else if (mLastPosition < newCount)
            {
                mLastPosition = mViewpager.CurrentItem;
            }
            else
            {
                mLastPosition = -1;
            }
            CreateIndicators();
        }

        private class Observer : DataSetObserver
        {
            public event EventHandler OnChangedEvent;
            public override void OnChanged()
            {
                base.OnChanged();
                OnChangedEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        private void CreateIndicators()
        {
            RemoveAllViews();
            int count = mViewpager.Adapter.Count;
            if (count <= 0)
            {
                return;
            }
            int currentItem = mViewpager.CurrentItem;
            int orientation = (int)Orientation;

            for (int i = 0; i < count; i++)
            {
                if (currentItem == i)
                {
                    AddIndicator(orientation, mIndicatorBackgroundResId, mImmediateAnimatorOut);
                }
                else
                {
                    AddIndicator(orientation, mIndicatorUnselectedBackgroundResId, mImmediateAnimatorIn);
                }
            }
        }

        private void AddIndicator(int orientation, int backgroundDrawableId, Animator animator)
        {
            if (animator.IsRunning)
            {
                animator.End();
                animator.Cancel();                
            }

            View Indicator = new View(this.Context);
            Indicator.SetBackgroundResource(backgroundDrawableId);
            AddView(Indicator, mIndicatorWidth, mIndicatorHeight);
            LayoutParams lp = (LayoutParams)Indicator.LayoutParameters;

            if (orientation == (int)Android.Widget.Orientation.Horizontal)
            {
                lp.LeftMargin = mIndicatorMargin;
                lp.RightMargin = mIndicatorMargin;
            }
            else
            {
                lp.TopMargin = mIndicatorMargin;
                lp.BottomMargin = mIndicatorMargin;
            }

            Indicator.LayoutParameters = lp;
            animator.SetTarget(Indicator);
            animator.Start();
        }

        public float GetInterpolation(float input)
        {
            return Math.Abs(1.0f - input);
        }

        public int Dip2px(float dpValue)
        {
            float scale = Resources.DisplayMetrics.Density;
            return (int)(dpValue * scale + 0.5f);
        }
    }
}