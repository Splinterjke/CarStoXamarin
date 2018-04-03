using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;
using static CarSto.Services.ViewInjectorHelper;
using CarSto.Presenters.TreeSearchPresenter;
using CarSto.Presenters;
using Android.Support.V7.Widget;
using Android.Support.V4.Content;
using Android.Graphics;
using static Android.Graphics.Drawables.Drawable;
using CarSto.CustomControls;
using System;
using System.Collections.Generic;
using CarSto.Adapters;

namespace CarSto.Fragments
{
    public class TreeSearchFragment : Fragment, ITreeSearchView, IStyleView
    {
        public TreeSearchPresenter presenter;

        [InjectView(Resource.Id.treesearch_target_text)]
        private TreeSearchEditText searchText;

        [InjectView(Resource.Id.treesearch_close_fragment_button)]
        private ImageButton closeFragmentButton;

        [InjectView(Resource.Id.treesearch_searchtype_radiogroup)]
        private RadioGroup searchTypeGroup;

        [InjectView(Resource.Id.treesearch_result_list)]
        private RecyclerView searchResultList;
        
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_treesearch, container, false);            
            presenter = new TreeSearchPresenter() { View = this };
            presenter.VrID = (this.ParentFragment as MechanicalCategoryFragment).vehRecId;
            ViewInjector.Inject(this, view);
            StyleView();
            SearchTypeGroup_CheckedChange(null, new RadioGroup.CheckedChangeEventArgs(Resource.Id.treesearch_searchtype_oem));
            searchText.SearchClick += SearchText_SearchClick;
            return view;
        }

        [InjectOnClick(Resource.Id.treesearch_close_fragment_button)]
        private void CloseFragmentButton_Click(object sender, System.EventArgs e)
        {
            FragmentManager.BeginTransaction().Remove(FragmentManager.FindFragmentById(Resource.Id.tree_search_container)).Commit();
        }

        private void SearchText_SearchClick(object sender, System.EventArgs e)
        {
            presenter.OnSearchClicked(searchText.Text);
        }

        [InjectOnRadioGroupCheckedChange(Resource.Id.treesearch_searchtype_radiogroup)]
        private void SearchTypeGroup_CheckedChange(object sender, RadioGroup.CheckedChangeEventArgs e)
        {
            if (searchTypeGroup.CheckedRadioButtonId != e.CheckedId)
                searchTypeGroup.Check(e.CheckedId);
            switch (e.CheckedId)
            {
                case Resource.Id.treesearch_searchtype_tree:                    
                    searchText.Hint = "Название узла";
                    presenter.SelectedSearchType = TreeSearchPresenter.searchType.tree;
                    break;
                case Resource.Id.treesearch_searchtype_oem:
                    searchText.Hint = "OEM";
                    presenter.SelectedSearchType = TreeSearchPresenter.searchType.oem;
                    break;
                case Resource.Id.treesearch_searchtype_name:
                    searchText.Hint = "Название детали";
                    presenter.SelectedSearchType = TreeSearchPresenter.searchType.name;
                    break;
            }
        }

        public void StyleView()
        {
            searchResultList.SetLayoutManager(new LinearLayoutManager(this.Context));
            var dividerItemDecoration = new DividerItemDecoration(searchResultList.Context, DividerItemDecoration.Vertical);
            dividerItemDecoration.SetDrawable(ContextCompat.GetDrawable(this.Context, Resource.Drawable.line_devider));
            searchResultList.AddItemDecoration(dividerItemDecoration);
        }

        public void ShowError()
        {
            
        }

        public void UpdateResultList(List<Dictionary<string,object>> result)
        {
            var adapter = searchResultList.GetAdapter() as TreeSearchResultAdapter;
            if(adapter == null)
            {
                adapter = new TreeSearchResultAdapter(result);                
                searchResultList.SetAdapter(adapter);
                //adapter.NotifyDataSetChanged();
                //searchResultList.Notify();
                return;
            }
        }
    }
}