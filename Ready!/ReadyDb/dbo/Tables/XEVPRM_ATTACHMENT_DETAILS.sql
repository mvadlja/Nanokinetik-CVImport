CREATE TABLE [dbo].[XEVPRM_ATTACHMENT_DETAILS] (
    [xevprm_attachment_details_PK] INT             IDENTITY (1, 1) NOT NULL,
    [attachment_FK]                INT             NULL,
    [file_name]                    NVARCHAR (200)  NULL,
    [file_type]                    NVARCHAR (10)   NULL,
    [attachment_name]              NVARCHAR (2000) NULL,
    [attachment_type]              NVARCHAR (10)   NULL,
    [language_code]                NVARCHAR (50)   NULL,
    [attachment_version]           NVARCHAR (10)   NULL,
    [attachment_version_date]      DATETIME        NULL,
    [operation_type]               INT             NULL,
    [ev_code]                      NVARCHAR (60)   NULL,
    CONSTRAINT [PK_XEVPRM_ATTACHMENT_DETAILS] PRIMARY KEY CLUSTERED ([xevprm_attachment_details_PK] ASC),
    CONSTRAINT [FK_XEVPRM_ATTACHMENT_DETAILS_ATTACHMENT] FOREIGN KEY ([attachment_FK]) REFERENCES [dbo].[ATTACHMENT] ([attachment_PK]) ON DELETE SET NULL
);

