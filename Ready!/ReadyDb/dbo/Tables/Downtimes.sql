CREATE TABLE [dbo].[Downtimes] (
    [IDDowntime]     INT            IDENTITY (1, 1) NOT NULL,
    [CountryID]      INT            NOT NULL,
    [DateFrom]       DATETIME       NOT NULL,
    [DateTo]         DATETIME       NULL,
    [Comment]        NVARCHAR (MAX) NULL,
    [DisplayComment] NVARCHAR (500) NOT NULL,
    [Active]         BIT            NOT NULL,
    [UserShutdowner] NVARCHAR (100) NULL,
    [RowVersion]     DATETIME       NOT NULL,
    CONSTRAINT [PK_Downtimes] PRIMARY KEY CLUSTERED ([IDDowntime] ASC)
);

