CREATE TABLE [dbo].[LOCATION] (
    [location_PK]          INT             IDENTITY (1, 1) NOT NULL,
    [unique_name]          NVARCHAR (450)  NOT NULL,
    [display_name]         NVARCHAR (1000) NULL,
    [description]          NVARCHAR (MAX)  NULL,
    [navigation_level]     INT             NULL,
    [generate_in_top_menu] BIT             NULL,
    [generate_in_tab_menu] BIT             NULL,
    [active]               BIT             NULL,
    [parent_unique_name]   NVARCHAR (450)  NULL,
    [location_target]      NVARCHAR (50)   NULL,
    [full_unique_path]     NVARCHAR (500)  NULL,
    [location_url]         NVARCHAR (500)  NULL,
    [old_location]         BIT             NULL,
    [menu_order]           INT             NULL,
    [show_location]        BIT             NULL,
    CONSTRAINT [PK_LOCATION] PRIMARY KEY CLUSTERED ([location_PK] ASC),
    CONSTRAINT [IX_LOCATION] UNIQUE NONCLUSTERED ([unique_name] ASC)
);

