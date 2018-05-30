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
                    DatePosted = DateTime.Now
                    //DatePosted

                };
                db.Comments.Add(newComment);
                db.SaveChanges();
            }

        }

        public void AddSong(User user, Song song)
        {
            using (var db = new AudiOceanEntities())
            {

                var newSong = new Song()
                {
                    SongName = song.SongName,
                    OwnerID = user.ID,
                    //DateUploaded
                    DateUploaded = DateTime.Now,
                    GenreID = song.GenreID,
                    URL = song.URL
                };
                db.Songs.Add(newSong);
                db.SaveChanges();
            }
        }

        public void AddSubscription(User user, User userToAddToSubscriptionList)
        {
            using (var db = new AudiOceanEntities())
            {
                var query = db.Users.Where(x => x.ID == user.ID).FirstOrDefault();
                var subquery = db.Users.Where(x => x.ID == userToAddToSubscriptionList.ID).FirstOrDefault();
                query.Subscriptions.Add(subquery);

                db.SaveChanges();
            }

        }

        public void AddUser(User u)
        {
            using (var db = new AudiOceanEntities())
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
                db.SaveChanges();
            }
        }

        public void DeleteSong(Song song)
        {
            using (var db = new AudiOceanEntities())
            {
                var query = db.Songs.Where(x => x.ID == song.ID).FirstOrDefault();
                db.Songs.Remove(query);
                db.SaveChanges();
            }
        }

        public void DeleteSubscription(User user, User userToDeleteFromSubscriptionList)
        {
            using (var db = new AudiOceanEntities())
            {
                var query = db.Users.Where(x => x.ID == user.ID).FirstOrDefault();
                var subquery = db.Users.Where(x => x.ID == userToDeleteFromSubscriptionList.ID).FirstOrDefault();
                if (query != null && subquery != null)
                {
                    query.Subscriptions.Remove(subquery);
                    db.SaveChanges();
                }
            }
        }

        public ICollection<Comment> GetCommentsForSong(Song song)
        {
            ICollection<Comment> AllCommentsForSong = new List<Comment>();
            using (var db = new AudiOceanEntities())
            {
                var query = db.Songs.Where(x => x.ID == song.ID).FirstOrDefault();

                foreach (Comment c in query.Comments)
                {
                    AllCommentsForSong.Add(c);
                }
                db.SaveChanges();
            }
            return AllCommentsForSong;
        }

        public Genre GetGenreWithName(string v)
        {
            Genre genre = null;
            using(var db = new AudiOceanEntities())
            {
                genre = db.Genres.Where(x => x.Name == v).FirstOrDefault();
            }

            return genre;
        }

        public ICollection<Song> GetMostRecentSongsUploads(int NumofSongs)
        {
            ICollection<Song> MostRecentSongs = new List<Song>();
            ICollection<Song> MostRecentSongsByNum = new List<Song>();

            using (var db = new AudiOceanEntities())
            {
                foreach (Song s in db.Songs)
                {
                    MostRecentSongs.Add(s);
                }
                db.SaveChanges();
            }
            Song[] SongArray = new Song[NumofSongs];
            for (int i = 0; i < NumofSongs; i++)
            {
                foreach (Song s in MostRecentSongs)
                {
                    SongArray[i] = s;
                }
                MostRecentSongsByNum.Add(SongArray[i]);
            }
            return MostRecentSongsByNum;

        }

        public Song GetSong(int id)
        {
            return GetSongByID(id);
        }

        public Song GetSongByID(int id)
        {
            Song song;
            using (var db = new AudiOceanEntities())
            {
                song = db.Songs.Where((s) => s.ID == id).FirstOrDefault();
            }
            return song;
        }

        public ICollection<Song> GetSongsUploadedByUser(User u)
        {
            ICollection<Song> UploadsByUser = new List<Song>();
            using (var db = new AudiOceanEntities())
            {
                var query = db.Users.Where(x => x.ID == u.ID).FirstOrDefault();
                foreach (Song s in query.Songs)
                {
                    UploadsByUser.Add(s);
                }
                db.SaveChanges();
            }

            return UploadsByUser;
        }

        public ICollection<User> GetSubscriptionsForUser(User user)
        {
            ICollection<User> SubscriptionList = new List<User>();
            using (var db = new AudiOceanEntities())
            {
                var query = db.Users.Where(x => x.ID == user.ID).FirstOrDefault();
                foreach (User q in query.Subscriptions)
                {
                    SubscriptionList.Add(q);
                }
                db.SaveChanges();
            }
            return SubscriptionList;
        }

        public User GetUser(int id)
        {
            User UserFound;
            using (var db = new AudiOceanEntities())
            {
                var query = db.Users.Where(x => x.ID == id).FirstOrDefault();
                UserFound = query;
            }
            return UserFound;
        }

        public User GetUserWithEmail(string email)
        {
            User UserFound;
            using (var db = new AudiOceanEntities())
            {
                var query = db.Users.Where(x => x.Email == email).FirstOrDefault();
                UserFound = query;

                db.SaveChanges();
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
                db.SaveChanges();
            }

        }
    }
}
