
-- Save
CREATE PROCEDURE [dbo].[proc_AMOUNT_Save]
	@amount_PK int = NULL,
	@quantity varchar(250) = NULL,
	@lownumvalue varchar(10) = NULL,
	@lownumunit varchar(250) = NULL,
	@lownumprefix varchar(250) = NULL,
	@lowdenomvalue varchar(10) = NULL,
	@lowdenomunit varchar(250) = NULL,
	@lowdenomprefix varchar(250) = NULL,
	@highnumvalue varchar(10) = NULL,
	@highnumunit varchar(250) = NULL,
	@highnumprefix varchar(250) = NULL,
	@highdenomvalue varchar(10) = NULL,
	@highdenomunit varchar(250) = NULL,
	@highdenomprefix varchar(250) = NULL,
	@average varchar(10) = NULL,
	@prefix varchar(250) = NULL,
	@unit varchar(250) = NULL,
	@nonnumericvalue varchar(250) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[AMOUNT]
	SET
	[quantity] = @quantity,
	[lownumvalue] = @lownumvalue,
	[lownumunit] = @lownumunit,
	[lownumprefix] = @lownumprefix,
	[lowdenomvalue] = @lowdenomvalue,
	[lowdenomunit] = @lowdenomunit,
	[lowdenomprefix] = @lowdenomprefix,
	[highnumvalue] = @highnumvalue,
	[highnumunit] = @highnumunit,
	[highnumprefix] = @highnumprefix,
	[highdenomvalue] = @highdenomvalue,
	[highdenomunit] = @highdenomunit,
	[highdenomprefix] = @highdenomprefix,
	[average] = @average,
	[prefix] = @prefix,
	[unit] = @unit,
	[nonnumericvalue] = @nonnumericvalue
	WHERE [amount_PK] = @amount_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[AMOUNT]
		([quantity], [lownumvalue], [lownumunit], [lownumprefix], [lowdenomvalue], [lowdenomunit], [lowdenomprefix], [highnumvalue], [highnumunit], [highnumprefix], [highdenomvalue], [highdenomunit], [highdenomprefix], [average], [prefix], [unit], [nonnumericvalue])
		VALUES
		(@quantity, @lownumvalue, @lownumunit, @lownumprefix, @lowdenomvalue, @lowdenomunit, @lowdenomprefix, @highnumvalue, @highnumunit, @highnumprefix, @highdenomvalue, @highdenomunit, @highdenomprefix, @average, @prefix, @unit, @nonnumericvalue)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
