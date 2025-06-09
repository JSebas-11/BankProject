CREATE DATABASE BankProject;
GO
USE BankProject;
GO

--TABLES CREATION
CREATE TABLE InvStatus (
	invId INT PRIMARY KEY IDENTITY(1, 1),
	typeDesc VARCHAR(24) UNIQUE NOT NULL
);

CREATE TABLE TransType (
	transTypeId INT PRIMARY KEY IDENTITY(1, 1),
	typeDesc VARCHAR(24) UNIQUE NOT NULL
);

CREATE TABLE Branches (
	branchId INT PRIMARY KEY IDENTITY(1, 1),
	branchLocation VARCHAR(24) UNIQUE NOT NULL
);

CREATE TABLE UserAccount (
	userId INT PRIMARY KEY IDENTITY(1, 1),
	number VARCHAR(10) UNIQUE NOT NULL,
	hashPassword VARCHAR(60) NOT NULL,
	ownerName VARCHAR(24) NOT NULL,
	funds Decimal(10, 2) NOT NULL,
	investedMoney Decimal(10, 2) NOT NULL,
	registeredAt DATETIME
);

CREATE TABLE AdminAccount (
	adminId INT PRIMARY KEY IDENTITY(1, 1),
	number VARCHAR(10) UNIQUE NOT NULL,
	hashPassword VARCHAR(60) NOT NULL,
	ownerName VARCHAR(24) NOT NULL
);

CREATE TABLE Trans (
    transId INT PRIMARY KEY IDENTITY(1, 1),
    transType INT,
	branchId INT,
	senderId INT,
    receiverId INT,
    amount DECIMAL(10,2) NOT NULL,
    dateTrans DATETIME NOT NULL,
    
    CONSTRAINT FK_Trans_TransType FOREIGN KEY (transType) REFERENCES TransType(transTypeId) ON DELETE NO ACTION,
	CONSTRAINT FK_Trans_Branches_BranchId FOREIGN KEY (branchId) REFERENCES Branches(branchId) ON DELETE NO ACTION,
    CONSTRAINT FK_Trans_UserAccount_Sender FOREIGN KEY (senderId) REFERENCES UserAccount(userId) ON DELETE NO ACTION,
	CONSTRAINT FK_Trans_UserAccount_Receiver FOREIGN KEY (receiverId) REFERENCES UserAccount(userId) ON DELETE NO ACTION
);

CREATE TABLE TransHistory (
    histId INT PRIMARY KEY IDENTITY(1, 1),
    userId INT,
	transId INT,
    
	CONSTRAINT FK_TransHistory_UserAccount FOREIGN KEY (userId) REFERENCES UserAccount(userId) ON DELETE NO ACTION,
    CONSTRAINT FK_TransHistory_Trans FOREIGN KEY (transId) REFERENCES Trans(transId) ON DELETE NO ACTION
);

CREATE TABLE CDT (
    cdtId INT PRIMARY KEY IDENTITY(1, 1),
	cdtStatus INT,
    userId INT,
    amount DECIMAL(10,2),
	profit DECIMAL(10, 2),
    interestRate DECIMAL(4, 2) NOT NULL,
    durationMonths INT NOT NULL,
    startDate DATETIME NOT NULL,
    endDate DATETIME NOT NULL,
    
    CONSTRAINT FK_CDT_InvStatus FOREIGN KEY (cdtStatus) REFERENCES InvStatus(invId) ON DELETE NO ACTION,
	CONSTRAINT FK_CDT_UserAccount FOREIGN KEY (userId) REFERENCES UserAccount(userId) ON DELETE NO ACTION
);

CREATE TABLE UserInfo(
	id INT PRIMARY KEY IDENTITY(1, 1),
	number VARCHAR(10) UNIQUE NOT NULL,
	ownerName VARCHAR(24) NOT NULL,
	registeredAt DATETIME NULL,
	deletedAt DATETIME NULL
);

GO

--TRIGGERS CREATION

CREATE TRIGGER Trans_AI
	ON Trans AFTER INSERT AS
	BEGIN
		INSERT INTO TransHistory (userId, transId)
			SELECT senderId, transId FROM inserted
	END;
GO

CREATE TRIGGER UserAccount_AI
	ON UserAccount AFTER INSERT AS
	BEGIN
		INSERT INTO UserInfo (number, ownerName, registeredAt)
			SELECT number, ownerName, registeredAt FROM inserted
	END;
GO

CREATE TRIGGER UserAccount_AU
	ON UserAccount AFTER UPDATE AS
	BEGIN
		UPDATE UserInfo 
			SET number = (SELECT TOP 1 number FROM inserted),
			ownerName = (SELECT TOP 1 ownerName FROM inserted)
			WHERE number = (SELECT TOP 1 number FROM deleted)
	END;
GO

CREATE TRIGGER UserAccount_AD
	ON UserAccount AFTER DELETE AS
	BEGIN
		UPDATE UserInfo SET deletedAt = GETDATE()
		WHERE number = (SELECT number FROM deleted)
	END;

--STATIC DATA INSERTION

INSERT INTO InvStatus
VALUES ('Cancelled'), ('Active'), ('Completed');

INSERT INTO TransType 
VALUES ('Withdrawal'), ('Deposit'), ('Transfer'), ('Investment');

INSERT INTO Branches
VALUES ('Medellin'), ('Itagui'), ('Envigado'), ('Bello'), ('Sabaneta'), ('La Estrella'), ('Aguadas'), ('Segovia'), ('Digital');