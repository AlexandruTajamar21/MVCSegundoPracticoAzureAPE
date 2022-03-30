using APISEgundoPracticoAzureAPE.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCSegundoPracticoAzureAPE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCSegundoPracticoAzureAPE.Controllers
{
    public class EmpresaController : Controller
    {
        private ServiceCliente service;

        public EmpresaController(ServiceCliente service)
        {
            this.service = service;
        }

        [Authorize]
        public async Task<IActionResult> Tickets()
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            List<Ticket> tickets = await this.service.GetTicketsUser(token);
            return View(tickets);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Tickets(int id)
        {
            return RedirectToAction("TicketId", new { id = id });
        }

        public async Task<IActionResult> TicketId(int id)
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            Ticket tickets = await this.service.GetTicketId(id, token);
            return View(tickets);
        }

        [Authorize]
        public IActionResult CreateTicket()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(Ticket ticket)
        {
            string token = HttpContext.User.FindFirst("TOKEN").Value;
            await this.service.CreateTicket(ticket.Fecha, ticket.Importe, ticket.Producto, ticket.FileName, ticket.StoragePath, token);
            return RedirectToAction("Tickets");
        }
    }
}
