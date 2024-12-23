using Microsoft.AspNetCore.Http;

namespace HubbaGolfAdmin.Database.Dtos
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public int? MenuId { get; set; }
        public int? CategoryId { get; set; }
        public string? TagId { get; set; }
        public string? Title { get; set; }
        public string? MetaTitle { get; set; }
        public string? Summary { get; set; }
        public string? Content { get; set; }
        public int? Status { get; set; }
        public string? HiddenStatus { get; set; }
        public string? HiddenParent { get; set; }
        public string? Author { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? Icon { get; set; }
        public IFormFile? FileIcon { get; set; }
        public string? UrlImage { get; set; }
        public IFormFile? FileImage { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public bool? IsParent { get; set; }
        public DateTime? DateExp { get; set; }

        public List<IFormFile>? FileDocument { get; set; }

        public string? Link { get; set; }
        public int? Rank { get; set; }
        public int? Type { get; set; }
        public decimal? Price { get; set; }
        public string? Childs { get; set; }
        public string? Itinerary { get; set; }
    }
}