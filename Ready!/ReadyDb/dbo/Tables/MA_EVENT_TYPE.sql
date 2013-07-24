CREATE TABLE [dbo].[MA_EVENT_TYPE] (
    [ma_event_type_PK]          INT            IDENTITY (1, 1) NOT NULL,
    [name]                      NVARCHAR (200) NULL,
    [enum_name]                 NVARCHAR (50)  NULL,
    [ma_event_type_severity_FK] INT            NULL,
    CONSTRAINT [PK_MA_EVENT_TYPE] PRIMARY KEY CLUSTERED ([ma_event_type_PK] ASC)
);

