CREATE TABLE [dbo].[TASK] (
    [task_PK]                       INT            IDENTITY (1, 1) NOT NULL,
    [activity_FK]                   INT            NULL,
    [user_FK]                       INT            NULL,
    [task_name_FK]                  INT            NULL,
    [description]                   NVARCHAR (MAX) NULL,
    [comment]                       NVARCHAR (MAX) NULL,
    [type_internal_status_FK]       INT            NULL,
    [start_date]                    DATETIME       NULL,
    [expected_finished_date]        DATETIME       NULL,
    [actual_finished_date]          DATETIME       NULL,
    [POM_internal_status]           INT            NULL,
    [pom_count]                     NVARCHAR (50)  NULL,
    [pom_MAN]                       NVARCHAR (50)  NULL,
    [automatic_alerts_on]           BIT            CONSTRAINT [DF_TASK_automatic_alerts_on] DEFAULT ((1)) NOT NULL,
    [prevent_start_date_alert]      BIT            CONSTRAINT [DF_TASK_prevent_automatic_alert] DEFAULT ((0)) NOT NULL,
    [prevent_exp_finish_date_alert] BIT            CONSTRAINT [DF_TASK_prevent_exp_finish_date_alert] DEFAULT ((0)) NOT NULL,
    [task_ID]                       NVARCHAR (100) NULL,
    CONSTRAINT [PK_TASK] PRIMARY KEY CLUSTERED ([task_PK] ASC),
    CONSTRAINT [FK_TASK_ACTIVITY_1] FOREIGN KEY ([activity_FK]) REFERENCES [dbo].[ACTIVITY] ([activity_PK])
);



