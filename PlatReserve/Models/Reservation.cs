using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlatReserve.Models
{
    public class Reservation : RealmObject
    {
        [PrimaryKey] 
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public DateTimeOffset DateReservation { get; set; } = DateTimeOffset.Now;
        public Personne Voyageur { get; set; }
        public Trajet TrajetConcerne { get; set; }
        public bool EstPayee { get; set; } = false;


        [Ignored] // Pas stocké en base, calculé en direct
        public bool AnnulationPossible =>
        TrajetConcerne != null && (TrajetConcerne.DateDepart - DateTimeOffset.Now).TotalHours >= 24;

        [Ignored]
        public string MessageAnnulation => AnnulationPossible
            ? "Annulation possible"
            : "Délai de 24h dépassé (Annulation impossible)";
    }
}
