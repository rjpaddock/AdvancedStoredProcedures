USE [StoredProcedureDemo]
GO

DECLARE @RC INT

DECLARE @NewId INT

-- TODO: Set parameter values here.

EXECUTE @RC = [dbo].AddSingleMovieWithOutput 
  1,'STAR WARS','1977',@NewId OUTPUT
  
SELECT @NewId

GO


