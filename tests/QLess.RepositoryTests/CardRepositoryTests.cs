using Microsoft.Extensions.Configuration;
using QLess.Core.Domain;
using QLess.Core.Enums;
using QLess.Core.Interface;
using QLess.DbScriptRunner;
using QLess.Infrastructure.Data;
using System.Threading.Tasks;
using Xunit;

namespace QLess.RepositoryTests
{
	public class CardRepositoryTests
	{
		private readonly ICardRepository _cardDetailRepository;
		private readonly Migrations _scriptRunner;

		public CardRepositoryTests()
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.Build();

			_cardDetailRepository = new CardRepository(config);
			_scriptRunner = new Migrations(config);

			ResetDatabase();
		}

		[Fact]
		public async Task FindByCardNumber_ReturnsCorrectObject_GivenValidCardNumber()
		{
			ResetDatabase();

			var input = new Card
			{
				CardTypeId = (int)CardType.Regular,
				CardNumber = "221802083101",
				Balance = 100m,
			};

			await _cardDetailRepository.CreateAsync(input);

			var result = _cardDetailRepository.FindByCardNumber("221802083101");

			Assert.NotNull(result);
		}

		[Fact]
		public async Task FindByCardNumber_ReturnsNullOrEmptyObject_GivenValidCardNumber()
		{
			ResetDatabase();

			var input = new Card
			{
				CardTypeId = (int)CardType.Regular,
				CardNumber = "221802083101",
				Balance = 100m,
			};

			await _cardDetailRepository.CreateAsync(input);

			var result = _cardDetailRepository.FindByCardNumber("221802083102");

			Assert.Null(result);

		}

		private void ResetDatabase()
		{
			_scriptRunner.RunReset();
			_scriptRunner.RunScripts();
		}
	}
}
