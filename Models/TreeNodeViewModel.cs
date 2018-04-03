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
    public class TreeNodeViewModel : IParentObject
    {
        private Guid id;
        public Guid Id { get => id; set => id = value; }

        public TreeNodeViewModel()
        {
            id = Guid.NewGuid();
        }

        public string Title { get; set; }

        private List<object> childObjectList;
        public List<object> ChildObjectList { get => childObjectList; set => childObjectList = value; }
    }
}