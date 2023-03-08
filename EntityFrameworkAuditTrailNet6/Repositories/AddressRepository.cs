using EntityFrameworkAuditTrailNet6.Data;
using EntityFrameworkAuditTrailNet6.DTOs;
using EntityFrameworkAuditTrailNet6.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkAuditTrailNet6.Repositories
{
    public interface IAddressRepository
    {
        Task CreateAddress(CreateEditAddressDTO createEditAddressDTO);
        Task DeleteAddress(int id);
        Task<AddressRowDTO[]> GetAddresses();
        Task<CreateEditAddressDTO> GetCreateEditAddressDTO(int id);
        Task UpdateAddress(CreateEditAddressDTO createEditAddressDTO);
    }

    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext context;

        public AddressRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<AddressRowDTO[]> GetAddresses()
        {
            return await context
                .Addresses
                .OrderBy(c => c.AddressLine1)
                .Select(c => new AddressRowDTO
                {
                    Id = c.Id,
                    AddressLine1 = c.AddressLine1,
                    City = c.City,
                    Postcode = c.Postcode
                }).ToArrayAsync();
        }

        public async Task<CreateEditAddressDTO> GetCreateEditAddressDTO(int id)
        {
            if (id == 0)
                return new CreateEditAddressDTO();

            var address = await context
                .Addresses
                .FindAsync(id);

            return new CreateEditAddressDTO
            {
                Id = address.Id,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                AddressLine3 = address.AddressLine3,
                City = address.City,
                County = address.County,
                Postcode = address.Postcode
            };
        }

        public async Task CreateAddress(CreateEditAddressDTO createEditAddressDTO)
        {
            var address = new Address
            {
                AddressLine1 = createEditAddressDTO.AddressLine1,
                AddressLine2 = createEditAddressDTO.AddressLine2,
                AddressLine3 = createEditAddressDTO.AddressLine3,
                City = createEditAddressDTO.City,
                County = createEditAddressDTO.County,
                Postcode = createEditAddressDTO.Postcode
            };

            await context.Addresses.AddAsync(address);

            await context.SaveChangesAsync();
        }

        public async Task UpdateAddress(CreateEditAddressDTO createEditAddressDTO)
        {
            var address = await context
                .Addresses
                .FindAsync(createEditAddressDTO.Id);

            address.AddressLine1 = createEditAddressDTO.AddressLine1;
            address.AddressLine2 = createEditAddressDTO.AddressLine2;
            address.AddressLine3 = createEditAddressDTO.AddressLine3;
            address.City = createEditAddressDTO.City;
            address.County = createEditAddressDTO.County;
            address.Postcode = createEditAddressDTO.Postcode;

            context.Addresses.Update(address);

            await context.SaveChangesAsync();
        }

        public async Task DeleteAddress(int id)
        {
            var address = await context.Addresses.FindAsync(id);

            context.Addresses.Remove(address);

            await context.SaveChangesAsync();
        }
    }
}