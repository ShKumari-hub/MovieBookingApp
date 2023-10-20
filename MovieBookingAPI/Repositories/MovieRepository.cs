using MongoDB.Bson;
using MongoDB.Driver;
using MovieBookingAPI.Interfaces;
using MovieBookingAPI.Models;
using System.Collections;

namespace MovieBookingAPI.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IMongoCollection<Movie> _movies;
        private readonly IMongoCollection<Ticket> _tickets;

        public MovieRepository(IMongoDbConfig config,IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(config.DatabaseName);
            _movies=database.GetCollection<Movie>(config.MovieCollectionName);
            _tickets=database.GetCollection<Ticket>(config.TicketCollectionName);

        }
        //public async Task<List<int>> GetAvailbleSeats(string id)
        //{
        //    var availSeats = await _movies.Find(x => x.Id == id).Project(x=>x.AvailableSeats).SingleOrDefaultAsync();
        //    return availSeats.ToList();
        //}

        public async Task<List<Movie>> GetMovies()
        {
            var movies = await _movies.FindAsync(m=>true);
            return movies.ToList();
        }

        public async Task<List<Movie>> SearchMovie(string movieName)
        {
            var movies = await _movies.FindAsync(Builders<Movie>.Filter.Regex("moviename",new BsonRegularExpression(movieName,"i")));
            return movies.ToList();
        }

        public async Task<bool> DeleteMovie(string moviename,string theatreName)
        {
            
            var result= await _movies.DeleteOneAsync(x=>x.MovieName.ToLower() == moviename.ToLower() && x.TheatreName.ToLower() == theatreName.ToLower());

            await _tickets.DeleteManyAsync(x => x.MovieName.ToLower() == moviename.ToLower() && x.TheatreName.ToLower() == theatreName.ToLower());
            if (result.DeletedCount ==0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        //public Task UpdateAvailableSeats(string id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
