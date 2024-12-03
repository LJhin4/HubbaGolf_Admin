using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubbaGolfAdmin.Database.Dtos
{
    public class ArticleGroupDto
    {
        public int Id { get; set; }

        public string? Code { get; set; }

        public int? MenuId { get; set; }

        public string? Name { get; set; }

        public int? Sort { get; set; }

        public bool? Status { get; set; }

        public int? Parent { get; set; }
        public string? Type { get; set; }
        public List<ArticleDto>? Courses { get; set; }
    }
}
