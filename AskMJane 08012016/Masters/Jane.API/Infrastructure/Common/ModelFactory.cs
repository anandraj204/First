using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;
using Jane.Data.EntityFramework.Entities;
using Jane.Data.EntityFramework.Managers;

namespace Jane.API.Infrastructure.Common
{
    public class ModelFactory
    {
        private UrlHelper _UrlHelper;
        private HgUserManager _hgUserManager;

        public ModelFactory(HttpRequestMessage request, HgUserManager appUserManager)
        {
            _UrlHelper = new UrlHelper(request);
            _hgUserManager = appUserManager;
        }

        public UserReturnModel Create(User appUser)
        {
            return new UserReturnModel
            {
                Url = _UrlHelper.Link("GetUserById", new {id = appUser.Id}),
                Id = appUser.Id,
                UserName = appUser.UserName,
                FullName = string.Format("{0} {1}", appUser.FirstName, appUser.LastName),
                Email = appUser.Email,
                EmailConfirmed = appUser.EmailConfirmed,
                // Level = appUser.Level,
                JoinDate =  DateTime.UtcNow,
                Roles = _hgUserManager.GetRolesAsync(appUser.Id).Result,
                Claims = _hgUserManager.GetClaimsAsync(appUser.Id).Result
            };
        }
        public RoleReturnModel Create(Role appRole)
        {

            return new RoleReturnModel
            {
                Url = _UrlHelper.Link("GetRoleById", new { id = appRole.Id }),
                Id = appRole.Id,
                Name = appRole.Name
            };
        }
    }

    public class RoleReturnModel
    {
        public string Url { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class UserReturnModel
    {
        public string Url { get; set; }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public int Level { get; set; }
        public DateTime JoinDate { get; set; }
        public IList<string> Roles { get; set; }
        public IList<System.Security.Claims.Claim> Claims { get; set; }
    }
}