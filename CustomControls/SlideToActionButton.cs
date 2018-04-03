using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Text;
using Android.Content.Res;

namespace CarSto.CustomControls
{
    public class SlideToActionButton : View
    {
        public interface IOnSlideToActionEventListener
        {
            void OnSlideToUnlockCanceled();

            void OnSlideToUnlockDone();
        }

        private int measuredWidth, measuredHeight;
        private float density;
        private IOnSlideToActionEventListener externalListener;
        private Paint mBackgroundPaint, mTextPaint, mSliderPaint;
        private float rx, ry; // Corner radius
        private Path mRoundedRectPath;
        private String text = "Unlock  →";

        float x;
        float event_x, event_y;
        float radius;
        float X_MIN, X_MAX;
        private bool ignoreTouchEvents;

        // Do we cancel when the Y coordinate leaves the view?
        private bool cancelOnYExit;
        private bool useDefaultCornerRadiusX, useDefaultCornerRadiusY;


        /**
         * Default values *
         */
        Color backgroundColor = Color.ParseColor("#FF807B7B");
        Color textColor = Color.ParseColor("#FFFFFFFF");
        Color sliderColor = Color.ParseColor("#AA404040");

        public SlideToActionButton(Context context) : base(context)
        {
            Init(context, null, 0);
        }

