﻿using QLess.Core.Domain;
using QLess.Core.Enums;

namespace QLess.Core.Interface
{
	public interface ICardService
	{
		Task<CreateCardResponse> CreateCard(CardType cardType, decimal initialBalance, string specialIdNumber = "");
	}
}
