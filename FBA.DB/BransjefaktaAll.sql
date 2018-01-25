CREATE TABLE [dbo].[BransjefaktaAll]
(
	[Id] INT           IDENTITY (1, 1) NOT NULL, 
    [orgnr] INT NOT NULL, 
    [dunsnr] INT NOT NULL, 
    [kommunenr] SMALLINT NOT NULL, 
    [kommune] NCHAR(31) NOT NULL, 
    [fylkesnr] TINYINT NOT NULL, 
    [fylke] NCHAR(17) NOT NULL, 
    [nace2] TINYINT NOT NULL, 
    [nace2_tekst] NCHAR(69) NOT NULL, 
    [nace5] NCHAR(57) NOT NULL, 
    [nace5_tekst] NCHAR(89) NOT NULL, 
    [regnskapsеr] NCHAR(89) NOT NULL, 
    [omsetning] NCHAR(66) NOT NULL, 
    [EBITDA] NCHAR(67) NOT NULL, 
    [Antall_ansatte] NCHAR(33) NULL, 
    [Oms_pr_ansatt] NCHAR(33) NULL, 
    [lonnskost_pr_ansatt] NCHAR(8) NULL, 
    [driftsmargin] NCHAR(8) NULL,
	CONSTRAINT [PK_BransjefaktaAll] PRIMARY KEY CLUSTERED ([Id] ASC)
)