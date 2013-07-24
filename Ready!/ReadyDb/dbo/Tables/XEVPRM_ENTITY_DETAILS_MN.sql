CREATE TABLE [dbo].[XEVPRM_ENTITY_DETAILS_MN] (
    [xevprm_entity_details_mn_PK] INT IDENTITY (1, 1) NOT NULL,
    [xevprm_message_FK]           INT NULL,
    [xevprm_entity_details_FK]    INT NULL,
    [xevprm_entity_type_FK]       INT NULL,
    [xevprm_entity_FK]            INT NULL,
    [xevprm_operation_type]       INT NULL,
    CONSTRAINT [PK_XEVPRM_ENTITY_DETAILS_MN] PRIMARY KEY CLUSTERED ([xevprm_entity_details_mn_PK] ASC),
    CONSTRAINT [FK_XEVPRM_ENTITY_DETAILS_MN_XEVPRM_ENTITY_TYPE] FOREIGN KEY ([xevprm_entity_type_FK]) REFERENCES [dbo].[XEVPRM_ENTITY_TYPE] ([xevprm_entity_type_PK]),
    CONSTRAINT [FK_XEVPRM_ENTITY_DETAILS_MN_XEVPRM_MESSAGE] FOREIGN KEY ([xevprm_message_FK]) REFERENCES [dbo].[XEVPRM_MESSAGE] ([xevprm_message_PK])
);






GO
CREATE NONCLUSTERED INDEX [IX_XEVPRM_ENTITY_DETAILS_FK]
    ON [dbo].[XEVPRM_ENTITY_DETAILS_MN]([xevprm_entity_details_FK] ASC);

