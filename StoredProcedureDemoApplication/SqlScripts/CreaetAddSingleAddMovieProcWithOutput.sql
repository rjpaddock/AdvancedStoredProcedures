USE StoredProcedureDemo
GO

DROP PROCEDURE AddSingleMovieWithOutput
GO


CREATE PROCEDURE AddSingleMovieWithOutput
  @ExternalId INT,
  @Name VARCHAR(1000),
  @Year CHAR(4),
  @NewId INT output

  AS
  INSERT INTO dbo.Movies
  (
      ExternalId,
      Name,
      Year
  )
  VALUES
  (   @ExternalId, @Name,@Year)

 
 SELECT @NewId = SCOPE_IDENTITY()
 
  GO


