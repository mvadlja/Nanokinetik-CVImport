CREATE TABLE [dbo].[SUBSTANCE_ATTACHMENT] (
    [substance_attachment_PK] INT           IDENTITY (1, 1) NOT NULL,
    [attachmentreference]     NVARCHAR (60) NULL,
    [resolutionmode]          INT           NULL,
    [validitydeclaration]     INT           NULL,
    CONSTRAINT [PK_SUBSTANCE_ATTACHMENT] PRIMARY KEY CLUSTERED ([substance_attachment_PK] ASC)
);

