USE Airport

INSERT INTO Planes([Name],[Seats],[Range]) VALUES
('Airbus 336',112,5132),
('Airbus 330',432,5325),
('Boeing 369',231,2355),
('Stelt 297',254,2143),
('Boeing 338',165,5111),
('Airbus 558',387,1342),
('Boeing 128',345,5541)

INSERT INTO LuggageTypes([Type]) VALUES
('Crossbody Bag'),
('School Backpack'),
('Shoulder Bag')


UPDATE Tickets
SET Price*=1.13
WHERE FlightId IN(SELECT [Id] FROM Flights
WHERE Destination = 'Carlsbad'
)

DELETE FROM Tickets
WHERE FlightId IN(
                   SELECT Id FROM Flights
				   WHERE Destination='Ayn Halagim'
 )

 DELETE FROM Flights
 WHERE Destination = 'Ayn Halagim'

 SELECT *
 FROM Planes
 WHERE [Name] LIKE '%Tr%'
 ORDER BY [Id],[Name],[Seats],[Range]

 SELECT *
 FROM Planes
 WHERE CHARINDEX('tr',[Name])>0
 ORDER BY [Id],[Name],[Seats],[Range]

 SELECT f.Id AS[FlightId], SUM(t.Price) AS[Price]
 FROM Flights AS f
 JOIN Tickets AS t
 ON t.FlightId=f.Id
 GROUP BY f.Id
 ORDER BY [Price] DESC,[FlightId]

 SELECT CONCAT(p.FirstName, ' ', p.LastName) AS [FullName],
 f.Origin,
 f.Destination
 FROM Passengers AS p
 JOIN Tickets AS t 
 ON t.PassengerId = p.Id
 JOIN Flights AS f
 ON t.FlightId = f.Id
 ORDER BY [FullName], f.Origin,f.Destination

 SELECT p.FirstName, p.LastName, p.Age
 FROM Passengers AS p
 LEFT OUTER JOIN  Tickets AS t
 ON t.PassengerId = p.Id
 WHERE t.Id IS NULL
 ORDER BY p.Age DESC, p.FirstName, p.LastName

 SELECT CONCAT(p.FirstName,' ', p.LAstName ) AS [Full Name],
 pl.[Name] AS [Plane Name],
 CONCAT (f.Origin,' - ' , f.Destination) AS [TRIP],
 lt.[Type] AS [Luggage Type]
 FROM Passengers AS p
 JOIN  Tickets AS t
 ON t.PassengerId = p.Id
 JOIN Flights AS f
 ON t.FlightId = f.Id
 JOIN Planes AS pl
 ON f.PlaneId = pl.Id
 JOIN Luggages AS l 
 ON t.LuggageId = l.Id
 JOIN LuggageTypes lt
 ON l.LuggageTypeId = lt.Id
 ORDER BY [Full Name], [Plane Name], f.Origin, f.Destination, lt.[Type]


 SELECT p.[Name], p.Seats, COUNT (t.Id) AS [Passengers Count]
 FROM Planes AS p
 LEFT OUTER JOIN Flights AS f
 ON p.Id = f.PlaneId
 LEFT OUTER JOIN Tickets AS t
 ON f.Id = t.FlightId
 GROUP BY p.[Name] , p.Seats
 ORDER BY [Passengers Count] DESC, p.[Name], p.Seats


 CREATE FUNCTION udf_CalculateTickets(@origin VARCHAR(50), @destination VARCHAR(50), @peopleCount INT) 
 RETURNS VARCHAR(50)
 AS
 BEGIN
 IF(@peopleCount<=0)
   BEGIN
   RETURN 'Invalid people count!'
   END
   DECLARE @flightId INT = (SELECT TOP(1) Id FROM Flights
   WHERE Origin = @origin AND Destination = @destination)

   IF(@flightId IS NULL)
   BEGIN
   RETURN 'Invalid flight!'
   END

   DECLARE @pricePerPerson DECIMAL(18,2) = (SELECT TOP(1) Price FROM Tickets AS t
                               				WHERE t.FLightId =@flightId)

DECLARE @totalPrice DECIMAL(24,2) = @pricePerPerson*@peopleCount;
RETURN CONCAT ('Total price ', @totalPrice);

 END

 SELECT dbo.udf_CalculateTickets('Kolyshley','Rancabolang', 33)