CREATE TABLE [dbo].[TYPE] (
    [type_PK]           INT            IDENTITY (1, 1) NOT NULL,
    [name]              NVARCHAR (100) NULL,
    [group]             NVARCHAR (20)  NULL,
    [entity_related]    NVARCHAR (50)  NULL,
    [form_related]      NVARCHAR (50)  NULL,
    [type]              NVARCHAR (50)  NULL,
    [description]       NVARCHAR (MAX) NULL,
    [group_description] NVARCHAR (50)  NULL,
    [ev_code]           NVARCHAR (20)  NULL,
    [custom_sort]       INT            NULL,
    CONSTRAINT [PK__TYPE_RC__2C013882284DF453] PRIMARY KEY CLUSTERED ([type_PK] ASC)
);

