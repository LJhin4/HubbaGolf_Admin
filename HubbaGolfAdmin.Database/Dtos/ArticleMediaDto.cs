using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubbaGolfAdmin.Database.Dtos
{
    public class ArticleMediaDto
    {
        public int? ArticleId { get; set; }

        public string? Name { get; set; }

        public string? Url { get; set; }

        public int? Type { get; set; }
    }
}
