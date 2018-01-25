CREATE TABLE [dbo].[Bransjefakta]
(
	[Id] INT IDENTITY (1, 1) NOT NULL, 
    [orgnr] INT NOT NULL, 
    [dunsnr] INT NOT NULL, 
    [kommuneID] INT NOT NULL, 
    [fylkeID] INT NOT NULL, 
    [nace2ID] INT NOT NULL, 
    [nace5ID] INT NOT NULL, 
    [regnskapsеr] SMALLINT NOT NULL, 
    [omsetning] INT NOT NULL, 
    [EBITDA] INT NOT NULL, 
    [Antall_ansatte] INT NULL, 
    [Oms_pr_ansatt] INT NULL, 
    [lonnskost_pr_ansatt] INT NULL, 
    [driftsmargin] INT NULL,
	CONSTRAINT [PK_Bransjefakta] PRIMARY KEY CLUSTERED ([Id] ASC)
)