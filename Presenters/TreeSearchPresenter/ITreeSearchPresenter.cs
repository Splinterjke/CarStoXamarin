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

namespace CarSto.Presenters.TreeSearchPresenter
{
    public interface ITreeSearchPresenter
    {
        ITreeSearchView View { get; set; }
        void OnSearchTypeChanged();
        void OnSearchClicked(string text);
        void OnCloseFragmentClicked();
        void OnPartClicked();
    }
}