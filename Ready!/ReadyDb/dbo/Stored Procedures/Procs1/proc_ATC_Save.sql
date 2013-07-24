-- Save
CREATE PROCEDURE  [dbo].[proc_ATC_Save]
	@atc_PK int = NULL,
	@operationtype int = NULL,
	@type_term int = NULL,
	@atccode nvarchar(50) = NULL,
	@newownerid nvarchar(60) = NULL,
	@atccode_desc nvarchar(200) = NULL,
	@versiondateformat int = NULL,
	@versiondate nvarchar(50) = NULL,
	@comments nvarchar(MAX) = NULL,
	@pom_code nvarchar(250) = NULL,
	@pom_subcode nvarchar(150) = NULL,
	@pom_ddd nvarchar(50) = NULL,
	@pom_u nvarchar(50) = NULL,
	@pom_ar nvarchar(50) = NULL,
	@pom_note nvarchar(250) = NULL,
	@name nvarchar(250) = NULL,
	@name_archive nvarchar(250) = NULL,
	@search_by nvarchar(550) = NULL,
	@is_group bit = NULL,
	@evpmd_code nvarchar(50) = NULL,
	@value nvarchar(50) = NULL,
	@is_maunal_entry bit = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ATC]
	SET
	[operationtype] = @operationtype,
	[type_term] = @type_term,
	[atccode] = @atccode,
	[newownerid] = @newownerid,
	[atccode_desc] = @atccode_desc,
	[versiondateformat] = @versiondateformat,
	[versiondate] = @versiondate,
	[comments] = @comments,
	[pom_code] = @pom_code,
	[pom_subcode] = @pom_subcode,
	[pom_ddd] = @pom_ddd,
	[pom_u] = @pom_u,
	[pom_ar] = @pom_ar,
	[pom_note] = @pom_note,
	[name] = @name,
	[name_archive] = @name_archive,
	[search_by] = @search_by,
	[is_group] = @is_group,
	[evpmd_code] = @evpmd_code,
	[value] = @value,
	[is_maunal_entry] = @is_maunal_entry
	WHERE [atc_PK] = @atc_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ATC]
		([operationtype], [type_term], [atccode], [newownerid], [atccode_desc], [versiondateformat], [versiondate], [comments], [pom_code], [pom_subcode], [pom_ddd], [pom_u], [pom_ar], [pom_note], [name], [name_archive], [search_by], [is_group], [evpmd_code], [value], [is_maunal_entry])
		VALUES
		(@operationtype, @type_term, @atccode, @newownerid, @atccode_desc, @versiondateformat, @versiondate, @comments, @pom_code, @pom_subcode, @pom_ddd, @pom_u, @pom_ar, @pom_note, @name, @name_archive, @search_by, @is_group, @evpmd_code, @value, @is_maunal_entry)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
