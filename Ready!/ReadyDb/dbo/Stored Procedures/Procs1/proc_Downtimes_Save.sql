-- Save
CREATE PROCEDURE  [dbo].[proc_Downtimes_Save]
	@IDDowntime int = NULL,
	@CountryID int = NULL,
	@DateFrom datetime = NULL,
	@DateTo datetime = NULL,
	@Comment nvarchar(MAX) = NULL,
	@DisplayComment nvarchar(500) = NULL,
	@Active bit = NULL,
	@UserShutdowner nvarchar(100) = NULL,
	@RowVersion datetime = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[Downtimes]
	SET
	[CountryID] = @CountryID,
	[DateFrom] = @DateFrom,
	[DateTo] = @DateTo,
	[Comment] = @Comment,
	[DisplayComment] = @DisplayComment,
	[Active] = @Active,
	[UserShutdowner] = @UserShutdowner,
	[RowVersion] = @RowVersion
	WHERE [IDDowntime] = @IDDowntime

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[Downtimes]
		([CountryID], [DateFrom], [DateTo], [Comment], [DisplayComment], [Active], [UserShutdowner], [RowVersion])
		VALUES
		(@CountryID, @DateFrom, @DateTo, @Comment, @DisplayComment, @Active, @UserShutdowner, @RowVersion)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
