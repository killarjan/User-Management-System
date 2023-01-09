namespace UserManagementSystem.DAL.Models
{
    public class UserFullDataDal
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }

        public string PhoneNumber { get; set; }
    }
}
