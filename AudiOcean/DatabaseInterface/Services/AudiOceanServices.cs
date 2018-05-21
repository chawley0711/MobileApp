using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInterface.Services
{
    public class AudiOceanServices : IAudiOceanServices
    {
        public void AddComment(User user, Song song, string comment)
        {
            using (var db = new AudiOceanEntities())
            {
                var newComment = new Comment()
                {
                    UserID = user.ID,
                    Text = comment,
                    SongID = song.ID,
                    //DatePosted
                    
                };
                db.Comments.Add(newComment);
            }
        }

        public void AddSong(User user, Song song)
        {
            using(var db = new AudiOceanEntities())
            {
                
                var newSong = new Song()
                {
                    SongName = song.SongName,
                    OwnerID = user.ID,
                    //DateUploaded
                    GenreID = song.Genre.ID,
                };
                db.Songs.Add(newSong);
            }
         }

        public void AddSubscription(User subscribers, User subscriptions)
        {
            throw new NotImplementedException();
        }

        public void AddUser(User u)
        {
            using(var db = new AudiOceanEntities())
            {
                var newUser = new User()
                {
                    ID = u.ID,
                    Name = u.Name,
                    DisplayName = u.DisplayName,
                    ProfilePictureURL = u.ProfilePictureURL,
                    Email = u.Email
                };
                db.Users.Add(newUser);
            }
        }

        public void DeleteSong(Song song)
        {
            using(var db = new AudiOceanEntities())
            {
                var query = db.Songs.Where(x => x.ID == song.ID).First();
                db.Songs.Remove(query);
            }
        }

        public void DeleteSubscription(User subscribers, User subscriptions)
        {
            using(var db = new AudiOceanEntities())
            {
                var query = db.Users.Where(x => x.ID == subscribers.ID).First();
            }
        }

        public ICollection<Comment> GetCommentsForSong(Song song)
        {
            ICollection<Comment> AllCommentsForSong = null;
            using(var db = new AudiOceanEntities())
            {
                var query = db.Songs.Where(x => x.ID == song.ID).First();
                
                foreach(Comment c in query.Comments)
                {
                    AllCommentsForSong.Add(c);
                }
            }
             return AllCommentsForSong;
        }

        public ICollection<Song> GetMostRecentSongsUploads(int NumofSongs)
        {
            ICollection<Song> MostRecentSongs = null;
            ICollection<Song> MostRecentSongsByNum = null;

            using (var db = new AudiOceanEntities())
            { 
                foreach(Song s in db.Songs)
                {
                    MostRecentSongs.Add(s);
                }
            }
            Song[] SongArray = new Song[NumofSongs];
            for(int i = 0; i < NumofSongs; i++)
            {
                foreach(Song s in MostRecentSongs)
                {
                    SongArray[i] = s;
                }
                MostRecentSongsByNum.Add(SongArray[i]);
            }
            return MostRecentSongsByNum;

        }

        public ICollection<Song> GetSongsPostedBySubscriptionsOrderedByDateUploaded(User user, int NumOfSongs)
        {
            throw new NotImplementedException();
        }

        public ICollection<Song> GetSongsUploadedByUser(User u)
        {
            ICollection<Song> UploadsByUser = null;
            using(var db = new AudiOceanEntities())
            {
                var query = db.Users.Where(x => x.ID == u.ID).First();
                foreach(Song s in query.Songs)
                {
                    UploadsByUser.Add(s);
                }
            }
            return UploadsByUser;
        }

        public ICollection<User> GetSubscriptionsForUser(User user)
        {
            throw new NotImplementedException();
        }

        public User GetUserWithEmail(string email)
        {
            User UserFound;
            using (var db = new AudiOceanEntities())
            {
                var query = db.Users.Where(x => x.Email == email).First();
                UserFound = query;
            }
            return UserFound;
        }

        public void RateSong(User user, Song song, int rating)
        {
            
            using (var db = new AudiOceanEntities())
            {
                var NewRating = new Rating()
                {   
                    UserID = user.ID,
                    SongID = song.ID,
                    Rating1 = rating
                };
                db.Ratings.Add(NewRating);
            }
            
        }
    }
}
