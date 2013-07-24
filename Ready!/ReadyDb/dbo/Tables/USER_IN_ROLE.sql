CREATE TABLE [dbo].[USER_IN_ROLE] (
    [user_in_role_PK] INT      IDENTITY (1, 1) NOT NULL,
    [user_FK]         INT      NULL,
    [user_role_FK]    INT      NULL,
    [row_version]     DATETIME NULL,
    [Visible]         BIT      NULL,
    CONSTRAINT [PK_USER_IN_ROLE] PRIMARY KEY CLUSTERED ([user_in_role_PK] ASC),
    CONSTRAINT [FK_USER_IN_ROLE_USER_ROLE1] FOREIGN KEY ([user_role_FK]) REFERENCES [dbo].[USER_ROLE] ([user_role_PK])
);

