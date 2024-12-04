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
                    {"@Name", "John Doe"},
                    {"@Email", "johndoe@gmail.com"},
                    {"@Phone", "0770000000"},
                    {"@ImagePath", "/images/students/john_doe.jpg"}
                },
                new Dictionary<string, object>
                {
                    {"@UserId", Guid.NewGuid()},
                    {"@NIC", "200431400981"},
                    {"@Password", "AQAAAAIAAYagAAAAEK/02L5TSdJsZbs8SYprqGDT8rXK5jzPnfrKy0bQANoS27C7sg/5pov7R/U1EuUn5Q=="},
                    {"@Role", (int)Role.Admin},
                    {"@Name", "Alice Brown"},
                    {"@Email", "alicebrown@gmail.com"},
                    {"@Phone", "0781112222"},
                    {"@ImagePath", "/images/students/alice_brown.jpg"}
                },
                new Dictionary<string, object>
                {
                    {"@UserId", Guid.NewGuid()},
                    {"@NIC", "200431400982"},
                    {"@Password", "AQAAAAIAAYagAAAAEK/02L5TSdJsZbs8SYprqGDT8rXK5jzPnfrKy0bQANoS27C7sg/5pov7R/U1EuUn5Q=="},
                    {"@Role", (int)Role.Admin},
                    {"@Name", "Bob Smith"},
                    {"@Email", "bobsmith@gmail.com"},
                    {"@Phone", "0792233445"},
                    {"@ImagePath", "/images/students/bob_smith.jpg"}
                },
                new Dictionary<string, object>
                {
                    {"@UserId", Guid.NewGuid()},
                    {"@NIC", "200431400983"},
                    {"@Password", "AQAAAAIAAYagAAAAEK/02L5TSdJsZbs8SYprqGDT8rXK5jzPnfrKy0bQANoS27C7sg/5pov7R/U1EuUn5Q=="},
                    {"@Role", (int)Role.Admin},
                    {"@Name", "Eve White"},
                    {"@Email", "evewhite@gmail.com"},
                    {"@Phone", "0784455667"},
                    {"@ImagePath", "/images/students/eve_white.jpg"}
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
            {"@FirstName", "John"},
            {"@LastName", "Doe"},
            {"@Email", "john.doe@student.com"},
            {"@Phone", "0781234567"},
            {"@ImagePath", "/images/students/john_doe.jpg"},
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
            {"@Email", "sara.kumari@student.com"},
            {"@Phone", "0778765432"},
            {"@ImagePath", "/images/students/sara_kumari.jpg"},
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
            {"@Email", "david.fernando@student.com"},
            {"@Phone", "0789988776"},
            {"@ImagePath", "/images/students/david_fernando.jpg"},
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
            {"@Email", "nadeesha.perera@student.com"},
            {"@Phone", "0773344556"},
            {"@ImagePath", "/images/students/nadeesha_perera.jpg"},
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
            {"@Duration", 6},
            {"@Fees", 1500.00m},
            {"@ImagePaths", "/images/courses/d5aa3ab1-c0e2-4919-b576-76a7d6e1e5fd.jpg"},
            {"@Description", "Learn the fundamentals of Python programming, ideal for beginners."}
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@CourseName", "Java"},
            {"@Level", "Intermediate"},
            {"@Duration", 12},
            {"@Fees", 2500.00m},
            {"@ImagePaths", "/images/courses/d5aa3ab1-c0e2-4919-b576-76a7d6e1e5fd.jpg"},
            {"@Description", "A deeper dive into Java programming, suitable for intermediate learners."}
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@CourseName", "C#"},
            {"@Level", "Beginner"},
            {"@Duration", 6},
            {"@Fees", 1200.00m},
            {"@ImagePaths", "/images/courses/d5aa3ab1-c0e2-4919-b576-76a7d6e1e5fd.jpg"},
            {"@Description", "A beginner-friendly course on C# programming, focusing on the basics."}
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@CourseName", "HTML/CSS"},
            {"@Level", "Beginner"},
            {"@Duration", 6},
            {"@Fees", 1000.00m},
            {"@ImagePaths", "/images/courses/d5aa3ab1-c0e2-4919-b576-76a7d6e1e5fd.jpg"},
            {"@Description", "Learn the basics of web development with HTML and CSS."}
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@CourseName", "JavaScript"},
            {"@Level", "Intermediate"},
            {"@Duration", 6},
            {"@Fees", 1800.00m},
            {"@ImagePaths", "/images/courses/d5aa3ab1-c0e2-4919-b576-76a7d6e1e5fd.jpg"},
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
    IF NOT EXISTS (SELECT 1 FROM Enrollments WHERE StudentNIC = @StudentNIC AND CourseId = @CourseId)
    BEGIN
        INSERT INTO Enrollments (Id, EnrollmentDate, PaymentPlan, IsComplete, StudentNIC, CourseId)
        VALUES (@Id, @EnrollmentDate, @PaymentPlan, @IsComplete, @StudentNIC, @CourseId);
    END";

            List<Dictionary<string, object>> enrollments = new List<Dictionary<string, object>>
    {
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@EnrollmentDate", new DateTime(2023, 5, 10)},  // Example: Old date (May 2023)
            {"@PaymentPlan", "Full"},
            {"@IsComplete", true},
            {"@StudentNIC", "200417002813"},  // Example: Student NIC
            {"@CourseId", new Guid("d5aa3ab1-c0e2-4919-b576-76a7d6e1e5fd")}  // Example: Course GUID
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@EnrollmentDate", new DateTime(2022, 8, 20)},  // Example: Older date (Aug 2022)
            {"@PaymentPlan", "Installment"},
            {"@IsComplete", false},
            {"@StudentNIC", "200417002814"},
            {"@CourseId", new Guid("d5aa3ab1-c0e2-4919-b576-76a7d6e1e5fd")}
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@EnrollmentDate", new DateTime(2023, 11, 12)},  // Example: More recent date (Nov 2023)
            {"@PaymentPlan", "Full"},
            {"@IsComplete", true},
            {"@StudentNIC", "200417002815"},
            {"@CourseId", new Guid("d5aa3ab1-c0e2-4919-b576-76a7d6e1e5fd")}
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@EnrollmentDate", new DateTime(2024, 1, 1)},  // Example: New year (Jan 2024)
            {"@PaymentPlan", "Installment"},
            {"@IsComplete", false},
            {"@StudentNIC", "200417002816"},
            {"@CourseId", new Guid("d5aa3ab1-c0e2-4919-b576-76a7d6e1e5fd")}
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@EnrollmentDate", DateTime.Today},  // Example: Current date (today)
            {"@PaymentPlan", "Full"},
            {"@IsComplete", true},
            {"@StudentNIC", "200417002817"},
            {"@CourseId", new Guid("d5aa3ab1-c0e2-4919-b576-76a7d6e1e5fd")}
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
    IF NOT EXISTS (SELECT 1 FROM Payments WHERE PaymentId = @PaymentId)
    BEGIN
        INSERT INTO Payments (PaymentId, PaymentDate, Amount, PaymentMethod, Status, StudentNIC, CourseId)
        VALUES (@PaymentId, @PaymentDate, @Amount, @PaymentMethod, @Status, @StudentNIC, @CourseId);
    END";

            List<Dictionary<string, object>> payments = new List<Dictionary<string, object>>
    {
        new Dictionary<string, object>
        {
            {"@PaymentId", Guid.NewGuid()},
            {"@PaymentDate", new DateTime(2023, 5, 15)},  // Example: Payment date (May 2023)
            {"@Amount", 1500.00m},
            {"@PaymentMethod", "Credit Card"},
            {"@Status", "Completed"},
            {"@StudentNIC", "200417002813"},  // Example: Student NIC
            {"@CourseId", new Guid("d5aa3ab1-c0e2-4919-b576-76a7d6e1e5fd")}  // Example: Course GUID
        },
        new Dictionary<string, object>
        {
            {"@PaymentId", Guid.NewGuid()},
            {"@PaymentDate", new DateTime(2023, 8, 20)},  // Example: Payment date (Aug 2023)
            {"@Amount", 1200.00m},
            {"@PaymentMethod", "Bank Transfer"},
            {"@Status", "Completed"},
            {"@StudentNIC", "200417002814"},
            {"@CourseId", new Guid("d5aa3ab1-c0e2-4919-b576-76a7d6e1e5fd")}
        },
        new Dictionary<string, object>
        {
            {"@PaymentId", Guid.NewGuid()},
            {"@PaymentDate", new DateTime(2023, 11, 12)},  // Example: Payment date (Nov 2023)
            {"@Amount", 2500.00m},
            {"@PaymentMethod", "Debit Card"},
            {"@Status", "Pending"},
            {"@StudentNIC", "200417002815"},
            {"@CourseId", new Guid("d5aa3ab1-c0e2-4919-b576-76a7d6e1e5fd")}
        },
        new Dictionary<string, object>
        {
            {"@PaymentId", Guid.NewGuid()},
            {"@PaymentDate", new DateTime(2024, 1, 1)},  // Example: Payment date (Jan 2024)
            {"@Amount", 1800.00m},
            {"@PaymentMethod", "Cash"},
            {"@Status", "Completed"},
            {"@StudentNIC", "200417002816"},
            {"@CourseId", new Guid("d5aa3ab1-c0e2-4919-b576-76a7d6e1e5fd")}
        },
        new Dictionary<string, object>
        {
            {"@PaymentId", Guid.NewGuid()},
            {"@PaymentDate", DateTime.Today},  // Example: Payment date (today)
            {"@Amount", 1000.00m},
            {"@PaymentMethod", "Online Transfer"},
            {"@Status", "Completed"},
            {"@StudentNIC", "200417002817"},
            {"@CourseId", new Guid("d5aa3ab1-c0e2-4919-b576-76a7d6e1e5fd")}
        }
    };

            foreach (var payment in payments)
            {
                ExecuteQuery(query, payment);
            }
        }


        public void AddAnnouncementSampleData()
        {
            string query = @"
    IF NOT EXISTS (SELECT 1 FROM Announcements WHERE Id = @Id)
    BEGIN
        INSERT INTO Announcements (Id, Title, Body, Date)
        VALUES (@Id, @Title, @Body, @Date);
    END";

            List<Dictionary<string, object>> announcements = new List<Dictionary<string, object>>
    {
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@Title", "New Course Created: Java Programming"},
            {"@Body", "We are excited to announce that a new course, 'Java Programming for Beginners', has been created. Start your learning journey with us today!"},
            {"@Date", new DateTime(2023, 5, 10)}  // Example: Announcement date (May 2023)
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@Title", "New Course Launched: Web Development"},
            {"@Body", "A new course on 'Web Development' has been launched. Learn to build websites and enhance your programming skills."},
            {"@Date", new DateTime(2023, 8, 20)}  // Example: Announcement date (Aug 2023)
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@Title", "New Course Available: Data Science Basics"},
            {"@Body", "We are pleased to announce a new course on 'Data Science Basics'. Get started with the world of data science and analytics."},
            {"@Date", new DateTime(2023, 11, 12)}  // Example: Announcement date (Nov 2023)
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@Title", "New Course Open: Digital Marketing 101"},
            {"@Body", "Enroll in our newly launched 'Digital Marketing 101' course. Learn the fundamentals of digital marketing and grow your business."},
            {"@Date", new DateTime(2024, 1, 1)}  // Example: Announcement date (Jan 2024)
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@Title", "Course Alert: Advanced Machine Learning"},
            {"@Body", "The 'Advanced Machine Learning' course is now available. Explore deep learning techniques and advance your AI knowledge."},
            {"@Date", DateTime.Today}  // Example: Announcement date (today)
        }
    };

            foreach (var announcement in announcements)
            {
                ExecuteQuery(query, announcement);
            }
        }




        public void AddContactUsSampleData()
        {
            string query = @"
    IF NOT EXISTS (SELECT 1 FROM ContactUs WHERE Id = @Id)
    BEGIN
        INSERT INTO ContactUs (Id, Name, Email, Message, Date)
        VALUES (@Id, @Name, @Email, @Message, @Date);
    END";

            List<Dictionary<string, object>> contactUsEntries = new List<Dictionary<string, object>>
    {
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@Name", "John Doe"},
            {"@Email", "johndoe@example.com"},
            {"@Message", "I am interested in enrolling in the 'Web Development' course. Could you provide more details about the course schedule?"},
            {"@Date", new DateTime(2023, 5, 10)}  // Example: Date (May 2023)
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@Name", "Alice Smith"},
            {"@Email", "alice.smith@example.com"},
            {"@Message", "Could you please assist me with resetting my password for the student portal?"},
            {"@Date", new DateTime(2023, 8, 20)}  // Example: Date (Aug 2023)
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@Name", "Bob Johnson"},
            {"@Email", "bob.johnson@example.com"},
            {"@Message", "I have a question regarding the 'Data Science' course. Can I enroll even though I have no prior experience?"},
            {"@Date", new DateTime(2023, 11, 12)}  // Example: Date (Nov 2023)
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@Name", "Emma Wilson"},
            {"@Email", "emma.wilson@example.com"},
            {"@Message", "I would like to know if there are any upcoming workshops on 'Digital Marketing' that I can attend."},
            {"@Date", new DateTime(2024, 1, 1)}  // Example: Date (Jan 2024)
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@Name", "Liam Brown"},
            {"@Email", "liam.brown@example.com"},
            {"@Message", "I am a current student and need assistance with accessing course materials for 'Machine Learning 101'. Can you help?"},
            {"@Date", DateTime.Today}  // Example: Date (today)
        }
    };

            foreach (var contactUs in contactUsEntries)
            {
                ExecuteQuery(query, contactUs);
            }
        }


        public void AddNotificationSampleData()
        {
            string query = @"
    IF NOT EXISTS (SELECT 1 FROM Notifications WHERE Id = @Id)
    BEGIN
        INSERT INTO Notifications (Id, Message, Date, StudentNIC)
        VALUES (@Id, @Message, @Date, @StudentNIC);
    END";

            List<Dictionary<string, object>> notifications = new List<Dictionary<string, object>>
    {
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@Message", "Reminder: Your 'Web Development' course starts tomorrow. Please ensure you have access to the course materials."},
            {"@Date", new DateTime(2023, 5, 9)},  // Example: Reminder before course start (May 2023)
            {"@StudentNIC", "200417002813"}  // Example: Student NIC
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@Message", "Your 'Data Science' course payment is due next week. Kindly make the payment before the due date."},
            {"@Date", new DateTime(2023, 8, 18)},  // Example: Payment reminder (Aug 2023)
            {"@StudentNIC", "200417002814"}
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@Message", "Good news! You have successfully completed the 'Machine Learning 101' course. Check your results on the portal."},
            {"@Date", new DateTime(2023, 11, 15)},  // Example: Completion notification (Nov 2023)
            {"@StudentNIC", "200417002815"}
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@Message", "Your enrollment for the 'Digital Marketing' course has been confirmed. You can now access the materials."},
            {"@Date", new DateTime(2024, 1, 2)},  // Example: Enrollment confirmation (Jan 2024)
            {"@StudentNIC", "200417002816"}
        },
        new Dictionary<string, object>
        {
            {"@Id", Guid.NewGuid()},
            {"@Message", "Urgent: The 'Advanced Python' course has been rescheduled. Please check your email for the new schedule."},
            {"@Date", DateTime.Today},  // Example: Urgent notification (today)
            {"@StudentNIC", "200417002817"}
        }
    };

            foreach (var notification in notifications)
            {
                ExecuteQuery(query, notification);
            }
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