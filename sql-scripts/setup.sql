USE master;
GO
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'urlshortener')
BEGIN
    CREATE DATABASE urlshortener;
END
GO
USE urlshortener; -- Switch to the new database
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'UrlMappings')
BEGIN
    CREATE TABLE UrlMappings (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        ShortKey NVARCHAR(255),
        LongUrl NVARCHAR(MAX)
    );
END