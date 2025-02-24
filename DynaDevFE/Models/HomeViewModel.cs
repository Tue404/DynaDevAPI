using DynaDevFE.Models;
using System.Collections.Generic;

namespace DynaDevFE.Models
{
    public class HomeViewModel
    {
        public List<SanPhamViewModel> Products { get; set; }
        public List<LoaiSPViewModel> Categories { get; set; }
    }
}
