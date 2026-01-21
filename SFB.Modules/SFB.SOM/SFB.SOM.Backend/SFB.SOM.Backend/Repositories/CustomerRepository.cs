using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.AMS;
using SFB.Shared.Backend.Helpers;
using SFB.Shared.Backend.Models;
using SFB.Shared.Backend.Repositories;

namespace SFB.SOM.Backend.Repositories
{
    public class CustomerRepository(SFBContext context) : BaseRepository<SFBContext>(context)
    {
        protected override List<string> GetFilterableProperties()
        {
            return new List<string> { "Person.FirstName", "Person.LastName" };
        }

        internal async Task<PagedListModel<ECustomer>> GetPage(string? filter, int pageSize, int pageNumber)
        {
            var query = Context.AMSCustomer
                .Include(c => c.Person)
                .AsQueryable();

            var result = await base.GetPage(query, filter, pageSize, pageNumber, new List<string> { "CustomerId" });

            return result;
        }

        internal async Task<ECustomer> Create(ECustomer customer)
        {
            if (customer.Person is null)
                throw new ControllerException("Debe registrar los datos de la persona.");

            var person = customer.Person;
            person.NroPerson = 0;
            Context.AMSPerson.Add(person);
            await Context.SaveChangesAsync();

            customer.NroPerson = person.NroPerson;
            customer.Person = person;
            Context.AMSCustomer.Add(customer);
            await Context.SaveChangesAsync();
            await Context.Entry(customer).Reference(c => c.Person).LoadAsync();
            return customer;
        }

        internal async Task<ECustomer> Update(ECustomer customer)
        {
            var entity = await Context.AMSCustomer
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);

            if (entity is null) throw new ControllerException("Cliente no encontrado");

            if (entity.Person is null)
                throw new ControllerException("Persona no encontrada");

            var person = customer.Person ?? throw new ControllerException("Debe registrar los datos de la persona.");
            entity.Person.FirstName = person.FirstName;
            entity.Person.LastName = person.LastName;
            entity.Person.DateBirth = person.DateBirth;
            entity.Person.Address = person.Address;

            await Context.SaveChangesAsync();
            await Context.Entry(entity).Reference(c => c.Person).LoadAsync();
            return entity;
        }

        public async Task<bool> Delete(int customerId)
        {
            var entity = await Context.AMSCustomer
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (entity is null) throw new ControllerException("Cliente no encontrado");

            try
            {
                Context.AMSCustomer.Remove(entity);
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException?.Message.Contains("FOREIGN KEY") == true)
                    throw new ControllerException("No es posible eliminar, item en uso");
                throw;
            }
        }
    }
}
