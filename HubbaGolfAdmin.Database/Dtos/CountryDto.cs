using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HubbaGolfAdmin.Database.Models;

namespace HubbaGolfAdmin.Database.Dtos
{
    public class CountryDto
    {
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
        public List<CategoryDto>? Provinces { get; set; }
    }
}
