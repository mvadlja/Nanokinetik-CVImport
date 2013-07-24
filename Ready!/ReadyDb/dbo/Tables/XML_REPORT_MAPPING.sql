CREATE TABLE [dbo].[XML_REPORT_MAPPING] (
    [xml_report_mapping_PK] INT            IDENTITY (1, 1) NOT NULL,
    [xml_tag]               NVARCHAR (100) NULL,
    [display_tag]           NVARCHAR (100) NULL,
    CONSTRAINT [PK_XML_REPORT_MAPPING] PRIMARY KEY CLUSTERED ([xml_report_mapping_PK] ASC)
);

