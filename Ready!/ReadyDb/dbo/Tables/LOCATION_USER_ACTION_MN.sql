CREATE TABLE [dbo].[LOCATION_USER_ACTION_MN] (
    [location_user_action_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [location_FK]                INT NULL,
    [user_action_FK]             INT NULL,
    CONSTRAINT [PK_LOCATION_USER_ACTION_MN] PRIMARY KEY CLUSTERED ([location_user_action_mn_PK] ASC),
    CONSTRAINT [FK_LOCATION_USER_ACTION_MN_LOCATION] FOREIGN KEY ([location_FK]) REFERENCES [dbo].[LOCATION] ([location_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_LOCATION_USER_ACTION_MN_USER_ACTION] FOREIGN KEY ([user_action_FK]) REFERENCES [dbo].[USER_ACTION] ([user_action_PK]) ON DELETE CASCADE
);

