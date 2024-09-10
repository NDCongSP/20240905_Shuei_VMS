using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Security.Claims;
using System;
using Radzen.Blazor;

namespace WebUI.Layout
{
    public partial class MainLayout
    {
        bool _sidebarExpanded = false;
        protected override async Task OnInitializedAsync()
        {

        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {

            }

            base.OnAfterRender(firstRender);
        }

        void OnParentClicked(RadzenProfileMenuItem args)
        {
            if (args.Text == "Logout")
                _authenServices.LogoutAsync();
        }

        public void Dispose()
        {
        }
    }
}
