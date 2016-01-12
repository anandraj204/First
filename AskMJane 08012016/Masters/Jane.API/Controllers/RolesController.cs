using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Jane.API.Infrastructure.Common;
using Jane.Core.Models;
using Jane.Data.EntityFramework.Entities;
using Microsoft.AspNet.Identity;

namespace Jane.API.Controllers
{
    [Authorize(Roles = "GlobalAdmin")]
    [RoutePrefix("api/roles")]
    public class RolesController : BaseApiController
    {

        [Route("{id}", Name = "GetRoleById")]
        public async Task<IHttpActionResult> GetRole(int Id)
        {
            var role = await this.HgRoleManager.FindByIdAsync(Id);

            if (role != null)
            {
                return Ok(TheModelFactory.Create(role));
            }

            return NotFound();

        }

        [Route("GetAllRoles")]
        public IHttpActionResult GetAllRoles()
        {
            var roles = HGContext.Roles.ToList();

            return Ok(Mapper.Map<List<RoleModel>>(roles));
        }

        [Route("create")]
        public async Task<IHttpActionResult> Create(CreateRoleBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = new Role { Name = model.Name };

            var result = await this.HgRoleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            Uri locationHeader = new Uri(Url.Link("GetRoleById", new { id = role.Id }));

            return Created(locationHeader, TheModelFactory.Create(role));

        }

        [Route("{id}")]
        public async Task<IHttpActionResult> DeleteRole(int Id)
        {

            var role = await this.HgRoleManager.FindByIdAsync(Id);

            if (role != null)
            {
                IdentityResult result = await this.HgRoleManager.DeleteAsync(role);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();
            }

            return NotFound();

        }

        [Route("ManageUsersInRole")]
        public async Task<IHttpActionResult> ManageUsersInRole(UsersInRoleModel model)
        {
            var role = await this.HgRoleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ModelState.AddModelError("", "Role does not exist");
                return BadRequest(ModelState);
            }

            foreach (var user in model.EnrolledUsers)
            {
                var appUser = await UserManager.FindByIdAsync(user);

                if (appUser == null)
                {
                    ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
                    continue;
                }

                if (!UserManager.IsInRole(user, role.Name))
                {
                    IdentityResult result = await UserManager.AddToRoleAsync(user, role.Name);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", String.Format("User: {0} could not be added to role", user));
                    }

                }
            }

            foreach (var user in model.RemovedUsers)
            {
                var appUser = await UserManager.FindByIdAsync(user);

                if (appUser == null)
                {
                    ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
                    continue;
                }

                IdentityResult result = await UserManager.RemoveFromRoleAsync(user, role.Name);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", String.Format("User: {0} could not be removed from role", user));
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [Route("getUsers")]
        public IHttpActionResult GetUsers()
        {
            var users = HGContext.Users.Include(u => u.Roles);
            List<UserWithRolesModel> result = new List<UserWithRolesModel>(users.Count());

            foreach (User user in users)
            {
                var userModel = Mapper.Map<UserWithRolesModel>(user);
                var ids = user.Roles.Select(r => r.RoleId);

                userModel.RolesList = Mapper.Map<List<RoleModel>>(HGContext.Roles.Where(r => ids.Contains(r.Id)));
                result.Add(userModel);
            }

            return Ok(result);
        }


        [Route("updateUserRoles")]
        [HttpPost]
        public IHttpActionResult UpdateUserRoles(UserWithRolesModel user)
        {
            User userInfo = HGContext.Users.Include(u => u.Roles).FirstOrDefault(u => u.Id == user.Id);
            if (userInfo == null)
                return NotFound();

            var ids = user.RolesList.Select(r => r.Id).ToList();
            userInfo.Roles.Clear();

            foreach (int id in ids)
            {
                userInfo.Roles.Add(new UserRole(){UserId = user.Id, RoleId = id});
            }
            HGContext.SaveChanges();

            return Ok();
        }
    }
}

