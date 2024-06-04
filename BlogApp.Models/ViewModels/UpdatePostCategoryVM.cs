using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Models.ViewModels
{
    public class UpdatePostCategoryVM
    {
        public int PostId { get; set; }
        public int CategoryId { get; set; }
    }
}
