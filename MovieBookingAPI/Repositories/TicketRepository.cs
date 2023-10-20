using MongoDB.Bson;
using MongoDB.Driver;
using MovieBookingAPI.Interfaces;
using MovieBookingAPI.Models;
using System.Runtime.InteropServices;

namespace MovieBookingAPI.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IMongoCollection<Movie> _movies;
        private readonly IMongoCollection<Ticket> _tickets;

        public TicketRepository(IMongoDbConfig config, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(config.DatabaseName);
            _movies = database.GetCollection<Movie>(config.MovieCollectionName);
            _tickets = database.GetCollection<Ticket>(config.TicketCollectionName);
        }
        public async Task<int> BookTickets(Ticket ticket)
        {
            var allotedTickets = getTotalTickets(ticket.MovieName,ticket.TheatreName);
            var bookedSeats = getBookedSeats(ticket.MovieName,ticket.TheatreName);
            var totalAvailableTickets = allotedTickets - bookedSeats.Count;
            if (totalAvailableTickets < ticket.NumberOfTickets)
            {
                return -1;
            }
            else
            {
                foreach(int i in ticket.SeatNumbers)
                {
                    if(bookedSeats.Contains(i))
                    {
                        return 0;
                    }
                }
                await _tickets.InsertOneAsync(ticket);
                return 1;
            }
            
            
        }

        public string UpdateTicketStatus(string moviename,string theatrename)
        {
            var bookedSeats = getBookedSeats(moviename,theatrename);
            var totalTickets = getTotalTickets(moviename, theatrename);
            if(totalTickets-bookedSeats.Count> 0)
            {
                return "BOOK ASAP";
            }
            else
            {
                return "SOLD OUT";
            }
        }

        public int getTotalTickets(string moviename,string theatrename)
        {
            return _movies.Find(m => m.MovieName.ToLower() == moviename.ToLower() && m.TheatreName.ToLower() == theatrename.ToLower()).SingleOrDefault().TotalTicketsAlloted;
        }
        public List<int> getBookedSeats(string moviename, string theatrename)
        {
            var tickets = _tickets.Find(t => t.MovieName.ToLower() == moviename.ToLower() && t.TheatreName.ToLower() == theatrename.ToLower()).ToList();

            var bookedSeats= new List<int>();
            foreach(var t in tickets)
            {
                bookedSeats.AddRange(t.SeatNumbers);
            }

            return bookedSeats;

        }

        public async Task<List<Ticket>> getTickets(string loginId)
        {
            var tickets = await _tickets.FindAsync(x => x.LoginId.ToLower() == loginId.ToLower());
            return tickets.ToList();
        }
    }
}
