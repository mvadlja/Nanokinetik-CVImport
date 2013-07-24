CREATE TABLE [dbo].[TIME_UNIT] (
    [time_unit_PK]      INT            IDENTITY (1, 1) NOT NULL,
    [task_FK]           INT            NULL,
    [user_FK]           INT            NULL,
    [time_unit_name_FK] INT            NULL,
    [description]       NVARCHAR (MAX) NULL,
    [comment]           NVARCHAR (MAX) NULL,
    [actual_date]       DATETIME       NULL,
    [time_hours]        INT            NULL,
    [time_minutes]      INT            NULL,
    [activity_FK]       INT            NULL,
    [inserted_by]       INT            NULL,
    CONSTRAINT [PK_TIME_UNIT] PRIMARY KEY CLUSTERED ([time_unit_PK] ASC),
    CONSTRAINT [FK_TIME_UNIT_ACTIVITY_1] FOREIGN KEY ([activity_FK]) REFERENCES [dbo].[ACTIVITY] ([activity_PK])
);




GO
CREATE NONCLUSTERED INDEX [IX_TIME_UNIT_ACTUAL_DATE]
    ON [dbo].[TIME_UNIT]([actual_date] ASC, [activity_FK] ASC);


GO
CREATE NONCLUSTERED INDEX [NonClusteredIndex-20121207-093310]
    ON [dbo].[TIME_UNIT]([time_unit_name_FK] ASC);

