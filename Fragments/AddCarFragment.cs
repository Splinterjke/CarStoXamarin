// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V4.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using CarSto.Services;
using CarSto.Adapters;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Android.Support.Design.Widget;
using CarSto.Activities;
using static Android.Support.Design.Widget.NavigationView;
using CarSto.Presenters.AddCar;
using static CarSto.Services.ViewInjectorHelper;
using CarSto.Presenters;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;

namespace CarSto.Fragments
{
    public class AddCarFragment : Fragment, IAddCarView, IStyleView
    {
        AddCarPresenter presenter;

        #region Widgets
        [InjectView(Resource.Id.addcar_stub_text)]
        TextView dummyText;

        [InjectView(Resource.Id.addcar_stub_btn)]
        ImageButton addCarBtn;

        [InjectView(Resource.Id.addcar_check_vin_text)]
        EditText vin;

        [InjectView(Resource.Id.addcar_check_vin_btn)]
        Button checkVinBtn;

        [InjectView(Resource.Id.addcar_stub_layout)]
        LinearLayout stubLayout;

        [InjectView(Resource.Id.addcar_fields_layout)]
        LinearLayout fieldsLayout;
        #endregion

        #region Base Methods
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_addcar, null);
            presenter = new AddCarPresenter() { View = this };
            ViewInjector.Inject(this, view);
            return view;
        }

        [InjectOnClick(Resource.Id.addcar_check_vin_btn)]
        private void CheckVinBtn_Click(object sender, EventArgs e)
        {
            vin.ClearFocus();
            presenter.VinValidating();
        }

        [InjectOnTextChanged(Resource.Id.addcar_check_vin_text)]
        private void Vin_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            presenter.VIN = e.Text.ToString();
            presenter.OnVinTextValidating();
        }

        [InjectOnClick(Resource.Id.addcar_stub_btn)]
        private void AddCarBtn_Click(object sender, EventArgs e)
        {
            stubLayout.Visibility = ViewStates.Gone;
            fieldsLayout.Visibility = ViewStates.Visible;
        }

        #endregion

        #region Methdods

        public void ShowAvailableCars(List<string> cars)
        {
            var carCount = fieldsLayout.FindViewWithTag("carListCount") as TextView;
            if (carCount == null)
            {
                var titleField = new TextView(this.Context);
                titleField.Tag = "carListCount";
                titleField.SetPadding(15, 20, 15, 20);
                titleField.SetTextColor(Color.White);
                titleField.TextSize = 14;
                titleField.SetText("Íàéäåíî àâòîìîáèëåé: " + cars.Count, TextView.BufferType.Normal);
                fieldsLayout.AddView(titleField);
            }
            else carCount.Text = "Íàéäåíî àâòîìîáèëåé: " + cars.Count;

            var carList = fieldsLayout.FindViewWithTag("carListView") as Spinner;
            if (carList == null)
            {
                var field = new Spinner(this.Context);
                field.Tag = "carListView";
                var adapter = new SpinAdapter(this.Activity, cars);
                field.Adapter = adapter;
                field.ItemSelected += (s, e) =>
                     {

                     };
                //field.ItemClick += (s, e) =>
                // {
                //     e.Position
                // };

                fieldsLayout.AddView(field);
            }
            else carList.Adapter = new SpinAdapter(this.Activity, cars);
        }

        public void ShowError(string errorMessage)
        {
            var toast = Toast.MakeText(this.Activity.ApplicationContext, errorMessage, ToastLength.Short);
            toast.Show();
        }

        public void ShowProcessing()
        {

        }

        public void HideProcessing()
        {

        }

        //[InjectOnClick(Resource.Id.login_signin_btn)]
        //private void MainLayout_Click(object sender, EventArgs e)
        //{
        //    KeyboardController.DissmisKeyboard(this.Activity);
        //}

        public void StyleView()
        {

        }

        public void EnabledCheckVinButton()
        {
            checkVinBtn.Enabled = true;
        }

        public void DisableCheckVinButton()
        {
            checkVinBtn.Enabled = false;
        }

        public void UpdateGarage()
        {
            (this.ParentFragment as GarageFragment).presenter.LoadGarage();
        }

        public void ShowFindCarsButton()
        {
            var findCarsButton = fieldsLayout.FindViewWithTag("findCarsButton");
            if (findCarsButton == null)
            {
                var button = new Button(this.Context);
                button.Tag = "findCarsButton";
                //button.LayoutParameters.Width = ViewGroup.LayoutParams.MatchParent;
                button.Text = "ÍÀÉÒÈ ÀÂÒÎ";
                button.SetBackgroundColor(new Color(ContextCompat.GetColor(this.Context, Resource.Color.Accent)));
                button.Click += delegate
                {
                    presenter.OnShowCarClicked();
                };
                fieldsLayout.AddView(button);
            }
        }

        public void ShowAddToGarageButton()
        {
            var addToGarageButton = fieldsLayout.FindViewWithTag("addToGarageButton");
            if (addToGarageButton == null)
            {
                var button = new Button(this.Context);
                button.Tag = "addToGarageButton";
                //button.LayoutParameters.Width = ViewGroup.LayoutParams.MatchParent;
                button.Text = "ÄÎÁÀÂÈÒÜ Â ÃÀÐÀÆ";
                button.SetBackgroundColor(new Color(ContextCompat.GetColor(this.Context, Resource.Color.Accent)));
                button.Click += delegate
                {
                    var carList = fieldsLayout.FindViewWithTag("carListView") as Spinner;
                    presenter.SelectedCar = presenter.carList[carList.SelectedItemPosition];
                    presenter.OnCarAddClicked();
                };
                fieldsLayout.AddView(button);
            }
        }

        public void ClearFields()
        {
            if (fieldsLayout.ChildCount > 2)
                fieldsLayout.RemoveViews(2, fieldsLayout.ChildCount - 2);
        }

        public void AddPrevFields(string title, string value)
        {
            var field = new Spinner(this.Context);
            var titleField = new TextView(this.Context);
            titleField.SetPadding(15, 20, 15, 20);
            titleField.SetTextColor(Color.White);
            titleField.TextSize = 14;
            titleField.SetText(title, TextView.BufferType.Normal);
            var adapter = new SpinAdapter(this.Activity, new List<string> { value });
            field.Adapter = adapter;
            fieldsLayout.AddView(titleField);
            fieldsLayout.AddView(field);
        }

        public void AddCurrentFields(string title, List<string> values, List<string> ssd)
        {
            var field = new Spinner(this.Context);
            var titleField = new TextView(this.Context);
            titleField.SetPadding(15, 20, 15, 20);
            titleField.SetTextColor(Color.White);
            titleField.TextSize = 14;
            titleField.SetText(title, TextView.BufferType.Normal);
            var adapter = new SpinAdapter(this.Activity, values, ssd);
            field.Adapter = adapter;
            field.ItemSelected += (s, e) =>
            {
                if (e.Position != 0)
                {
                    var selectedSSD = ((s as Spinner).Adapter as SpinAdapter).ssd[e.Position - 1];
                    presenter.SSD = selectedSSD;
                    presenter.SelectionStep();
                }
            };
            fieldsLayout.AddView(titleField);
            fieldsLayout.AddView(field);
        }
        #endregion
    }
}