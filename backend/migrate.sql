--- ========== ------ ========== ------ ========== ------ ========== ------ ========== ---
-- NotesAppDbDb Migration Script
-- Run it while connected to any database
-- This script will create the NotesAppDb database and tables if they do not exist
-- We using Docker / SQL Server with (mssql) extension in VS Code
--- ========== ------ ========== ------ ========== ------ ========== ------ ========== ---

-- Create the NotesAppDb database if it does not exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'NotesAppDb')
BEGIN
    CREATE DATABASE NotesAppDb;
    -- Message if the database is created
    PRINT 'Database NotesAppDb created successfully.';
END
ELSE
BEGIN
    -- Message if the database already exists
    PRINT 'Database NotesAppDb already exists.';
END
GO

-- Because by default, SQL Server it have master database,
-- we need to switch to NotesAppDb database to create tables
use NotesAppDb;
GO

-- Create Users table if it does not exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users(
        id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
        Username NVARCHAR(100) NOT NULL,
        Email NVARCHAR(255) NOT NULL,
        Password NVARCHAR(255) NOT NULL, -- BCrypt hash, never plain text
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

        CONSTRAINT UQ_Users_Email UNIQUE (Email), -- Ensure email uniqueness
        CONSTRAINT UQ_Users_Username UNIQUE (Username) -- Ensure username uniqueness
    );

    CREATE INDEX IX_Users_Email ON Users (Email); -- Index for faster email lookups
    CREATE INDEX IX_Users_Username ON Users (Username); -- Index for faster username lookups

    -- Message if the Users table is created
    PRINT 'Table Users created successfully.';
END
ELSE
BEGIN
    -- Message if the Users table already exists
    PRINT 'Table Users already exists.';
END
GO


-- Create Notes table if it does not exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Notes')
BEGIN
    CREATE TABLE Notes(
        id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() PRIMARY KEY,
        UserId UNIQUEIDENTIFIER NOT NULL,
        Title NVARCHAR(255) NOT NULL,
        Content NVARCHAR(MAX) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

        CONSTRAINT FK_Notes_Users FOREIGN KEY (UserId)
            REFERENCES Users(id)
            ON DELETE CASCADE -- If a user is deleted, their notes it will deleted
    );

    CREATE INDEX IX_Notes_UserId ON Notes (UserId); -- Index for faster lookups of notes by user
    CREATE INDEX IX_Notes_CreatedAt ON Notes (CreatedAt DESC); -- Index for faster retrieval of recent notes

    -- Message if the Notes table is created
    PRINT 'Table Notes created successfully.';

END
ELSE
BEGIN
    -- Message if the Notes table already exists
    PRINT 'Table Notes already exists.';
END
GO

-- Verify everything was created successfully
SELECT
    t.name AS TableName, -- Table name
    c.name AS ColumnName, -- Column name
    tp.name AS DataType, -- Data type
    c.max_length as MaxLength, -- Max length for string types
    c.is_nullable AS ISNULLable -- Whether the column allows NULL values
FROM
    sys.tables t -- Get all tables in the current database
    JOIN sys.columns c ON t.object_id = c.object_id -- Join to get columns for each table
    JOIN sys.types tp ON c.user_type_id = tp.user_type_id -- Join to get data type information
WHERE
    t.name IN ('Users', 'Notes') -- Only show our relevant tables
ORDER BY
    t.name, c.column_id; -- Order by table and column order
GO

-- Final message to indicate the migration script has completed
PRINT '*****=====**********=====**********=====*****'
PRINT 'Migration completed! NotesAppDb is ready.'
PRINT '*****=====**********=====**********=====*****'