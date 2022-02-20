using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using MudBlazor;
using QLess.Core.Domain;
using QLess.Web.Interfaces;
using QLess.Web.Models;

namespace QLess.Web.Pages
{
    public partial class CreateCard : ComponentBase
    {
        [Inject]
        public NavigationManager Navigation { get; set; }

        [Inject]
        public ICardClientService CardClientService { get; set; }

        private CreateCardRequest _model = new();
        private CreateCardResponse _apiResponse = new();
        private bool _isBusy = false;
        private bool _showAlert = false;
        private bool _showCardNumber = false;
        private string _message = string.Empty;
        
        private async Task SubmitForm()
        {
            _isBusy = true;
            _showAlert = false;
            _showCardNumber = false;
            _apiResponse = await CardClientService.CreateCard(_model);
            
            if (string.IsNullOrEmpty(_apiResponse.CardNumber))
            {
                _showAlert = true;
                _message = _apiResponse.ErrorMessage;
            }
            else
            {
                _showCardNumber = true;
            }

            _isBusy = false;
        }

        private void GoToMainMenu()
        {
            Navigation.NavigateTo("/");
        }
    }
}