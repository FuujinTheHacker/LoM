
-- CREATE TABLE

CREATE TABLE [dbo].[ActivationKey] (
    [ID]      BIGINT     IDENTITY (1, 1) NOT NULL,
    [Typ]     TINYINT    NOT NULL,
    [User_ID] BIGINT     NOT NULL,
    [Key]     nvarchar (100) NOT NULL,
    [TS]      DATETIME   DEFAULT (getdate()) NOT NULL,
    --CONSTRAINT [PK_ActivationKey] PRIMARY KEY CLUSTERED ([ID] ASC),
    --CONSTRAINT [uc_ActivationKey_Key] UNIQUE NONCLUSTERED ([Key] ASC),
    --CONSTRAINT [FK_ActivationKey_User] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[Log] (
    [ID]      BIGINT            IDENTITY (1, 1) NOT NULL,
    [User_ID] BIGINT         NOT NULL,
    [IP]      VARBINARY (16) NOT NULL,
    [TS]      DATETIME       DEFAULT (getdate()) NOT NULL,
    --CONSTRAINT [PK_Table] PRIMARY KEY CLUSTERED ([ID] ASC),
    --CONSTRAINT [FK_Log_User] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[User] ([ID])
);

CREATE TABLE [dbo].[Product] (
    [ID]			BIGINT       IDENTITY (1, 1) NOT NULL,
    [DisplayName]	nvarchar(45)	NOT NULL,
	[ProductDesc]	nvarchar(200)	NOT NULL,
	[ProductPrice]	float			NOT NULL,
	[Imagealink]	nvarchar(45)	NULL,
    [TS]			DATETIME		DEFAULT(getdate())		NOT NULL,
    --CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([ID] ASC),
    --CONSTRAINT [uc_Product_DisplayName] UNIQUE NONCLUSTERED ([DisplayName] ASC)
);

CREATE TABLE [dbo].[Product_has_TagTyp] (
    [ID]         BIGINT   IDENTITY (1, 1) NOT NULL,
    [Product_ID] BIGINT   NOT NULL,
    [TagTyp_ID]  BIGINT   NOT NULL,
    [TS]         DATETIME DEFAULT (getdate()) NOT NULL,
    --CONSTRAINT [PK_Product_has_TagTyp] PRIMARY KEY CLUSTERED ([ID] ASC),
	--CONSTRAINT [uc_Product_has_TagTyp_Product_ID_and_TagTyp_ID] UNIQUE CLUSTERED ([Product_ID] ASC,[TagTyp_ID] ASC),
    --CONSTRAINT [FK_Product_has_TagTyp_Product_ID] FOREIGN KEY ([Product_ID]) REFERENCES [dbo].[Product] ([ID]) ON DELETE CASCADE,
    --CONSTRAINT [FK_Product_has_TagTyp_TagTyp_ID] FOREIGN KEY ([TagTyp_ID]) REFERENCES [dbo].[TagTyp] ([ID]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[ProductAssociatedFile] (
    [ID]          BIGINT       IDENTITY (1, 1) NOT NULL,
    [DisplayName] nvarchar (45) NOT NULL,
    [Product_ID]  BIGINT       NOT NULL,
    [FilePath]    nvarchar (65) NOT NULL,
    [TS]          DATETIME     DEFAULT (getdate()) NOT NULL,
    --CONSTRAINT [PK_ProductAssociatedFile] PRIMARY KEY CLUSTERED ([ID] ASC),
    --CONSTRAINT [uc_ProductAssociatedFile_FilePath] UNIQUE NONCLUSTERED ([FilePath] ASC),
    --CONSTRAINT [uc_ProductAssociatedFile_DisplayName] UNIQUE NONCLUSTERED ([DisplayName] ASC),
    --CONSTRAINT [FK_ProductAssociatedFile_Product] FOREIGN KEY ([Product_ID]) REFERENCES [dbo].[Product] ([ID])
);

CREATE TABLE [dbo].[Link] (
    [ID]          BIGINT       IDENTITY (1, 1) NOT NULL,
    [Product_ID]  BIGINT       NULL,
    [ProductAssociatedFile_ID] BIGINT   NULL,
	[Key]		  nvarchar (100) NOT NULL,
    [TS]          DATETIME     DEFAULT (getdate()) NOT NULL,
    --CONSTRAINT [PK_Link] PRIMARY KEY CLUSTERED ([ID] ASC),
	--CONSTRAINT [FK_Link_Product_ID] FOREIGN KEY ([Product_ID]) REFERENCES [dbo].[Product] ([ID]) ON DELETE CASCADE
	--CONSTRAINT [FK_Link_ProductAssociatedFile_ID] FOREIGN KEY ([ProductAssociatedFile_ID]) REFERENCES [dbo].[ProductAssociatedFile] ([ID]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[TagTyp] (
    [ID]          BIGINT       IDENTITY (1, 1) NOT NULL,
    [DisplayName] nvarchar (45) NOT NULL,
    [TS]          DATETIME     DEFAULT (getdate()) NOT NULL,
    --CONSTRAINT [PK_TagTyp] PRIMARY KEY CLUSTERED ([ID] ASC),
    --CONSTRAINT [uc_TagTyp_DisplayName] UNIQUE NONCLUSTERED ([DisplayName] ASC)
);

CREATE TABLE [dbo].[User] (
    [ID]        BIGINT       IDENTITY (1, 1) NOT NULL,
    [OrgName]   nvarchar (50) NOT NULL,
    [Tele]      nvarchar (50) NOT NULL,
    [UserName]  nvarchar (50) NOT NULL,
    [Activated] BIT       DEFAULT (0) NOT NULL,
    [Pw]        BINARY (64) DEFAULT(CONVERT(BINARY(64),'')) NOT NULL ,
    [Salt]      INT          NOT NULL,
    [Epost]     nvarchar (70) NOT NULL,
    [TempKey]   nvarchar (100)   NULL,
    [TS]        DATETIME     DEFAULT (getdate()) NOT NULL,
    --CONSTRAINT [uc_User_Epost] UNIQUE NONCLUSTERED ([Epost] ASC),
    --CONSTRAINT [uc_User_OrgName] UNIQUE NONCLUSTERED ([OrgName] ASC),
    --CONSTRAINT [uc_User_UserName] UNIQUE NONCLUSTERED ([UserName] ASC)
);

-- PRIMARY KEY

ALTER TABLE [dbo].[ActivationKey]
ADD 
CONSTRAINT [PK_ActivationKey] PRIMARY KEY CLUSTERED ([ID] ASC);

ALTER TABLE [dbo].[Log]
ADD 
CONSTRAINT [PK_Table] PRIMARY KEY CLUSTERED ([ID] ASC);

ALTER TABLE [dbo].[Product]
ADD 
CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([ID] ASC);

ALTER TABLE [dbo].[Product_has_TagTyp]
ADD 
CONSTRAINT [PK_Product_has_TagTyp] PRIMARY KEY CLUSTERED ([ID] ASC);

ALTER TABLE [dbo].[ProductAssociatedFile]
ADD 
CONSTRAINT [PK_ProductAssociatedFile] PRIMARY KEY CLUSTERED ([ID] ASC);

ALTER TABLE [dbo].[TagTyp]
ADD 
CONSTRAINT [PK_TagTyp] PRIMARY KEY CLUSTERED ([ID] ASC);

ALTER TABLE [dbo].[User]
ADD 
CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC);

ALTER TABLE [dbo].[Link]
ADD 
CONSTRAINT [PK_Link] PRIMARY KEY CLUSTERED ([ID] ASC);

-- CONSTRAINT

ALTER TABLE [dbo].[ActivationKey]
ADD 
CONSTRAINT [uc_ActivationKey_Key] UNIQUE NONCLUSTERED  ([Key] ASC),
CONSTRAINT [FK_ActivationKey_User] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[User] ([ID]) ON DELETE CASCADE;

ALTER TABLE [dbo].[Log]
ADD 
CONSTRAINT [FK_Log_User] FOREIGN KEY ([User_ID]) REFERENCES [dbo].[User] ([ID]);

ALTER TABLE [dbo].[Product]
ADD 
CONSTRAINT [uc_Product_DisplayName] UNIQUE NONCLUSTERED  ([DisplayName] ASC);

ALTER TABLE [dbo].[Product_has_TagTyp]
ADD 
CONSTRAINT [uc_Product_has_TagTyp_Product_ID_and_TagTyp_ID] UNIQUE NONCLUSTERED  ([Product_ID] ASC,[TagTyp_ID] ASC),
CONSTRAINT [FK_Product_has_TagTyp_Product_ID] FOREIGN KEY ([Product_ID]) REFERENCES [dbo].[Product] ([ID]) ON DELETE CASCADE,
CONSTRAINT [FK_Product_has_TagTyp_TagTyp_ID] FOREIGN KEY ([TagTyp_ID]) REFERENCES [dbo].[TagTyp] ([ID]) ON DELETE CASCADE;

ALTER TABLE [dbo].[ProductAssociatedFile]
ADD 
CONSTRAINT [uc_ProductAssociatedFile_FilePath] UNIQUE NONCLUSTERED  ([FilePath] ASC),
CONSTRAINT [uc_ProductAssociatedFile_DisplayName] UNIQUE NONCLUSTERED  ([DisplayName] ASC),
CONSTRAINT [FK_ProductAssociatedFile_Product] FOREIGN KEY ([Product_ID]) REFERENCES [dbo].[Product] ([ID]);

ALTER TABLE [dbo].[TagTyp]
ADD 
CONSTRAINT [uc_TagTyp_DisplayName] UNIQUE NONCLUSTERED  ([DisplayName] ASC);

ALTER TABLE [dbo].[User]
ADD 
CONSTRAINT [uc_User_Epost] UNIQUE NONCLUSTERED  ([Epost] ASC),
CONSTRAINT [uc_User_OrgName] UNIQUE NONCLUSTERED  ([OrgName] ASC),
CONSTRAINT [uc_User_UserName] UNIQUE NONCLUSTERED  ([UserName] ASC);

ALTER TABLE [dbo].[Link]
ADD 
CONSTRAINT [FK_Link_Product_ID] FOREIGN KEY ([Product_ID]) REFERENCES [dbo].[Product] ([ID]) ON DELETE CASCADE,
CONSTRAINT [FK_Link_ProductAssociatedFile_ID] FOREIGN KEY ([ProductAssociatedFile_ID]) REFERENCES [dbo].[ProductAssociatedFile] ([ID]) ON DELETE CASCADE;