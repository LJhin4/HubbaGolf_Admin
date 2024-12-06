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

    /// <summary>
    /// 1: It&apos;s content for 4 container: course/package/simulator/shopping
    /// </summary>
    public bool? IsParent { get; set; }

    public string? Description { get; set; }

    public DateTime? DateExp { get; set; }

    /// <summary>
    /// What type of article does the article belong to among the 4 types: course/simulator/shoping/package
    /// </summary>
    public int? Type { get; set; }

    /// <summary>
    /// know which courses are in the package
    /// </summary>
    public string? Childs { get; set; }

    /// <summary>
    /// package schedule
    /// </summary>
    public string? Itinerary { get; set; }

    public int? RecordStatus { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedName { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedName { get; set; }
}
