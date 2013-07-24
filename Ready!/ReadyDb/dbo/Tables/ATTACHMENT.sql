CREATE TABLE [dbo].[ATTACHMENT] (
    [attachment_PK]          INT                        IDENTITY (1, 1) NOT NULL,
    [document_FK]            INT                        NULL,
    [attachmentname]         NVARCHAR (2000)            NULL,
    [filetype]               NVARCHAR (50)              NULL,
    [userID]                 INT                        NULL,
    [pom_type]               NVARCHAR (50)              NULL,
    [session_id]             UNIQUEIDENTIFIER           NULL,
    [ev_code]                NVARCHAR (255)             NULL,
    [type_for_fts]           NVARCHAR (8)               NULL,
    [file_id]                UNIQUEIDENTIFIER           DEFAULT (newid()) ROWGUIDCOL NOT NULL,
    [disk_file]              VARBINARY (MAX) FILESTREAM NULL,
    [modified_date]          DATETIME                   CONSTRAINT [DF_ATTACHMENT_modified_date] DEFAULT (getdate()) NULL,
    [EDMSDocumentId]         NVARCHAR (128)             NULL,
    [EDMSBindingRule]        NVARCHAR (128)             NULL,
    [EDMSAttachmentFormat]   NVARCHAR (128)             NULL,
    [lock_owner_FK]          INT                        NULL,
    [lock_date]              DATETIME                   NULL,
    [check_in_for_attach_FK] INT                        NULL,
    [check_in_session_id]    UNIQUEIDENTIFIER           NULL,
    CONSTRAINT [PK_ATTACHMENT] PRIMARY KEY CLUSTERED ([attachment_PK] ASC),
    CONSTRAINT [FK_ATTACHMENT_DOCUMENT] FOREIGN KEY ([document_FK]) REFERENCES [dbo].[DOCUMENT] ([document_PK]) ON DELETE CASCADE,
    CONSTRAINT [FK_ATTACHMENT_USER] FOREIGN KEY ([lock_owner_FK]) REFERENCES [dbo].[USER] ([user_PK]),
    UNIQUE NONCLUSTERED ([file_id] ASC)
) FILESTREAM_ON [ReadyFileGroup];







