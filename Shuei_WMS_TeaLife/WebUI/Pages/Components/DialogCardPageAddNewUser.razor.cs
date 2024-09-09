using Application.DTOs.Request.Account;
using Application.DTOs.Response.Account;
using Radzen;
using Radzen.Blazor;

namespace WebUI.Pages.Components
{
    public partial class DialogCardPageAddNewUser
    {
        private CreateAccountRequestDTO _model = new CreateAccountRequestDTO();
        List<CreateRoleRequestDTO> _roles = new List<CreateRoleRequestDTO>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var result = await _authenServices.GetRolesAsync();

            foreach (var role in result)
            {
                _roles.Add(new CreateRoleRequestDTO() { Name = role.Name });
            }
        }

        async void Submit(CreateAccountRequestDTO arg)
        {
            var confirm = await _dialogService.Confirm($"Bạn chắc chắn muốn tạo user: {arg.UserName}", "Tạo user", new ConfirmOptions()
            {
                OkButtonText = "Yes",
                CancelButtonText = "No",
                AutoFocusFirstElement = true,
            });

            if (confirm == null || confirm == false) return;

            var res = await _authenServices.CreateAccountAsync(arg);

            if (res.Flag)
            {
                _notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Success",
                    Detail = res.Message,
                    Duration = 2000
                });

                _dialogService.Close("Success");
            }
            else
            {
                _notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = res.Message,
                    Duration = 2000
                });
            }
        }
    }
}
