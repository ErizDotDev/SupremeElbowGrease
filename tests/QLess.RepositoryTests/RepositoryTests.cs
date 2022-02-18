using Microsoft.Extensions.Configuration;
using QLess.Core.Domain;
using QLess.Core.Enums;
using QLess.Core.Interface;
using QLess.DbScriptRunner;
using QLess.Infrastructure.Data;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace QLess.RepositoryTests
{
	public class RepositoryTests
	{
		private readonly IRepository<Card> _cardDetailRepository;
		private readonly Migrations _scriptRunner;

		public RepositoryTests()
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.Build();

			_cardDetailRepository = new Repository<Card>(config);
			_scriptRunner = new Migrations(config);

			ResetDatabase();
		}

		[Fact]
		public async Task CreateAsync_ReturnsTrue_GivenValidData()
		{
			ResetDatabase();

			var input = new Card
			{
				CardTypeId = (int)CardType.Regular,
				CardNumber = "221802083101",
				Balance = 100m,
			};

			var result = await _cardDetailRepository.CreateAsync(input);

			Assert.True(result);
		}

		[Fact]
		public async Task CreateAsync_ReturnsFalse_GivenNullData()
		{
			ResetDatabase();

			Card input = null;

			var result = await _cardDetailRepository.CreateAsync(input);

			Assert.False(result);
		}

		[Fact]
		public void Create_ReturnsTrueAndCorrectId_GivenCreatedVersions()
		{
			ResetDatabase();

			long id = 0;

			var input = new Card
			{
				CardTypeId = (int)CardType.Regular,
				CardNumber = "221802085302",
				Balance = 100m,
			};

			_cardDetailRepository.Create(input, out id);

			var input2 = new Card
			{
				CardTypeId = (int)CardType.Discounted,
				CardNumber = "221802085302",
				SpecialIdNumber = "XXXXXXXXXX",
				Balance = 500m,
			};

			var result = _cardDetailRepository.Create(input2, out id);

			Assert.True(result);
			Assert.True(id == 2);
		}

		[Fact]
		public async Task FindByIdAsync_ReturnsCorrectObject_WithValidId()
		{
			ResetDatabase();

			var input = new Card
			{
				CardTypeId = (int)CardType.Regular,
				CardNumber = "221802083101",
				Balance = 100m,
			};

			await _cardDetailRepository.CreateAsync(input);

			var fetchObjectResult = await _cardDetailRepository.FindByIdAsync(1);

			Assert.NotNull(fetchObjectResult);
			Assert.Equal(1, fetchObjectResult.Id);
			Assert.Equal(input.CardNumber, fetchObjectResult.CardNumber);
			Assert.Equal(input.Balance, fetchObjectResult.Balance);
		}

		[Fact]
		public async Task FindByIdAsync_ReturnsNullOrEmptyObject_WithInvalidId()
		{
			ResetDatabase();

			var input = new Card
			{
				CardTypeId = (int)CardType.Regular,
				CardNumber = "221802083101",
				Balance = 100m,
			};

			await _cardDetailRepository.CreateAsync(input);

			var fetchObjectResult = await _cardDetailRepository.FindByIdAsync(2);

			Assert.Null(fetchObjectResult);
		}

		[Fact]
		public async Task FindAllAsync_ReturnsEmptyOrNull_WithoutAnyDataInDb()
		{
			ResetDatabase();

			var results = await _cardDetailRepository.FindAllAsync();

			Assert.True(results.Count() == 0);
		}

		[Fact]
		public async Task FindAllAsync_ReturnsCorrectObjects_WithDataInDb()
		{
			ResetDatabase();

			await AddTestData();

			var results = await _cardDetailRepository.FindAllAsync();

			Assert.True(results.Count() > 0);
		}

		[Fact]
		public async Task UpdateAsync_UpdateDataSuccessfully()
		{
			ResetDatabase();

			var input = new Card
			{
				CardTypeId = (int)CardType.Regular,
				CardNumber = "221802083101",
				Balance = 100m,
			};

			await _cardDetailRepository.CreateAsync(input);

			var fetchResult = await _cardDetailRepository.FindByIdAsync(1);
			fetchResult.Balance = 90m;

			var updateResult = await _cardDetailRepository.UpdateAsync(fetchResult);

			Assert.True(updateResult);
		}

		private void ResetDatabase()
		{
			_scriptRunner.RunReset();
			_scriptRunner.RunScripts();
		}

		private async Task AddTestData()
		{
			var input = new Card
			{
				CardTypeId = (int)CardType.Regular,
				CardNumber = "221802085302",
				Balance = 100m,
			};

			await _cardDetailRepository.CreateAsync(input);

			var input2 = new Card
			{
				CardTypeId = (int)CardType.Discounted,
				CardNumber = "221802085302",
				SpecialIdNumber = "XXXXXXXXXX",
				Balance = 500m,
			};

			await _cardDetailRepository.CreateAsync(input2);
		}
	}
}
