-- create schemas
CREATE SCHEMA audit
GO

-- create tables
CREATE TABLE audit.products (
	Id BIGINT IDENTITY (1, 1) PRIMARY KEY,
	EventType NVARCHAR (250) NOT NULL CHECK (LEN(EventType) > 0),
	LoginName NVARCHAR (250) NOT NULL CHECK (LEN(LoginName) > 0),
	ObjJson NVARCHAR (MAX) NOT NULL CHECK (LEN(ObjJson) > 0),
	AuditDateTime DATETIME NOT NULL CHECK(CONVERT(DATE, AuditDateTime) = CONVERT(DATE, getdate()))
)
GO

-- create triggers
CREATE TRIGGER production.products_tr
ON CwRetail.production.products
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
	DECLARE @EventData XML
	SELECT @EventData = EVENTDATA()

	INSERT INTO CwRetail.audit.products 
	(EventType, LoginName, ObjJson, AuditDateTime)
	VALUES
	(
	@EventData.value('(/EVENT_INSTANCE/EventType)[1]', 'NVARCHAR (250)'),
	@EventData.value('(/EVENT_INSTANCE/LoginName)[1]', 'NVARCHAR (250)'),
	@EventData.value('(/EVENT_INSTANCE/TSQLCommand)[1]', 'NVARCHAR (MAX)'),
	GETDATE()
	) 
END
GO

CREATE TRIGGER production.products_tr_delete
ON CwRetail.production.products
AFTER DELETE
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @DeleteId BIGINT
	SELECT @DeleteId = Id from deleted

	DECLARE @DeleteName NVARCHAR(100)
	SELECT @DeleteName = Name from deleted

	DECLARE @DeletePrice DECIMAL(18,2)
	SELECT @DeletePrice = Price from deleted

	DECLARE @DeleteType NVARCHAR(50)
	SELECT @DeleteType = Type from deleted

	DECLARE @DeleteActive BIT
	SELECT @DeleteActive = Active from deleted

	INSERT INTO CwRetail.audit.products 
	(EventType, LoginName, ObjJson, AuditDateTime)
	VALUES
	(
	'DELETE',
	CONVERT(NVARCHAR(250), CURRENT_USER),
	'{ "Id" : ' + CAST(@DeleteId AS VARCHAR(max)) + ', "Name" : "' + @DeleteName + '", "Price" : ' + CAST(@DeletePrice AS VARCHAR(max)) + ', "Type" : "' + @DeleteType + '", "Active" : ' + CAST(@DeleteActive AS VARCHAR(max)) + ' }',
	GETDATE()
	) 
END
GO
