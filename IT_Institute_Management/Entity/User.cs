namespace IT_Institute_Management.Entity
{
    public class User
    {
        public Guid Id { get; set; }
        public string NIC { get; set; }
        public string Password { get; set; }    
        public Role Role { get; set; }

        public Student Student { get; set; }
        public Admin Admin { get; set; }
    }

    public enum Role
    {
        Admin = 1,
        Student = 2,
        MasterAdmin = 3

    }
}
