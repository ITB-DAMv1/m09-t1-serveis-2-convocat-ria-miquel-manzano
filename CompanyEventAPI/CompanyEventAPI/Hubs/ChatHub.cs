using CompanyEventAPI.Models;
using Microsoft.AspNetCore.SignalR;

namespace CompanyEventAPI.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string usuari, string missatge)
        {
            // Envia el missatge a tots els clients connectats
            await Clients.All.SendAsync("RepMissatge", usuari, missatge);
        }
    }
}
