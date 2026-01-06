using ISEMES.Models;

namespace ISEMES.Services
{
    public interface ITicketService
    {
        Task<List<SearchTicket>> SearchTickets(DateTime? fromDate, DateTime? toDate);
        Task<List<TicketType>> GetTicketType();
        Task<List<TicketLot>> GetTicketLots();
        Task<List<TicketLotDetail>> GetTicketLineItemLots(string lotNumbers);
        Task<bool> UpsertTicket(string upsertJson, string? ticketAttachments, string? reviewerAttachments);
        Task<TicketDetail> GetTicketDetail(int ticketId);
        Task<bool> VoidTicket(int ticketID);
    }
}



