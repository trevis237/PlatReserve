using PlatReserve.Models;

namespace PlatReserve.Services
{
    public class AuthService
    {
    public Personne User { get; set; }
        public bool IsAdmin => User?.Role == UserRole.Admin;
    }
}
