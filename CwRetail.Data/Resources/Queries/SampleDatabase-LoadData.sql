USE CwRetail

DECLARE @i INT = 0
DECLARE @TypeEnum SMALLINT

WHILE @i < 10
BEGIN
    SET @i = @i + 1
    SET @TypeEnum = CONVERT(SMALLINT, 1 + (6-1)*RAND(CHECKSUM(NEWID())))
	EXEC production.products_sp @TypeEnum
END
GO