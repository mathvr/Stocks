IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='STOCKOVERVIEW' AND XTYPE='U')
    CREATE TABLE STOCKOVERVIEW (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID() NOT NULL,
        NAME VARCHAR(255) NOT NULL, 
        SYMBOL VARCHAR(255) NOT NULL,
        CIK INT UNIQUE NOT NULL, 
        INDUSTRY VARCHAR(255) NOT NULL, 
        EXCHANGE VARCHAR(255) NOT NULL,
        DESCRIPTION TEXT NULL, 
        CURRENCY VARCHAR(255) NOT NULL, 
        COUNTRY VARCHAR(255) NOT NULL, 
        SECTOR VARCHAR(255) NOT NULL, 
        FISCALYEAREND VARCHAR(255) NOT NULL, 
        MARKETCAPITALIZATION DECIMAL(20,2),
        BOOKVALUE DECIMAL(20, 2), 
        PROFITMARGIN DECIMAL(20,2),
        LATESTQUARTED DATETIMEOFFSET, 
        CREATEDBY VARCHAR(255) NULL, 
        CREATEDON DATETIMEOFFSET NULL, 
        MODIFIEDBY VARCHAR(255) NULL, 
        MODIFIEDON DATETIMEOFFSET NULL
    )
GO

IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='STOCKHISTORY' AND XTYPE='U')
    CREATE TABLE STOCKHISTORY (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID() NOT NULL,
        STOCKOVERVIEWID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES STOCKOVERVIEW(ID) NOT NULL,
        OPENVALUE DECIMAL (20,2) NOT NULL, 
        CLOSEVALUE DECIMAL (20,2) NULL, 
        DATE DATETIMEOFFSET NOT NULL,
        CREATEDBY VARCHAR(255) NULL, 
        CREATEDON DATETIMEOFFSET NULL, 
        MODIFIEDBY VARCHAR(255) NULL, 
        MODIFIEDON DATETIMEOFFSET NULL
    )
GO

IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='APPUSER' AND XTYPE='U')
    CREATE TABLE APPUSER (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID() NOT NULL,
        USERNUMBER VARCHAR(255) UNIQUE NOT NULL, 
        FIRSTNAME VARCHAR(255) NULL, 
        LASTNAME VARCHAR(255) NULL,
        EMAIL VARCHAR(255) NOT NULL, 
        PASSWORD VARCHAR(255) NOT NULL, 
        ACCESSLEVEL VARCHAR(255) NOT NULL, 
        CREATEDBY VARCHAR(255) NULL, 
        CREATEDON DATETIMEOFFSET NULL, 
        MODIFIEDBY VARCHAR(255) NULL, 
        MODIFIEDON DATETIMEOFFSET NULL
    )
GO

IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='CONNECTION' AND XTYPE='U')
    CREATE TABLE CONNECTION (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID() NOT NULL,
        NAME VARCHAR(255) NOT NULL, 
        BASEURL VARCHAR(255) NULL,
        CLIENTSECRET VARCHAR(255) NULL,
        CREATEDBY VARCHAR(255) NULL, 
        CREATEDON DATETIMEOFFSET NULL, 
        MODIFIEDBY VARCHAR(255) NULL, 
        MODIFIEDON DATETIMEOFFSET NULL
    )
GO
        
IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='STOCKOVERVIEW_APPUSER' AND XTYPE='U')
CREATE TABLE STOCKOVERVIEW_APPUSER
(
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID() NOT NULL,
    APPUSERID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES APPUSER(ID) NOT NULL,
    STOCKOVERVIEWID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES STOCKOVERVIEW(ID) NOT NULL
)
GO