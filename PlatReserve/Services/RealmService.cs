using PlatReserve.Models;
using Realms;
using System.IO;

namespace PlatReserve.Services
{
    public class RealmService : IRealmService
    {
        public Realm GetRealm()
        {
            // On crée une configuration spécifique pour notre base de données
            var config = new RealmConfiguration("PlatReserveDB.realm")
            {
                // LA MAGIE EST ICI 👇
                // Si la structure de nos classes change, Realm supprime l'ancienne base 
                // et en recrée une nouvelle automatiquement sans crasher !
                ShouldDeleteIfMigrationNeeded = true
            };

            var realm = Realm.GetInstance(config);
            CreateDefaultAdmin(realm);
            // On retourne l'instance avec notre configuration magique
            return realm;
           


        }


        private void CreateDefaultAdmin(Realm realm)
        {
            // var realm = _realmService.GetRealm();

            // 1. On cherche si l'admin existe déjà par son numéro "Clé"
            var admin = realm.All<Personne>().FirstOrDefault(x => x.Telephone == "657341358");

            if (admin == null)
            {
                // 2. S'il n'existe pas, on le crée
                realm.Write(() =>
                {
                    var nouveauAdmin = new Personne
                    {
                        Nom = "Sonna",
                        Telephone = "657341358",
                        MotDePasse = "1234",
                        Role = UserRole.Admin // On utilise l'Enum !
                    };

                    // On utilise l'ajout simple car c'est une création
                    realm.Add(nouveauAdmin);
                });

                System.Diagnostics.Debug.WriteLine("✅ Admin 'Sonna' créé avec succès.");
            }
        }
    }
}