CREATE TABLE [dbo].[DOCUMENT] (
    [document_PK]          INT             IDENTITY (1, 1) NOT NULL,
    [person_FK]            INT             NULL,
    [type_FK]              INT             NULL,
    [name]                 NVARCHAR (2000) NULL,
    [description]          NVARCHAR (MAX)  NULL,
    [comment]              NVARCHAR (MAX)  NULL,
    [document_code]        NVARCHAR (250)  NULL,
    [regulatory_status]    INT             NULL,
    [version_number]       INT             NULL,
    [version_label]        INT             NULL,
    [change_date]          DATETIME        NULL,
    [effective_start_date] DATETIME        NULL,
    [effective_end_date]   DATETIME        NULL,
    [version_date]         DATETIME        NULL,
    [localnumber]          NVARCHAR (50)   NULL,
    [version_date_format]  NVARCHAR (50)   NULL,
    [attachment_name]      NVARCHAR (2000) NULL,
    [attachment_type_FK]   INT             NULL,
    [EVCODE]               INT             NULL,
    [LanguageCodes]        NVARCHAR (MAX)  NULL,
    [Attachments]          NVARCHAR (MAX)  NULL,
    [RelatedEntities]      NVARCHAR (MAX)  NULL,
    [EDMSDocumentId]       NVARCHAR (128)  NULL,
    [EDMSBindingRule]      NVARCHAR (128)  NULL,
    [EDMSModifyDate]       DATETIME        NULL,
    [EDMSVersionNumber]    NVARCHAR (128)  NULL,
    [EDMSDocument]         BIT             CONSTRAINT [DF_DOCUMENT_EDMSDocument] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_DOCUMENT] PRIMARY KEY CLUSTERED ([document_PK] ASC),
    CONSTRAINT [FK_DOCUMENT_PERSON] FOREIGN KEY ([person_FK]) REFERENCES [dbo].[PERSON] ([person_PK])
);









