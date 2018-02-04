using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using userGroup_Management.DAL;
using userGroup_Management.Entities;

namespace userGroup_Management
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            //jsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            CheckTestData();
        }
        /// <summary>
        /// create groups and users like test task
        /// </summary>
        private static void CheckTestData()
        {
            var context = new ApplicationDbContext();

            var usersCheck = !context.Users.Any();
            var groupsCheck = !context.Groups.Any();
            if (usersCheck && groupsCheck)
            {
                //groups
                var groupEmployees = new Group() { Name = "Employees" };
                var groupUSA = new Group() { Name = "USA" };
                var groupManagers = new Group() { Name = "Managers" };
                var groupUkraine = new Group() { Name = "Ukraine" };
                var groupDevelopers = new Group() { Name = "Developers" };
                var groupMainOffice = new Group() { Name = "Main Office" };
                
                //users
                var userOCole = new User() { Name = "O. Cole", userGroups = new List<Group>() };
                var userJShane = new User() { Name = "J. Shane", userGroups = new List<Group>() };
                var userVPetrov = new User() { Name = "V. Petrov", userGroups = new List<Group>() };
                var userMPopov = new User() { Name = "M. Popov", userGroups = new List<Group>() };
                
                //relation user to group
                userOCole.userGroups.Add(groupUSA);
                userJShane.userGroups.Add(groupUSA);
                userJShane.userGroups.Add(groupManagers);

                userVPetrov.userGroups.Add(groupMainOffice);
                userMPopov.userGroups.Add(groupMainOffice);

                context.Groups.Add(groupEmployees);
                context.Groups.Add(groupUSA);
                context.Groups.Add(groupManagers);
                context.Groups.Add(groupUkraine);
                context.Groups.Add(groupDevelopers);
                context.Groups.Add(groupMainOffice);

                context.Users.Add(userOCole);
                context.Users.Add(userJShane);
                context.Users.Add(userVPetrov);
                context.Users.Add(userMPopov);

                context.SaveChanges();

                var groupEmployeesdb = context.Groups.FirstOrDefault(f => f.Name == groupEmployees.Name);
                var groupUSAdb = context.Groups.FirstOrDefault(f => f.Name == groupUSA.Name);
                var groupManagersdb = context.Groups.FirstOrDefault(f => f.Name == groupManagers.Name);
                var groupUkrainedb = context.Groups.FirstOrDefault(f => f.Name == groupUkraine.Name);
                var groupDevelopersdb = context.Groups.FirstOrDefault(f => f.Name == groupDevelopers.Name);
                var groupMainOfficedb = context.Groups.FirstOrDefault(f => f.Name == groupMainOffice.Name);

                //set relaions with groups
                //for USA
                context.GroupsRelation.Add(new GroupRelation
                {
                    childGroupId = groupUSAdb.Id,
                    childGroupName = groupUSAdb.Name,
                    parentGroupId = groupEmployeesdb.Id,
                    parentGroupName = groupEmployeesdb.Name
                });
                //for managers
                context.GroupsRelation.Add(new GroupRelation
                {
                    childGroupId = groupManagersdb.Id,
                    childGroupName = groupManagersdb.Name,
                    parentGroupId = groupEmployeesdb.Id,
                    parentGroupName = groupEmployeesdb.Name
                });
                //for ukraine
                context.GroupsRelation.Add(new GroupRelation
                {
                    childGroupId = groupUkrainedb.Id,
                    childGroupName = groupUkrainedb.Name,
                    parentGroupId = groupEmployeesdb.Id,
                    parentGroupName = groupEmployeesdb.Name
                });
                //for developers
                context.GroupsRelation.Add(new GroupRelation
                {
                    childGroupId = groupDevelopersdb.Id,
                    childGroupName = groupDevelopersdb.Name,
                    parentGroupId = groupEmployeesdb.Id,
                    parentGroupName = groupEmployeesdb.Name
                });
                //for main office
                context.GroupsRelation.Add(new GroupRelation
                {
                    childGroupId = groupMainOfficedb.Id,
                    childGroupName = groupMainOfficedb.Name,
                    parentGroupId = groupUkrainedb.Id,
                    parentGroupName = groupUkrainedb.Name
                });
                //for main office
                context.GroupsRelation.Add(new GroupRelation
                {
                    childGroupId = groupMainOfficedb.Id,
                    childGroupName = groupMainOfficedb.Name,
                    parentGroupId = groupDevelopersdb.Id,
                    parentGroupName = groupDevelopersdb.Name
                });

                context.SaveChanges();
            }

        }
    }
}
