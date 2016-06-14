IF DB_Id('EvaluatingWebsite')IS NULL
CREATE DATABASE EvaluatingWebsite
GO

USE EvaluatingWebsite
IF(NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Websites'))
CREATE TABLE Websites(
Id INT IDENTITY NOT NULL PRIMARY KEY,
ParentId INT NULL,
Url NVARCHAR(400) NOT NULL, 
MillisecondsOfLoading bigint NOT NULL,
IsProcessed BIT NOT NULL)

