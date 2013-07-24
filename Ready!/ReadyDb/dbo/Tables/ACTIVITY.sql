CREATE TABLE [dbo].[ACTIVITY] (
    [activity_PK]                   INT            IDENTITY (1, 1) NOT NULL,
    [user_FK]                       INT            NULL,
    [mode_FK]                       INT            NULL,
    [procedure_type_FK]             INT            NULL,
    [name]                          NVARCHAR (350) NULL,
    [description]                   NVARCHAR (MAX) NULL,
    [comment]                       NVARCHAR (MAX) NULL,
    [regulatory_status_FK]          INT            NULL,
    [start_date]                    DATETIME       NULL,
    [expected_finished_date]        DATETIME       NULL,
    [actual_finished_date]          DATETIME       NULL,
    [approval_date]                 DATETIME       NULL,
    [submission_date]               DATETIME       NULL,
    [procedure_number]              NVARCHAR (250) NULL,
    [legal]                         NVARCHAR (150) NULL,
    [cost]                          NVARCHAR (150) NULL,
    [internal_status_FK]            INT            NULL,
    [activity_ID]                   NVARCHAR (100) NULL,
    [automatic_alerts_on]           BIT            CONSTRAINT [DF_ACTIVITY_automatic_alerts_on] DEFAULT ((1)) NOT NULL,
    [prevent_start_date_alert]      BIT            CONSTRAINT [DF_ACTIVITY_prevent_automatic_alert] DEFAULT ((0)) NOT NULL,
    [prevent_exp_finish_date_alert] BIT            CONSTRAINT [DF_ACTIVITY_prevent_exp_finish_date_alert] DEFAULT ((0)) NOT NULL,
    [billable]                      BIT            NULL,
    CONSTRAINT [PK_ACTIVITY] PRIMARY KEY CLUSTERED ([activity_PK] ASC),
    CONSTRAINT [FK_ACTIVITY_ACTIVITY] FOREIGN KEY ([activity_PK]) REFERENCES [dbo].[ACTIVITY] ([activity_PK]),
    CONSTRAINT [FK_ACTIVITY_TYPE] FOREIGN KEY ([internal_status_FK]) REFERENCES [dbo].[TYPE] ([type_PK]),
    CONSTRAINT [FK_ACTIVITY_TYPE1] FOREIGN KEY ([procedure_type_FK]) REFERENCES [dbo].[TYPE] ([type_PK]),
    CONSTRAINT [FK_ACTIVITY_TYPE2] FOREIGN KEY ([regulatory_status_FK]) REFERENCES [dbo].[TYPE] ([type_PK]),
    CONSTRAINT [FK_ACTIVITY_TYPE3] FOREIGN KEY ([mode_FK]) REFERENCES [dbo].[TYPE] ([type_PK])
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [ui_Act]
    ON [dbo].[ACTIVITY]([activity_PK] ASC);

