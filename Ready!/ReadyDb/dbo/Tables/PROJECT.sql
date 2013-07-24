CREATE TABLE [dbo].[PROJECT] (
    [project_PK]                    INT            IDENTITY (1, 1) NOT NULL,
    [user_FK]                       INT            NULL,
    [name]                          NVARCHAR (150) NULL,
    [comment]                       NVARCHAR (MAX) NULL,
    [start_date]                    DATETIME       NULL,
    [expected_finished_date]        DATETIME       NULL,
    [actual_finished_date]          DATETIME       NULL,
    [description]                   NVARCHAR (MAX) NULL,
    [internal_status_type_FK]       INT            NULL,
    [pom_prod]                      NVARCHAR (50)  NULL,
    [pom_cli]                       NVARCHAR (50)  NULL,
    [pom_count]                     NVARCHAR (50)  NULL,
    [pom_status]                    NVARCHAR (50)  NULL,
    [automatic_alerts_on]           BIT            CONSTRAINT [DF_PROJECT_automatic_alerts_on] DEFAULT ((1)) NOT NULL,
    [prevent_start_date_alert]      BIT            CONSTRAINT [DF_PROJECT_prevent_automatic_alert] DEFAULT ((0)) NOT NULL,
    [prevent_exp_finish_date_alert] BIT            CONSTRAINT [DF_PROJECT_prevent_exp_finish_date_alert] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PROJECT] PRIMARY KEY CLUSTERED ([project_PK] ASC)
);

