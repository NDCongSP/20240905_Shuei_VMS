using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity.Authentication
{
    [Table("ROleToPermission")]
    public class RoleToPermissions
    {
        [Key] public Guid Id { get; set; }
        public Guid IdRole { get; set; }
        public Guid IdPermission { get; set; }
        public string PermisionName { get; set; }
    }
}
