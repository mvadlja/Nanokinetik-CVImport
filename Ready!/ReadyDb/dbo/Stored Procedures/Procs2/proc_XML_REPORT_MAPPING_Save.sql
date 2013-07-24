-- Save
CREATE PROCEDURE  [dbo].[proc_XML_REPORT_MAPPING_Save]
	@xml_report_mapping_PK int = NULL,
	@xml_tag nvarchar(100) = NULL,
	@display_tag nvarchar(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[XML_REPORT_MAPPING]
	SET
	[xml_tag] = @xml_tag,
	[display_tag] = @display_tag
	WHERE [xml_report_mapping_PK] = @xml_report_mapping_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[XML_REPORT_MAPPING]
		([xml_tag], [display_tag])
		VALUES
		(@xml_tag, @display_tag)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
