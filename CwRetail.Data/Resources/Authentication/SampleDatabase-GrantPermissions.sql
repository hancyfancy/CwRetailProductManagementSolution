USE CwRetail

GRANT 
	SELECT, 
	INSERT, 
	UPDATE,  
	DELETE
ON 
	production.products 
TO 
	TestUser
GO

GRANT 
	SELECT,
	DELETE
ON 
	audit.products 
TO 
	TestUser
GO

GRANT 
	SELECT, 
	INSERT, 
	UPDATE,  
	DELETE
ON 
	auth.users 
TO 
	TestUser
GO

GRANT 
	SELECT, 
	INSERT, 
	UPDATE,  
	DELETE
ON 
	auth.userverification 
TO 
	TestUser
GO

GRANT 
	SELECT, 
	INSERT, 
	UPDATE,  
	DELETE
ON 
	auth.usertokens 
TO 
	TestUser
GO

GRANT 
	SELECT, 
	INSERT, 
	UPDATE,  
	DELETE
ON 
	auth.userroles 
TO 
	TestUser
GO

GRANT 
	SELECT
ON 
	auth.roles 
TO 
	TestUser
GO

