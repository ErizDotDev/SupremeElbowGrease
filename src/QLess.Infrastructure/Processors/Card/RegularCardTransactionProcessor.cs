using QLess.Core.Domain;

namespace QLess.Infrastructure.Processors
{
	public class RegularCardTransactionProcessor : BaseCardTransactionProcessor
	{
		public override CreateCardResponse CreateCardDetails(decimal initialBalance, string specialIdNumber = "")
		{
			const int minimumRegularCardTypeLoad = 100;

			if (initialBalance < minimumRegularCardTypeLoad)
			{
				return new CreateCardResponse
				{
					CardNumber = string.Empty,
					ErrorMessage = $"Minimum initial load balance not reached. Please load your card with at least P{minimumRegularCardTypeLoad}.00"
				};
			}

			string cardNumber = GenerateCardNumber();

			return new CreateCardResponse { CardNumber = cardNumber };
		}
	}
}
