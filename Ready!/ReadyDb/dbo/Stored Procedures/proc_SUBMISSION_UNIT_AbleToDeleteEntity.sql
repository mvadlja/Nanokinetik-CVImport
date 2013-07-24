
CREATE PROCEDURE  [dbo].[proc_SUBMISSION_UNIT_AbleToDeleteEntity]
	@submissionUnitPk int = NULL
AS
DECLARE @NumberOfNeeSAttachments INT = NULL;
DECLARE @NumberOfeCTDAttachments INT = NULL;
DECLARE @NumberOfAttachments INT = NULL;

DECLARE @NeeSFk INT = NULL;
DECLARE @eCTDFk INT = NULL;
DECLARE @documentFk INT = NULL;

BEGIN
	SET NOCOUNT ON;

	IF (@submissionUnitPk IS NULL)
		SELECT 0 AS AbleToDelete;
	ELSE
	BEGIN
		SET @NeeSFk = (SELECT TOP 1 ness_FK FROM dbo.SUBMISSION_UNIT WHERE subbmission_unit_PK = @submissionUnitPk);
		SET @eCTDFk = (SELECT TOP 1 ectd_FK FROM dbo.SUBMISSION_UNIT WHERE subbmission_unit_PK = @submissionUnitPk);
		SET @documentFk = (SELECT TOP 1 document_FK FROM dbo.SUBMISSION_UNIT WHERE subbmission_unit_PK = @submissionUnitPk);

		SELECT @NumberOfNeeSAttachments = COUNT(*)
		FROM [dbo].[ATTACHMENT] a
		WHERE (a.[document_FK] = @NeeSFk)
		
		SELECT @NumberOfeCTDAttachments = COUNT(*)
		FROM [dbo].[ATTACHMENT] a
		WHERE (a.[document_FK] = @eCTDFk)
		
		SELECT @NumberOfAttachments = COUNT(*)
		FROM [dbo].[ATTACHMENT] a
		WHERE (a.[document_FK] = @documentFk)
	
		SET @NumberOfNeeSAttachments = ISNULL (@NumberOfNeeSAttachments, 0);
		SET @NumberOfeCTDAttachments = ISNULL (@NumberOfeCTDAttachments, 0);
		SET @NumberOfAttachments = ISNULL (@NumberOfAttachments, 0);

		IF ((@NumberOfNeeSAttachments + @NumberOfeCTDAttachments + @NumberOfeCTDAttachments) = 0)
			SELECT 1 AS AbleToDelete;
		ELSE
			SELECT 0 AS AbleToDelete;
	END
END