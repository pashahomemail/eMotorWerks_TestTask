using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace userGroup_Management.Entities
{
    public class GroupRelation
    {
        public int id { get; set; }
        [Required]
        public int parentGroupId { get; set; }
        [Required]
        public string parentGroupName { get; set; }
        [Required]
        public int childGroupId { get; set; }
        [Required]
        public string childGroupName { get; set; }
    }
}