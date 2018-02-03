using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using userGroup_Management.DAL;
using userGroup_Management.Entities;
using userGroup_Management.Models;

namespace userGroup_Management.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult getUserDetails([FromUri]int id)
        {
            var user = context.Users.FirstOrDefault(f => f.Id == id);
            if (user == null)
            {
                return Ok();
            }

            return Ok(user);
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult getAllUsers()
        {
            var users = context.Users.ToList();

            if (!users.Any())
            {
                List<User> testResult = new List<User>();
                testResult.Add(new User { Id = 1, Name = "test" });

                context.Users.AddRange(testResult);
                context.SaveChanges();

                return Ok(testResult);
            }

            return Ok(users);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult addUser(NewUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Users.Add(new User
            {
                Name = model.name
            });
            context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult deleteUser([FromUri]int id)
        {
            var user = context.Users.FirstOrDefault(f => f.Id == id);

            if (user == null)
            {
                return Content(HttpStatusCode.NotFound, "User not found!");
            }

            context.Users.Remove(user);
            context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("addGroup")]
        public IHttpActionResult addUserToGroup(AddUserToGroupModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = context.Users.FirstOrDefault(f => f.Id == model.userId);
            if (user == null)
            {
                return Content(HttpStatusCode.NotFound, $"user not found by id: {model.userId}");
            }
            var group = context.Groups.FirstOrDefault(f => f.Id == model.groupId);

            user.userGroups.Add(group);
            context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        [Route("{id}/removeGroup/{groupId}")]
        public IHttpActionResult removeGroupFromUser([FromUri]int id, [FromUri]int groupId)
        {
            var user = context.Users.FirstOrDefault(f => f.Id == id);
            if (user == null)
            {
                return Ok();
            }

            var group = user.userGroups.FirstOrDefault(f => f.Id == groupId);
            user.userGroups.Remove(group);
            context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("{id}/free")]
        public IHttpActionResult getFreeGroupsForUser([FromUri]int id)
        {
            var ids = context.Users.FirstOrDefault(f => f.Id == id).userGroups.Select(s => s.Id);
            var groups = context.Groups.AsNoTracking().Where(w => !ids.Contains(w.Id)).Select(s => new { s.Id, s.Name });
            return Ok(groups);
        }
    }
}
