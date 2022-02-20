using Microsoft.AspNetCore.Components;
using QLess.Core.Domain;
using QLess.Web.Interfaces;
using QLess.Web.Models;

namespace QLess.Web.Pages
{
	public partial class LoadCard
    {
        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        public IQLessClientService QLessClientService { get; set; }

        private CardLoadRequest _model = new();
        private CardLoadResponse _apiResponse = new();
        private bool _isBusy = false;
        private bool _showAlert = false;
        private bool _showReceipt = false;
        private string _message = string.Empty;
        
        private async Task SubmitForm()
        {
            _isBusy = true;
            _showAlert = false;
            _showReceipt = false;
            _apiResponse = await QLessClientService.LoadCard(_model);

            if (_apiResponse.Succeeded)
            {
                _showReceipt = true;
			}
            else
            {
                _showAlert = true;
                _message = _apiResponse.Message;
			}

            _isBusy = false;
        }

        private void GoToMainMenu()
        {
            Navigation.NavigateTo("/");
        }
    }
}