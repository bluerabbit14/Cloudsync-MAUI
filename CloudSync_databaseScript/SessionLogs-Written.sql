--sessionLogs TABLE TO STORE EACH SESSION DETAIL WITH USERID
create table sessionLogs
(
   SessionId INT IDENTITY(9000,1) PRIMARY KEY,
   UserId INT,
   LoginTime DATETIME DEFAULT GETDATE(),
   LogoutTime DATETIME,
   IPAddress VARCHAR(50),
   DeviceInfo VARCHAR(255)
);


--fetch all session data from sessionlogs table
--GET/api/Session
SELECT* FROM sessionlogs; 


--fetch individual data from sessionlogs table
--GET/api/Session/{id}    //example: sessionId=9000
Select* from sessionlogs
where SessionId=9000;


--To MAKE a new Session entry in sessionlogs
--POST/api/Session/Session-Resistor  
CREATE PROCEDURE AddSession 
    @UserId INT,
    @LoginTime DATETIME,
	@LogoutTime DATETIME,
	@IPAddress VARCHAR(50),
	@DeviceInfo VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO sessionLogs (UserId, LoginTime)
    VALUES (@UserId, @LoginTime);
END;
GO  

EXEC AddSession @UserId = 1,@LoginTime=GETDATE,@LogoutTime=null,@IPAddress=null,@DeviceInfo=null;
--I Have Skiped the rest Column as they can be null and can be updated later



--TO UPDATE LogoutTime WHEN SESSION ENDS 
--PUT/api/Session/LogoutTime-Updat/{id}    //example sessionId=9000
CREATE PROCEDURE updateLogoutTime
    @SessionId INT
AS
BEGIN
    SET NOCOUNT ON; --suppress the updation message to user

    UPDATE sessionlogs
    SET LogoutTime = GETDATE()
    WHERE SessionId = @SessionId;
END;
Go

EXEC updateLogoutTime @SessionId=9000;