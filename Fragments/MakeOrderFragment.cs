// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using CarSto.CustomControls;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CarSto.Services;
using CarSto.Activities;
using Android.Support.V4.Content;

namespace CarSto.Fragments
{
    public class MakeOrderFragment : Fragment
    {
        Dictionary<string, object> carViewModel;
        int selectedCarIndex;
        #region Widgets
        Button diagnosticsBtn, mechanicalBtn, bodypaintBtn, extrasBtn, wheelalignBtn, commentBtn, completeBtn;
        TextInputLayout diagnosticsLayout, mechanicalLayout, bodypaintLayout, extrasLayout, wheelalignLayout, commentLayout;
        EditTextClearBtn diagnosticsText, mechanicalText, bodypaintText, extrasText, wheelalignText, commentText;
        TextView carName;
        ImageView carLeftBtn, carRightBtn, carAva;
        #endregion

        #region Base Methods
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_makeorder, null);
            ToolbarHelper.SetToolbarStyle("Создание заказ-наряда", false);
            GetElements(view);
            SetEventHandlers();
            InitData();
            return view;
        }

        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    switch (item.ItemId)
        //    {
        //        case Android.Resource.Id.Home:
        //            Activity.SupportFragmentManager.BeginTransaction().Remove(this).Commit();
        //            return true;
        //        default:
        //            return base.OnOptionsItemSelected(item); ;
        //    }
        //}
        #endregion

        #region Methods
        private void GetElements(View view)
        {
            diagnosticsBtn = view.FindViewById<Button>(Resource.Id.makeorder_diagnostics_btn);
            mechanicalBtn = view.FindViewById<Button>(Resource.Id.makeorder_mechanical_btn);
            bodypaintBtn = view.FindViewById<Button>(Resource.Id.makeorder_bodypaint_btn);
            extrasBtn = view.FindViewById<Button>(Resource.Id.makeorder_extras_btn);
            wheelalignBtn = view.FindViewById<Button>(Resource.Id.makeorder_wheelalign_btn);
            commentBtn = view.FindViewById<Button>(Resource.Id.makeorder_comment_btn);
            completeBtn = view.FindViewById<Button>(Resource.Id.makeorder_complete_btn);
            diagnosticsLayout = view.FindViewById<TextInputLayout>(Resource.Id.makeorder_input_layout_diagnostics);
            mechanicalLayout = view.FindViewById<TextInputLayout>(Resource.Id.makeorder_input_layout_mechanical);
            bodypaintLayout = view.FindViewById<TextInputLayout>(Resource.Id.makeorder_input_layout_bodypaint);
            extrasLayout = view.FindViewById<TextInputLayout>(Resource.Id.makeorder_input_layout_extras);
            wheelalignLayout = view.FindViewById<TextInputLayout>(Resource.Id.makeorder_input_layout_wheelalign);
            commentLayout = view.FindViewById<TextInputLayout>(Resource.Id.makeorder_input_layout_comment);
            diagnosticsText = view.FindViewById<EditTextClearBtn>(Resource.Id.makeorder_diagnostics_text);
            mechanicalText = view.FindViewById<EditTextClearBtn>(Resource.Id.makeorder_mechanical_text);
            bodypaintText = view.FindViewById<EditTextClearBtn>(Resource.Id.makeorder_bodypaint_text);
            extrasText = view.FindViewById<EditTextClearBtn>(Resource.Id.makeorder_extras_text);
            wheelalignText = view.FindViewById<EditTextClearBtn>(Resource.Id.makeorder_wheelalign_text);
            commentText = view.FindViewById<EditTextClearBtn>(Resource.Id.makeorder_comment_text);
            carName = view.FindViewById<TextView>(Resource.Id.makeorder_car_name);
            carLeftBtn = view.FindViewById<ImageView>(Resource.Id.makeorder_car_left);
            carRightBtn = view.FindViewById<ImageView>(Resource.Id.makeorder_car_right);
            carAva = view.FindViewById<ImageView>(Resource.Id.makerorder_car_ava);
        }

        private void SetEventHandlers()
        {
            diagnosticsBtn.Click += (s, e) =>
            {
                OnTakeFocusVisibilityState(s, diagnosticsText);
            };
            mechanicalBtn.Click += (s, e) =>
            {
                this.Activity.SupportFragmentManager.BeginTransaction().AddToBackStack("MechanicalOrderFragment").Replace(Resource.Id.detail_content, new MechanicalCategoryFragment("114")).Commit();
                //OnTakeFocusVisibilityState(s, mechanicalText);
            };
            bodypaintBtn.Click += (s, e) =>
            {
                OnTakeFocusVisibilityState(s, bodypaintText);
            };
            extrasBtn.Click += (s, e) =>
            {
                OnTakeFocusVisibilityState(s, extrasText);
            };
            wheelalignBtn.Click += (s, e) =>
            {
                OnTakeFocusVisibilityState(s, wheelalignText);
            };
            commentBtn.Click += (s, e) =>
            {
                OnTakeFocusVisibilityState(s, commentText);
            };
            completeBtn.Click += CompleteMakeOrder;
            diagnosticsText.FocusChange += (s, e) =>
            {
                OnLostFocusVisibiltyState(diagnosticsBtn, diagnosticsText);
            };
            mechanicalText.FocusChange += (s, e) =>
            {
                OnLostFocusVisibiltyState(mechanicalBtn, mechanicalText);
            };
            bodypaintText.FocusChange += (s, e) =>
            {
                OnLostFocusVisibiltyState(bodypaintBtn, bodypaintText);
            };
            extrasText.FocusChange += (s, e) =>
            {
                OnLostFocusVisibiltyState(extrasBtn, extrasText);
            };
            wheelalignText.FocusChange += (s, e) =>
            {
                OnLostFocusVisibiltyState(wheelalignBtn, wheelalignText);
            };
            commentText.FocusChange += (s, e) =>
            {
                OnLostFocusVisibiltyState(commentBtn, commentText);
            };
            diagnosticsText.TextChanged += Category_TextChanged;
            mechanicalText.TextChanged += Category_TextChanged;
            bodypaintText.TextChanged += Category_TextChanged;
            extrasText.TextChanged += Category_TextChanged;
            wheelalignText.TextChanged += Category_TextChanged;
            commentText.TextChanged += Category_TextChanged;
            carLeftBtn.Click += (s, e) =>
            {
                //if (selectedCarIndex == 0)
                //{
                //    carViewModel = DataPreferences.Instance.UserGarage[DataPreferences.Instance.UserGarage.Count - 1];
                //    selectedCarIndex = DataPreferences.Instance.UserGarage.Count - 1;
                //}
                //else
                //{
                //    selectedCarIndex--;
                //    carViewModel = DataPreferences.Instance.UserGarage[selectedCarIndex];
                //}
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

        private void CompleteMakeOrder(object sender, EventArgs e)
        {
            return;
            //List<string[]> ReasonsAndComms = new List<string[]>();
            //if (!string.IsNullOrEmpty(diagnosticsText.Text))
            //    ReasonsAndComms.Add(new string[2] { "Диагностика", diagnosticsText.Text });
            //if (!string.IsNullOrEmpty(mechanicalText.Text))
            //    ReasonsAndComms.Add(new string[2] { "Слесарные работы", mechanicalText.Text });
            //if (!string.IsNullOrEmpty(bodypaintText.Text))
            //    ReasonsAndComms.Add(new string[2] { "Малярно-кузовные", bodypaintText.Text });
            //if (!string.IsNullOrEmpty(extrasText.Text))
            //    ReasonsAndComms.Add(new string[2] { "Доп. оборудование", extrasText.Text });
            //if (!string.IsNullOrEmpty(wheelalignText.Text))
            //    ReasonsAndComms.Add(new string[2] { "ШМ, Р/С, кондиционер", wheelalignText.Text });
            //if (!string.IsNullOrEmpty(commentText.Text))
            //    ReasonsAndComms.Add(new string[2] { "Общий комментарий", commentText.Text });
            //var model = new Dictionary<string, object>
            //{
            //    { "ReasonsAndComms", ReasonsAndComms },
            //    { "UserCarId", carViewModel["Id"] }
            //};
            //var response = await ClientAPI.PostAsync("Order", model);
            //if (response == null)
            //    return;
            //var orderData = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Item2);
            //if (DataPreferences.Instance.selectedParts != null && DataPreferences.Instance.selectedParts.Count > 0)
            //    for (int i = 0; i < DataPreferences.Instance.selectedParts.Count; i++)
            //    {
            //        response = await ClientAPI.PostAsync($"Order/{orderData["Id"]}/Part", DataPreferences.Instance.selectedParts[i]);
            //        if (response == null)
            //            continue;


            //    }
            //if (DataPreferences.Instance.selectedLabors != null && DataPreferences.Instance.selectedLabors.Count > 0)
            //    for (int i = 0; i < DataPreferences.Instance.selectedLabors.Count; i++)
            //    {
            //        response = await ClientAPI.PostAsync($"Order/{orderData["Id"]}/Labor", DataPreferences.Instance.selectedLabors[i]);
            //        if (response == null)
            //            continue;
            //    }
            //DataPreferences.Instance.selectedParts?.Clear();
            //DataPreferences.Instance.selectedLabors?.Clear();
            //var succesDialog = AlertDialogs.SimpleAlertDialog("Заказ наряд успешно создан.", this.Context);
            //succesDialog.DismissEvent += delegate
            //{
            //    //this.ChildFragmentManager.BeginTransaction().Replace(Resource.Id.detail_content, new OrderFragment()).Commit();
            //};
            //succesDialog.Show();
        }

        private void CheckCompleteBtnState()
        {
            if ((DataPreferences.Instance.selectedParts != null && DataPreferences.Instance.selectedParts.Count != 0) || (DataPreferences.Instance.selectedLabors != null && DataPreferences.Instance.selectedLabors.Count != 0))
            {
                completeBtn.Visibility = ViewStates.Visible;
                return;
            }
            if (!string.IsNullOrEmpty(diagnosticsText.Text) || !string.IsNullOrEmpty(mechanicalText.Text) || !string.IsNullOrEmpty(bodypaintText.Text) || !string.IsNullOrEmpty(extrasText.Text) || !string.IsNullOrEmpty(wheelalignText.Text))
                completeBtn.Visibility = ViewStates.Visible;
            else completeBtn.Visibility = ViewStates.Gone;
        }

        private void OnTakeFocusVisibilityState(object button, object edittext)
        {
            if (button is Button && edittext is EditTextClearBtn)
            {
                (button as Button).Visibility = ViewStates.Gone;
                (edittext as EditTextClearBtn).Visibility = ViewStates.Visible;
                (edittext as EditTextClearBtn).RequestFocus();
            }
        }

        private void OnLostFocusVisibiltyState(object button, object edittext)
        {
            if (button is Button && edittext is EditTextClearBtn)
            {
                var text = (edittext as EditTextClearBtn).Text?.Trim(' ');
                if (!(edittext as EditTextClearBtn).HasFocus && string.IsNullOrEmpty(text))
                {
                    (edittext as EditTextClearBtn).Visibility = ViewStates.Gone;
                    (button as Button).Visibility = ViewStates.Visible;
                }
            }
        }

        private void InitData()
        {
            try
            {
                //carViewModel = DataPreferences.Instance.UserGarage[0];
                carName.Text = "New Passat 2012";//carViewModel["Model"].ToString();
                if ((DataPreferences.Instance.selectedParts != null && DataPreferences.Instance.selectedParts.Count != 0) || (DataPreferences.Instance.selectedLabors != null && DataPreferences.Instance.selectedLabors.Count != 0))
                    completeBtn.Visibility = ViewStates.Visible;
                else completeBtn.Visibility = ViewStates.Gone;
            }
            catch (JsonSerializationException ex)
            {
                AlertDialogs.SimpleAlertDialog(ex.ToString(), this.Context).Show();
                return;
            }
        }
        #endregion

        #region Events
        private void Category_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            if (e.Text.ToString() == " ")
                (sender as EditTextClearBtn).Text = string.Empty;
            CheckCompleteBtnState();
        }
        #endregion
    }
}