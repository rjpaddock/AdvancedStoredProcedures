CREATE DATABASE StoredProcedureDemo
GO

USE StoredProcedureDemo
GO

CREATE TABLE Movies 
  (Id INT PRIMARY KEY NOT NULL IDENTITY,
   ExternalId INT,
   [Name] VARCHAR(1000),
   [Year] CHAR(4))
GO

