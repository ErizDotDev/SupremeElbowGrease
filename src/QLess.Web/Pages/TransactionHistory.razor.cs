using Microsoft.AspNetCore.Components;
using MudBlazor;
using QLess.Core.Data;
using QLess.Core.Domain;
using QLess.Web.Interfaces;

namespace QLess.Web.Pages
{
	public partial class TransactionHistory : ComponentBase
	{
		[Inject]
		public NavigationManager Navigation { get; set; }

		[Inject]
		public IQLessClientService QLessClientService { get; set; }

		private List<Transaction> _transactions = new();
		private bool _isBusy = false;
		private bool _showDataTable = false;
		private bool _showAlert = false;
		private string _cardNumber = string.Empty;

		private async Task Submit()
		{
			_isBusy = true;
			_showAlert = false;
			_showDataTable = false;
			_transactions = await QLessClientService.GetCardTransactionHistory(_cardNumber);

			if (_transactions == null || _transactions.Count == 0)
				_showAlert = true;
			else
				_showDataTable = true;

			_isBusy = false;
		}

		private string GetTransactionTypeText(int transactionTypeId)
		{
			if (transactionTypeId == TransactionType.InitialLoad.Id)
				return TransactionType.InitialLoad.Name;

			if (transactionTypeId == TransactionType.PayTrip.Id)
				return TransactionType.PayTrip.Name;

			if (transactionTypeId == TransactionType.ReloadCard.Id)
				return TransactionType.ReloadCard.Name;

			return String.Empty;
		}

		private void GoToMainMenu()
		{
			Navigation.NavigateTo("/");
		}
	}
}