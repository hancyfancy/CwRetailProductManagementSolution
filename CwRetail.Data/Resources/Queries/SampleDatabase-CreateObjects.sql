-- create schemas
CREATE SCHEMA production
GO

-- create tables
CREATE TABLE production.products (
	ProductId BIGINT IDENTITY (1, 1) PRIMARY KEY,
	Name NVARCHAR (100) NOT NULL CHECK (LEN(Name) > 0),
	Price DECIMAL (18,2) NOT NULL CHECK (Price > 0),
	Type NVARCHAR (50) NOT NULL CHECK (Type = 'Toys' OR Type = 'Food' OR Type = 'Electronics' OR Type = 'Furniture' OR Type = 'Books'),
	Active BIT NOT NULL
)
GO

-- create stored procedures
CREATE PROCEDURE production.products_sp
	@TypeEnum SMALLINT = NULL
AS 
BEGIN
	INSERT INTO production.products
	(                    
		Name,
		Price,
		Type,
		Active
	) 
	VALUES 
	( 
		CONVERT(NVARCHAR(100), NEWID()),
		CONVERT(DECIMAL(18, 2), 5 + (2000-5)*RAND(CHECKSUM(NEWID()))),
		CASE @TypeEnum WHEN 1 THEN 'Books' WHEN 2 THEN 'Electronics' WHEN 3 THEN 'Food' WHEN 4 THEN 'Furniture' WHEN 5 THEN 'Toys' ELSE 'NA' END,
		ABS(CHECKSUM(NEWID())) % 2
	)
END
GO