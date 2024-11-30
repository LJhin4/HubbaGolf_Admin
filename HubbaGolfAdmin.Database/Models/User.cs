using System;
using System.Collections.Generic;

namespace HubbaGolfAdmin.Database.Models;

public partial class User
{
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? DisplayName { get; set; }

    public string? Password { get; set; }

    public string? Description { get; set; }

    public int? Active { get; set; }

    public DateTime? LastLoginOn { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int? Slug { get; set; }

    public int? Level { get; set; }

    public int? RecordStatus { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedName { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedName { get; set; }
}
