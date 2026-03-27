using Realms;
using MongoDB.Bson;

namespace PlatReserve.Models
{
    public class Billet : RealmObject
    {
        [PrimaryKey] 
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public string ReferenceBillet { get; set; } // Code unique (ex: ABC-123)
        public Reservation InfoReservation { get; set; }
        public Agence InfoAgence { get; set; }
    }
}
