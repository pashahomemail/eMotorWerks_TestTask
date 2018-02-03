using EntityFramework.Extensions;
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
    [RoutePrefix("api/groups")]
    public class GroupsController : ApiController
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult getGroupInfo([FromUri]int id)
        {
            var result = new GroupsResponseModel();
            var group = context.Groups.FirstOrDefault(f => f.Id == id);
            var userModel = group.Users.Select(s => new UserModel { id = s.Id, name = s.Name }).ToList();

            if (group == null)
            {
                return Ok();
            }
            var mainQuery = context.Groups.AsNoTracking().ToList();


            var idsChildrens = context.GroupsRelation.AsNoTracking().Where(w => w.parentGroupId == id).Select(s => s.childGroupId);
            var idsParents = context.GroupsRelation.AsNoTracking().Where(w => w.childGroupId == id).Select(s => s.parentGroupId);
            var childrens = mainQuery.Where(w => idsChildrens.Contains(w.Id)).Select(s => new GroupModel { id = s.Id, name = s.Name }).ToList();
            var parents = mainQuery.Where(w => idsParents.Contains(w.Id)).Select(s => new GroupModel { id = s.Id, name = s.Name }).ToList();

            result.childrens = childrens;
            result.id = id;
            result.name = group.Name;
            result.parents = parents;
            result.users = userModel;

            return Ok(result);
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult getGroups()
        {
            var result = new List<GroupsResponseModel>();
            var mainQuery = context.Groups.AsNoTracking().ToList();

            foreach (var item in mainQuery)
            {
                var userModel = item.Users.Select(s => new UserModel { id = s.Id, name = s.Name }).ToList();
                var idsChildrens = context.GroupsRelation.AsNoTracking().Where(w => w.parentGroupId == item.Id).Select(s => s.childGroupId);
                var idsParents = context.GroupsRelation.AsNoTracking().Where(w => w.childGroupId == item.Id).Select(s => s.parentGroupId);
                var childrens = mainQuery.Where(w => idsChildrens.Contains(w.Id)).Select(s => new GroupModel { id = s.Id, name = s.Name }).ToList();
                var parents = mainQuery.Where(w => idsParents.Contains(w.Id)).Select(s => new GroupModel { id = s.Id, name = s.Name }).ToList();
                result.Add(new GroupsResponseModel
                {
                    id = item.Id,
                    name = item.Name,
                    parents = parents,
                    childrens = childrens,
                    users = userModel
                });
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult addNewGroup(NewGroupModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            context.Groups.Add(new Group { Name = model.name });
            context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult deleteGroup([FromUri]int id)
        {
            var group = context.Groups.FirstOrDefault(f => f.Id == id);
            if (group == null)
            {
                return Content(HttpStatusCode.NotFound, $"Group is not found by id: {id}");
            }

            context.Groups.Remove(group);
            context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        [Route("parent")]
        public IHttpActionResult addGroupParent(AddGroupToGroupModel model)
        {
            var groupParent = context.Groups.FirstOrDefault(f => f.Id == model.parentId);
            var groupChildren = context.Groups.FirstOrDefault(f => f.Id == model.childrenId);
            if (groupParent == null || groupChildren == null)
            {
                return Content(HttpStatusCode.NotFound, "groups not found");
            }

            context.GroupsRelation.Add(new Entities.GroupRelation
            {
                childGroupId = groupChildren.Id,
                childGroupName = groupChildren.Name,
                parentGroupId = groupParent.Id,
                parentGroupName = groupParent.Name
            });
            context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        [Route("{id}/parent")]
        public IHttpActionResult deleteParentFromGroup([FromUri]int id, DeleteGroupToGroupModel model)
        {
            var group = context.Groups.FirstOrDefault(f => f.Id == model.id);
            var ids = context.GroupsRelation.AsNoTracking().Where(w => model.id == w.parentGroupId && w.childGroupId == id).Select(s => s.parentGroupId);
            context.GroupsRelation.Where(w => ids.Contains(w.parentGroupId)).Delete();

            return Ok();
        }

        [HttpPost]
        [Route("children")]
        public IHttpActionResult addGroupChildren(AddGroupToGroupModel model)
        {
            var groupParent = context.Groups.FirstOrDefault(f => f.Id == model.parentId);
            var groupChildren = context.Groups.FirstOrDefault(f => f.Id == model.childrenId);
            if (groupParent == null || groupChildren == null)
            {
                return Content(HttpStatusCode.NotFound, "groups not found");
            }

            context.GroupsRelation.Add(new Entities.GroupRelation
            {
                childGroupId = groupChildren.Id,
                childGroupName = groupChildren.Name,
                parentGroupId = groupParent.Id,
                parentGroupName = groupParent.Name
            });
            context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        [Route("children")]
        public IHttpActionResult deleteChildrenFromGroup(DeleteGroupToGroupModel model)
        {
            var group = context.Groups.FirstOrDefault(f => f.Id == model.id);
            var ids = context.GroupsRelation.AsNoTracking().Where(w => model.toDeleteIds.Contains(w.childGroupId) && w.parentGroupId == model.id).Select(s => s.childGroupId);
            context.GroupsRelation.Where(w => ids.Contains(w.childGroupId)).Delete();

            return Ok();
        }
    }
}
