CREATE TABLE [dbo].[USER_ACTION] (
    [user_action_PK] INT             IDENTITY (1, 1) NOT NULL,
    [unique_name]    NVARCHAR (450)  NOT NULL,
    [display_name]   NVARCHAR (1000) NULL,
    [description]    NVARCHAR (MAX)  NULL,
    [active]         BIT             NULL,
    CONSTRAINT [PK_USER_ACTION] PRIMARY KEY CLUSTERED ([user_action_PK] ASC),
    CONSTRAINT [IX_USER_ACTION] UNIQUE NONCLUSTERED ([unique_name] ASC)
);

