-- TaskManager Database Initialization Script
-- Creates Users and Tasks tables with proper constraints and audit columns

-- Drop existing tables if they exist (for development/testing)
IF OBJECT_ID('Tasks', 'U') IS NOT NULL DROP TABLE Tasks;
IF OBJECT_ID('Users', 'U') IS NOT NULL DROP TABLE Users;

-- Users Table
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    CreatedAt DATETIME DEFAULT GETUTCDATE()
);

-- Tasks Table
CREATE TABLE Tasks (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NULL,
    Status INT NOT NULL DEFAULT 1,  -- 1=Pending, 2=InProgress, 3=Done
    DueDate DATETIME NULL,
    UserId UNIQUEIDENTIFIER NOT NULL,
    CreatedAt DATETIME DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Tasks_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Create indexes for common queries
CREATE INDEX IX_Tasks_UserId ON Tasks(UserId);
CREATE INDEX IX_Tasks_Status ON Tasks(Status);

PRINT 'Database schema created successfully!';
