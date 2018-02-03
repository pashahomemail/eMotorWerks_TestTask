using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace userGroup_Management.Models
{
    public class AddGroupToGroupModel
    {
        public int childrenId { get; set; }
        public int parentId { get; set; }
    }
}