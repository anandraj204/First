using System.Collections.Generic;
using Jane.Core.Models;

namespace Jane.Web.Areas.Menu.Models
{
    public class MenuViewModel
    {
        public UserModel User { get; set; }
        public List<DispensaryProductModel> Products { get; set; }
        public OrderModel Cart { get; set; }
    }
}