CREATE TABLE [dbo].[TASK_NAME] (
    [task_name_PK] INT            IDENTITY (1, 1) NOT NULL,
    [task_name]    NVARCHAR (150) NULL,
    CONSTRAINT [PK_TASK_NAME] PRIMARY KEY CLUSTERED ([task_name_PK] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [uxTaskName]
    ON [dbo].[TASK_NAME]([task_name_PK] ASC);

