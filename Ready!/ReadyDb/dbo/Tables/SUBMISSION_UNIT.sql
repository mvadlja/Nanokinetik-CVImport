CREATE TABLE [dbo].[SUBMISSION_UNIT] (
    [subbmission_unit_PK]    INT            IDENTITY (1, 1) NOT NULL,
    [task_FK]                INT            NULL,
    [submission_ID]          NVARCHAR (200) NULL,
    [agency_role_FK]         INT            NULL,
    [comment]                NVARCHAR (MAX) NULL,
    [s_format_FK]            INT            NULL,
    [description_type_FK]    INT            NULL,
    [dispatch_date]          DATETIME       NULL,
    [receipt_date]           DATETIME       NULL,
    [sequence]               NVARCHAR (100) NULL,
    [dtd_schema_FK]          NCHAR (10)     NULL,
    [organization_agency_FK] INT            NULL,
    [document_FK]            INT            NULL,
    [ness_FK]                INT            NULL,
    [ectd_FK]                INT            NULL,
    [person_FK]              INT            NULL,
    CONSTRAINT [PK_SUBMISSION_UNIT] PRIMARY KEY CLUSTERED ([subbmission_unit_PK] ASC),
    CONSTRAINT [FK_SUBMISSION_UNIT_DOCUMENT] FOREIGN KEY ([document_FK]) REFERENCES [dbo].[DOCUMENT] ([document_PK]),
    CONSTRAINT [FK_SUBMISSION_UNIT_DOCUMENT1] FOREIGN KEY ([ectd_FK]) REFERENCES [dbo].[DOCUMENT] ([document_PK]),
    CONSTRAINT [FK_SUBMISSION_UNIT_DOCUMENT2] FOREIGN KEY ([ness_FK]) REFERENCES [dbo].[DOCUMENT] ([document_PK]),
    CONSTRAINT [FK_SUBMISSION_UNIT_TASK_2] FOREIGN KEY ([task_FK]) REFERENCES [dbo].[TASK] ([task_PK])
);





