using Realms;
using MongoDB.Bson;


namespace PlatReserve.Models
{
    public enum UserRole { Voyageur, Admin }
    public class Personne : RealmObject
    {
        [PrimaryKey] 
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public string Nom { get; set; }
        public string Telephone { get; set; }
        public string MotDePasse { get; set; } // Nouveau
        public int RoleValue { get; set; } // Stocké en int pour Realm

        [Ignored]
        public UserRole Role
        {
            get => (UserRole)RoleValue;
            set => RoleValue = (int)value;
        }
    }
}
