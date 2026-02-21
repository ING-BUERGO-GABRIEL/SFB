using Mapster;
using Microsoft.AspNetCore.Mvc;
using SFB.Infrastructure.Contexts;
using SFB.Infrastructure.Entities.TAM;
using SFB.Shared.Backend.Controller;
using SFB.Shared.Backend.Helpers;
using SFB.TAM.Backend.Repositories;
using SFB.TAM.Shared.Models;

namespace SFB.TAM.Backend.Controllers
{
    [Route("api/TAM/[controller]/[action]")]
    public class TeacherTaskController(SFBContext context) : BaseController<SFBContext, TeacherTaskRepository>(context)
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MTeacherTask formTeacher)
        {
            try
            {
                var eTeacherTask = formTeacher.Adapt<ETeacherTask>();
                var result = await Repository.Create(eTeacherTask);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetListByFilters(
            [FromQuery] int? nroPerson = null,
            [FromQuery] string? codStatus = null,
            [FromQuery] string? codStatusDis = null)
        {
            try
            {
                var result = await Repository.GetListByFilters(nroPerson, codStatus, codStatusDis);

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPageByFilters(
        [FromQuery] string? filter = null)
        {
            try
            {
                var result = await Repository.GetPageByFilters(filter);

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MTeacherTask formTeacher)
        {
            try
            {
                var entity = await Repository.GetByNroTask(formTeacher.NroTask);
                var eTeacherTask = formTeacher.Adapt<ETeacherTask>();

                var user = User.FindFirst("NameUser")?.Value;
                eTeacherTask.UserReg = entity.UserReg;
                eTeacherTask.DateReg = entity.DateReg;

                eTeacherTask.UserUpd = user;
                eTeacherTask.DateUpd = DateTime.Now;

                var result = await Repository.Update(eTeacherTask,entity);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPatch("{nroTask}")]
        public async Task<IActionResult> Update(int nroTask, 
            [FromQuery] string? codStatus = null,
            [FromQuery] int? NroPersonAssigned = null)
        {
            try
            {
                var eTeacherTask = await Repository.GetByNroTask(nroTask);

                var user = User.FindFirst("NameUser")?.Value;
                eTeacherTask.UserUpd = user;
                eTeacherTask.DateUpd = DateTime.Now;

                var result = await Repository.Update(eTeacherTask, codStatus, NroPersonAssigned);

                return OkResult(result);
            }
            catch (ControllerException ex)
            {
                return ControlledException(ex);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetStatus()
        {
            try
            {
                var result = Repository.GetStatus();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
