using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.ViewModels
{
    public  class RoleFormViewModel
    {
        [Required, StringLength(256)]
        public string Name { get; set; }=string.Empty;
    }
}
