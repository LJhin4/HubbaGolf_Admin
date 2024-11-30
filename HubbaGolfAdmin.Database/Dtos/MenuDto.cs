namespace HubbaGolfAdmin.Database.Dtos
{
    public class MenuDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Section { get; set; }
        public string? Location { get; set; }
        public int? Sort { get; set; }
        public string? Icon { get; set; }
        public string? Link { get; set; }
        public string? Action { get; set; }
        public string? Controller { get; set; }
        public int? Parent { get; set; }
        public string? GroupMenu { get; set; }
        public string? Params { get; set; }
        public string? CategoryName { get; set; }
        public string? QueryCode { get; set; }
    }
}
