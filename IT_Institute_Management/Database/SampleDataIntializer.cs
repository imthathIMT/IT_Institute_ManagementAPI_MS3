using IT_Institute_Management.Entity;
using Microsoft.Data.SqlClient;

namespace IT_Institute_Management.Database
{
    public class SampleDataIntializer
    {
        private readonly string _connectionString = "Server =(localdb)\\MSSQLLocalDB; Database=DevHub; Trusted_Connection=True; TrustServerCertificate=True;";

        public void AddMasterAdminSampleData()
        {
            string query = @"
            IF NOT EXISTS (SELECT 1 FROM Users WHERE NIC = @NIC)
            BEGIN
                INSERT INTO Users (Id, NIC, Password, Role) 
                VALUES (@Id, @NIC, @Password, @Role);

                INSERT INTO Admins (NIC, Name, Password, Email, Phone, ImagePath, UserId) 
                VALUES (@NIC, @Name, @Password, @Email, @Phone, @ImagePath, @UserId);
            END";

            Guid userId = Guid.NewGuid();
            string nic = "200206601718";
            string password = "AQAAAAIAAYagAAAAENEU8PTCmOGdKOGoyJ/GereRGnBy0J7AB/YA2edgYRyn4+e0YMS1kchDulbdkH6MoA==";
            string name = "Imthath";
            string email = "mhdimthathHameem@gmail.com";
            string phone = "0768210306";
            string imagePath = "/images/admins/10627cac-b151-4ca4-b9c8-68931f9418fb.jpg";

            ExecuteQuery(query, new Dictionary<string, object>
            {
                {"@Id", userId},
                {"@NIC", nic},
                {"@Password", password},
                {"@Role", (int)Role.MasterAdmin},
                {"@Name", name},
                {"@Email", email},
                {"@Phone", phone},
                {"@ImagePath", imagePath},
                {"@UserId", userId}
            });
        }

        public void AddAdminSampleData()
        {
            string query = @"
            IF NOT EXISTS (SELECT 1 FROM Users WHERE NIC = @NIC)
            BEGIN
                INSERT INTO Users (Id, NIC, Password, Role) 
                VALUES (@UserId, @NIC, @Password, @Role);

                INSERT INTO Admins (NIC, Name, Password, Email, Phone, ImagePath, UserId) 
                VALUES (@NIC, @Name, @Password, @Email, @Phone, @ImagePath, @UserId);
            END";

            Guid userId = Guid.NewGuid();
            string nic = "987654321V";
            string password = "Admin@123";
            string name = "Pppiragash";
            string email = "admin@example.com";
            string phone = "+123456789";
            string imagePath = "/images/admin.jpg";

            ExecuteQuery(query, new Dictionary<string, object>
            {
                {"@UserId", userId},
                {"@NIC", nic},
                {"@Password", password},
                {"@Role", (int)Role.Admin},
                {"@Name", name},
                {"@Email", email},
                {"@Phone", phone},
                {"@ImagePath", imagePath}
            });
        }

        public void AddStudentSampleData()
        {
            string query = @"
            IF NOT EXISTS (SELECT 1 FROM Users WHERE NIC = @NIC)
            BEGIN
                INSERT INTO Users (Id, NIC, Password, Role) 
                VALUES (@UserId, @NIC, @Password, @Role);

                INSERT INTO Students (NIC, FirstName, LastName, Email, Phone, Password, ImagePath, IsLocked, FailedLoginAttempts, UserId) 
                VALUES (@NIC, @FirstName, @LastName, @Email, @Phone, @Password, @ImagePath, @IsLocked, @FailedLoginAttempts, @UserId);

                INSERT INTO Address (Id, AddressLine1, AddressLine2, City, State, PostalCode, Country, StudentNIC)
                VALUES (@AddressId, @AddressLine1, @AddressLine2, @City, @State, @PostalCode, @Country, @NIC);

                INSERT INTO SocialMediaLinks (Id, LinkedIn, Instagram, Facebook, GitHub, WhatsApp, StudentNIC)
                VALUES (@SocialMediaId, @LinkedIn, @Instagram, @Facebook, @GitHub, @WhatsApp, @NIC);
            END";

            Guid userId = Guid.NewGuid();
            string nic = "123456789V";
            string password = "Student@123";
            string firstName = "Mohamed";
            string lastName = "safeek";
            string email = "student@example.com";
            string phone = "0761234567";
            string imagePath = "/images/student.jpg";
            bool isLocked = false;
            int failedLoginAttempts = 0;

            Guid addressId = Guid.NewGuid();
            string addressLine1 = "123 Main Street";
            string addressLine2 = "Apt 4B";
            string city = "Colombo";
            string state = "Western";
            string postalCode = "12345";
            string country = "Sri Lanka";

            Guid socialMediaId = Guid.NewGuid();
            string linkedIn = "https://linkedin.com/in/student";
            string instagram = "https://instagram.com/student";
            string facebook = "https://facebook.com/student";
            string gitHub = "https://github.com/student";
            string whatsapp = "https://wa.me/+947654321";

            ExecuteQuery(query, new Dictionary<string, object>
            {
                {"@UserId", userId},
                {"@NIC", nic},
                {"@Password", password},
                {"@Role", (int)Role.Student},
                {"@FirstName", firstName},
                {"@LastName", lastName},
                {"@Email", email},
                {"@Phone", phone},
                {"@ImagePath", imagePath},
                {"@IsLocked", isLocked},
                {"@FailedLoginAttempts", failedLoginAttempts},
                {"@AddressId", addressId},
                {"@AddressLine1", addressLine1},
                {"@AddressLine2", addressLine2},
                {"@City", city},
                {"@State", state},
                {"@PostalCode", postalCode},
                {"@Country", country},
                {"@SocialMediaId", socialMediaId},
                {"@LinkedIn", linkedIn},
                {"@Instagram", instagram},
                {"@Facebook", facebook},
                {"@GitHub", gitHub},
                {"@WhatsApp", whatsapp}
            });
        }

        private void ExecuteQuery(string query, Dictionary<string, object> parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }

                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Sample data processed successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}