using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInterface.Services
{
    public interface IAudiOceanServices
    {
        User GetUserWithEmail(string email);
        User GetUser(int id);
        Song GetSong(int id);
        
        ICollection<Song> GetSongsUploadedByUser(User u);
        ICollection<Comment> GetCommentsForSong(Song song);
        ICollection<Song> GetMostRecentSongsUploads(int NumofSongs);
        ICollection<User> GetSubscriptionsForUser(User user);
         void AddUser(User u);
        void AddSong(User user, Song song);
        void AddSubscription(User subscribers, User subscriptions);
        void AddComment(User user, Song song, string comment);
        void RateSong(User user, Song song, int rating);
        void DeleteSubscription(User subscribers, User subscriptions);
        void DeleteSong(Song song);
        Song GetSongByID(int v);
        Genre GetGenreWithName(string v);
        int GetAverageRating(Song song);
    }
}
