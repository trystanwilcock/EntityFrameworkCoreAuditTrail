using EntityFrameworkAuditTrailNet6.Data;
using EntityFrameworkAuditTrailNet6.DTOs;
using EntityFrameworkAuditTrailNet6.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkAuditTrailNet6.Repositories
{
    public interface IContactRepository
    {
        Task CreateContact(CreateEditContactDTO createEditContactDTO);
        Task DeleteContact(int id);
        Task<ContactRowDTO[]> GetContacts();
        Task<CreateEditContactDTO> GetCreateEditContactDTO(int id);
        Task UpdateContact(CreateEditContactDTO createEditContactDTO);
    }

    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext context;

        public ContactRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<ContactRowDTO[]> GetContacts()
        {
            return await context
                .Contacts
                .OrderBy(c => c.LastName)
                .Select(c => new ContactRowDTO
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName
                }).ToArrayAsync();
        }

        public async Task<CreateEditContactDTO> GetCreateEditContactDTO(int id)
        {
            if (id == 0)
                return new CreateEditContactDTO();

            var contact = await context
                .Contacts
                .FindAsync(id);

            return new CreateEditContactDTO
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                DateOfBirth = contact.DateOfBirth,
                Email = contact.Email,
                Phone = contact.Phone
            };
        }

        public async Task CreateContact(CreateEditContactDTO createEditContactDTO)
        {
            var contact = new Contact
            {
                FirstName = createEditContactDTO.FirstName,
                LastName = createEditContactDTO.LastName,
                DateOfBirth = createEditContactDTO.DateOfBirth,
                Email = createEditContactDTO.Email,
                Phone = createEditContactDTO.Phone
            };

            await context.Contacts.AddAsync(contact);

            await context.SaveChangesAsync();
        }

        public async Task UpdateContact(CreateEditContactDTO createEditContactDTO)
        {
            var contact = await context
                .Contacts
                .FindAsync(createEditContactDTO.Id);

            contact.FirstName = createEditContactDTO.FirstName;
            contact.LastName = createEditContactDTO.LastName;
            contact.DateOfBirth = createEditContactDTO.DateOfBirth;
            contact.Email = createEditContactDTO.Email;
            contact.Phone = createEditContactDTO.Phone;

            context.Contacts.Update(contact);

            await context.SaveChangesAsync();
        }

        public async Task DeleteContact(int id)
        {
            var contact = await context.Contacts.FindAsync(id);

            context.Contacts.Remove(contact);

            await context.SaveChangesAsync();
        }
    }
}