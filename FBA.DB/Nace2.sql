﻿CREATE TABLE [dbo].[Nace2]
(
	[Id] INT IDENTITY (1, 1) NOT NULL, 
    [nace2] NCHAR(2) NOT NULL, 
    [nace2_tekst] NCHAR(97) NOT NULL, 
	CONSTRAINT [PK_Nace2] PRIMARY KEY CLUSTERED ([Id] ASC)
)