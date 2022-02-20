using QLess.Core.Domain;
using QLess.Core.Enums;
using QLess.Web.Interfaces;
using QLess.Web.Models;
using System.Net.Http.Json;

namespace QLess.Web.Services
{
	public class QLessClientService : IQLessClientService
	{
		private readonly HttpClient _client;

		public QLessClientService(HttpClient client)
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

		public async Task<TripPaymentResponse> PayTrip(string cardNumber)
		{
			var response = await _client.PostAsJsonAsync<string>("api/trip/pay", cardNumber);

			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadFromJsonAsync<TripPaymentResponse>();
				return result;
			}
			else
				throw new Exception("Failed to get API response");
		}

		public async Task<CardLoadResponse> LoadCard(CardLoadRequest cardLoadRequest)
		{
			var response = await _client.PostAsJsonAsync<CardLoadRequest>("api/card/load", cardLoadRequest);

			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadFromJsonAsync<CardLoadResponse>();
				return result;
			}
			else
				throw new Exception("Failed to get API response");
		}
	}
}
