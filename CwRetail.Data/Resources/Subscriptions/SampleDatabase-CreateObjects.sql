-- create schemas
CREATE SCHEMA audit
GO

-- create tables
CREATE TABLE audit.products (
	ProductAuditId BIGINT IDENTITY (1, 1) PRIMARY KEY,
	ProductId BIGINT NOT NULL,
	EventType NVARCHAR (250) NOT NULL CHECK (LEN(EventType) > 0),
	LoginName NVARCHAR (250) NOT NULL CHECK (LEN(LoginName) > 0),
	ObjJson NVARCHAR (MAX) NOT NULL CHECK (LEN(ObjJson) > 0),
	AuditDateTime DATETIME NOT NULL CHECK(CONVERT(DATE, AuditDateTime) = CONVERT(DATE, getdate())),
	CONSTRAINT FK_ProductAudit_Product FOREIGN KEY (ProductId) REFERENCES production.products(ProductId)
)
GO

-- create triggers
CREATE TRIGGER production.products_tr_delete
ON CwRetail.production.products
AFTER DELETE
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @Id BIGINT
	SELECT @Id = ProductId FROM deleted

	DECLARE @Name NVARCHAR(100)
	SELECT @Name = Name FROM deleted

	DECLARE @Price DECIMAL(18,2)
	SELECT @Price = Price FROM deleted

	DECLARE @Type NVARCHAR(50)
	SELECT @Type = Type FROM deleted

	DECLARE @Active BIT
	SELECT @Active = Active FROM deleted

	INSERT INTO CwRetail.audit.products 
	(ProductId, EventType, LoginName, ObjJson, AuditDateTime)
	VALUES
	(
	@Id,
	'DELETE',
	CONVERT(NVARCHAR(250), CURRENT_USER),
	'{ "Name" : "' + @Name + '", "Price" : ' + CAST(@Price AS NVARCHAR(max)) + ', "Type" : "' + @Type + '", "Active" : ' + CAST(@Active AS NVARCHAR(max)) + ' }',
	GETDATE()
	) 
END
GO

CREATE TRIGGER production.products_tr_insert
ON CwRetail.production.products
AFTER INSERT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @Id BIGINT
	SELECT @Id = ProductId FROM inserted

	DECLARE @Name NVARCHAR(100)
	SELECT @Name = Name FROM inserted

	DECLARE @Price DECIMAL(18,2)
	SELECT @Price = Price FROM inserted

	DECLARE @Type NVARCHAR(50)
	SELECT @Type = Type FROM inserted

	DECLARE @Active BIT
	SELECT @Active = Active FROM inserted

	INSERT INTO CwRetail.audit.products 
	(ProductId, EventType, LoginName, ObjJson, AuditDateTime)
	VALUES
	(
	@Id,
	'INSERT',
	CONVERT(NVARCHAR(250), CURRENT_USER),
	'{ "Name" : "' + @Name + '", "Price" : ' + CAST(@Price AS NVARCHAR(max)) + ', "Type" : "' + @Type + '", "Active" : ' + CAST(@Active AS NVARCHAR(max)) + ' }',
	GETDATE()
	) 
END
GO

CREATE TRIGGER production.products_tr_update
ON CwRetail.production.products
AFTER UPDATE
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @DeletedId BIGINT
	SELECT @DeletedId = ProductId FROM deleted

	DECLARE @DeletedName NVARCHAR(100)
	SELECT @DeletedName = Name FROM deleted

	DECLARE @DeletedPrice DECIMAL(18,2)
	SELECT @DeletedPrice = Price FROM deleted

	DECLARE @DeletedType NVARCHAR(50)
	SELECT @DeletedType = Type FROM deleted

	DECLARE @DeletedActive BIT
	SELECT @DeletedActive = Active FROM deleted

	DECLARE @InsertedId BIGINT
	SELECT @InsertedId = ProductId FROM inserted

	DECLARE @InsertedName NVARCHAR(100)
	SELECT @InsertedName = Name FROM inserted

	DECLARE @InsertedPrice DECIMAL(18,2)
	SELECT @InsertedPrice = Price FROM inserted

	DECLARE @InsertedType NVARCHAR(50)
	SELECT @InsertedType = Type FROM inserted

	DECLARE @InsertedActive BIT
	SELECT @InsertedActive = Active FROM inserted

	INSERT INTO CwRetail.audit.products 
	(ProductId, EventType, LoginName, ObjJson, AuditDateTime)
	VALUES
	(
	@InsertedId,
	'UPDATE',
	CONVERT(NVARCHAR(250), CURRENT_USER),
	'[{ "Name" : "' + @DeletedName + '", "Price" : ' + CAST(@DeletedPrice AS NVARCHAR(max)) + ', "Type" : "' + @DeletedType + '", "Active" : ' + CAST(@DeletedActive AS NVARCHAR(max)) + ' },{ "Name" : "' + @InsertedName + '", "Price" : ' + CAST(@InsertedPrice AS NVARCHAR(max)) + ', "Type" : "' + @InsertedType + '", "Active" : ' + CAST(@InsertedActive AS NVARCHAR(max)) + ' }]',
	GETDATE()
	) 
END
GO