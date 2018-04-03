// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using Android.Content;
using CarSto.Services;
using Android.Views;
using Android.Util;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Widget;

namespace CarSto.CustomControls
{
    public class EditTextClearBtn : EditText
    {
        private Drawable clearBtn;

        public EditTextClearBtn(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        private void Init()
        {
            clearBtn = ContextCompat.GetDrawable(this.Context, Resource.Drawable.audi);
            clearBtn.SetBounds(0, 0, clearBtn.IntrinsicWidth / 2, clearBtn.IntrinsicHeight / 2);
            this.SetSingleLine(true);
            this.CompoundDrawablePadding = 20;
            this.SetPadding(this.PaddingStart, this.PaddingTop, 20, this.PaddingBottom);
            HandleClearButton();
            Click += (s,e) => { RequestFocus(); };
            Touch += EditTextClearBtn_Touch;
            TextChanged += EditTextClearBtn_TextChanged;
        }

        private void EditTextClearBtn_Touch(object sender, TouchEventArgs e)
        {
            if (this.GetCompoundDrawables()[2] == null)
            {
                if (!HasFocus)
                    RequestFocus();
                KeyboardController.ShowKeyboard((View)sender, Context);
                return;
            }

            if (e.Event.Action != MotionEventActions.Up)
            {
                if (!HasFocus)
                    RequestFocus();
                KeyboardController.ShowKeyboard((View)sender, Context);
                return;
            }

            if (e.Event.GetX() > this.Width - this.PaddingRight - clearBtn.IntrinsicWidth)
            {
                this.Text = string.Empty;
                HandleClearButton();
            }
        }

        private void EditTextClearBtn_TextChanged(object sender, TextChangedEventArgs e)
        {
            HandleClearButton();
        }

        private void HandleClearButton()
        {
            if (string.IsNullOrEmpty(this.Text))
            {
                this.SetCompoundDrawables(this.GetCompoundDrawables()[0], this.GetCompoundDrawables()[1], null, this.GetCompoundDrawables()[3]);
            }
            else
            {
                this.SetCompoundDrawables(this.GetCompoundDrawables()[0], this.GetCompoundDrawables()[1], clearBtn, this.GetCompoundDrawables()[3]);
            }
        }
    }
}