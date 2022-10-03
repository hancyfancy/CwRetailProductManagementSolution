use CwRetail;

DECLARE @TypeEnum SMALLINT 
SET @TypeEnum = CONVERT(SMALLINT, 1 + (5-1)*RAND(CHECKSUM(NEWID())))
EXEC production.products_sp @TypeEnum
GO