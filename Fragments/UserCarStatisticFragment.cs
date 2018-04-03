// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using Android.Support.V4.App;
using Android.OS;
using Android.Views;

namespace CarSto.Fragments
{
    public class UserCarStatisticFragment : Fragment
    {
        #region Widgets
        private Android.Support.V7.Widget.RecyclerView statList;
        #endregion

        #region Base Methods
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_usercarstatistic, container, false);
            return view;
        }
        #endregion

        #region Methods
        #endregion
    }
}