CREATE TABLE [dbo].[PP_MEDICAL_DEVICE] (
    [medicaldevice_PK]  INT           IDENTITY (1, 1) NOT NULL,
    [medicaldevicecode] NVARCHAR (60) NULL,
    [ev_code]           NVARCHAR (50) NULL,
    CONSTRAINT [PK_PP_MEDICAL_DEVICE] PRIMARY KEY CLUSTERED ([medicaldevice_PK] ASC)
);

