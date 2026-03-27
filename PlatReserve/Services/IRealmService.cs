using Realms;

namespace PlatReserve.Services
//C'est le "contrat" qui dit ce que notre service sait faire
{
    //ce service va servir a configurer la BD et injecter ce service automatiquement dans nos ViewModels  
    public interface IRealmService
    {
        Realm GetRealm()
        {
            // On configure la base de données (nom du fichier, version, etc.)
            var config = new RealmConfiguration("TicketReservation")
            {
                SchemaVersion = 1,
            };
            // On retourne l'instance de Realm prête à être utilisée
            return Realm.GetInstance(config);
        }
    }
}
