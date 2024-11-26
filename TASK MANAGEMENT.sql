USE master;
GO

-- Set the database to single-user mode to disconnect all other users
ALTER DATABASE TaskManagement SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO


CREATE DATABASE TaskManagement;
GO

USE TaskManagement;
GO


CREATE TABLE Tasks (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TaskName NVARCHAR(100) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255),
    DueDate DATE,
    PriorityLevel NVARCHAR(20),
    Status NVARCHAR(20)
);
GO
