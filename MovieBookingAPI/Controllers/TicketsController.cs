using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBookingAPI.Interfaces;
using MovieBookingAPI.Models;
using System.Data;

namespace MovieBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketsController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }


        [Authorize(Roles = "Admin, Member")]
        [HttpPost]
        [Route("booktickets")]
        public async Task<ActionResult> BookTickets([FromBody] Ticket ticket)
        {
            var response =await _ticketRepository.BookTickets(ticket);
            if (response == -1)
            {
                return BadRequest("Booking failed as there is no requested number of seats");
            }
            else if(response == 0){
                return BadRequest("You are trying to book already booked seat");
            }
            return Ok("Booked Successfully");
        }


        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{moviename}/update/{theatrename}")]
        public ActionResult UpdateStatus(string moviename,string theatrename)
        {
            var status = _ticketRepository.UpdateTicketStatus(moviename, theatrename);
            return Ok(status);
        }

        [Authorize(Roles = "Admin, Member")]
        [HttpGet]
        [Route("{moviename}/getBookingInfo/{theatrename}")]
        public  ActionResult GetBookInfo(string moviename,string theatrename)
        {
            var getBookedTickets =  _ticketRepository.getBookedSeats(moviename,theatrename);
            var totalSeats = _ticketRepository.getTotalTickets(moviename, theatrename);
            return Ok(new 
            {
                totalSeatsAvailable=totalSeats,
                bookedTickets=getBookedTickets,
                
            });
        }

        [Authorize(Roles = "Admin, Member")]
        [HttpGet]
        [Route("getticketsbyuser/{loginId}")]
        public async Task<ActionResult> getTickets(string loginId)
        {
            var tickets = await _ticketRepository.getTickets(loginId);
            return Ok(tickets);
        }
    }
}
