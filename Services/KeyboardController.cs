// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
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
using Android.InputMethodServices;
using Android.Views.InputMethods;

namespace CarSto.Services
{
    public static class KeyboardController
    {
        public static void DissmisKeyboard(Activity activity)
        {
            var view = activity.CurrentFocus;
            if (view != null)
            {
                var imm = (InputMethodManager)activity.GetSystemService("input_method");
                imm.HideSoftInputFromWindow(view.WindowToken, 0);
            }
        }

        public static void ShowKeyboard(View view, Context context)
        {
            if (view.HasFocus)
            {
                var imm = (InputMethodManager)context.GetSystemService("input_method");
                imm.ToggleSoftInputFromWindow(view.WindowToken, ShowSoftInputFlags.Forced, 0);
            }
        }
    }
}