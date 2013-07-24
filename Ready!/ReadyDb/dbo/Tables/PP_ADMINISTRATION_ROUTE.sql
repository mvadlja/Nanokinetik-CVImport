CREATE TABLE [dbo].[PP_ADMINISTRATION_ROUTE] (
    [adminroute_PK]  INT           IDENTITY (1, 1) NOT NULL,
    [adminroutecode] NVARCHAR (60) NULL,
    [resolutionmode] INT           NULL,
    [ev_code]        NVARCHAR (50) NULL,
    CONSTRAINT [PK_PP_ADMINISTRATION_ROUTE] PRIMARY KEY CLUSTERED ([adminroute_PK] ASC)
);

