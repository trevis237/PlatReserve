using Realms;
using MongoDB.Bson;
using System;

namespace PlatReserve.Models
{
    public class Agence : RealmObject
    {
        [PrimaryKey]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        public string Nom { get; set; }
        public string NumeroLicence { get; set; }

        // Relation inverse : Une agence "possède" plusieurs véhicules
        [Backlink(nameof(Bus.AgenceProprietaire))]
        public IQueryable<Bus> Flotte { get; }
    }
}
