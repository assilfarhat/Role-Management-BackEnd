using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Role_Management_.NET.Context;
using Role_Management_BackEnd.Models;
using System.Security;

namespace Role_Management_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RolesController(AppDbContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpGet("getRoles")]
        public async Task<ActionResult<IEnumerable<Roles>>> GetAllRoles()
        {
            var rolesWithPermissionsAndUsers = await _context.Roles
                .Include(r => r.Permissions) // Include associated permissions
           
                .ToListAsync();

            return Ok(rolesWithPermissionsAndUsers);
        }


        [Authorize]
        [HttpPost("addrole")]
        public async Task<IActionResult> AddRole([FromBody] Roles role)
        {
            if (role == null)
            {
                return BadRequest("Role data is invalid.");
            }

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Status = 200,
                Message = "Role Added!"
            });
        }
        [Authorize]
        [HttpGet("get-permissions/{roleId}")]
        public async Task<IActionResult> GetPermissionsForRole(int roleId)
        {
            var role = await _context.Roles
                .Include(r => r.Permissions) // Make sure to include the permissions collection
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
            {
                return NotFound("Role not found.");
            }

            return Ok(role.Permissions);
        }


/*
        [Authorize]
        [HttpPost("addPermissionsToRole/{roleId}")]
        public async Task<IActionResult> AddPermissionsToRole(int roleId, [FromBody] List<string> permissionNames)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role == null)
            {
                return NotFound("Role not found.");
            }

            if (permissionNames == null || permissionNames.Count == 0)
            {
                return BadRequest("No permission names provided.");
            }

            var permissionsToAdd = await _context.Permissions
                .Where(p => permissionNames.Contains(p.PermissionName))
                .ToListAsync();

            foreach (var permission in permissionsToAdd)
            {
                role.Permissions.Add(permission);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Status = 200,
                Message = "Permissions added to role!"
            });
        }

*/

        /*[Authorize]
        [HttpDelete("deletePermissionsFromRole/{roleId}")]
        public async Task<IActionResult> DeletePermissionsFromRole(int roleId, [FromBody] List<int> permissionIds)
        {
            var role = await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
            {
                return NotFound("Role not found.");
            }

            if (permissionIds == null || permissionIds.Count == 0)
            {
                return BadRequest("No permission IDs provided.");
            }

            var permissionsToDelete = role.Permissions
                .Where(p => permissionIds.Contains(p.Id))
                .ToList();

            foreach (var permission in permissionsToDelete)
            {
                role.Permissions.Remove(permission);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Status = 200,
                Message = "Permissions removed from role!"
            });
        }
*/
        [Authorize]
        [HttpGet("getAllPermissions")]
        public async Task<ActionResult<IEnumerable<Permissions>>> GetAllPermissions()
        {
            var permissions = await _context.Permissions.ToListAsync();
            return Ok(permissions);
        }

        private async Task UpdateRolePermissions(Roles role, List<string> newPermissionNames)
        {
            // Get the existing permissions of the role
            var existingPermissionNames = await _context.Permissions
                .Where(p => role.Permissions.Contains(p))
                .Select(p => p.PermissionName)
                .ToListAsync();

            // Find permissions to add
            var permissionsToAdd = newPermissionNames.Except(existingPermissionNames);

            // Find permissions to remove
            var permissionsToRemove = existingPermissionNames.Except(newPermissionNames);

            // Add new permissions to the role
            foreach (var permissionNameToAdd in permissionsToAdd)
            {
                var permissionToAdd = await _context.Permissions.FirstOrDefaultAsync(p => p.PermissionName == permissionNameToAdd);
                if (permissionToAdd != null)
                {
                    role.Permissions.Add(permissionToAdd);
                }
            }

            // Remove permissions from the role
            foreach (var permissionNameToRemove in permissionsToRemove)
            {
                var permissionToRemove = await _context.Permissions.FirstOrDefaultAsync(p => p.PermissionName == permissionNameToRemove);
                if (permissionToRemove != null)
                {
                    role.Permissions.Remove(permissionToRemove);
                }
            }

            await _context.SaveChangesAsync();
        }

        [Authorize]
        [HttpPut("updateRolePermissions/{roleId}")]
        public async Task<IActionResult> UpdateRolePermissions(int roleId, [FromBody] List<string> newPermissionNames)
        {
            var role = await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
            {
                return NotFound("Role not found.");
            }

            await UpdateRolePermissions(role, newPermissionNames);

            return Ok(new
            {
                Status = 200,
                Message = "Role permissions updated!"
            });
        }


        [Authorize]
        [HttpGet("getPermissionNamesByRoleName/{roleName}")]
        public async Task<ActionResult<IEnumerable<string>>> GetPermissionNamesByRoleName(string roleName)
        {
            var permissionNames = await _context.Roles
                .Where(r => r.RoleName == roleName)
                .SelectMany(r => r.Permissions)
                .Select(p => p.PermissionName)
                .ToListAsync();

            if (permissionNames == null || permissionNames.Count == 0)
            {
                return NotFound("Permission names not found.");
            }

            return Ok(permissionNames);
        }




        [Authorize]
        [HttpDelete("deleteRole/{id}")]
         public async Task<IActionResult> DeleteRole(int id)
         {
             var role = await _context.Roles
               
                 .FirstOrDefaultAsync(r => r.Id == id);

             if (role == null)
             {
                 return NotFound("Role not found.");
             }

            
             _context.Roles.Remove(role);
             await _context.SaveChangesAsync();

             return NoContent();
         }

    }
}
