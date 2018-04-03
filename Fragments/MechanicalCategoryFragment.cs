// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com


using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using CarSto.Activities;
using CarSto.Adapters;
using CarSto.Models;
using CarSto.Services;
using Newtonsoft.Json;
using XamDroid.ExpandableRecyclerView;
using System;
using System.Reflection;
using CarSto.Adapters.ViewHolders;
using Android.Content.Res;
using Android.Support.Design.Widget;
using CarSto.CustomControls;
using Android.Support.V4.View;
using Com.Nex3z.Notificationbadge;

namespace CarSto.Fragments
{
    public class MechanicalCategoryFragment : Fragment
    {
        public CustomViewPager viewPager;
        private List<Fragment> fragments;
        private TabLayout tabLayout;
        public NotificationBadge notifBadge;
        private bool isActiveOrder;
        public string orderId, carId, vehRecId;

        public MechanicalCategoryFragment(string carId, bool isActiveOrder = false, string orderId = null, string vehRecId = null)
        {
            this.isActiveOrder = isActiveOrder;
            this.carId = carId;
            this.orderId = orderId;
            this.vehRecId = vehRecId;
        }        

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_mechanical_category, container, false);
            viewPager = view.FindViewById<CustomViewPager>(Resource.Id.mechanicalcategory_viewPager);
            tabLayout = view.FindViewById<TabLayout>(Resource.Id.mechanicalcategory_tablayout);
            notifBadge = view.FindViewById<NotificationBadge>(Resource.Id.mechanicalcategory_badge);
            notifBadge.SetBadgeBackgroundResource(Resource.Drawable.badge_bg_with_shadow);
            notifBadge.SetMaxTextLength(2);
            return view;
        }

        public async override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            var tree = await LoadData();
            fragments = new List<Fragment> { new TreeFragment(tree), new PartListFragment(), new PreOrderListFragment() };
            var fragmentTitles = new List<Java.Lang.String>(fragments.Count) { new Java.Lang.String("Узлы"), new Java.Lang.String("Детали"), new Java.Lang.String("Предзаказ") };
            TabsAdapter adapter = new TabsAdapter(this.ChildFragmentManager, fragments, fragmentTitles);
            viewPager.Adapter = adapter;
            viewPager.PageSelected += ViewPager_PageSelected;
            tabLayout.SetupWithViewPager(viewPager);
        }

        private void ViewPager_PageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            if (e.Position == 1)
                viewPager.PagingEnabled = false;
            else viewPager.PagingEnabled = true;
        }

        private async Task<List<IParentObject>> LoadData()
        {
            var response = await ClientAPI.GetAsync($"Garage/{carId}/Tree");
            if (response.Item1)
                return null;
            var treeList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(response.Item2);
            treeList.Sort((x, y) => Convert.ToInt32(x["Id"]).CompareTo(Convert.ToInt32(y["Id"])));
            var parentList = new List<IParentObject>();
            foreach (var item in treeList)
            {
                var parentId = item["ParentId"].ToString();
                if (parentId != "0")
                    continue;
                var childListTemp = treeList.Where(x => x["ParentId"].ToString() == item["Id"].ToString()).ToList();
                List<object> childList = new List<object>(childListTemp.Count);
                foreach (var child in childListTemp)
                {
                    childList.Add(DictionaryToObject(child));
                }
                parentList.Add(new TreeNodeViewModel() { Title = item["Title"].ToString(), ChildObjectList = childList });
            }
            return parentList;
        }

        private object DictionaryToObject(IDictionary<string, object> dict)
        {
            var t = dict;
            PropertyInfo[] properties = t.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (!dict.Any(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                    continue;

                KeyValuePair<string, object> item = dict.First(x => x.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase));

                // Find which property type (int, string, double? etc) the CURRENT property is...
                Type tPropertyType = t.GetType().GetProperty(property.Name).PropertyType;

                // Fix nullables...
                Type newT = Nullable.GetUnderlyingType(tPropertyType) ?? tPropertyType;

                // ...and change the type
                object newA = Convert.ChangeType(item.Value, newT);
                t.GetType().GetProperty(property.Name).SetValue(t, newA, null);
            }
            return t;
        }
    }
}