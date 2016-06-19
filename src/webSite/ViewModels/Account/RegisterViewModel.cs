using Microsoft.AspNet.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webSite.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "下单手机号")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "宝宝小名")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "宝宝生日")]
        public DateTime BirthDate { get; set; }

        [Required]
        [Display(Name = "所在城市")]
        public string City { get; set; }

        [Required]
        [Display(Name = "宝贝性别")]
        public bool Sex { get; set; }
    }
}
