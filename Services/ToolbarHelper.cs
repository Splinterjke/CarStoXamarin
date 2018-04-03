using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Widget;

namespace CarSto.Services
{
    public static class ToolbarHelper
    {
        private static AppCompatActivity Context;
        private static Android.Support.V7.Widget.Toolbar Toolbar;
        private static Android.Support.V4.App.Fragment Fragment;
        private static TextView Title;
        //public static Context

        public static void Init(AppCompatActivity context, Android.Support.V7.Widget.Toolbar toolbar, TextView title)
        {
            Context = context;
            Toolbar = toolbar;
            Title = title;
        }

        public static void SetToolbar()
        {
            Context.SetSupportActionBar(Toolbar);
            Context.SupportActionBar.SetDisplayShowTitleEnabled(false);
            Context.SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            Toolbar.NavigationClick += Toolbar_NavigationClick;
        }

        private static void Toolbar_NavigationClick(object sender, Android.Support.V7.Widget.Toolbar.NavigationClickEventArgs e)
        {
            if (Fragment != null)
            {
                Context.SupportFragmentManager.BeginTransaction().Remove(Fragment).Commit();
                Context.SupportFragmentManager.PopBackStack();
                Fragment = null;
            }
        }

        public static void SetToolbarStyle(string title, bool HomeAsUpEnabled, Android.Support.V4.App.Fragment fragment = null)
        {
            Fragment = fragment;
            Title.Text = title;
            Context.SupportActionBar.SetDisplayHomeAsUpEnabled(HomeAsUpEnabled);            
        }
    }
}