using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace userGroup_Management.Entities
{
    public class User
    {
        public User()
        {

        }
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Group> userGroups { get; set; }
    }
}