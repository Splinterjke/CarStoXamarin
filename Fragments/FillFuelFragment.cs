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
using System.Threading.Tasks;
using CarSto.Services;
using CarSto.CustomControls;
using CarSto.Activities;
using Newtonsoft.Json;
using Android.Support.V4.Content;

namespace CarSto.Fragments
{
    public class FillFuelFragment : Fragment
    {
        private Dictionary<string, object> carViewModel;
        private int selectedCarIndex;

        private EditTextClearBtn millage, cost, amount;
        private Button completeBtn;
        private TextView carName;
        private ImageView carLeftBtn, carRightBtn, carAva;

        #region Base Methods
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_fillfuel, null);
            ToolbarHelper.SetToolbarStyle("Заправка", false);
            GetElements(view);
            InitData();
            SetEventHandlers();
            return view;
        }

        private void SetEventHandlers()
        {
            completeBtn.Click += async delegate
            {
                var model = new Dictionary<string, object> { { "UserCarMillage", Convert.ToInt32(millage.Text) }, { "Amount", Convert.ToInt32(amount.Text) }, { "Cost", Convert.ToInt32(cost.Text) }, { "FillTime", DateTime.Now }, };
                var response = await ClientAPI.PostAsync($"Garage/{carViewModel["Id"]}/GasFill", model);
                if (response == null)
                    return;
                millage.Text = string.Empty;
                cost.Text = string.Empty;
                amount.Text = string.Empty;
                amount.ClearFocus();
            };
            carLeftBtn.Click += (s, e) =>
            {
                if (selectedCarIndex == 0)
                {
                    //carViewModel = DataPreferences.Instance.UserGarage[DataPreferences.Instance.UserGarage.Count - 1];
                    //selectedCarIndex = DataPreferences.Instance.UserGarage.Count - 1;
                }
                else
                {
                    selectedCarIndex--;
                    //carViewModel = DataPreferences.Instance.UserGarage[selectedCarIndex];
                }
                if (selectedCarIndex == 1)
                    carAva.SetImageDrawable(ContextCompat.GetDrawable(this.Context, Resource.Drawable.audi));
                else carAva.SetImageDrawable(ContextCompat.GetDrawable(this.Context, Resource.Drawable.avacar));
                carName.Text = carViewModel["Model"].ToString();
            };
            carRightBtn.Click += (s, e) =>
            {
                //if (selectedCarIndex == DataPreferences.Instance.UserGarage.Count - 1)
                //{
                //    carViewModel = DataPreferences.Instance.UserGarage[0];
                //    selectedCarIndex = 0;
                //}
                //else
                //{
                //    selectedCarIndex++;
                //    carViewModel = DataPreferences.Instance.UserGarage[selectedCarIndex];
                //}
                if (selectedCarIndex == 1)
                    carAva.SetImageDrawable(ContextCompat.GetDrawable(this.Context, Resource.Drawable.audi));
                else carAva.SetImageDrawable(ContextCompat.GetDrawable(this.Context, Resource.Drawable.avacar));
                carName.Text = carViewModel["Model"].ToString();
            };
        }

        private void GetElements(View view)
        {
            carAva = view.FindViewById<ImageView>(Resource.Id.fillfuel_car_ava);
            carLeftBtn = view.FindViewById<ImageView>(Resource.Id.fillfuel_car_left);
            carRightBtn = view.FindViewById<ImageView>(Resource.Id.fillfuel_car_right);
            carName = view.FindViewById<TextView>(Resource.Id.fillfuel_car_name);
            millage = view.FindViewById<EditTextClearBtn>(Resource.Id.fillfuel_millage);
            cost = view.FindViewById<EditTextClearBtn>(Resource.Id.fillfuel_cost);
            amount = view.FindViewById<EditTextClearBtn>(Resource.Id.fillfuel_amount);
            completeBtn = view.FindViewById<Button>(Resource.Id.fillfuel_complete_btn);
        }
        #endregion

        #region Methods
        private void InitData()
        {
            (Activity as MenuActivity).SupportActionBar.Title = "Заправка автомобиля";
            //carViewModel = DataPreferences.Instance.UserGarage[0];
            carName.Text = carViewModel["Model"].ToString();
        }
        #endregion
    }
}