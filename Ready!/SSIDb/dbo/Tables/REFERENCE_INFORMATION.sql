CREATE TABLE [dbo].[REFERENCE_INFORMATION] (
    [reference_info_PK] INT            IDENTITY (1, 1) NOT NULL,
    [comment]           VARCHAR (4000) NULL,
    CONSTRAINT [PK_SSI_REFERENCE_INFORMATION] PRIMARY KEY CLUSTERED ([reference_info_PK] ASC)
);

