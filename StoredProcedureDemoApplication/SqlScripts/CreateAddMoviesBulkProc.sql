USE StoredProcedureDemo
GO

Create Type Movie AS Table
         ( ExternalId int,
		   Name VARCHAR(1000),
		   Year CHAR(4))
      
GO
      
Create Procedure AddMoviesBulkLoad
@MovieData Movie READONLY
as
      
Insert Into Movies (ExternalId,Name,year)
  Select ExternalId,Name,year
    From @MovieData
GO