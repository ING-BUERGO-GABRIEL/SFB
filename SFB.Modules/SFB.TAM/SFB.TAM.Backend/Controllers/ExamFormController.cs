using SFB.Infrastructure.Contexts;
using SFB.Shared.Backend.Controller;
using SFB.TAM.Backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SFB.TAM.Shared.Models;
using SFB.Infrastructure.Entities.TAM;
using Mapster;
using SFB.Shared.Backend.Helpers;

namespace SFB.TAM.Backend.Controllers
{
    [Route("api/TAM/[controller]/[action]")]
    public class ExamFormController(SFBContext context) : BaseController<SFBContext, FormTeacherRepository>(context)
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] MFormTeacher formTeacher)
        {
            try
            {
                var eFromTeacher = formTeacher.Adapt<EFormTeacher>();
                var result = await Repository.Create(eFromTeacher);
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPage(
            [FromQuery] int pageSize,
            [FromQuery] int PageNumber,
            [FromQuery] string? filter = null,
            [FromQuery] string? codStatus = null)
        {
            try
            {
                 var result = await Repository.GetPage(filter,codStatus, pageSize, PageNumber);

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetListByCodStatus([FromQuery] string? CodStatus = null)
        {
            try
            {

                var formTeacherList = await Repository.GetListByCodStatus(CodStatus);

                var result = formTeacherList.Adapt<List<MFormTeacher>>();

                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet("{nroForm}")]
        public async Task<IActionResult> GetByNroForm(int nroForm)
        {
            try
            {
                var formTeacher = await Repository.GetByNroForm(nroForm);
                var result = formTeacher.Adapt<MFormTeacher>();
                return OkResult(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPatch("{nroForm}")]
        public async Task<IActionResult> Update(int nroForm,
                [FromQuery] string? codStatus = null)
        {
            try
            {
                var eFormTeacher = await Repository.GetByNroForm(nroForm);

                var result = await Repository.Update(eFormTeacher, codStatus);

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


        [HttpPut("{nroForm}")]
        public async Task<IActionResult> Update(int nroForm,[FromBody] MFormTeacher formTeacher)
        {
            try
            {
                var eFormTeacher = await Repository.GetByNroForm(nroForm);

                var result = await Repository.Update(eFormTeacher, formTeacher);

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
