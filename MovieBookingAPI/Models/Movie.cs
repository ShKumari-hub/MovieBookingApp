using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MovieBookingAPI.Models
{
    [BsonIgnoreExtraElements]
    public class Movie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("moviename")]
        public string MovieName { get; set; } = string.Empty;

        [BsonElement("theatrename")]
        public string TheatreName { get; set; } = string.Empty;

        [BsonElement("totalticketsalloted")]
        [BsonRepresentation(BsonType.Int32)]
        public int TotalTicketsAlloted { get; set; }

        //[BsonElement("Availableseats")]
        //[BsonRepresentation(BsonType.Array)]
        //public List<int> AvailableSeats { get; set; } = new List<int>();
    }
}
