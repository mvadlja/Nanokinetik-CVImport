CREATE TABLE [dbo].[ENTITY_LAST_CHANGE] (
    [last_change_PK] INT           IDENTITY (1, 1) NOT NULL,
    [change_table]   NVARCHAR (50) NOT NULL,
    [change_date]    DATETIME      NOT NULL,
    [user_FK]        INT           NOT NULL,
    [entity_FK]      INT           NOT NULL,
    CONSTRAINT [PK_ENTITY_LAST_CHANGE] PRIMARY KEY CLUSTERED ([last_change_PK] ASC)
);

