using Realms;
using MongoDB.Bson;

namespace PlatReserve.Models
{
    public class Trajet : RealmObject
    {
        [PrimaryKey] 
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public string Depart { get; set; }
        public string Arrivee { get; set; }
        public DateTimeOffset DateDepart { get; set; }
        public DateTimeOffset DateArrivee { get; set; }
        public double Prix { get; set; }
        public Bus BusAssigne { get; set; }

        // On dit à Realm : "Regarde toutes les Réservations qui pointent vers ce trajet"
        [Backlink(nameof(Reservation.TrajetConcerne))]
        public IQueryable<Reservation> LesReservations { get; }

    }
}
