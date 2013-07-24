CREATE TABLE [dbo].[MA_EVENT_TYPE_SEVERITY] (
    [ma_event_type_severity_PK] INT            IDENTITY (1, 1) NOT NULL,
    [name]                      NVARCHAR (200) NULL,
    [enum_name]                 NVARCHAR (50)  NULL,
    CONSTRAINT [PK_MA_EVENT_TYPE_SEVERITY] PRIMARY KEY CLUSTERED ([ma_event_type_severity_PK] ASC)
);

