CREATE TABLE [dbo].[STRUCT_REPRESENTATION_TYPE] (
    [struct_repres_type_PK] INT           IDENTITY (1, 1) NOT NULL,
    [name]                  NVARCHAR (50) NULL,
    CONSTRAINT [PK_STRUCT_REPRESENTATION_TYPE] PRIMARY KEY CLUSTERED ([struct_repres_type_PK] ASC)
);

