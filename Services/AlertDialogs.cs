// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
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
using Android.Support.Design.Widget;

namespace CarSto.Services
{
	public static class AlertDialogs
	{
		public static Dialog SimpleAlertDialog(string message, Context context)
		{
			//set alert for executing the task
			Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(context, Resource.Style.DialogNoBorder);
            alert.SetMessage(message);
			alert.SetPositiveButton("ÎÊ", (senderAlert, args) => { alert.Dispose(); });
			Dialog dialog = alert.Create();
            dialog.SetCancelable(false);
			return dialog;
		}

		public static void SingleButtonSnackBar(string strText, string actionText, bool isLengthIndefinite, View view)
		{
			Snackbar SnackBar = Snackbar.Make(view, strText, isLengthIndefinite ? Snackbar.LengthIndefinite : Snackbar.LengthShort).SetAction(actionText, (v) =>
			{
				//set action for action button
				//TODO: Action for SnackBar
			});
            //set action button text color and size
            SnackBar.SetActionTextColor(Android.Graphics.Color.Yellow);
			var txtAction = SnackBar.View.FindViewById<TextView>(Resource.Id.snackbar_action);
			txtAction.SetTextSize(Android.Util.ComplexUnitType.Dip, 14);

			//set message text color and size
			var txtMessage = SnackBar.View.FindViewById<TextView>(Resource.Id.snackbar_text);
			txtMessage.SetTextColor(Android.Graphics.Color.White);
			txtMessage.SetTextSize(Android.Util.ComplexUnitType.Dip, 13);
			SnackBar.Show();
		}
	}
}