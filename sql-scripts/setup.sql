USE master;
GO
CREATE DATABASE urlshortenertest;
GO
USE urlshortenertest; -- Switch to the new database
CREATE TABLE UrlMapping (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ShortKey NVARCHAR(255),
    LongUrl NVARCHAR(MAX)
);
GO
