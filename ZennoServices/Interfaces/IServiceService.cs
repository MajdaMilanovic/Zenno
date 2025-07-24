using Zenno.Model;
using Zenno.Model.SearchObjects;

namespace ZennoServices.Interfaces
{
    public interface IServiceService
    {
        Task<List<Service>> GetAllAsync();
        Task<Service?> GetByIdAsync(int id);
        Task<Service> CreateAsync(Service service);
        Task<Service?> UpdateAsync(int id, Service service);
        Task DeleteAsync(int id);
        Task<List<Service>> SearchAsync(ServiceSearchObject searchObject);
        Task<List<Service>> GetReservationServicesAsync(int reservationId);
        Task AddServiceToReservationAsync(int reservationId, int serviceId, int quantity);
        Task RemoveServiceFromReservationAsync(int reservationId, int serviceId);
    }
} 