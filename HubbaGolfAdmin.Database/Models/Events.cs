using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;

namespace HubbaGolfAdmin.Database.Models
{
    public partial class Events
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Location { get; set; }
        public bool? IsAllDay { get; set; } = false;
        public int? StatusRecord { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int? PlayerNumber { get; set; }
        public string? Country { get; set; }
        public string? Course { get; set; }
        public string? OrderNumber { get; set; }
    }
}
