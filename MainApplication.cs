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
using Calligraphy;
using CarSto.Services;

namespace CarSto
{
	[Application]
	public class MainApplication : Application, Application.IActivityLifecycleCallbacks
	{
		public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
		{
		}

		public override void OnCreate()
		{
			base.OnCreate();
            CalligraphyConfig.InitDefault(new CalligraphyConfig.Builder()
                    .SetDefaultFontPath("fonts/azbuka.ttf")
                    .SetFontAttrId(Resource.Attribute.fontPath)
                // Adding a custom view that support adding a typeFace
                // .AddCustomViewWithSetTypeface(Java.Lang.Class.FromType(typeof(CustomViewWithTypefaceSupport)))
                // Adding a custom style
                // .AddCustomStyle(Java.Lang.Class.FromType(typeof(TextField)), Resource.Attribute.textFieldStyle)
                .Build()
            );
            RegisterActivityLifecycleCallbacks(this);
            Android.Support.V7.App.AppCompatDelegate.CompatVectorFromResourcesEnabled = true;
        }

		public override void OnTerminate()
		{
			base.OnTerminate();
			UnregisterActivityLifecycleCallbacks(this);
		}

		public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
		{
			
		}

		public void OnActivityDestroyed(Activity activity)
		{
		}

		public void OnActivityPaused(Activity activity)
		{
		}

		public void OnActivityResumed(Activity activity)
		{
			
		}

		public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
		{
		}

		public void OnActivityStarted(Activity activity)
		{
			
		}

		public void OnActivityStopped(Activity activity)
		{
		}
    }
}