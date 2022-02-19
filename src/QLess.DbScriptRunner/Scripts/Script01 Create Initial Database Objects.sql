CREATE TABLE [CardType]
(
	[Id] INT PRIMARY KEY,
	[Description] NVARCHAR(50) NOT NULL
)

INSERT INTO [CardType] VALUES(1, 'Regular')
INSERT INTO [CardType] VALUES(2, 'Discounted')

CREATE TABLE [CardDetail]
(
	[Id] BIGINT PRIMARY KEY IdENTITY,
	[CardTypeId] INT FOREIGN KEY REFERENCES [CardType]([Id]),
	[CardNumber] NVARCHAR(15) NOT NULL,
	[SpecialIdNumber] NVARCHAR(12) NULL,
	[Balance] DECIMAL NOT NULL,
	[DateLastUsed] DATETIME NULL
)

CREATE TABLE [TransactionType]
(
	[Id] INT PRIMARY KEY,
	[Description] NVARCHAR(50)
)

INSERT INTO [TransactionType] VALUES(1, 'Initial Load')
INSERT INTO [TransactionType] VALUES(2, 'Pay Trip')
INSERT INTO [TransactionType] VALUES(3, 'Reload Card')

CREATE TABLE [Transaction]
(
	[Id] BIGINT PRIMARY KEY IDENTITY,
	[CardId] BIGINT FOREIGN KEY REFERENCES [Card]([Id]),
	[TransactionDate] DATETIME NOT NULL,
	[TransactionTypeId] INT FOREIGN KEY REFERENCES [TransactionType]([Id]),
	[TransactionAmount] DECIMAL NOT NULL,
	[PreviousBalance] DECIMAL NOT NULL,
	[NewBalance] DECIMAL NOT NULL
)