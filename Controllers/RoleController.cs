using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Role_Management_.NET.Context;
using Role_Management_BackEnd.Models;

namespace Role_Management_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {

        private readonly AppDbContext _authContext;
        public RoleController(AppDbContext context)
        {
            _authContext = context;
        }

         [Authorize]
         [HttpGet("getroles")]
         public async Task<ActionResult<Roles>> GetAllRoles()
         {
          return Ok(await _authContext.Roles.ToListAsync());
         }

    }
}

