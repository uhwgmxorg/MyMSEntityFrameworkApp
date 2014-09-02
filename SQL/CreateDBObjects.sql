USE TestDB
GO

IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'myschema' AND TABLE_NAME = 'Name'))
BEGIN
	DROP TABLE myschema.Name
    PRINT 'TABLE Name has been dropped.'  
END
GO
CREATE TABLE myschema.Name(
	Id			bigint IDENTITY(1,1) NOT NULL PRIMARY KEY,
	FirstName	nvarchar(100) NOT NULL,
	LastName	nvarchar(100) NOT NULL,
	Age         int NOT NULL,
	InsertDate	datetime2 NOT NULL DEFAULT getdate()
)
GO
