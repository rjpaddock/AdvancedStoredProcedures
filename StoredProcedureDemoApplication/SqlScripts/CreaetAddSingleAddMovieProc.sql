USE StoredProcedureDemo
GO

DROP PROCEDURE AddSingleMovie
GO


CREATE PROCEDURE AddSingleMovie
  @ExternalId INT,
  @Name VARCHAR(1000),
  @Year CHAR(4)

  AS
  INSERT INTO dbo.Movies
  (
      ExternalId,
      Name,
      Year
  )
  VALUES
  (   @ExternalId, @Name,@Year)

 
 
  GO


