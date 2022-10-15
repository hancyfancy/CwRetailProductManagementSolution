-- create schemas
CREATE SCHEMA audit
GO

-- create tables
CREATE TABLE audit.products (
	Id BIGINT IDENTITY (1, 1) PRIMARY KEY,
	EventType NVARCHAR (250) NOT NULL CHECK (LEN(EventType) > 0),
	LoginName NVARCHAR (250) NOT NULL CHECK (LEN(LoginName) > 0),
	SqlCommand NVARCHAR (MAX) NOT NULL CHECK (LEN(SqlCommand) > 0),
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
	(EventType, LoginName, SqlCommand, AuditDateTime)
	VALUES
	(
	@EventData.value('(/EVENT_INSTANCE/EventType)[1]', 'NVARCHAR (250)'),
	@EventData.value('(/EVENT_INSTANCE/LoginName)[1]', 'NVARCHAR (250)'),
	@EventData.value('(/EVENT_INSTANCE/TSQLCommand)[1]', 'NVARCHAR (MAX)'),
	GETDATE()
	) 
END
GO

CREATE TRIGGER production.products_tr_test
ON CwRetail.production.products
AFTER DELETE
AS
BEGIN
	SET NOCOUNT ON
	SELECT Id, Name, Price, Type, Active from deleted
END
GO
