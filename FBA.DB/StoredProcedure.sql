--USE [FBA.DB];
--GO

--IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'NewLine')
--DROP PROCEDURE dbo.NewLine
--GO

CREATE PROCEDURE dbo.NewLine
    @orgnr INT, 
    @dunsnr INT, 
    @kommunenr nvarchar(4),
    @kommune nvarchar(31), 
    @fylkesnr nvarchar(2), 
    @fylke nvarchar(17),
    @nace2 nvarchar(2),
    @nace2_tekst nvarchar(97), 
    @nace5 nvarchar(5),
    @nace5_tekst nvarchar(122), 
    @regnskapser SMALLINT,
    @omsetning INT, 
    @EBITDA INT,
    @Antall_ansatte INT, 
    @Oms_pr_ansatt INT,
    @lonnskost_pr_ansatt INT,
	@driftsmargin INT,
	@new_identity nvarchar(100) = NULL output
AS 
    SET NOCOUNT ON;

	DECLARE @kommuneID INT; 
	DECLARE @fylkeID INT; 
	DECLARE @nace2ID INT; 
	DECLARE @nace5ID INT; 

	SELECT @kommuneID = Id FROM dbo.Kommune WHERE kommunenr = @kommunenr AND kommune = @kommune 
IF 
	@kommuneID IS NULL 
BEGIN
	INSERT INTO dbo.Kommune (kommunenr, kommune) VALUES (@kommunenr, @kommune);
	SET @kommuneID = SCOPE_IDENTITY();
END

	SELECT @fylkeID = Id FROM dbo.Fylke WHERE fylkesnr = @fylkesnr AND fylke = @fylke 
IF 
	@fylkeID IS NULL 
BEGIN
	INSERT INTO dbo.Fylke (fylkesnr, fylke) VALUES (@fylkesnr, @fylke);
	SET @fylkeID = SCOPE_IDENTITY();
END

	SELECT @nace2ID = Id FROM dbo.Nace2 WHERE nace2 = @nace2 AND nace2_tekst = @nace2_tekst 
IF 
	@nace2ID IS NULL 
BEGIN
	INSERT INTO dbo.Nace2 (nace2, nace2_tekst) VALUES (@nace2, @nace2_tekst);
	SET @nace2ID = SCOPE_IDENTITY();
END

	SELECT @nace5ID = Id FROM dbo.Nace5 WHERE nace5 = @nace5 AND nace5_tekst = @nace5_tekst 
IF 
	@nace5ID IS NULL 
BEGIN
	INSERT INTO dbo.Nace5 (nace5, nace5_tekst) VALUES (@nace5, @nace5_tekst);
	SET @nace5ID = SCOPE_IDENTITY();
END

	INSERT 
		INTO dbo. Bransjefakta 
		(orgnr, dunsnr, kommuneID, fylkeID, nace2ID, nace5ID, regnskapsеr, omsetning, EBITDA, Antall_ansatte, Oms_pr_ansatt, lonnskost_pr_ansatt, driftsmargin) 
		VALUES 
		(@orgnr, @dunsnr, @kommuneID, @fylkeID, @nace2ID, @nace5ID, @regnskapser, @omsetning, @EBITDA, @Antall_ansatte, @Oms_pr_ansatt, @lonnskost_pr_ansatt, @driftsmargin);
	SET @new_identity = 'ok';
GO
  