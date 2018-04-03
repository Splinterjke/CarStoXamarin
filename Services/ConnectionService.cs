// This is an independent project of an individual developer. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CarSto.Services
{
	static class ConnectionService
	{
		internal static Boolean IsConnected(Context context)
		{
			try
			{
				var connectionManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
				NetworkInfo networkInfo = connectionManager.ActiveNetworkInfo;
				if (networkInfo != null && networkInfo.IsConnected)
				{
					return true;
				}
			}
			catch (Exception ex)
			{
				return false;
			}
			return false;
		}
	}
}