using QLess.Core.Domain;
using QLess.Core.Enums;
using QLess.Web.Interfaces;
using QLess.Web.Models;
using System.Net.Http.Json;

namespace QLess.Web.Services
{
	public class CardClientService : ICardClientService
	{
		private readonly HttpClient _client;

		public CardClientService(HttpClient client)
		{
			_client = client;
		}

		public async Task<CreateCardResponse> CreateCard(CreateCardRequest createCardRequest)
		{
			var response = await _client.PostAsJsonAsync<CreateCardRequest>("/api/card/create", createCardRequest);

			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadFromJsonAsync<CreateCardResponse>();
				return result;
			}
			else
				throw new Exception("Failed to get API response");
		}
	}
}
