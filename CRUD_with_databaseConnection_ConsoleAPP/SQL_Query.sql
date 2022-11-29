use UserDB;

create table Users (
	ID int not null identity(1,1),
	Name varchar(100),
	Email varchar(100),
	DOB varchar(100)
)

select ID, Name, Email, DOB from Users order by ID ASC;

insert into Users(Name, Email, DOB) values('Eder Militao', 'militao@gmail.com', '6-23-1995');

UPDATE Users SET Name = 'Neymar Junior', Email = 'neymar@gmail.com', DOB = '01-01-1990' WHERE ID = 1;

DELETE FROM Users where ID=7;