        public SlideToActionButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context, attrs, 0);
        }

        public SlideToActionButton(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Init(context, attrs, defStyleAttr);
        }

        public IOnSlideToActionEventListener GetExternalListener()
        {
            return externalListener;
        }

        public void SetExternalListener(IOnSlideToActionEventListener externalListener)
        {
            this.externalListener = externalListener;
        }

        private void Init(Context context, IAttributeSet attrs, int style)
        {

            Resources res = Resources;
            density = res.DisplayMetrics.Density;

            TypedArray a = Context.ObtainStyledAttributes(attrs, Resource.Styleable.SlideToAction, style, 0);

            string tmp = a.GetString(Resource.Styleable.SlideToAction_slideToUnlockText);
            text = string.IsNullOrEmpty(tmp) ? text : tmp;
            rx = a.GetDimension(Resource.Styleable.SlideToAction_cornerRadiusX, rx);
            useDefaultCornerRadiusX = rx == 0;
            ry = a.GetDimension(Resource.Styleable.SlideToAction_cornerRadiusX, ry);
            useDefaultCornerRadiusY = ry == 0;
            backgroundColor = a.GetColor(Resource.Styleable.SlideToAction_slideToUnlockBackgroundColor, backgroundColor);
            textColor = a.GetColor(Resource.Styleable.SlideToAction_slideToUnlockTextColor, textColor);
            sliderColor = a.GetColor(Resource.Styleable.SlideToAction_sliderColor, sliderColor);
            cancelOnYExit = a.GetBoolean(Resource.Styleable.SlideToAction_cancelOnYExit, false);

            a.Recycle();

            mRoundedRectPath = new Path();

            mBackgroundPaint = new Paint(PaintFlags.AntiAlias);
            mBackgroundPaint.SetStyle(Paint.Style.Fill);
            mBackgroundPaint.Color = backgroundColor;

            mTextPaint = new Paint(PaintFlags.AntiAlias);
            mTextPaint.SetStyle(Paint.Style.Fill);
            mTextPaint.Color = textColor;
            mTextPaint.SetTypeface(Typeface.Create("Roboto-Thin", TypefaceStyle.Normal));

            mSliderPaint = new Paint(PaintFlags.AntiAlias);
            mSliderPaint.SetStyle(Paint.Style.FillAndStroke);
            mSliderPaint.Color = sliderColor;
            mSliderPaint.StrokeWidth = 2 * density;

            if (!IsInEditMode)
            {
                // Edit mode does not support shadow layers
                // mSliderPaint.setShadowLayer(10.0f, 0.0f, 2.0f, 0xFF000000);
                //mSliderPaint.setMaskFilter(new EmbossMaskFilter(new float[]{1, 1, 1}, 0.4f, 10, 8.2f));
                float[] direction = new float[] { 0.0f, -1.0f, 0.5f };
                MaskFilter filter = new EmbossMaskFilter(direction, 0.8f, 15f, 1f);
                mSliderPaint.SetMaskFilter(filter);
                //mSliderPaint.setShader(new LinearGradient(8f, 80f, 30f, 20f, Color.RED,Color.WHITE, Shader.TileMode.MIRROR));
                //mSliderPaint.setShader(new RadialGradient(8f, 80f, 90f, Color.RED,Color.WHITE, Shader.TileMode.MIRROR));
                //mSliderPaint.setShader(new SweepGradient(80, 80, Color.RED, Color.WHITE));
                //mSliderPaint.setMaskFilter(new BlurMaskFilter(15, BlurMaskFilter.Blur.OUTER));
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            measuredHeight = GetDefaultSize(SuggestedMinimumHeight, heightMeasureSpec);
            measuredWidth = GetDefaultSize(SuggestedMinimumWidth, widthMeasureSpec);

            if (useDefaultCornerRadiusX)
            {
                rx = measuredHeight * 0.52f;
            }
            if (useDefaultCornerRadiusY)
            {
                ry = measuredHeight * 0.52f;
            }
            mTextPaint.TextSize = measuredHeight / 3.0f;

            radius = measuredHeight * 0.45f;
            X_MIN = 1.2f * radius;
            X_MAX = measuredWidth - X_MIN;
            x = X_MIN;

            SetMeasuredDimension(measuredWidth, measuredHeight);
        }

        private void DrawRoundRect(Canvas c)
        {
            mRoundedRectPath.Reset();
            mRoundedRectPath.MoveTo(rx, 0);
            mRoundedRectPath.LineTo(measuredWidth - rx, 0);
            mRoundedRectPath.QuadTo(measuredWidth, 0, measuredWidth, ry);
            mRoundedRectPath.LineTo(measuredWidth, measuredHeight - ry);
            mRoundedRectPath.QuadTo(measuredWidth, measuredHeight, measuredWidth - rx, measuredHeight);
            mRoundedRectPath.LineTo(rx, measuredHeight);
            mRoundedRectPath.QuadTo(0, measuredHeight, 0, measuredHeight - ry);
            mRoundedRectPath.LineTo(0, ry);
            mRoundedRectPath.QuadTo(0, 0, rx, 0);
            c.DrawPath(mRoundedRectPath, mBackgroundPaint);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            if (measuredHeight <= 0 || measuredWidth <= 0)
            {
                // There is not much we can draw :/
                return;
            }

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                canvas.DrawRoundRect(0, 0, measuredWidth, measuredHeight, rx, ry, mBackgroundPaint);
            }
            else
            {
                DrawRoundRect(canvas);
            }


            // Draw the text in center
            float xPos = ((measuredWidth - mTextPaint.MeasureText(text)) / 2.0f);
            float yPos = (measuredHeight / 2.0f);
            float titleHeight = Math.Abs(mTextPaint.Descent() + mTextPaint.Ascent());
            yPos += titleHeight / 2.0f;
            canvas.DrawText(text, xPos, yPos, mTextPaint);


            canvas.DrawCircle(x, measuredHeight * 0.5f, radius, mSliderPaint);
        }

        private void OnCancel()
        {
            Reset();
            if (externalListener != null)
            {
                externalListener.OnSlideToUnlockCanceled();
            }
        }

        private void OnUnlock()
        {
            if (externalListener != null)
            {
                externalListener.OnSlideToUnlockDone();
            }
        }

        private void Reset()
        {
            x = X_MIN;
            Invalidate();
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Up:
                    ignoreTouchEvents = false;
                    Reset();
                    return true;
                case MotionEventActions.Down:
                    // Is within the circle??
                    event_x = e.GetX(0);
                    event_y = e.GetY(0);
                    double squareRadius = radius * radius;
                    double squaredXDistance = (event_x - X_MIN) * (event_x - X_MIN);
                    double squaredYDistance = (event_y - measuredHeight / 2) * (event_y - measuredHeight / 2);
                    if (squaredXDistance + squaredYDistance > squareRadius)
                    {
                        // User touched outside the button, ignore his touch
                        ignoreTouchEvents = true;
                    }
                    return true;
                case MotionEventActions.Cancel:
                    ignoreTouchEvents = true;
                    OnCancel();
                    return true;
                case MotionEventActions.Move:
                    if (!ignoreTouchEvents)
                    {
                        event_x = e.GetX(0);
                        if (cancelOnYExit)
                        {
                            event_y = e.GetY(0);
                            if (event_y < 0 || event_y > measuredHeight)
                            {
                                ignoreTouchEvents = true;
                                OnCancel();
                            }
                        }

                        x = event_x > X_MAX ? X_MAX : event_x < X_MIN ? X_MIN : event_x;
                        if (event_x >= X_MAX)
                        {
                            ignoreTouchEvents = true;
                            OnUnlock();
                        }
                        Invalidate();
                    }
                    return true;
                default:
                    return base.OnTouchEvent(e);
            }
        }
    }
}