-- create schemas
CREATE SCHEMA auth
GO

-- create functions
CREATE FUNCTION auth.users_checkvalidemail (@EMAIL VARCHAR(100)) RETURNS BIT AS
BEGIN     
  DECLARE @bitEmailVal AS BIT
  DECLARE @EmailText VARCHAR(100)

  SET @EmailText=LTRIM(RTRIM(ISNULL(@EMAIL,'')))

  SET @bitEmailVal = CASE WHEN @EmailText = '' THEN 0
                          WHEN @EmailText LIKE '% %' THEN 0
                          WHEN @EmailText LIKE ('%["(),:;<>\]%') THEN 0
                          WHEN SUBSTRING(@EmailText,CHARINDEX('@',@EmailText),LEN(@EmailText)) LIKE ('%[!#$%&*+/=?^`_{|]%') THEN 0
                          WHEN (LEFT(@EmailText,1) LIKE ('[-_.+]') OR RIGHT(@EmailText,1) LIKE ('[-_.+]')) THEN 0                                                                                    
                          WHEN (@EmailText LIKE '%[%' OR @EmailText LIKE '%]%') THEN 0
                          WHEN @EmailText LIKE '%@%@%' THEN 0
                          WHEN @EmailText NOT LIKE '_%@_%._%' THEN 0
                          ELSE 1 
                      END
  RETURN @bitEmailVal
END 
GO

-- create tables
CREATE TABLE auth.users (
	UserId BIGINT IDENTITY (1, 1) PRIMARY KEY,
	Username NVARCHAR (100) NOT NULL CHECK (LEN(Username) > 4),
	Email NVARCHAR (100) NOT NULL CHECK (auth.users_checkvalidemail(Email) = 1),
    Phone NVARCHAR (20) NOT NULL,
	LastActive DATETIME NOT NULL CHECK (LastActive < GETDATE()),
	UNIQUE(Username)
)
GO

CREATE TABLE auth.userverification (
	UserVerificationId BIGINT IDENTITY (1, 1) PRIMARY KEY,
	UserId BIGINT NOT NULL FOREIGN KEY REFERENCES auth.users(UserId) ON DELETE CASCADE,
	EmailVerified BIT NOT NULL,
    PhoneVerified BIT NOT NULL
)
GO

CREATE TABLE auth.userroles (
	UserRoleId BIGINT IDENTITY (1, 1) PRIMARY KEY,
	UserId BIGINT NOT NULL FOREIGN KEY REFERENCES auth.users(UserId) ON DELETE NO ACTION,
	RoleId BIGINT NOT NULL FOREIGN KEY REFERENCES auth.users(UserId) ON DELETE NO ACTION,
	UNIQUE (UserId, RoleId),
	UNIQUE (UserId)
)
GO

CREATE TABLE auth.usertokens (
	UserTokenId BIGINT IDENTITY (1, 1) PRIMARY KEY,
	UserId BIGINT NOT NULL FOREIGN KEY REFERENCES auth.users(UserId) ON DELETE CASCADE,
	Token NVARCHAR (200) NOT NULL CHECK (LEN(Token) = 200),
	RefreshAt DATETIME NOT NULL CHECK (RefreshAt > GETDATE()),
	UNIQUE (UserId),
	UNIQUE (Token)
)
GO

CREATE TABLE auth.roles (
	RoleId BIGINT IDENTITY (1, 1) PRIMARY KEY,
	Role NVARCHAR (100) NOT NULL CHECK (Role = 'User' OR Role = 'Specialist' OR Role = 'Admin'),
	SubRole NVARCHAR (100) NOT NULL CHECK (SubRole = 'Standard' OR SubRole = 'Bronze' OR SubRole = 'Silver' OR SubRole = 'Gold' OR SubRole = 'Platinum')
)
GO

-- create triggers
CREATE TRIGGER auth.roles_tr_delete
ON CwRetail.auth.roles
AFTER DELETE
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @Id BIGINT
	SELECT @Id = RoleId FROM deleted

	DELETE FROM 
		CwRetail.auth.userroles
	WHERE
		RoleId = @Id
END
GO

CREATE TRIGGER auth.users_tr_delete
ON CwRetail.auth.users
AFTER DELETE
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @Id BIGINT
	SELECT @Id = UserId FROM deleted

	DELETE FROM 
		CwRetail.auth.userroles
	WHERE
		UserId = @Id
END
GO