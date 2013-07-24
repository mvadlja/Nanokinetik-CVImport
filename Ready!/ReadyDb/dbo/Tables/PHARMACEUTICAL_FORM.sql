CREATE TABLE [dbo].[PHARMACEUTICAL_FORM] (
    [pharmaceutical_form_PK] INT            IDENTITY (1, 1) NOT NULL,
    [name]                   NVARCHAR (200) NULL,
    [ev_code]                NVARCHAR (60)  NULL,
    [deleted]                BIT            CONSTRAINT [DF_PHARMACEUTICAL_FORM_deleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_PHARMACEUTICAL_FORM] PRIMARY KEY CLUSTERED ([pharmaceutical_form_PK] ASC)
);







