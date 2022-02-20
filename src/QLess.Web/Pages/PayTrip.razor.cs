using Microsoft.AspNetCore.Components;
using QLess.Core.Domain;
using QLess.Web.Interfaces;

namespace QLess.Web.Pages
{
	public partial class PayTrip
    {
        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        public IQLessClientService CardClientService { get; set; }

        private TripPaymentResponse _apiResponse = new();
        private string _cardNumber = string.Empty;
        private string _message = string.Empty;
        private bool _isBusy = false;
        private bool _showAlert = false;
        private bool _showReceipt = false;

        private async Task Submit()
        {
            _isBusy = true;
            _showAlert = false;
            _showReceipt = false;
            _apiResponse = await CardClientService.PayTrip(_cardNumber);
            
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