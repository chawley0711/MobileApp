
Create Table Users(ID int Primary Key identity(1,1)
					, [Name] varchar(20)
					, DisplayName varchar(20)
					, ProfilePictureURL varchar(max)
					, Email varchar(40) Unique)
go

Create Table Genres(ID int Primary Key identity(1,1), [Name] varchar(20))

go

Create Table Songs(ID int Primary Key identity(1,1),
					SongName varchar(30),
					OwnerID int Foreign Key references Users(ID), 
					DateUploaded timestamp, 
					GenreID int foreign key references Genres(ID),
					URL varchar(30) not null)
go

Create Table Ratings(UserID int Foreign Key References Users(ID)
					,SongID int Foreign Key References Songs(ID)
					,Rating int,
					Primary Key(UserID, SongID))
go

Create Table Subscriptions(SubscriberID int Foreign Key References Users(ID)
							,UserID int Foreign Key References Users(ID),
							Primary Key(SubscriberID, UserID))
go

Create Table Comments(CommentID int Primary Key identity(1,1)
						,SongID int foreign Key References Songs(ID)
						,[Text] varchar(max)
						,DatePosted timestamp
						,UserID int Foreign Key References Users(ID))
go
