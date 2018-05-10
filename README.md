# AudiOcean Requirements

## Executive Summary:
We aim to provide a platform upon which people may upload, share, and react to music produced by aspiring artists. After music has been uploaded, community members may listen and post comments sharing their thoughts about what they just heard.
## Targeted Users:
	Our target demographic is those with a passion for music whether they be creators or consumers.
## Technical Summary:
### Primary Goals:
1.	Upload Music Files of MP3 and WAV Formats from Client’s Mobile Device to the Server
2.	Stream Music from WAV Format from the Server to the Client’s Mobile Device
3.	Display User Profiles with Relevant Information
a.	Name
b.	Profile Picture
c.	List of Songs Created by the User
4.	5 Star Rating System
### Stretch Goals:
1.	Display an Oscillator that Responds to the Music that is Currently Streaming
2.	Subscription Features such as Upload or Comment Notifications for Other Users of which a Particular User is Subscribed
3.	Comment Feature so Users May Share their Thoughts with Musicians and Each Other.
## Technology Stack:
	Languages: C#, JavaScript
	Database: SQL Server
	Database Interface API: EntityFramework
	Audio Conversion Library: NAudio
	Integrated Development Environment: Visual Studios
	User Accounts and Authentication: Google Identity Platform
## The How
Using C# and JavaScript, our group will create an audio streaming app in Visual Studios with Xamarin for Android mobile devices. Users will be able to upload music files of either MP3 or WAV file formats, but MP3s will be converted to the WAV format. Google Authenticator will be used to allow users to create and authenticate their accounts. The user’s account and profile information will be stored in an SQL database and EntityFramework will be utilized by our server application in order to interface with the database. Audio files will be stored on a server’s hard drive and its file path will be stored in the database. 
### Team Members:
	Jeffrey Henderson
	Boris Ho
	Collin Hawley
	Jordon Bowden
