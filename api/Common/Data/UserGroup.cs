namespace api.Common.Data
{
    public class UserGroup
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }

        public User User { get; set; } = null!;
        public Group Group { get; set; } = null!;
    }
}