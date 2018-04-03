using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using CarSto.Adapters;
using XamDroid.ExpandableRecyclerView;
using Newtonsoft.Json;
using CarSto.Services;
using static CarSto.Services.ViewInjectorHelper;
using CarSto.Presenters;
using CarSto.CustomControls;
using Android.Support.Design.Widget;

namespace CarSto.Fragments
{
    public class TreeFragment : Fragment, IStyleView
    {
        [InjectView(Resource.Id.tree)]
        RecyclerView tree;

        [InjectView(Resource.Id.tree_search_fab)]
        private FloatingActionButton searchFab;

        List<IParentObject> treeList;

        public TreeFragment(List<IParentObject> treeList)
        {
            this.treeList = treeList;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_tree, container, false);
            ViewInjector.Inject(this, view);
            StyleView();
            var adapter = new MakeOrderTreeViewAdapter(this.Context, treeList);
            adapter.ItemClick += TreeViewAdapter_ItemClick;
            adapter.SetParentClickableViewAnimationDefaultDuration();
            tree.SetAdapter(adapter);
            return view;
        }

        [InjectOnClick(Resource.Id.tree_search_fab)]
        private void SearchFab_Click(object sender, EventArgs e)
        {
            var fragment = new TreeSearchFragment();
            FragmentManager.BeginTransaction().Replace(Resource.Id.tree_search_container, fragment).Commit();
        }

        public void StyleView()
        {
            tree.SetLayoutManager(new LinearLayoutManager(this.Context));
            var dividerItemDecoration = new DividerItemDecoration(tree.Context, DividerItemDecoration.Vertical);
            dividerItemDecoration.SetDrawable(ContextCompat.GetDrawable(this.Context, Resource.Drawable.line_devider));
            tree.AddItemDecoration(dividerItemDecoration);
        }

        private async void TreeViewAdapter_ItemClick(object sender, Dictionary<string, object> e)
        {
            (this.ParentFragment as MechanicalCategoryFragment).viewPager.CurrentItem = 1;
            var response = await ClientAPI.GetAsync($"Vehicle/{(ParentFragment as MechanicalCategoryFragment).vehRecId}/Part?treeNodeId={e["Id"]}");
            if (response.Item1)
                return;
            var partsHolderList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(response.Item2);
            if (partsHolderList == null)
                return;
            var partListFragment = (((this.ParentFragment as MechanicalCategoryFragment).viewPager.Adapter as TabsAdapter).GetItem(1) as PartListFragment);
            partListFragment.UpdateList(partsHolderList);

            response = await ClientAPI.GetAsync($"Vehicle/{(ParentFragment as MechanicalCategoryFragment).vehRecId}/Picture?pictureId={e["PictureId"]}");
            if (response.Item1)
                return;
            var imageData = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Item2);
            if (imageData == null)
                return;
            var coordIndexList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(imageData["CoordSchema"].ToString());
            partListFragment.UpdatePicture(imageData["Data"].ToString(), coordIndexList);
        }
    }
}