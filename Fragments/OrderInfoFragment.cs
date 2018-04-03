// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using Android.Support.V4.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.Generic;
using Newtonsoft.Json;
using CarSto.Services;
using System;

namespace CarSto.Fragments
{
    public class OrderInfoFragment : Fragment
    {
        string orderType;
        #region Widgets
        private TextView orderInfo;
        private Button chooseStoBtn, acceptBtn, deleteBtn, chooseTimeBtn;
        #endregion

        public OrderInfoFragment(string orderType)
        {
            this.orderType = orderType;
        }

        #region Base Methods
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_orderinfo, container, false);
            GetElements(view);
            InitData();
            SetEventHandlers();
            return view;
        }
        #endregion

        #region Methods
        private void GetElements(View view)
        {
            orderInfo = view.FindViewById<TextView>(Resource.Id.order_info);
            chooseStoBtn = view.FindViewById<Button>(Resource.Id.order_choosesto_button);
            acceptBtn = view.FindViewById<Button>(Resource.Id.order_accept_button);
            deleteBtn = view.FindViewById<Button>(Resource.Id.order_delete_button);            
            chooseTimeBtn = view.FindViewById<Button>(Resource.Id.order_choosetime_button);
        }

        private void SetEventHandlers()
        {
            //if(orderType == "Preorder")
            //{
            //    return;
            //}
            //if(orderType == "Order")
            //{
            //    Fragment parent;
            //    if (this.ParentFragment is PreOrderFragment)
            //        parent = this.ParentFragment as PreOrderFragment;
            //    else parent = this.ParentFragment as OrderFragment;
            //    chooseStoBtn.Click += async delegate
            //    {
            //        var getStoResponse = await ClientAPI.GetAsync("Sto");
            //        if (getStoResponse == null)
            //            return;
            //        var stoList = JsonConvert.DeserializeObject<Dictionary<string, object>[]>(getStoResponse.Item2);
            //        var fragment = new ChooseStoFragment(stoList);
            //        //fragment.Show(this.FragmentManager, "chooseSto");
            //        //fragment.ItemClick += (s, e) =>
            //        //{
            //        //    ChangeOrderStatus($"Order/{parent.orderId}/Station/{e}");
            //        //};
            //    };
            //    acceptBtn.Click += delegate
            //    {
            //        ChangeOrderStatus($"Order/{parent.orderData["Id"]}/Accept");
            //    };
            //    deleteBtn.Click += delegate
            //    {
            //        ChangeOrderStatus($"Order/{parent.orderData["Id"]}/Del");
            //    };
            //    chooseTimeBtn.Click += async delegate
            //    {
            //        var today = DateTime.Today;
            //        var response = await ClientAPI.GetAsync("");
            //        if (response == null)
            //            return;
            //        double workLength = 0;
            //        var length = parent.laborList.Count;
            //        for (int i = 0; i < length; i++)
            //        {
            //            var workTime = Convert.ToDouble(parent.laborList[i]["Time"]);
            //            if (workTime > 0)
            //                workLength += workTime;
            //        }
            //        var schedule = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(response.Item2);
            //        //this.Activity.SupportFragmentManager.BeginTransaction().AddToBackStack(null).Replace(Resource.Id.detail_content, new ChooseOrderTimeFragment(parent.orderId, schedule, Convert.ToInt32(parent.stoData["LineCount"]), (DateTime)parent.stoData["WorkStartTime"], (DateTime)parent.stoData["WorkStopTime"], Convert.ToInt32(workLength * 60))).Commit();
            //    };
            //}            
        }

        private void ChangeOrderStatus(string link)
        {
            string orderResponse = string.Empty;
            if (link.Contains("Del"))
            {
                orderResponse = "1";// await ClientAPI.DeleteAsync(link);
                this.Activity.SupportFragmentManager.PopBackStack();
                return;
            }
            else orderResponse = "1";// await ClientAPI.PutAsync(link, null);
            if (orderResponse == null)
                return;
            if (ParentFragment is PreOrderFragment)
                (this.ParentFragment as PreOrderFragment).OrderDataUpdated(orderResponse);
        }

        private void InitData()
        {
            try
            {
                if (ParentFragment is PreOrderFragment)
                {
                    var parent = this.ParentFragment as PreOrderFragment;
                }
                else
                {
                    //var parent = this.ParentFragment as OrderFragment;
                    //acceptBtn.Visibility = (bool)parent["CanAccept"] ? ViewStates.Visible : ViewStates.Gone;
                    //deleteBtn.Visibility = (bool)parent["CanDelete"] ? ViewStates.Visible : ViewStates.Gone;
                    //chooseTimeBtn.Visibility = (bool)parent["CanChooseTime"] ? ViewStates.Visible : ViewStates.Gone;                    

                    //string status = null;
                    //if (parent.statesData != null)
                    //{
                    //    switch (statesData["State"])
                    //    {
                    //        case "Raw":
                    //            status = "Черновик";
                    //            chooseStoBtn.Visibility = ViewStates.Visible;
                    //            break;
                    //        case "Archive":
                    //            status = "Архив";
                    //            chooseStoBtn.Visibility = ViewStates.Gone;
                    //            break;
                    //        case "Active":
                    //            var progress = statesData["Progress"].ToString() == "NO" ? string.Empty : "в работе";
                    //            string confirmation = string.Empty;
                    //            switch (statesData["Confirmation"].ToString())
                    //            {
                    //                case "W4FirstSto":
                    //                    confirmation = "ожидает первичного подтверждения СТО";
                    //                    break;
                    //                case "W4FirstUser":
                    //                    confirmation = "ожидает первичного подтверждения пользователем";
                    //                    break;
                    //                case "W4Sto":
                    //                    confirmation = "ожидает подтверждения СТО";
                    //                    break;
                    //                case "W4User":
                    //                    confirmation = "ожидает подтверждения пользователем";
                    //                    break;
                    //            }
                    //            status = $"Активный {progress} {confirmation}";
                    //            chooseStoBtn.Visibility = ViewStates.Gone;
                    //            break;
                    //        case "Recommendation":
                    //            status = "Рекоммендация";
                    //            chooseStoBtn.Visibility = ViewStates.Gone;
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //    //status = $"{status} {statesData["Progress"]} {statesData["Confirmation"]}".Trim(' ');
                    //}
                    //.Text = (status != null) ? $"Статус: {status}" : "NULL";
                    //var totalPartCost = 0;
                    //var totalLaborCost = 0d;
                    //if (parent.partList != null)
                    //    for (int i = 0; i < parent.partList.Count; i++)
                    //    {
                    //        totalPartCost += Convert.ToInt32(parent.partList[i]["Price"]);
                    //    }
                    //if (parent.laborList != null && parent.stoData != null)
                    //    for (int i = 0; i < parent.laborList.Count; i++)
                    //    {
                    //        totalLaborCost += Convert.ToDouble(parent.laborList[i]["Time"]) * Convert.ToDouble(parent.stoData["StdCost"]);
                    //    }
                    //partCost.Text = $"Стоимость деталей: {totalPartCost} руб.";
                    //laborCost.Text = $"Стоимость работ: {totalLaborCost} руб.";
                }
            }
            catch (JsonSerializationException ex)
            {
                AlertDialogs.SimpleAlertDialog(ex.ToString(), this.Context).Show();
                return;
            }
        }
        #endregion

        #region Events
        
        
        #endregion
    }
}