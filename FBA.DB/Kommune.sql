CREATE TABLE [dbo].[Kommune]
(
	[Id] INT IDENTITY (1, 1) NOT NULL, 
    [kommunenr] NCHAR(4) NOT NULL, 
    [kommune] NCHAR(31) NOT NULL 
    CONSTRAINT [PK_Kommune] PRIMARY KEY CLUSTERED ([Id] ASC)
)