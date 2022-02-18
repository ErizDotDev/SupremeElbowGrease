﻿using QLess.Core.Domain;

namespace QLess.Infrastructure.Processors
{
	public abstract class BaseCardTransactionProcessor
	{
		public virtual CreateCardResponse TryCreateCardNumber(decimal initialBalance, string specialIdNumber = "")
			=> new CreateCardResponse();

		internal string GenerateCardNumber()
			=> DateTime.Now.ToString("yyddMMhhmmss");
	}
}
