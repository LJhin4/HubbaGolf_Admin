using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubbaGolfAdmin.Database.Dtos
{
    public class MenuHeaderDto
    {
        public string? title {  get; set; }
        public string? link { get; set; }
        public List<ItemCountry>? subMenu { get; set; }
    }
    public class ItemCountry
    {
        public int? id { get; set; }
        public string? title { get; set; }
        public List<ItemProcince>? items { get; set; }
    }
    public class ItemProcince
    {
        public int? id { get; set; }
        public string? name { get; set; }
    }
}
