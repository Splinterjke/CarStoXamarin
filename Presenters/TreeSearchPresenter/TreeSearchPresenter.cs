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
using CarSto.Presenters.TreeSearchPresenter;
using CarSto.Services;
using Newtonsoft.Json;

namespace CarSto.Presenters.TreeSearchPresenter
{
    public class TreeSearchPresenter : ITreeSearchPresenter
    {
        public enum searchType
        {
            tree,
            oem,
            name
        }

        public string VrID { get; set; }
        public ITreeSearchView View { get; set; }
        public searchType SelectedSearchType { get; set; }

        public void OnCloseFragmentClicked()
        {
            
        }

        public void OnPartClicked()
        {
            
        }

        public async void OnSearchClicked(string text)
        {
            Tuple<bool, string> response = new Tuple<bool, string>(true, null);
            switch (SelectedSearchType)
            {
                case searchType.tree:
                    break;
                case searchType.oem:
                    response = await ClientAPI.GetAsync($"Catalog/{VrID}/Detail/{text}");
                    break;
                case searchType.name:
                    response = await ClientAPI.GetAsync($"Catalog/{VrID}/Detail/Name/{text}");
                    break;
            }
            if(response.Item1)
            {
                View.ShowError();
                return;
            }
            var data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(response.Item2);
            View.UpdateResultList(data);
        }

        public void OnSearchTypeChanged()
        {
            
        }
    }
}