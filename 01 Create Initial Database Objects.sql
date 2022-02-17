CREATE TABLE [card_types]
(
	[id] INT PRIMARY KEY IDENTITY,
	[description] NVARCHAR(50) NOT NULL
)

INSERT INTO [card_types] VALUES(1, 'Regular')
INSERT INTO [card_types] VALUES(2, 'Discounted')

CREATE TABLE [cards]
(
	[id] BIGINT PRIMARY KEY IDENTITY,
	[card_type] INT FOREIGN KEY REFERENCES [card_types]([id]),
	[card_number] NVARCHAR(15) NOT NULL,
	[special_id_number] NVARCHAR(12) NULL,
	[balance] DECIMAL NOT NULL,
	[date_last_used] DATETIME
)