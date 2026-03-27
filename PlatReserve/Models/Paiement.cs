using Realms;
using MongoDB.Bson;
using System;

namespace PlatReserve.Models
{
    public enum StatutPaiement { EnAttente, Reussi, Echoue }
    public class Paiement : RealmObject
    {
        [PrimaryKey] 
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public double Montant { get; set; }
        public string Methode { get; set; } // "Orange Money", "Momo"
        public string TransactionId { get; set; }
        public Reservation ReservationAssociee { get; set; }
    }
}
