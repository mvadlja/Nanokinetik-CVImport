CREATE TABLE [dbo].[QPPV_CODE] (
    [qppv_code_PK] INT           IDENTITY (1, 1) NOT NULL,
    [person_FK]    INT           NOT NULL,
    [qppv_code]    NVARCHAR (50) NULL,
    CONSTRAINT [PK_QPPV_CODE] PRIMARY KEY CLUSTERED ([qppv_code_PK] ASC),
    CONSTRAINT [FK_QPPV_CODE_PERSON] FOREIGN KEY ([person_FK]) REFERENCES [dbo].[PERSON] ([person_PK]) ON DELETE CASCADE
);



