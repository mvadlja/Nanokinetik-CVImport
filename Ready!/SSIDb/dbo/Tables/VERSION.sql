CREATE TABLE [dbo].[VERSION] (
    [version_PK]     INT            IDENTITY (1, 1) NOT NULL,
    [version_number] INT            NOT NULL,
    [effectve_date]  VARCHAR (10)   NULL,
    [change_made]    VARCHAR (4000) NULL,
    CONSTRAINT [PK_SSI_VERSION] PRIMARY KEY CLUSTERED ([version_PK] ASC)
);

