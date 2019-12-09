USE [Service]
INSERT INTO Employees([FirstName],[LastName],[Birthdate] ,[DepartmentId]) VALUES
('Marlo','O''Malley','1958-9-21',1),
('Niki','Stanaghan','1969-11-26',4),
('Ayrton','Senna','1960-03-21',9),
('Ronnie','Peterson','1944-02-14',9),
('Giovanna','Amati','1959-07-20',5)

INSERT INTO Reports([CategoryId],[StatusId],[OpenDate],[CloseDate],[Description],[UserId],[EmployeeId])VALUES
(1,	1,'2017-04-13','',	'Stuck Road on Str.133',	6,	2),
(6,3,'2015-09-05','2015-12-06',	'Charity trail running',	3,	5),
(14,2,	'2015-09-07',	'',	'Falling bricks on Str.58'	,5	,2),
(4,	3	,'2017-07-03'	,'2017-07-06',	'Cut off streetlight on Str.11',	1,	1)


UPDATE Reports
SET CloseDate = GETDATE()
WHERE CLoseDate IS NULL


DELETE FROM Reports
WHERE StatusId IN(
                   SELECT Id FROM [Status]
				   WHERE Id=4
 )

 SELECT Description ,  FORMAT(OpenDate,'dd-MM-yyyy')
 FROM Reports
 WHERE EmployeeId IS NULL
 ORDER BY OpenDate , Description

 SELECT r.Description , c.Name AS CategoryName
 FROM Reports as r
 JOIN Categories as c
 ON r.CategoryId = c.Id
 ORDER BY r.Description , c.Name

 SELECT TOP(5) c.Name , COunt(r.Id) AS ReportsNumber
  FROM Reports as r
  JOIN Categories as c
  ON r.CategoryId = c.Id
  GROUP BY c.Name
  ORDER BY ReportsNumber DESC , c.Name

  
  SELECT u.Username , c.Name AS CategoryName
  FROM Users AS u
  JOIN Reports AS r 
  ON u.Id = r.UserId
  JOIN Categories AS c 
  ON c.Id = r.CategoryId
 WHERE FORMAT(r.OpenDate, 'dd-MM') = FORMAT(u.Birthdate, 'dd-MM')
 ORDER BY u.Username , CategoryName


  SELECT CONCAT(e.FirstName, ' ', e.LastName) AS FullName, COUNT(u.Id) AS UsersCount
     FROM Employees AS e
LEFT JOIN Reports AS r ON r.EmployeeId = e.Id
LEFT JOIN Users AS u ON r.UserId = u.Id
 GROUP BY e.FirstName, e.LastName
 ORDER BY UsersCount DESC, FullName


 SELECT IIF(e.firstName IS NULL AND e.lastName IS NULL, 'None', CONCAT(e.FirstName, ' ', e.LastName)) AS Employee, 
          IIF(d.[Name] IS NULL, 'None', d.[Name]) AS Department, 
          c.[Name] AS Category, 
          r.[Description], 
          FORMAT(r.OpenDate, 'dd.MM.yyyy') AS OpenDate, 
          s.Label AS [Status], 
          IIF(u.[Name] IS NULL, 'None', u.[Name])  AS [User]
     FROM Reports AS r
LEFT JOIN Categories AS c ON r.CategoryId = c.Id
LEFT JOIN Employees AS e ON r.EmployeeId = e.Id
LEFT JOIN Departments AS d on d.Id = e.DepartmentId
LEFT JOIN Users AS u ON r.UserId = u.Id
LEFT JOIN Status AS s ON s.Id = r.StatusId
 ORDER BY e.FirstName DESC, e.LastName DESC, d.[Name], c.[Name], r.[Description], CONVERT(DATETIME, OpenDate, 101), s.Label, u.[Name]

 
 CREATE FUNCTION udf_HoursToComplete(@StartDate DATETIME, @EndDate DATETIME)
RETURNS INT
AS
BEGIN
	IF(@StartDate IS NULL OR @EndDate IS NULL)
	BEGIN
		RETURN 0
	END

	RETURN DATEDIFF(HOUR, @StartDate, @EndDate)
END

GO

CREATE PROC usp_AssignEmployeeToReport(@EmployeeId INT, @ReportId INT) 
AS
BEGIN
	DECLARE @employeeDepartmentId INT = (SELECT DepartmentId 
										   FROM Employees 
										  WHERE Id = @EmployeeId)

	DECLARE @reportDepartmentId INT = (SELECT TOP(1) d.Id 
											 FROM Reports AS r 
											 JOIN Categories AS c ON r.CategoryId = c.Id 
											 JOIN Departments AS d ON c.DepartmentId = d.Id 
											WHERE r.id = @ReportId) 

	IF(@employeeDepartmentId <> @reportDepartmentId)
	BEGIN
		RAISERROR('Employee doesn''t belong to the appropriate department!', 16, 1)
		RETURN
	END

	UPDATE Reports
	SET EmployeeId = @employeeDepartmentId
	WHERE Id = @ReportId
END
   
