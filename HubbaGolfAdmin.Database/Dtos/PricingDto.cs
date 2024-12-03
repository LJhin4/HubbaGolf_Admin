using HubbaGolfAdmin.Database.Models;
using System;
using System.Collections.Generic;

namespace HubbaGolfAdmin.Database.Dtos;

public class PricingDto
{
    public int Id { get; set; }

    public int? ArticleId { get; set; }
    public string? ArticleTitle { get; set; }

    public decimal? Price { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public string? PricingType { get; set; }

    public DateTime? SpecificDate { get; set; }

    public int? Slug { get; set; }

    public string? Description { get; set; }

    public int? RecordStatus { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedName { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedName { get; set; }
}

public class PricingView
{
    public List<PricingDto>? Pricing { get; set; }
    public List<Article>? Course { get; set; }
}
