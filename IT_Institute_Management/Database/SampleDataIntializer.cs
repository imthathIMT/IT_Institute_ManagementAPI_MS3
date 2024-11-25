using IT_Institute_Management.Entity;
using Microsoft.Data.SqlClient;

namespace IT_Institute_Management.Database
{
    public class SampleDataIntializer
    {
        // Define the connection string to the DevHub database
        private readonly string _connectionString = "Server =(localdb)\\MSSQLLocalDB; Database=DevHub; Trusted_Connection=True; TrustServerCertificate=True;";

        public void AddMasterAdminSampleData()
        {
            string query = @"
            INSERT INTO Users (Id, NIC, Password, Role) 
            VALUES (@Id, @NIC, @Password, @Role);

            INSERT INTO Admins (NIC, Name, Password, Email, Phone, ImagePath, UserId) 
            VALUES (@NIC, @Name, @Password, @Email, @Phone, @ImagePath, @UserId);
        ";

            // Sample Data
            Guid userId = Guid.NewGuid();
            string nic = "200206601718";
            string password = "AQAAAAIAAYagAAAAENEU8PTCmOGdKOGoyJ/GereRGnBy0J7AB/YA2edgYRyn4+e0YMS1kchDulbdkH6MoA=="; // Imthath@2002
            string name = "Imthath";
            string email = "mhdimthathHameem@gmail.com";
            string phone = "0768210306";
            string imagePath = "/images/admins/10627cac-b151-4ca4-b9c8-68931f9418fb.jpg";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@Id", userId);
                        command.Parameters.AddWithValue("@NIC", nic);
                        command.Parameters.AddWithValue("@Password", password); // Hash this in production
                        command.Parameters.AddWithValue("@Role", (int)Role.MasterAdmin);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Phone", phone);
                        command.Parameters.AddWithValue("@ImagePath", imagePath);
                        command.Parameters.AddWithValue("@UserId", userId);

                        // Open the database connection and execute the query
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("MasterAdmin sample data inserted successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void AddAdminSampleData()
        {
            string query = @"
            INSERT INTO Users (Id, NIC, Password, Role) 
            VALUES (@UserId, @NIC, @Password, @Role);

            INSERT INTO Admins (NIC, Name, Password, Email, Phone, ImagePath, UserId) 
            VALUES (@NIC, @Name, @Password, @Email, @Phone, @ImagePath, @UserId);
        ";

            // Admin Sample Data
            Guid userId = Guid.NewGuid();
            string nic = "987654321V";
            string password = "Admin@123"; // Ensure to hash passwords in production
            string name = "Admin User";
            string email = "admin@example.com";
            string phone = "+123456789";
            string imagePath = "/images/admin.jpg";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@NIC", nic);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@Role", (int)Role.Admin);
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Phone", phone);
                        command.Parameters.AddWithValue("@ImagePath", imagePath);

                        // Execute the query
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Admin sample data inserted successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding admin data: {ex.Message}");
            }
        }

        public void AddStudentSampleData()
        {
            string query = @"
            INSERT INTO Users (Id, NIC, Password, Role) 
            VALUES (@UserId, @NIC, @Password, @Role);

            INSERT INTO Students (NIC, FirstName, LastName, Email, Phone, Password, ImagePath, IsLocked, FailedLoginAttempts, UserId) 
            VALUES (@NIC, @FirstName, @LastName, @Email, @Phone, @Password, @ImagePath, @IsLocked, @FailedLoginAttempts, @UserId);

            INSERT INTO Addresses (Id, AddressLine1, AddressLine2, City, State, PostalCode, Country, StudentNIC)
            VALUES (@AddressId, @AddressLine1, @AddressLine2, @City, @State, @PostalCode, @Country, @NIC);

            INSERT INTO SocialMediaLinks (Id, LinkedIn, Instagram, Facebook, GitHub, WhatsApp, StudentNIC)
            VALUES (@SocialMediaId, @LinkedIn, @Instagram, @Facebook, @GitHub, @WhatsApp, @NIC);
        ";

            // Student Sample Data
            Guid userId = Guid.NewGuid();
            string nic = "123456789V";
            string password = "Student@123"; // Ensure to hash passwords in production
            string firstName = "John";
            string lastName = "Doe";
            string email = "student@example.com";
            string phone = "0761234567";
            string imagePath = "/images/student.jpg";
            bool isLocked = false;
            int failedLoginAttempts = 0;

            // Address Sample Data
            Guid addressId = Guid.NewGuid();
            string addressLine1 = "123 Main Street";
            string addressLine2 = "Apt 4B";
            string city = "Colombo";
            string state = "Western";
            string postalCode = "12345";
            string country = "Sri Lanka";

            // Social Media Links Sample Data
            Guid socialMediaId = Guid.NewGuid();
            string linkedIn = "https://linkedin.com/in/student";
            string instagram = "https://instagram.com/student";
            string facebook = "https://facebook.com/student";
            string gitHub = "https://github.com/student";
            string whatsapp = "https://wa.me/+947654321";

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters for Users
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@NIC", nic);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@Role", (int)Role.Student);

                        // Add parameters for Students
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Phone", phone);
                        command.Parameters.AddWithValue("@ImagePath", imagePath);
                        command.Parameters.AddWithValue("@IsLocked", isLocked);
                        command.Parameters.AddWithValue("@FailedLoginAttempts", failedLoginAttempts);

                        // Add parameters for Address
                        command.Parameters.AddWithValue("@AddressId", addressId);
                        command.Parameters.AddWithValue("@AddressLine1", addressLine1);
                        command.Parameters.AddWithValue("@AddressLine2", addressLine2);
                        command.Parameters.AddWithValue("@City", city);
                        command.Parameters.AddWithValue("@State", state);
                        command.Parameters.AddWithValue("@PostalCode", postalCode);
                        command.Parameters.AddWithValue("@Country", country);

                        // Add parameters for SocialMediaLinks
                        command.Parameters.AddWithValue("@SocialMediaId", socialMediaId);
                        command.Parameters.AddWithValue("@LinkedIn", linkedIn);
                        command.Parameters.AddWithValue("@Instagram", instagram);
                        command.Parameters.AddWithValue("@Facebook", facebook);
                        command.Parameters.AddWithValue("@GitHub", gitHub);
                        command.Parameters.AddWithValue("@WhatsApp", whatsapp);

                        // Execute the query
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine("Student sample data inserted successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding student data: {ex.Message}");
            }
        }
    }
}
