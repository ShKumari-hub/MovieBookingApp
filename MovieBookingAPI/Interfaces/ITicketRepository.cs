using MovieBookingAPI.Models;

namespace MovieBookingAPI.Interfaces
{
    public interface ITicketRepository
    {
        public Task<int> BookTickets(Ticket ticket);

        public List<int> getBookedSeats(string moviename, string theatrename);

        public int getTotalTickets(string moviename, string theatrename);

        public string UpdateTicketStatus(string moviename,string theatrename);

        public Task<List<Ticket>> getTickets(string username);

    }
}
