using System;
using System.Collections.Generic;

namespace HubbaGolfAdmin.Database.Models;

public partial class Menu
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Section { get; set; }

    public string? Location { get; set; }

    public int? Sort { get; set; }

    public string? Icon { get; set; }

    public string? Link { get; set; }

    public string? Action { get; set; }

    public string? Controller { get; set; }

    public string? Params { get; set; }

    public int? Parent { get; set; }

    public string? GroupMenu { get; set; }

    public int? RecordStatus { get; set; }

    public string? QueryCode { get; set; }

    public int? Slug { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedName { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedName { get; set; }
}
