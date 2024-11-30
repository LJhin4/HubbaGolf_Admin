namespace HubbaGolfAdmin.Database.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? UserName { get; set; }
        public string? Description { get; set; }
        public DateTime? LastLoginOn { get; set; }
        public int? Active { get; set; }
        public string? Password { get; set; }
        public string? PasswordOld { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public string? DisplayName { get; set; }

        public bool PasswordChanged
        {
            get
            {
                if (!string.IsNullOrEmpty(Password)
                    && !string.IsNullOrEmpty(PasswordOld)
                    && Password?.ToLower() != PasswordOld?.ToLower())
                {
                    return true;
                }

                if (string.IsNullOrEmpty(PasswordOld))
                    return true;

                return false;
            }
        }
    }
}
