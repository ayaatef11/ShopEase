using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.ViewModels
{
    public class PermissionsFormViewModel
    {
        public string RoleId { get; set; } = string.Empty;
        public string RoleName { get; set; }=string.Empty;
        public List<CheckBoxViewModel> RoleCalims { get; set; }=new List<CheckBoxViewModel>();      
    }
}
