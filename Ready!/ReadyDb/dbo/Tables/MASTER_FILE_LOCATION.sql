CREATE TABLE [dbo].[MASTER_FILE_LOCATION] (
    [master_file_location_PK] INT            IDENTITY (1, 1) NOT NULL,
    [localnumber]             NVARCHAR (60)  NULL,
    [ev_code]                 NVARCHAR (60)  NULL,
    [mflcompany]              NVARCHAR (100) NULL,
    [mfldepartment]           NVARCHAR (100) NULL,
    [mflbuilding]             NVARCHAR (100) NULL,
    [mflstreet]               NVARCHAR (100) NULL,
    [mflcity]                 NVARCHAR (50)  NULL,
    [mflstate]                NVARCHAR (50)  NULL,
    [mflpostcode]             NVARCHAR (50)  NULL,
    [mflcountrycode]          NVARCHAR (50)  NULL,
    [comments]                NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_MASTER_FILE_LOCATION] PRIMARY KEY CLUSTERED ([master_file_location_PK] ASC)
);

