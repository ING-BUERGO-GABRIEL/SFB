﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SFB.Shared.Backend.Helpers;
using SFB.Shared.Backend.Repositories;
using SFB.Shared.Models;

namespace SFB.Shared.Backend.Controller
{
    [ApiController]
    [Authorize]
    public abstract class BaseController<TContext, TRepository> : ControllerBase
        where TContext : DbContext
        where TRepository : BaseRepository<TContext>
    {
        protected TRepository Repository;
        public BaseController(TContext context)
        {
            Repository = (TRepository)Activator.CreateInstance(typeof(TRepository), context);
        }
        public BaseController()
        {

        }
        protected IActionResult OkResult(dynamic model)
        {
            var ApiResult = new ApiResult<dynamic> { IsSuccess = true, Data = model };
            return StatusCode(StatusCodes.Status200OK, ApiResult);
        }
        protected IActionResult OkMessage(string message)
        {
            var ApiResult = new ApiResult<dynamic> { IsSuccess = true, Message = message };
            return StatusCode(StatusCodes.Status200OK, ApiResult);
        }
        protected IActionResult CreatedResult(dynamic model)
        {
            var ApiResult = new ApiResult<dynamic> { IsSuccess = true, Data = model };
            return StatusCode(StatusCodes.Status201Created, ApiResult);
        }
        protected IActionResult DeletedResult()
        {
            var ApiResult = new ApiResult<bool> { IsSuccess = true, Data = true };
            return StatusCode(StatusCodes.Status202Accepted, ApiResult);
        }
        protected IActionResult UpdatedResult(dynamic model)
        {
            var ApiResult = new ApiResult<dynamic> { IsSuccess = true, Data = model };
            return StatusCode(StatusCodes.Status202Accepted, ApiResult);
        }
        protected IActionResult BadRequestResult()
        {
            var ApiResult = new ApiResult<dynamic> { IsSuccess = false, Message = $"Valores no válidos para la realizar la operación." };
            return BadRequest(ApiResult);
        }
        protected IActionResult BadRequestResult(string message)
        {
            var ApiResult = new ApiResult<dynamic> { IsSuccess = false, Message = message };
            return BadRequest(ApiResult);
        }
        protected IActionResult SelectNoContentResult()
        {
            var ApiResult = new ApiResult<dynamic> { IsSuccess = false, Message = $"No existen registros con cumplan con el criterio de búsqueda." };
            return StatusCode(StatusCodes.Status200OK, ApiResult);
        }
        protected IActionResult ControlledException(string message)
        {
            var ApiResult = new ApiResult<dynamic> { IsSuccess = false, Message = message };
            return StatusCode(StatusCodes.Status200OK, ApiResult);
        }
        protected IActionResult ControlledException(ControllerException ex)
        {
            var ApiResult = new ApiResult<dynamic> { IsSuccess = ex.Status, Data = ex.ErrorData, Message = ex.Message };
            return StatusCode(ex.CodeStatus, ApiResult);
        }
        protected IActionResult InternalServerError(Exception ex)
        {
            var ApiResult = new ApiResult<dynamic> { IsSuccess = false, Message = $"Internal server error: {ex.Message} - {ex.InnerException?.Message}" };
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResult);
        }

        protected IActionResult InternalServerError(string message)
        {
            var ApiResult = new ApiResult<dynamic> { IsSuccess = false, Message = message };
            return StatusCode(StatusCodes.Status500InternalServerError, ApiResult);
        }
    }
}
