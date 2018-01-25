﻿CREATE TABLE [dbo].[Fylke]
(
	[Id] INT IDENTITY (1, 1) NOT NULL, 
    [fylkesnr] NCHAR(2) NOT NULL, 
    [fylke] NCHAR(17) NOT NULL, 
	CONSTRAINT [PK_Fylke] PRIMARY KEY CLUSTERED ([Id] ASC)
)