using Realms;
using MongoDB.Bson;
using System.Linq;

namespace PlatReserve.Models
{
    public class Bus : RealmObject
    {
        [PrimaryKey] public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public string Immatriculation { get; set; }
        public int NombreDePlaces { get; set; }
        public int PlacesRestantes { get; set; }
        public string NomChauffeur { get; set; }
        public Agence AgenceProprietaire { get; set; }

    }
}
