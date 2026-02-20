using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.TAM;
using SFB.Shared.Backend.Helpers;
using SFB.Shared.Backend.Models;
using SFB.Shared.Backend.Repositories;
using SFB.TAM.Shared.Models;
using SFB.TAM.Shared.Sealed;

namespace SFB.TAM.Backend.Repositories
{
    public class FormTeacherRepository(SFBContext context) : BaseRepository<SFBContext>(context)
    {

        protected override List<string> GetFilterableProperties()
        {
            return new List<string> { "NroForm", "PhoneNumber", "NameTeache" };
        }
        internal async Task<EFormTeacher> Create(EFormTeacher formTeacher)
        {
            Context.TAMFormTeachers.Add(formTeacher);
            await Context.SaveChangesAsync();
            return formTeacher;
        }

        internal async Task<EFormTeacher> GetByNroForm(int nroForm)
        {
            var result = await Context.TAMFormTeachers.FirstOrDefaultAsync(c => c.NroForm == nroForm);
            if (result is null)
                throw new ControllerException($"No se encontro la entidad con Id : {nroForm}");

            return result;
        }

        internal async Task<List<EFormTeacher>> GetListByCodStatus(string? codStatus)
        {
            var result = Context.TAMFormTeachers.AsQueryable();
            result = result.ConditionalWhere(codStatus, f => f.CodStatus == codStatus);
            return await result.AsNoTracking().OrderByDescending(f=>f.NroForm).ToListAsync();
        }

        internal async Task<PagedListModel<EFormTeacher>> GetPage(string? filter,string codStatus, int pageSize, int pageNumber)
        {
            var query = Context.TAMFormTeachers.AsQueryable();
            query = query.ConditionalWhere(codStatus, c => c.CodStatus == codStatus);

            var result = await base.GetPage(query, filter, pageSize, pageNumber, new List<string> { "NroForm:desc" });

            return result;
        }

        internal async Task<dynamic> Update(EFormTeacher eFormTeacher, string? codStatus)
        {
            if (!string.IsNullOrEmpty(codStatus))
                eFormTeacher.CodStatus = codStatus;

            Context.TAMFormTeachers.Update(eFormTeacher);

            await Context.SaveChangesAsync();
            return eFormTeacher;
        }

        internal async Task<MFormTeacher> Update(EFormTeacher eFormTeacher, MFormTeacher formTeacher)
        {
            Context.Entry(eFormTeacher).CurrentValues.SetValues(formTeacher);
            await Context.SaveChangesAsync();
            return formTeacher;
        }

        internal IEnumerable<FormStatus> GetStatus() => FormStatus.List();
        
    }
}
