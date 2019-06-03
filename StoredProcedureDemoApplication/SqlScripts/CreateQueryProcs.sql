USE StoredProcedureDemo
GO

Create Type StringParam AS Table
         ( 
		   StringValue CHAR(4))
      
GO
      
Create Procedure GetMoviesByYears
@YearList StringParam READONLY
as
      
	  SELECT * FROM Movies 
	  WHERE Year IN (SELECT StringValue FROM @YearList)

GO


Create Procedure GetMoviesByYearsAndName
@Name VARCHAR(1000),
@YearList StringParam READONLY
as
      
	  SELECT * FROM Movies 
	  WHERE Year IN (SELECT StringValue FROM @YearList)
	   AND Name LIKE @Name

GO