-- create schemas
CREATE SCHEMA production
GO

-- create tables
CREATE TABLE production.products (
	Id BIGINT IDENTITY (1, 1) PRIMARY KEY,
	Name NVARCHAR (100) NOT NULL,
	Price DECIMAL (18,2) NOT NULL,
	Type NVARCHAR (50) NOT NULL,
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
		CONVERT(BIT, 0 + (1-0)*RAND(CHECKSUM(NEWID()))
	)
)
END
GO