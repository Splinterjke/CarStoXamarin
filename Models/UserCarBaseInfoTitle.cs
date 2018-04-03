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
using XamDroid.ExpandableRecyclerView;

namespace CarSto.Models
{
	public class UserCarBaseInfoTitle : IParentObject
	{
		private Guid id;
		public Guid Id { get => id; set => id = value; }

		public UserCarBaseInfoTitle()
		{
			id = Guid.NewGuid();
		}

		public string Title { get; set; }

		private List<object> childObjectList;
		public List<object> ChildObjectList { get => childObjectList; set => childObjectList = value; }
	}
}