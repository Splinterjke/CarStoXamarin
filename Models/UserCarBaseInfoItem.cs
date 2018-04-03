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

namespace CarSto.Models
{
	public class UserCarBaseInfoItem
	{
		public string Title;
		public string Description;
		public UserCarBaseInfoItem(string title, string description)
		{
			Title = title;
			Description = description;
		}
	}
}