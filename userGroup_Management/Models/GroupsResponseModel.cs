using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using userGroup_Management.Entities;

namespace userGroup_Management.Models
{
    public class GroupsResponseModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public ICollection<UserModel> users { get; set; }
        public ICollection<GroupModel> parents { get; set; }
        public ICollection<GroupModel> childrens { get; set; }
        public ICollection<GroupModel> freeParents { get; set; }
        public ICollection<GroupModel> freeChildrens { get; set; }
        public ICollection<GroupModel> free { get; set; }
    }
}