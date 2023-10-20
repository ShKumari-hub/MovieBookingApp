using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieBookingAPI.Interfaces;
using MovieBookingAPI.Models;

namespace MovieBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;

        public MoviesController(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        [Authorize(Roles = "Admin, Member")]
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<Movie>>> GetAll()
        {
            var movies = await _movieRepository.GetMovies();
            if(movies.Count == 0 )
            {
                return NoContent();
            }
            else
            {
                return Ok(movies);
            }
        }

        [Authorize(Roles = "Admin, Member")]
        [HttpGet]
        [Route("search/{moviename}")]
        public async Task<ActionResult<List<Movie>>> GetByName(string moviename)
        {
            var movies = await _movieRepository.SearchMovie(moviename);
            if (movies.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return Ok(movies);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{moviename}/delete/{theatrename}")]
        public async Task<ActionResult<string>> DeleteMovie(string moviename,string theatrename)
        {
            var response = await _movieRepository.DeleteMovie(moviename, theatrename);
            if (response)
            {
                return Ok($"{moviename} movie deleted successfully.");
            }
            else
            {
                return BadRequest($"{moviename} movie doesn't exists");
            }
        }


    }
}
