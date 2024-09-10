using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace WebUI.Layout
{
    public partial class NavMenu
    {
		[Parameter]
		public bool sidebarExpanded { get; set; } = true;

		void OnParentClicked(MenuItemEventArgs args)
		{
			GlobalVariable.BreadCrumbData = null;
			GlobalVariable.BreadCrumbData = new List<BreadCrumbModel>();
			GlobalVariable.BreadCrumbData.Add(new BreadCrumbModel()
			{
				Text = args.Text,
				Path = args.Path
			});
		}

		void OnChildClicked(MenuItemEventArgs args)
		{
			GlobalVariable.BreadCrumbData = null;
			GlobalVariable.BreadCrumbData = new List<BreadCrumbModel>();

			GlobalVariable.BreadCrumbData.Add(new BreadCrumbModel()
			{
				Text = "Orders",
			});

			GlobalVariable.BreadCrumbData.Add(new BreadCrumbModel()
			{
				Text = args.Text,
				Path = args.Path
			});
		}
	}
}
