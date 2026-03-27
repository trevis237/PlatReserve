using System.Linq;
using PlatReserve.Models;
using PlatReserve.Services;
using Realms;

namespace PlatReserve.ViewModels
{
    public class ReservationViewModel
    {
        private readonly Realm _realm;

        // La personne actuellement connectée sur l'application
        public Personne UtilisateurConnecter { get; private set; }

        // IQueryable permet d'avoir une liste des trajets toujours à jour (Live Query)
        public IQueryable<Trajet> TousLesTrajets { get; private set; }

        // L'Injection de Dépendances par le constructeur ! 
        // MAUI va automatiquement la IRealmFactory qu'on a enregistrée tout à l'heure.
        public ReservationViewModel(IRealmService realmFactory)
        {
            // 1. On récupère notre base de données locale
            _realm = realmFactory.GetRealm();

            // 2. On charge tous les trajets directement
            TousLesTrajets = _realm.All<Trajet>();
            GenererFauxTrajets();
        }

        private void GenererFauxTrajets()
        {
            // On vérifie s'il y a déjà des trajets dans la base. 
            // Si c'est vide (Count() == 0), on en crée.
            if (!_realm.All<Trajet>().Any())
            {
                // On ouvre une transaction car on va écrire dans la base
                _realm.Write(() =>
                {
                    // 1. On crée des véhicules
                    var bus1 = new Bus
                    {
                        Immatriculation = "LT-123-AB",
                        NomChauffeur = "Jean Paul",
                        NombreDePlaces = 50,
                        PlacesRestantes = 50
                    };

                    var bus2 = new Bus
                    {
                        Immatriculation = "CE-456-XY",
                        NomChauffeur = "Samuel Eto'o",
                        NombreDePlaces = 30,
                        PlacesRestantes = 30
                    };

                    // 2. On crée les trajets avec ces véhicules
                    var trajet1 = new Trajet
                    {
                        Depart = "Douala",
                        Arrivee = "Yaoundé",
                        DateArrivee = DateTimeOffset.Now,
                        DateDepart = DateTimeOffset.Now.AddHours(5),
                        BusAssigne = bus1
                    };

                    var trajet2 = new Trajet
                    {
                        Depart = "Yaoundé",
                        Arrivee = "Bafoussam",
                        DateArrivee = DateTimeOffset.Now,
                        DateDepart = DateTimeOffset.Now.AddHours(5),
                        BusAssigne = bus2
                    };

                    // 3. On sauvegarde le tout dans Realm
                    _realm.Add(trajet1);
                    _realm.Add(trajet2);
                    // Pas besoin d'ajouter les véhicules manuellement, 
                    // Realm comprend qu'ils sont liés aux trajets et les sauvegarde avec !
                });
            }
        }

        /* 
        * Note d'Yvan : Si jamais tu es dans une classe où tu ne peux pas utiliser 
        * le constructeur pour récupérer la Factory, tu peux utiliser cette astuce MAUI :
        * var factory = IPlatformApplication.Current.Services.GetService<IRealmFactory>();
        */

        // 1. SIGNUP (S'inscrire)
        public bool Signup(string nom, string numeroTelephone)
        {
            // On vérifie si le numéro existe déjà
            var existe = _realm.All<Personne>().FirstOrDefault(p => p.Telephone == numeroTelephone);
            if (existe != null)
            {
                return false; //l'utilisateur existe deja
            }

            // Tout changement dans Realm DOIT se faire dans un bloc "Write" (Transaction)
            _realm.Write(() =>
            {
                var nouvellePersonne = new Personne
                {
                    Nom = nom,
                    Telephone = numeroTelephone
                };

                // On ajoute la personne dans la base de données
                _realm.Add(nouvellePersonne);
                UtilisateurConnecter = nouvellePersonne;
            });

            return true;  // SUCCÈS : Inscription réussie
        }
        // 2. LOGIN (Se connecter)
        public bool Login(string numeroTelephone)
        {
            var existant = _realm.All<Personne>().FirstOrDefault(p => p.Telephone == numeroTelephone);
            if (existant != null)
            {
                UtilisateurConnecter = existant;
                return true; //connexion reussie
            }

            return false; //echec
        }

        // 3. ADD TRAJET (Ajouter un trajet)
        public void AjouterTrajet(string depart, string arrive, Bus vehiculeAsigne, DateTimeOffset DDepart, DateTimeOffset Darrivee)
        {
            _realm.Write(() =>
            {
                var nouveauTrajet = new Trajet
                {
                    Depart = depart,
                    Arrivee = arrive,
                    DateDepart = DDepart,
                    DateArrivee = Darrivee,
                    BusAssigne = vehiculeAsigne
                };

                // Lors de la création, les places restantes du véhicule sont égales à sa capacité totale
                vehiculeAsigne.NombreDePlaces = vehiculeAsigne.NombreDePlaces;

                _realm.Add(nouveauTrajet);
            });
        }

        // 4. RESERVER BILLET 
        public void ReserverBillet(Trajet trajetChoisi)
        {
            // 1. Sécurités
            if (UtilisateurConnecter == null) return;

            // On vérifie les places dans le BusAssigne (nouveau nom de ta V2)
            if (trajetChoisi.BusAssigne.PlacesRestantes <= 0) return;

            _realm.Write(() =>
            {
                // ÉTAPE A : Créer d'abord la Réservation
                // C'est l'objet "milieu" qui lie le passager au trajet
                var nouvelleResa = new Reservation
                {
                    Voyageur = UtilisateurConnecter,
                    TrajetConcerne = trajetChoisi,
                    DateReservation = DateTimeOffset.Now,
                    EstPayee = true // On simule que c'est payé pour l'instant
                };
                _realm.Add(nouvelleResa); // On l'ajoute à la DB

                // ÉTAPE B : Créer le Billet
                // Le billet pointe vers la réservation qu'on vient de créer
                var nouveauBillet = new Billet
                {
                    InfoReservation = nouvelleResa,
                    // On génère une référence unique (ex: TKT-A1B2C3)
                    ReferenceBillet = "TKT-" + Guid.NewGuid().ToString().Substring(0, 6).ToUpper()
                };
                _realm.Add(nouveauBillet);

                // ÉTAPE C : Mettre à jour le Bus
                // On utilise "BusAssigne" et "PlacesRestantes" (noms de ta V2)
                trajetChoisi.BusAssigne.PlacesRestantes -= 1;

                // Note : Si tu as une liste de personnes dans Trajet, tu peux l'ajouter ici
                // trajetChoisi.ListeDesPassagers.Add(UtilisateurConnecter);
            });
        }
    }
    }
