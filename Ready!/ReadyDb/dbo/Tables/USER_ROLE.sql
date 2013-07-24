CREATE TABLE [dbo].[USER_ROLE] (
    [user_role_PK] INT            IDENTITY (1, 1) NOT NULL,
    [name]         NVARCHAR (200) NULL,
    [display_name] NVARCHAR (200) NULL,
    [description]  NVARCHAR (MAX) NULL,
    [active]       BIT            NULL,
    [row_version]  DATETIME       NULL,
    [visible]      BIT            NULL,
    CONSTRAINT [PK_USER_ROLE_1] PRIMARY KEY CLUSTERED ([user_role_PK] ASC)
);



