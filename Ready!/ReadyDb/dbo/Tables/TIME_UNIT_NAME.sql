CREATE TABLE [dbo].[TIME_UNIT_NAME] (
    [time_unit_name_PK] INT            IDENTITY (1, 1) NOT NULL,
    [time_unit_name]    NVARCHAR (150) NULL,
    [billable]          BIT            NULL,
    CONSTRAINT [PK_TIME_UNIT_NAME] PRIMARY KEY CLUSTERED ([time_unit_name_PK] ASC)
);



