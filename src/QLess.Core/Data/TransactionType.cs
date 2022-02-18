namespace QLess.Core.Data
{
	public class TransactionType
	{
		public int Id { get; private set; }
		
		public string Name { get; private set; }

		public TransactionType(int id, string name)
		{
			Id = id;
			Name = name;
		}

		public static TransactionType InitialLoad => new TransactionType(1, "Initial Load");

		public static TransactionType PayTrip => new TransactionType(1, "Pay Trip");

		public static TransactionType ReloadCard => new TransactionType(1, "Reload Card");
	}
}
