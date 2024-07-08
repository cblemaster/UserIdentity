USE master
GO

DECLARE @SQL nvarchar(1000);
IF EXISTS (SELECT 1 FROM sys.databases WHERE name = N'UserIdentity')
BEGIN
    SET @SQL = N'USE UserIdentity;

                 ALTER DATABASE UserIdentity SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                 USE master;

                 DROP DATABASE UserIdentity;';
    EXEC (@SQL);
END;

CREATE DATABASE UserIdentity
GO

USE UserIdentity
GO

CREATE TABLE [User] (
	UserId INT IDENTITY(1,1) NOT NULL,
	Username VARCHAR(50) NOT NULL,
	PasswordHash VARCHAR(200) NOT NULL,
	Salt VARCHAR(200) NOT NULL,
	FirstName VARCHAR(255) NOT NULL,
	LastName VARCHAR(255) NOT NULL,
	Email VARCHAR(255) NOT NULL,
	Phone CHAR(10) NOT NULL,
	CreateDate DATETIME NOT NULL,
	UpdateDate DATETIME NULL,
	CONSTRAINT PK_User PRIMARY KEY(UserId),
	CONSTRAINT UC_User_Username UNIQUE(Username),
	CONSTRAINT UC_User_Email UNIQUE(Email),
	CONSTRAINT UC_User_Phone UNIQUE(Phone),
)
GO

CREATE TABLE Role (
	RoleId INT IDENTITY(1,1) NOT NULL,
	Role VARCHAR(50) NOT NULL,
	CONSTRAINT PK_Role PRIMARY KEY(RoleID),
	CONSTRAINT UC_Role_Role UNIQUE(Role),
)
GO

-- optional sample data
--INSERT INTO Role(Role) VALUES ('Admin');
--INSERT INTO Role(Role) VALUES ('Customer');
--INSERT INTO Role(Role) VALUES ('Vendor');
--INSERT INTO Role(Role) VALUES ('Sales');
--INSERT INTO Role(Role) VALUES ('Marketing');
--INSERT INTO Role(Role) VALUES ('Reporting');
--INSERT INTO Role(Role) VALUES ('Support');

CREATE TABLE RoleUser (
	UserId INT NOT NULL,
	RoleId INT NOT NULL,
	CONSTRAINT PK_RoleUser PRIMARY KEY (UserId, RoleId),
	CONSTRAINT FK_RoleUser_User FOREIGN KEY(UserId) REFERENCES [User](UserId),
	CONSTRAINT FK_RoleUser_Role FOREIGN KEY(RoleId) REFERENCES Role(RoleId),
)
GO

