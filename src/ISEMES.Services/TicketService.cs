using ISEMES.Models;
using ISEMES.Repositories;

namespace ISEMES.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _repository;

        public TicketService(ITicketRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<SearchTicket>> SearchTickets(DateTime? fromDate, DateTime? toDate)
        {
            return await _repository.SearchTickets(fromDate, toDate);
        }

        public async Task<List<TicketType>> GetTicketType()
        {
            return await _repository.GetTicketType();
        }

        public async Task<List<TicketLot>> GetTicketLots()
        {
            return await _repository.GetTicketLots();
        }

        public async Task<List<TicketLotDetail>> GetTicketLineItemLots(string lotNumbers)
        {
            return await _repository.GetTicketLineItemLots(lotNumbers);
        }

        public async Task<bool> UpsertTicket(string upsertJson, string? ticketAttachments, string? reviewerAttachments)
        {
            return await _repository.UpsertTicket(upsertJson, ticketAttachments, reviewerAttachments);
        }

        public async Task<TicketDetail> GetTicketDetail(int ticketId)
        {
            return await _repository.GetTicketDetail(ticketId);
        }

        public async Task<bool> VoidTicket(int ticketId)
        {
            return await _repository.VoidTicket(ticketId);
        }
    }
}

