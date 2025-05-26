--users TABLE TO STORE USER DATA
CREATE TABLE users
(
    UserId INT IDENTITY(1,1) PRIMARY KEY ,
	UserName VARCHAR(50) NOT Null,
	Password VARCHAR(100) NOT NULL,
	Email VARCHAR(100) UNIQUE NOT NULL,
	CreatedAt DATETIME DEFAULT GETDATE(),
	IsActive BIT DEFAULT 1 -- default true
);

--GET/api/User
SELECT* FROM users;

--GET/api/User/{id}
Select* FROM users
where UserId=1;

--Too delete the Individual user entry from users table
--DELETE/api/User/{id}
CREATE PROCEDURE DeleteUser
    @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM users
    WHERE UserId = @UserId;
END;
GO
EXEC DeleteUser @UserId = 1;

--Add NEW user individual in users table
--POST/api/User/register
CREATE PROCEDURE AddUser
    @UserName VARCHAR(50),
    @Password VARCHAR(100),
    @Email VARCHAR(100),
    @CreatedAt DATETIME,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO users (UserName, Password, Email, CreatedAt, IsActive)
    VALUES (@UserName, @Password, @Email, @CreatedAt, @IsActive);
END;
GO
EXEC AddUser @UserName = 'ASIF ABBAS', @Password = '#Asif001', @Email = '14asifcr7@gmail.com', @CreatedAt = GETDATE(),  @IsActive = 1;

--A Login it takes Email and Password  return user object with successful or error message
--POST/api/User/login
CREATE PROCEDURE CheckUserLogin
    @Email VARCHAR(100),
    @Password VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if a matching user exists
    IF EXISTS (
        SELECT 1 FROM users WHERE Email = @Email AND Password = @Password
    )
    BEGIN
        -- Return full user record along with a success message
        SELECT 
            'Login successful' AS Message,
            UserId,
            UserName,
            Email,
            CreatedAt,
            IsActive
        FROM users
        WHERE Email = @Email AND Password = @Password;
    END
    ELSE
    BEGIN
        -- Only return a message when no match is found
        SELECT 'Invalid email or password' AS Message;
    END
END;
GO
EXEC CheckUserLogin 
    @Email = '14asifcr7@gmail.com', 
    @Password = '#Asif001';


--PUT/api/User/forgot-password/{id}
CREATE PROCEDURE UpdatePassword
    @UserId INT,
    @Password VARCHAR(100)  -- Assuming password is stored as a string
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE users
    SET Password = @Password
    WHERE UserId = @UserId;
END;
GO

-- Now you can execute the procedure
EXEC UpdatePassword @UserId = 1, @Password = 'X001ASIF';
