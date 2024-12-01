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
            string password = "AQAAAAIAAYagAAAAENEU8PTCmOGdKOGoyJ/GereRGnBy0J7AB/YA2edgYRyn4+e0YMS1kchDulbdkH6MoA==";//Imthath@2002
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
            string nic = "200431400979";
            string password = "AQAAAAIAAYagAAAAEK/02L5TSdJsZbs8SYprqGDT8rXK5jzPnfrKy0bQANoS27C7sg/5pov7R/U1EuUn5Q==";//911@Pira
            string name = "Pathmarasan Piragash";
            string email = "pppiragash2004@gmail.com";
            string phone = "0766931772";
            string imagePath = "/images/students/fcdf0f4c-c81d-4908-97b7-3a13a7d190d6.jpg";

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
            string nic = "200417002813";
            string password = "AQAAAAIAAYagAAAAEFAeSIHG89Q+sYECoGAu3dQx4Pc2s+/m344EwdV6xSnK1sqMsxuUPUtc/gSB3N1Ipg==";//Safeek@3211
            string firstName = "Mohamed";
            string lastName = "safeek";
            string email = "ut03211tic@gmail.com";
            string phone = "0743773745";
            string imagePath = "/images/students/557b0251-1ede-49d6-90e2-0f8cc58a2408.jpg";
            bool isLocked = false;
            int failedLoginAttempts = 0;

            Guid addressId = Guid.NewGuid();
            string addressLine1 = "Old Village";
            string addressLine2 = "Sooduventhe pilavu";
            string city = "Vavumiya";
            string state = "Northern";
            string postalCode = "43000";
            string country = "Sri Lanka";

            Guid socialMediaId = Guid.NewGuid();
            string linkedIn = "https://www.linkedin.com/in/safeek-mohamed-0ab486302?utm_source=share&utm_campaign=share_via&utm_content=profile&utm_medium=android_app ";
            string instagram = "https://www.instagram.com/person_0618/";
            string facebook = "https://facebook.com/student";
            string gitHub = "https://github.com/SafeekMohamed18";
            string whatsapp = "https://wa.me/+94743773745";

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