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
using Android.Util;

namespace CarSto.CustomControls
{
    public class ZoomableLayout : RelativeLayout
    {
        private readonly ScaleGestureDetector _scaleDetector;
        private readonly GestureDetector _moveDetector;
        private static readonly int InvalidPointerId = -1;

        private int _activePointerId = InvalidPointerId;
        private float _lastTouchX;
        private float _lastTouchY;
        private float _posX;
        private float _posY;
        public float _scaleFactor = 1.0f;
        private float _maxWidth = 0.0f;
        private float _maxHeight = 0.0f;
        private float _width;
        private float _height;

        public ZoomableLayout(Android.Content.Context context, IAttributeSet attrs) : base(context, attrs)
        {
            _scaleDetector = new ScaleGestureDetector(context, new ScaleListener(this));
            _moveDetector = new GestureDetector(context, new GestureListener(this));
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            _width = MeasureSpec.GetSize(widthMeasureSpec);
            _height = MeasureSpec.GetSize(heightMeasureSpec);
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            base.OnTouchEvent(e);
            _scaleDetector.OnTouchEvent(e);
            _moveDetector.OnTouchEvent(e);

            MotionEventActions action = e.Action & MotionEventActions.Mask;
            int pointerIndex;

            switch (action)
            {
                case MotionEventActions.Down:
                    _lastTouchX = e.GetX();
                    _lastTouchY = e.GetY();
                    _activePointerId = e.GetPointerId(0);
                    break;

                case MotionEventActions.Move:
                    pointerIndex = e.FindPointerIndex(_activePointerId);
                    if (pointerIndex > 0)
                    {
                        float x = e.GetX(pointerIndex);
                        float y = e.GetY(pointerIndex);
                        if (!_scaleDetector.IsInProgress)
                        {
                            // Only move the ScaleGestureDetector isn't already processing a gesture.
                            float deltaX = x - _lastTouchX;
                            float deltaY = y - _lastTouchY;
                            _posX += deltaX;
                            _posY += deltaY;

                            if (_posX > 0.0f)
                                _posX = 0.0f;
                            else if (_posX < _maxWidth)
                                _posX = _maxWidth;

                            if (_posY > 0.0f)
                                _posY = 0.0f;
                            else if (_posY < _maxHeight)
                                _posY = _maxHeight;

                            _lastTouchX = x;
                            _lastTouchY = y;
                            Invalidate();
                        }
                    }

                    break;

                case MotionEventActions.Up:
                case MotionEventActions.Cancel:
                    // We no longer need to keep track of the active pointer.
                    _activePointerId = InvalidPointerId;
                    break;

                case MotionEventActions.PointerUp:
                    // check to make sure that the pointer that went up is for the gesture we're tracking.
                    pointerIndex = (int)(e.Action & MotionEventActions.PointerIndexMask) >> (int)MotionEventActions.PointerIndexShift;
                    int pointerId = e.GetPointerId(pointerIndex);
                    if (pointerId == _activePointerId)
                    {
                        // This was our active pointer going up. Choose a new
                        // action pointer and adjust accordingly
                        int newPointerIndex = pointerIndex == 0 ? 1 : 0;
                        _lastTouchX = e.GetX(newPointerIndex);
                        _lastTouchY = e.GetY(newPointerIndex);
                        _activePointerId = e.GetPointerId(newPointerIndex);
                    }
                    break;

            }
            return true;
        }

        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            base.OnDraw(canvas);
            canvas.Save();
            canvas.Translate(_posX, _posY);
            canvas.Scale(_scaleFactor, _scaleFactor);
            canvas.Restore();
        }

        protected override void DispatchDraw(Android.Graphics.Canvas canvas)
        {
            canvas.Save();
            if (_scaleFactor == 1.0f)
            {
                _posX = 0.0f;
                _posY = 0.0f;
            }
            canvas.Translate(_posX, _posY);
            canvas.Scale(_scaleFactor, _scaleFactor);
            base.DispatchDraw(canvas);
            canvas.Restore();
            Invalidate();
        }

        private class ScaleListener : ScaleGestureDetector.SimpleOnScaleGestureListener
        {
            private readonly ZoomableLayout _view;

            public ScaleListener(ZoomableLayout view)
            {
                _view = view;
            }
            public override bool OnScale(ScaleGestureDetector detector)
            {
                _view._scaleFactor *= detector.ScaleFactor;
                _view._scaleFactor = Math.Max(1.0f, Math.Min(_view._scaleFactor, 5.0f));
                _view._maxWidth = _view._width - (_view._width * _view._scaleFactor);
                _view._maxHeight = _view._height - (_view._height * _view._scaleFactor);
                _view.Invalidate();
                return true;
            }
        }

        private class GestureListener : GestureDetector.SimpleOnGestureListener
        {
            private readonly ZoomableLayout _view;

            public GestureListener(ZoomableLayout view)
            {
                _view = view;
            }

            public override bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
            {
                base.OnScroll(e1, e2, distanceX, distanceY);
                if (_view._scaleFactor > 1)
                {
                    //if (((_view.ScrollX * _view._scaleFactor) < _view.Width) || ((_view.ScrollY * _view._scaleFactor) < _view.Height))
                    //{
                    _view.ScrollBy((int)distanceX, (int)distanceY);
                    _view.Invalidate();
                    //}
                }
                else
                {
                    var left = _view.Left;
                    var width = _view.Width;
                    var top = _view.Top;
                    if (left - _view.ScrollX != 0 || top - _view.ScrollY != 0)
                    {
                        _view.ScrollBy(left - _view.ScrollX, top - _view.ScrollY);
                        _view.Invalidate();
                    }
                }
                return true;

            }
        }
    }
}