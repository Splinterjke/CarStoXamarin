using Android.OS;
using Android.Views;
using Android.Support.V4.App;
using Android.Widget;
using Android.Support.V7.App;
using System.Collections.Generic;
using CarSto.Adapters;
using Android.Content;
using System;

namespace CarSto.Fragments
{
    public class ChooseStoFragment : Fragment
    {
        #region Objects
        private event EventHandler MapReady;
        #endregion

        #region variables
        private Dictionary<string, object>[] stoList;
        public event EventHandler<string> ItemClick;
        private string selectedStoID;
        #endregion

        #region Widgets
        private ListView stoListView;
        private Button cancelButton, okButton;
        #endregion

        public ChooseStoFragment(Dictionary<string, object>[] stoList)
        {
            this.stoList = stoList;
        }

        #region Base Methods
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_choosesto, null, false);
            //GetElements(view);
            //InitData();
            //SetEventHandlers();
            //SetupMap();
            return view;
        }
        #endregion

        #region Methods
        //private void GetElements(View view)
        //{
        //    stoListView = view.FindViewById<ListView>(Resource.Id.choosesto_stolist);
        //    cancelButton = view.FindViewById<Button>(Resource.Id.choosesto_cancel_btn);
        //    okButton = view.FindViewById<Button>(Resource.Id.choosesto_ok_btn);
        //}

        //private void InitData()
        //{
        //    var stoNames = new Dictionary<string, string>(stoList.Length);
        //    for (int i = 0; i < stoList.Length; i++)
        //    {
        //        var stoName = stoList[i]["Name"] != null ? stoList[i]["Name"].ToString() : stoList[i]["Id"].ToString();
        //        stoNames.Add(stoList[i]["Id"].ToString(), stoName);
        //    }
        //    var adapter = new StoListAdapter(this.Context, Android.Resource.Layout.SimpleListItem1, stoNames);
        //    //var adapter = new StoListAdapter<string>(this.Context, Android.Resource.Layout.SimpleListItem1, stoNames);
        //    stoListView.Adapter = adapter;
        //    stoListView.ItemClick += (s, e) =>
        //    {
        //        selectedStoID = Convert.ToString((e.View as TextView).Tag);
        //        System.Diagnostics.Debug.WriteLine("STO ID: " + selectedStoID);
        //    };
        //}

        //private void SetupMap()
        //{
        //    using (var handler = new Handler(Looper.MainLooper))
        //        handler.Post(() =>
        //        {
        //            mapFragment?.GetMapAsync(this);
        //        });
        //}

        //public void OnMapReady(GoogleMap map)
        //{
        //    this.Map = map;
        //    MapReady?.Invoke(this, EventArgs.Empty);
        //    clubgaragePosition = new LatLng(59.9969032, 30.2357647);
        //    var mo = new MarkerOptions();
        //    mo.SetPosition(clubgaragePosition);
        //    mo.SetTitle("Клубный гараж");
        //    mo.SetSnippet("СТО на Оптиков 8");
        //    Map.AddMarker(mo);
        //    Map.MyLocationEnabled = true;
        //    Map.UiSettings.MyLocationButtonEnabled = true;
        //    Map.UiSettings.ZoomControlsEnabled = true;
        //    Map.MarkerClick += (s, e) => { e.Marker.ShowInfoWindow(); };
        //    MoveToCurrentLocation(clubgaragePosition);
        //}

        //// animate zooming to LatLng location
        //private void MoveToCurrentLocation(LatLng currentLocation)
        //{
        //    //var gmap = new GMapService(Map);
        //    //gmap.DrawPath(clubgaragePosition, new LatLng(30.9969032, 50.2357647), "Вы", "Назначение");
        //    CameraPosition cameraPosition = new CameraPosition.Builder().Target(currentLocation).Zoom(15).Build();
        //    Map?.AnimateCamera(CameraUpdateFactory.NewCameraPosition(cameraPosition));
        //    //var randrompoint = new LatLng(39.9969032, 50.2357647);
        //    //var mo = new MarkerOptions();
        //    //mo.SetPosition(randrompoint);
        //    //mo.SetTitle("Рандомная точка");
        //    //Map.AddMarker(mo);
        //    //PolylineOptions rectOptions = new PolylineOptions().InvokeColor(Android.Graphics.Color.DarkRed);
        //    //rectOptions.Add(clubgaragePosition);
        //    //rectOptions.Add(randrompoint);
        //    //Map.AddPolyline(rectOptions);
        //}

        //private void SetEventHandlers()
        //{
        //    stoListView.ItemSelected += delegate
        //    {
        //        okButton.Visibility = ViewStates.Visible;

        //    };            
        //}
        #endregion
    }
}