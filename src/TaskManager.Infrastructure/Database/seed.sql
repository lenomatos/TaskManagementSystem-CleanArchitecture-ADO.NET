-- TaskManager Database Seed Script
-- Inserts demo data for development/testing

-- Insert demo user
-- ⚠️ NOTE: Password should be hashed using BCrypt before inserting into production
-- BCrypt hash of "demo123": $2a$11$encrypted_hash_here
-- For development purposes, using placeholder - update with actual BCrypt hash in production

DECLARE @DemoUserId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Users (Id, Username, PasswordHash)
VALUES 
    (@DemoUserId, 'demo', '$2a$11$N9qo8uLOickgx2ZMRZoMye'); -- Replace with actual BCrypt hash

-- Insert sample tasks for demo user
INSERT INTO Tasks (Id, Title, Description, Status, DueDate, UserId)
VALUES
    (NEWID(), 'Setup Project', 'Initialize the TaskManager project structure', 2, DATEADD(DAY, 7, GETUTCDATE()), @DemoUserId),
    (NEWID(), 'Database Design', 'Design and implement database schema', 3, DATEADD(DAY, -2, GETUTCDATE()), @DemoUserId),
    (NEWID(), 'API Development', 'Build REST API endpoints', 1, DATEADD(DAY, 14, GETUTCDATE()), @DemoUserId),
    (NEWID(), 'Unit Tests', 'Write unit tests for services', 1, DATEADD(DAY, 21, GETUTCDATE()), @DemoUserId),
    (NEWID(), 'Documentation', 'Create API documentation', 1, DATEADD(DAY, 30, GETUTCDATE()), @DemoUserId);

PRINT 'Seed data inserted successfully!';

-- SECURITY NOTE:
-- In production, replace the PasswordHash with a real BCrypt hash.
-- Use BCrypt.Net-Next NuGet package to generate:
-- string hashedPassword = BCrypt.Net.BCrypt.HashPassword("demo123");
-- Then insert that hash into the database.
