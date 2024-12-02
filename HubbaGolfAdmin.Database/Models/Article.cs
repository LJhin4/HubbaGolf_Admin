using System;
using System.Collections.Generic;

namespace HubbaGolfAdmin.Database.Models;

public partial class Article
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public int? MenuId { get; set; }

    public string? Title { get; set; }

    public string? Summary { get; set; }

    public string? Content { get; set; }

    public string? UrlImage { get; set; }

    public string? Icon { get; set; }

    public string? Author { get; set; }

    public DateTime? Date { get; set; }

    public int? Status { get; set; }

    public int? Rank { get; set; }

    public string? Link { get; set; }

    public string? Keyword { get; set; }

    public string? Location { get; set; }

    public bool? IsParent { get; set; }

    public string? Description { get; set; }

    public DateTime? DateExp { get; set; }

    public int? RecordStatus { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedName { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedName { get; set; }
}
