CREATE TABLE [dbo].[USER_GRID_SETTINGS] (
    [user_grid_settings_PK] INT            IDENTITY (1, 1) NOT NULL,
    [user_FK]               INT            NULL,
    [grid_layout]           NVARCHAR (MAX) NULL,
    [isdefault]             BIT            NULL,
    [timestamp]             DATETIME       NULL,
    [ql_visible]            BIT            NULL,
    [grid_ID]               NVARCHAR (100) NULL,
    [display_name]          NVARCHAR (100) NULL,
    CONSTRAINT [PK_USER_GRID_SETTINGS] PRIMARY KEY CLUSTERED ([user_grid_settings_PK] ASC)
);

