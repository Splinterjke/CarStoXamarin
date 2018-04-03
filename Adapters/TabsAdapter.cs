// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Collections.Generic;
using Android.Support.V4.App;
using Java.Lang;

namespace CarSto.Adapters
{
	public class TabsAdapter : FragmentStatePagerAdapter
	{
        List<Fragment> fragments;
        List<String> fragmentsTitle;

        public TabsAdapter(FragmentManager fm, List<Fragment> fragments, List<String> fragmentsTitle = null) : base(fm)
        {
            this.fragments = fragments;
            if (fragmentsTitle != null)
                this.fragmentsTitle = fragmentsTitle;
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return fragmentsTitle[position];
        }

        //public override int GetItemPosition(Object @object)
        //{
        //    return PositionNone;
        //}

        public override int Count => fragments.Count;

		public override Android.Support.V4.App.Fragment GetItem(int position)
		{
			return fragments[position];
		}
	}
}