using System.Collections.Generic;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using Newtonsoft.Json;
using static Android.Support.Design.Widget.BottomSheetBehavior;
using Android.Support.Design.Widget;
using Android.Content;
using Android.Util;
using Android.Support.V4.Content;
using CarSto.Services;
using CarSto.Adapters;
using Com.Nex3z.Notificationbadge;
using Android.Graphics.Drawables;
using Android.Graphics;
using System;
using CarSto.CustomControls;
using Android.Widget;

namespace CarSto.Fragments
{
    public class PartFragmentBehavior : BottomSheetCallback
    {
        public override void OnSlide(View bottomSheet, float slideOffset)
        {
            var layoutParam = (bottomSheet as CardView).LayoutParameters as CoordinatorLayout.LayoutParams;
            if (Android.OS.Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
                layoutParam.SetMargins(DpToPx(14 - 24 * slideOffset, bottomSheet.Context), 0, DpToPx(14 - 24 * slideOffset, bottomSheet.Context), 0);
            else layoutParam.SetMargins(DpToPx(14 - 14 * slideOffset, bottomSheet.Context), 0, DpToPx(14 - 14 * slideOffset, bottomSheet.Context), 0);
            (bottomSheet as CardView).LayoutParameters = layoutParam;
        }

        public override void OnStateChanged(View bottomSheet, int newState)
        {

        }

        private int DpToPx(float dp, Context context)
        {
            int px = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, context.Resources.DisplayMetrics);
            return px;
        }
    }

    public class PartListFragment : Fragment
    {
        public RecyclerView partList;
        private CardView cardView;
        private Android.Widget.ImageView image;
        private ZoomableLayout zoomLayout;
        BottomSheetBehavior bottomSheetBehavior;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_makeorderpartlist, container, false);
            partList = view.FindViewById<RecyclerView>(Resource.Id.makeorderpartlist_partList);
            partList.SetLayoutManager(new LinearLayoutManager(this.Context));
            var dividerItemDecoration = new DividerItemDecoration(partList.Context, DividerItemDecoration.Vertical);
            dividerItemDecoration.SetDrawable(ContextCompat.GetDrawable(this.Context, Resource.Drawable.line_devider));
            partList.AddItemDecoration(dividerItemDecoration);

            cardView = view.FindViewById<CardView>(Resource.Id.makeorderpartlist_bottom_sheet);

            bottomSheetBehavior = From(cardView);
            bottomSheetBehavior.SetBottomSheetCallback(new PartFragmentBehavior());
            bottomSheetBehavior.PeekHeight = 140;
            image = view.FindViewById<Android.Widget.ImageView>(Resource.Id.makeorderpartlist_image);
            zoomLayout = view.FindViewById<ZoomableLayout>(Resource.Id.zoomableLayout);

            var list = new List<Dictionary<string, object>>();
            var adapter = new PartListAdapter(null);
            adapter.ItemClick += PartList_ItemClick;
            adapter.LaborClick += PartList_LaborClick;
            partList.SetAdapter(adapter);
            return view;
        }

        internal void UpdatePicture(string imgData, List<Dictionary<string, object>> coordIndexList)
        {
            var picture = BinaryToBitmap(imgData);
            if (picture != null)
                image.SetImageBitmap(picture);
            if (coordIndexList == null && coordIndexList.Count == 0)
                return;
            for (int i = 0; i < coordIndexList.Count; i++)
            {
                var x = Convert.ToInt32(coordIndexList[i]["X"]);
                var y = Convert.ToInt32(coordIndexList[i]["Y"]);
                var control = new Button(zoomLayout.Context);
                var param = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
                param.SetMargins(x, y, 0, 0);
                param.Width = DpToPx(30, this.Context);
                param.Height = DpToPx(30, this.Context);
                control.LayoutParameters = param;
                control.SetBackgroundColor(Color.Black);
                control.Text = coordIndexList[i]["Index"].ToString();
                control.SetTextColor(Color.White);
                zoomLayout.AddView(control);
            }
        }

        private int DpToPx(float px, Context context)
        {
            int dp = (int)TypedValue.ApplyDimension(ComplexUnitType.Px, px, context.Resources.DisplayMetrics);
            return dp;
        }

        private Bitmap BinaryToBitmap(string bin)
        {
            var byteArray = System.Convert.FromBase64String(bin);
            BitmapFactory.Options options = new BitmapFactory.Options();
            return BitmapFactory.DecodeByteArray(byteArray, 0, byteArray.Length, options);
        }

        private void PartList_LaborClick(object sender, int e)
        {   
            //var laborIds = JsonConvert.DeserializeObject<List<int>>((partList.GetAdapter() as PartListAdapter).Items[e]["LaborIds"].ToString());
            var laborFragment = new LaborListDialogFragment((partList.GetAdapter() as PartListAdapter).Items[e]["Oem"].ToString(), "Выберите работы");
            //laborFragment.SetStyle(AppCompatDialogFragment.StyleNormal, Resource.Style.DialogNoBorder);
            laborFragment.Show(this.FragmentManager, "LaborFragment");
        }

        private void PartList_ItemClick(object sender, int e)
        {
            //var laborsIds = JsonConvert.DeserializeObject<List<int>>((partList.GetAdapter() as PartLaborListAdapter).Items[e]]["Oem"].ToString();
            var adapter = partList.GetAdapter() as PartListAdapter;
            var preOrderFragment = ((this.ParentFragment as MechanicalCategoryFragment).viewPager.Adapter as TabsAdapter).GetItem(2) as PreOrderListFragment;
            var preOrderPartAdapter = preOrderFragment.preOrderPartList.GetAdapter() as PreOrderAdapter;
            preOrderPartAdapter.items.Add(adapter.Items[e]);
            preOrderPartAdapter.NotifyItemRangeChanged(0, preOrderPartAdapter.ItemCount);
            var preOrderLaborAdapter = preOrderFragment.preOrderLaborList.GetAdapter() as PreOrderAdapter;
            ((this.ParentFragment as MechanicalCategoryFragment).notifBadge as NotificationBadge).SetNumber(preOrderPartAdapter.ItemCount + preOrderLaborAdapter.ItemCount);

        }

        public void UpdateList(List<Dictionary<string, object>> newList)
        {
            var adapter = partList.GetAdapter() as PartListAdapter;
            adapter.Items = newList;
            adapter.NotifyItemRangeChanged(0, adapter.ItemCount);
        }
    }
}