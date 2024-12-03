USE master;
GO

--delete the database if it exists
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'TaskManagement')
BEGIN
    ALTER DATABASE TaskManagement SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE TaskManagement;
END

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

-- Insert some sample data
INSERT INTO Tasks (TaskName, Category, Description, DueDate, PriorityLevel, Status)
VALUES ('Task 1', 'School', 'Description 1', '2024-12-03', 'High', 'In Progress'),
       ('Task 2', 'Work', 'Description 2', '2024-12-03', 'Medium', 'Not Started'),
       ('Task 3', 'Home', 'Description 3', '2024-12-03', 'Low', 'Completed'),
       ('Task 4', 'Personal', 'Description 4', '2024-12-04', 'High', 'In Progress'),
       ('Task 5', 'School', 'Description 5', '2024-12-04', 'Medium', 'Not Started'),
       ('Task 6', 'Work', 'Description 6', '2024-12-04', 'Low', 'Completed'),
       ('Task 7', 'Home', 'Description 7', '2024-12-04', 'High', 'In Progress'),
       ('Task 8', 'Personal', 'Description 8', '2024-12-04', 'Medium', 'Not Started'),
       ('Task 9', 'School', 'Description 9', '2024-12-04', 'Low', 'Completed'),
       ('Task 10', 'Work', 'Description 10', '2024-12-04', 'High', 'In Progress');
GO

