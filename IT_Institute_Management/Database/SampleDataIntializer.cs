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

            List<Dictionary<string, object>> admins = new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"@UserId", Guid.NewGuid()},
                    {"@NIC", "200431400979"},
                    {"@Password", "AQAAAAIAAYagAAAAEK/02L5TSdJsZbs8SYprqGDT8rXK5jzPnfrKy0bQANoS27C7sg/5pov7R/U1EuUn5Q=="}, // 911@Pira
                    {"@Role", (int)Role.Admin},
                    {"@Name", "Pathmarasan Piragash"},
                    {"@Email", "pppiragash2004@gmail.com"},
                    {"@Phone", "0766931772"},
                    {"@ImagePath", "/images/students/fcdf0f4c-c81d-4908-97b7-3a13a7d190d6.jpg"}
                },
                new Dictionary<string, object>
                {
                    {"@UserId", Guid.NewGuid()},
                    {"@NIC", "200431400980"},
                    {"@Password", "AQAAAAIAAYagAAAAEK/02L5TSdJsZbs8SYprqGDT8rXK5jzPnfrKy0bQANoS27C7sg/5pov7R/U1EuUn5Q=="},
                    {"@Role", (int)Role.Admin},
                    {"@Name", "Thuvakaran"},
                    {"@Email", "thuvakaran@gmail.com"},
                    {"@Phone", "0778400521"},
                    {"@ImagePath", "/images/students/d5f04527-0499-48ff-a32c-4bdb55c75de7.jpg"}
                },
                new Dictionary<string, object>
                {
                    {"@UserId", Guid.NewGuid()},
                    {"@NIC", "200431400981"},
                    {"@Password", "AQAAAAIAAYagAAAAEK/02L5TSdJsZbs8SYprqGDT8rXK5jzPnfrKy0bQANoS27C7sg/5pov7R/U1EuUn5Q=="},
                    {"@Role", (int)Role.Admin},
                    {"@Name", "Jathushan"},
                    {"@Email", "jathu@gmail.com"},
                    {"@Phone", "0781112222"},
                    {"@ImagePath", "/images/students/d5f04527-0499-48ff-a32c-4bdb55c75de7.jpg"}
                },
                new Dictionary<string, object>
                {
                    {"@UserId", Guid.NewGuid()},
                    {"@NIC", "200431400982"},
                    {"@Password", "AQAAAAIAAYagAAAAEK/02L5TSdJsZbs8SYprqGDT8rXK5jzPnfrKy0bQANoS27C7sg/5pov7R/U1EuUn5Q=="},
                    {"@Role", (int)Role.Admin},
                    {"@Name", "Denojan"},
                    {"@Email", "denojan@gmail.com"},
                    {"@Phone", "0792233445"},
                    {"@ImagePath", "/images/students/d5f04527-0499-48ff-a32c-4bdb55c75de7.jpg"}
                },
                new Dictionary<string, object>
                {
                    {"@UserId", Guid.NewGuid()},
                    {"@NIC", "200431400983"},
                    {"@Password", "AQAAAAIAAYagAAAAEK/02L5TSdJsZbs8SYprqGDT8rXK5jzPnfrKy0bQANoS27C7sg/5pov7R/U1EuUn5Q=="},
                    {"@Role", (int)Role.Admin},
                    {"@Name", "Divyesh"},
                    {"@Email", "divyeshe@gmail.com"},
                    {"@Phone", "0784455667"},
                    {"@ImagePath", "/images/students/d5f04527-0499-48ff-a32c-4bdb55c75de7.jpg"}
                }
            };

            foreach (var admin in admins)
            {
                ExecuteQuery(query, admin);
            }
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

            List<Dictionary<string, object>> students = new List<Dictionary<string, object>>
    {
        new Dictionary<string, object>
        {
            {"@UserId", Guid.NewGuid()},
            {"@NIC", "200417002813"},
            {"@Password", "AQAAAAIAAYagAAAAEFAeSIHG89Q+sYECoGAu3dQx4Pc2s+/m344EwdV6xSnK1sqMsxuUPUtc/gSB3N1Ipg=="}, // Safeek@3211
            {"@Role", (int)Role.Student},
            {"@FirstName", "Mohamed"},
            {"@LastName", "Safeek"},
            {"@Email", "ut03211tic@gmail.com"},
            {"@Phone", "0743773745"},
            {"@ImagePath", "/images/students/557b0251-1ede-49d6-90e2-0f8cc58a2408.jpg"},
            {"@IsLocked", false},
            {"@FailedLoginAttempts", 0},
            {"@AddressId", Guid.NewGuid()},
            {"@AddressLine1", "Old Village"},
            {"@AddressLine2", "Sooduventhe pilavu"},
            {"@City", "Vavumiya"},
            {"@State", "Northern"},
            {"@PostalCode", "43000"},
            {"@Country", "Sri Lanka"},
            {"@SocialMediaId", Guid.NewGuid()},
            {"@LinkedIn", "https://www.linkedin.com/in/safeek-mohamed-0ab486302"},
            {"@Instagram", "https://www.instagram.com/person_0618/"},
            {"@Facebook", "https://facebook.com/student"},
            {"@GitHub", "https://github.com/SafeekMohamed18"},
            {"@WhatsApp", "https://wa.me/+94743773745"}
        },
        new Dictionary<string, object>
        {
            {"@UserId", Guid.NewGuid()},
            {"@NIC", "200417002814"},
            {"@Password", "AQAAAAIAAYagAAAAEFAeSIHG89Q+sYECoGAu3dQx4Pc2s+/m344EwdV6xSnK1sqMsxuUPUtc/gSB3N1Ipg=="},
            {"@Role", (int)Role.Student},
            {"@FirstName", "Thangarasa"},
            {"@LastName", "Kanistan"},
            {"@Email", "kani@gmail.com"},
            {"@Phone", "0781234567"},
            {"@ImagePath", "/images/students/e13371cb-0b16-463b-8b56-15d156e42544.jpg"},
            {"@IsLocked", false},
            {"@FailedLoginAttempts", 0},
            {"@AddressId", Guid.NewGuid()},
            {"@AddressLine1", "Main Street"},
            {"@AddressLine2", "Near the park"},
            {"@City", "Colombo"},
            {"@State", "Western"},
            {"@PostalCode", "10100"},
            {"@Country", "Sri Lanka"},
            {"@SocialMediaId", Guid.NewGuid()},
            {"@LinkedIn", "https://www.linkedin.com/in/johndoe"},
            {"@Instagram", "https://www.instagram.com/johndoe/"},
            {"@Facebook", "https://facebook.com/johndoe"},
            {"@GitHub", "https://github.com/johndoe"},
            {"@WhatsApp", "https://wa.me/+94781234567"}
        },
        new Dictionary<string, object>
        {
            {"@UserId", Guid.NewGuid()},
            {"@NIC", "200417002815"},
            {"@Password", "AQAAAAIAAYagAAAAEFAeSIHG89Q+sYECoGAu3dQx4Pc2s+/m344EwdV6xSnK1sqMsxuUPUtc/gSB3N1Ipg=="},
            {"@Role", (int)Role.Student},
            {"@FirstName", "Sara"},
            {"@LastName", "Kumari"},
            {"@Email", "sarakumari@gmail.com"},
            {"@Phone", "0778765432"},
            {"@ImagePath", "/images/students/e13371cb-0b16-463b-8b56-15d156e42544.jpg"},
            {"@IsLocked", false},
            {"@FailedLoginAttempts", 0},
            {"@AddressId", Guid.NewGuid()},
            {"@AddressLine1", "Queen Street"},
            {"@AddressLine2", "Near the temple"},
            {"@City", "Kandy"},
            {"@State", "Central"},
            {"@PostalCode", "20000"},
            {"@Country", "Sri Lanka"},
            {"@SocialMediaId", Guid.NewGuid()},
            {"@LinkedIn", "https://www.linkedin.com/in/sara-kumari"},
            {"@Instagram", "https://www.instagram.com/sara_kumari/"},
            {"@Facebook", "https://facebook.com/sara.kumari"},
            {"@GitHub", "https://github.com/sarakumari"},
            {"@WhatsApp", "https://wa.me/+94778765432"}
        },
        new Dictionary<string, object>
        {
            {"@UserId", Guid.NewGuid()},
            {"@NIC", "200417002816"},
            {"@Password", "AQAAAAIAAYagAAAAEFAeSIHG89Q+sYECoGAu3dQx4Pc2s+/m344EwdV6xSnK1sqMsxuUPUtc/gSB3N1Ipg=="},
            {"@Role", (int)Role.Student},
            {"@FirstName", "David"},
            {"@LastName", "Fernando"},
            {"@Email", "davidfernando@gmail.com"},
            {"@Phone", "0789988776"},
            {"@ImagePath", "/images/students/e13371cb-0b16-463b-8b56-15d156e42544.jpg"},
            {"@IsLocked", false},
            {"@FailedLoginAttempts", 0},
            {"@AddressId", Guid.NewGuid()},
            {"@AddressLine1", "Sunset Boulevard"},
            {"@AddressLine2", "Opposite the mall"},
            {"@City", "Galle"},
            {"@State", "Southern"},
            {"@PostalCode", "80000"},
            {"@Country", "Sri Lanka"},
            {"@SocialMediaId", Guid.NewGuid()},
            {"@LinkedIn", "https://www.linkedin.com/in/david-fernando"},
            {"@Instagram", "https://www.instagram.com/david_fernando/"},
            {"@Facebook", "https://facebook.com/david.fernando"},
            {"@GitHub", "https://github.com/davidfernando"},
            {"@WhatsApp", "https://wa.me/+94789988776"}
        },
        new Dictionary<string, object>
        {
            {"@UserId", Guid.NewGuid()},
            {"@NIC", "200417002817"},
            {"@Password", "AQAAAAIAAYagAAAAEFAeSIHG89Q+sYECoGAu3dQx4Pc2s+/m344EwdV6xSnK1sqMsxuUPUtc/gSB3N1Ipg=="},
            {"@Role", (int)Role.Student},
            {"@FirstName", "Nadeesha"},
            {"@LastName", "Perera"},
            {"@Email", "nadeeshaperera@gmail.com"},
            {"@Phone", "0773344556"},
            {"@ImagePath", "/images/students/e13371cb-0b16-463b-8b56-15d156e42544.jpg"},
            {"@IsLocked", false},
            {"@FailedLoginAttempts", 0},
            {"@AddressId", Guid.NewGuid()},
            {"@AddressLine1", "River Road"},
            {"@AddressLine2", "Next to the bridge"},
            {"@City", "Anuradhapura"},
            {"@State", "North Central"},
            {"@PostalCode", "50000"},
            {"@Country", "Sri Lanka"},
            {"@SocialMediaId", Guid.NewGuid()},
            {"@LinkedIn", "https://www.linkedin.com/in/nadeesha-perera"},
            {"@Instagram", "https://www.instagram.com/nadeesha_perera/"},
            {"@Facebook", "https://facebook.com/nadeesha.perera"},
            {"@GitHub", "https://github.com/nadeeshaperera"},
            {"@WhatsApp", "https://wa.me/+94773344556"}
        }
    };

            foreach (var student in students)
            {
                ExecuteQuery(query, student);
            }
        }


        public void AddCourseSampleData()
        {
            string query = @"
    IF NOT EXISTS (SELECT 1 FROM Courses WHERE CourseName = @CourseName)
    BEGIN
        INSERT INTO Courses (Id, CourseName, Level, Duration, Fees, ImagePaths, Description)
        VALUES (@Id, @CourseName, @Level, @Duration, @Fees, @ImagePaths, @Description);
    END";

            List<Dictionary<string, object>> courses = new List<Dictionary<string, object>>
              {
                    new Dictionary<string, object>
                    {
                        {"@Id", Guid.NewGuid()},
                        {"@CourseName", "Python"},
                        {"@Level", "Beginner"},
                        {"@Duration", 6}, // Duration in months
                        {"@Fees", 12000}, // Course fee in your currency
                        {"@ImagePaths", "/images/courses/ff20c3e3-67b3-4d9e-8312-fa03acecd363.png"},
                        {"@Description", "Learn the fundamentals of Python programming, ideal for beginners."}
                    },
                    new Dictionary<string, object>
                    {
                        {"@Id", Guid.NewGuid()},
                        {"@CourseName", "Java"},
                        {"@Level", "Intermediate"},
                        {"@Duration", 2}, // Duration in months
                        {"@Fees", 24000}, // Course fee
                        {"@ImagePaths", "/images/courses/1d5c0cb9-f8ee-47e6-b1f5-82feaaaab3b2.jpg"},
                        {"@Description", "A deeper dive into Java programming, suitable for intermediate learners."}
                    },
                    new Dictionary<string, object>
                    {
                        {"@Id", Guid.NewGuid()},
                        {"@CourseName", "C#"},
                        {"@Level", "Beginner"},
                        {"@Duration", 6}, // Duration in months
                        {"@Fees", 6000}, // Course fee
                        {"@ImagePaths", "/images/courses/6cadc346-67ad-4856-af1f-e4ee13f46667.webp"},
                        {"@Description", "A beginner-friendly course on C# programming, focusing on the basics."}
                    },
                    new Dictionary<string, object>
                    {
                        {"@Id", Guid.NewGuid()},
                        {"@CourseName", "HTML/CSS"},
                        {"@Level", "Beginner"},
                        {"@Duration", 2}, // Duration in months
                        {"@Fees", 30000}, // Course fee
                        {"@ImagePaths", "/images/courses/0adeaedf-35d1-4278-a66e-1736e33c0154.png"},
                        {"@Description", "Learn the basics of web development with HTML and CSS."}
                    },
                    new Dictionary<string, object>
                    {
                        {"@Id", Guid.NewGuid()},
                        {"@CourseName", "JavaScript"},
                        {"@Level", "Intermediate"},
                        {"@Duration", 6}, // Duration in months
                        {"@Fees", 18000}, // Course fee
                        {"@ImagePaths", "/images/courses/fdaf92fe-5dbe-4724-8ffc-bbb6d5783c35.png"},
                        {"@Description", "An intermediate course to master JavaScript for web development."}
                    }
               };

            foreach (var course in courses)
            {
                ExecuteQuery(query, course);
            }
        }



        public void AddEnrollmentSampleData()
        {
            string query = @"
    IF NOT EXISTS (SELECT 1 FROM Enrollment WHERE StudentNIC = @StudentNIC AND CourseId = @CourseId)
    BEGIN
        INSERT INTO Enrollment (Id, EnrollmentDate, PaymentPlan, IsComplete, StudentNIC, CourseId)
        VALUES (@Id, @EnrollmentDate, @PaymentPlan, @IsComplete, @StudentNIC, @CourseId);
    END";

            List<Dictionary<string, object>> enrollments = new List<Dictionary<string, object>>
    {
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@EnrollmentDate", new DateTime(2024, 9, 10)},
            {"@PaymentPlan", "Full"},
            {"@IsComplete", true},
            {"@StudentNIC", "200417002813"},
            {"@CourseId", new Guid("FCCAD0D6-64DF-4CF0-9CA1-1B7AC60E2C82")}
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@EnrollmentDate", new DateTime(2024, 9, 20)},
            {"@PaymentPlan", "Installment"},
            {"@IsComplete", false},
            {"@StudentNIC", "200417002813"},
            {"@CourseId", new Guid("B60806D6-930C-4B81-A170-451A1CC00598")}
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@EnrollmentDate", new DateTime(2024, 10, 10)},
            {"@PaymentPlan", "Full"},
            {"@IsComplete", true},
            {"@StudentNIC", "200417002814"},
            {"@CourseId", new Guid("FC2E17D9-EF71-4338-A9F0-57C1A8D27FB7")}
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@EnrollmentDate", new DateTime(2024, 10, 22)},
            {"@PaymentPlan", "Installment"},
            {"@IsComplete", false},
            {"@StudentNIC", "200417002816"},
            {"@CourseId", new Guid("DE5E98E9-E0FA-4754-AED6-C8F983F34621")}
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@EnrollmentDate", DateTime.Today},
            {"@PaymentPlan", "Full"},
            {"@IsComplete", true},
            {"@StudentNIC", "200417002817"},
            {"@CourseId", new Guid("E3743C02-BA07-4414-BF12-FDB7B7DE8162")}
        }
    };

            foreach (var enrollment in enrollments)
            {
                ExecuteQuery(query, enrollment);
            }
        }



        public void AddPaymentSampleData()
        {
            string query = @"
    IF NOT EXISTS (SELECT 1 FROM Payments WHERE Id = @PaymentId)
    BEGIN
        INSERT INTO Payments (Id, Amount, PaymentDate, EnrollmentId, DueAmount, TotalPaidAmount)
        VALUES (@PaymentId, @Amount, @PaymentDate, @EnrollmentId, @DueAmount, @TotalPaidAmount);
    END";

            List<Dictionary<string, object>> payments = new List<Dictionary<string, object>>
    {
        new Dictionary<string, object>
        {
            {"@PaymentId", Guid.NewGuid()},
            {"@PaymentDate", new DateTime(2024, 9, 12)},
            {"@Amount", 12000.00m},
            {"@DueAmount", 0.00m}, // Example of remaining balance
            {"@TotalPaidAmount", 12000.00m}, // Paid amount
            {"@EnrollmentId", new Guid("E42B3273-E8D5-4399-B446-443088CB1943")}, // EnrollmentId for Python course
        },
        new Dictionary<string, object>
        {
            {"@PaymentId", Guid.NewGuid()},
            {"@PaymentDate", new DateTime(2024, 9, 23)},
            {"@Amount", 1000.00m},
            {"@DueAmount", 5000.00m},
            {"@TotalPaidAmount", 1000.00m},
            {"@EnrollmentId", new Guid("6BE764C5-74D5-42FE-A905-047663C8614C")}, // EnrollmentId for Java course
        },
        new Dictionary<string, object>
        {
            {"@PaymentId", Guid.NewGuid()},
            {"@PaymentDate", new DateTime(2024, 12, 05)},
            {"@Amount", 24000.00m},
            {"@DueAmount", 0.00m},
            {"@TotalPaidAmount", 24000.00m},
            {"@EnrollmentId", new Guid("4C49274B-89D5-4CEC-83FA-B10EC9FBC917")}, // EnrollmentId for C# course
        },
        new Dictionary<string, object>
        {
            {"@PaymentId", Guid.NewGuid()},
            {"@PaymentDate", new DateTime(2024, 10, 15)},
            {"@Amount", 18000.00m},
            {"@DueAmount", 0},
            {"@TotalPaidAmount", 18000.00m},
            {"@EnrollmentId", new Guid("818946DC-68A3-4BFF-8D0E-CE06AEC495C7")}, // EnrollmentId for HTML/CSS course
        },
        new Dictionary<string, object>
        {
            {"@PaymentId", Guid.NewGuid()},
            {"@PaymentDate", DateTime.Today},
            {"@Amount", 15000.00m},
            {"@DueAmount", 15000.00m},
            {"@TotalPaidAmount", 15000.00m},
            {"@EnrollmentId", new Guid("0C13EA6F-3A3B-4DB0-8CA4-D3323EF54ABD")}, // EnrollmentId for JavaScript course
        }
    };

            foreach (var payment in payments)
            {
                ExecuteQuery(query, payment);
            }
        }



        //    public void AddAnnouncementSampleData()
        //    {
        //        string query = @"
        //IF NOT EXISTS (SELECT 1 FROM Announcements WHERE Id = @Id)
        //BEGIN
        //    INSERT INTO Announcements (Id, Title, Body, Date)
        //    VALUES (@Id, @Title, @Body, @Date);
        //END";

        //        List<Dictionary<string, object>> announcements = new List<Dictionary<string, object>>
        //{
        //    new Dictionary<string, object>
        //    {
        //        {"@Id", Guid.NewGuid()},
        //        {"@Title", "New Course Created: Java Programming"},
        //        {"@Body", "We are excited to announce that a new course, 'Java Programming for Beginners', has been created. Start your learning journey with us today!"},
        //        {"@Date", new DateTime(2024, 9, 10)}
        //    },
        //    new Dictionary<string, object>
        //    {
        //        {"@Id", Guid.NewGuid()},
        //        {"@Title", "New Course Launched: Web Development"},
        //        {"@Body", "A new course on 'Web Development' has been launched. Learn to build websites and enhance your programming skills."},
        //        {"@Date", new DateTime(2024, 11, 20)}
        //    },
        //    new Dictionary<string, object>
        //    {
        //        {"@Id", Guid.NewGuid()},
        //        {"@Title", "New Course Available: Data Science Basics"},
        //        {"@Body", "We are pleased to announce a new course on 'Data Science Basics'. Get started with the world of data science and analytics."},
        //        {"@Date", new DateTime(2024, 11, 22)}
        //    },
        //    new Dictionary<string, object>
        //    {
        //        {"@Id", Guid.NewGuid()},
        //        {"@Title", "New Course Open: Digital Marketing 101"},
        //        {"@Body", "Enroll in our newly launched 'Digital Marketing 101' course. Learn the fundamentals of digital marketing and grow your business."},
        //        {"@Date", new DateTime(2024, 12, 2)}
        //    },
        //    new Dictionary<string, object>
        //    {
        //        {"@Id", Guid.NewGuid()},
        //        {"@Title", "Course Alert: Advanced Machine Learning"},
        //        {"@Body", "The 'Advanced Machine Learning' course is now available. Explore deep learning techniques and advance your AI knowledge."},
        //        {"@Date", DateTime.Today}
        //    }
        //};

        //        foreach (var announcement in announcements)
        //        {
        //            ExecuteQuery(query, announcement);
        //        }
        //    }




        //    public void AddContactUsSampleData()
        //    {
        //        string query = @"
        //IF NOT EXISTS (SELECT 1 FROM ContactUs WHERE Id = @Id)
        //BEGIN
        //    INSERT INTO ContactUs (Id, Name, Email, Message, Date)
        //    VALUES (@Id, @Name, @Email, @Message, @Date);
        //END";

        //        List<Dictionary<string, object>> contactUsEntries = new List<Dictionary<string, object>>
        //{
        //    new Dictionary<string, object>
        //    {
        //        {"@Id", Guid.NewGuid()},
        //        {"@Name", "John Doe"},
        //        {"@Email", "johndoe@example.com"},
        //        {"@Message", "I am interested in enrolling in the 'Web Development' course. Could you provide more details about the course schedule?"},
        //        {"@Date", new DateTime(2024, 9, 10)}
        //    },
        //    new Dictionary<string, object>
        //    {
        //        {"@Id", Guid.NewGuid()},
        //        {"@Name", "Alice Smith"},
        //        {"@Email", "alicesmith@example.com"},
        //        {"@Message", "Could you please assist me with resetting my password for the student portal?"},
        //        {"@Date", new DateTime(2024, 10, 20)}
        //    },
        //    new Dictionary<string, object>
        //    {
        //        {"@Id", Guid.NewGuid()},
        //        {"@Name", "Bob Johnson"},
        //        {"@Email", "bobjohnson@example.com"},
        //        {"@Message", "I have a question regarding the 'Data Science' course. Can I enroll even though I have no prior experience?"},
        //        {"@Date", new DateTime(2024, 11, 12)}
        //    },
        //    new Dictionary<string, object>
        //    {
        //        {"@Id", Guid.NewGuid()},
        //        {"@Name", "Emma Wilson"},
        //        {"@Email", "emmawilson@example.com"},
        //        {"@Message", "I would like to know if there are any upcoming workshops on 'Digital Marketing' that I can attend."},
        //        {"@Date", new DateTime(2024, 12, 2)}
        //    },
        //    new Dictionary<string, object>
        //    {
        //        {"@Id", Guid.NewGuid()},
        //        {"@Name", "Liam Brown"},
        //        {"@Email", "liambrown@example.com"},
        //        {"@Message", "I am a current student and need assistance with accessing course materials for 'Machine Learning 101'. Can you help?"},
        //        {"@Date", DateTime.Today}
        //    }
        //};

        //        foreach (var contactUs in contactUsEntries)
        //        {
        //            ExecuteQuery(query, contactUs);
        //        }
        //    }


        //    public void AddNotificationSampleData()
        //    {
        //        string query = @"
        //IF NOT EXISTS (SELECT 1 FROM Notifications WHERE Id = @Id)
        //BEGIN
        //    INSERT INTO Notifications (Id, Message, Date, StudentNIC)
        //    VALUES (@Id, @Message, @Date, @StudentNIC);
        //END";

        //        List<Dictionary<string, object>> notifications = new List<Dictionary<string, object>>
        //{
        //    new Dictionary<string, object>
        //    {
        //        {"@Id", Guid.NewGuid()},
        //        {"@Message", "Reminder: Your 'Web Development' course starts tomorrow. Please ensure you have access to the course materials."},
        //        {"@Date", new DateTime(2024, 10, 9)},
        //        {"@StudentNIC", "200417002813"}
        //    },
        //    new Dictionary<string, object>
        //    {
        //        {"@Id", Guid.NewGuid()},
        //        {"@Message", "Your 'Data Science' course payment is due next week. Kindly make the payment before the due date."},
        //        {"@Date", new DateTime(2024, 11, 18)},
        //        {"@StudentNIC", "200417002814"}
        //    },
        //    new Dictionary<string, object>
        //    {
        //        {"@Id", Guid.NewGuid()},
        //        {"@Message", "Good news! You have successfully completed the 'Machine Learning 101' course. Check your results on the portal."},
        //        {"@Date", new DateTime(2024, 12, 1)},
        //        {"@StudentNIC", "200417002815"}
        //    },
        //    new Dictionary<string, object>
        //    {
        //        {"@Id", Guid.NewGuid()},
        //        {"@Message", "Your enrollment for the 'Digital Marketing' course has been confirmed. You can now access the materials."},
        //        {"@Date", new DateTime(2024, 12, 3)},
        //        {"@StudentNIC", "200417002816"}
        //    },
        //    new Dictionary<string, object>
        //    {
        //        {"@Id", Guid.NewGuid()},
        //        {"@Message", "Urgent: The 'Advanced Python' course has been rescheduled. Please check your email for the new schedule."},
        //        {"@Date", DateTime.Today},
        //        {"@StudentNIC", "200417002817"}
        //    }
        //};

        //        foreach (var notification in notifications)
        //        {
        //            ExecuteQuery(query, notification);
        //        }
        //    }






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