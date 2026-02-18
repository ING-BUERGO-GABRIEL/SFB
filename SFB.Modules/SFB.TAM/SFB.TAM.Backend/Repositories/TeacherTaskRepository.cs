using Microsoft.EntityFrameworkCore;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.TAM;
using SFB.Shared.Backend.Helpers;
using SFB.Shared.Backend.Repositories;

namespace SFB.TAM.Backend.Repositories
{
    public class TeacherTaskRepository(SFBContext context) : BaseRepository<SFBContext>(context)
    {
        internal async Task<ETeacherTask> Create(ETeacherTask teacherTask)
        {
            //teacherTask.DeliveryDate = teacherTask.DeliveryDate.Date + DateTime.Now.TimeOfDay;
            Context.TAMTeacherTasks.Add(teacherTask);
            var formTeacher = await Context.TAMFormTeachers.FirstAsync(f => f.NroForm == teacherTask.NroFromTeacher);
            formTeacher.CodStatus = "UTI";
            await Context.SaveChangesAsync();
            teacherTask.FormTeacher = formTeacher;
            return teacherTask;
        }

        internal async Task<List<ETeacherTask>> GetListByFilters(int? nroPerson, string? codStatus, string? codStatusDis)
        {
            IQueryable<ETeacherTask> query = Context.TAMTeacherTasks.Include(c => c.FormTeacher).AsQueryable();

            query = query.ConditionalWhere(codStatus, c => c.CodStatus == codStatus);
            query = query.ConditionalWhere(codStatusDis, c => c.CodStatus != codStatusDis);

            if (nroPerson == 0)
                query = query.ConditionalWhere(true, t => t.NroPersonAssigned == null);
            else
                query = query.ConditionalWhere(nroPerson, t => t.NroPersonAssigned == nroPerson);

            return await query.AsNoTracking().ToListAsync();
        }

        internal async Task<List<ETeacherTask>> GetPageByFilters(string? filter)
        {
            IQueryable<ETeacherTask> query = Context.TAMTeacherTasks.Include(c => c.FormTeacher).AsQueryable();

            return await query.AsNoTracking().OrderByDescending(t=>t.NroTask).ToListAsync();
        }

        internal async Task<ETeacherTask> Update(ETeacherTask eTeacherTask, ETeacherTask oldTeacherTask)
        {
            Context.Entry(oldTeacherTask).CurrentValues.SetValues(eTeacherTask);
            await Context.SaveChangesAsync();
            return eTeacherTask;
        }

        internal async Task<ETeacherTask> Update(ETeacherTask eTeacherTask, string? codStatus, int? NroPersonAssigned)
        {
            if (!string.IsNullOrEmpty(codStatus))
                eTeacherTask.CodStatus = codStatus;

            if (NroPersonAssigned.HasValue)
                eTeacherTask.NroPersonAssigned = NroPersonAssigned.Value == 0 ? null : NroPersonAssigned;

            Context.TAMTeacherTasks.Update(eTeacherTask);

            await Context.SaveChangesAsync();
            return eTeacherTask;
        }

        internal async Task<ETeacherTask> GetByNroTask(int nroTask)
        {
            var teacherTask = await Context.TAMTeacherTasks.FirstOrDefaultAsync(t => t.NroTask == nroTask);

            if (teacherTask is null)
                throw new ControllerException($"No se encontro la entidad con Id : {nroTask}");

            return teacherTask;
        }

    }

}
