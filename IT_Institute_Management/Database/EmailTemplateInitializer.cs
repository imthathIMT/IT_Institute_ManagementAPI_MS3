using Microsoft.Data.SqlClient;

namespace IT_Institute_Management.Database
{
    public class EmailTemplateInitializer
    {
        private readonly string _connectionString = "Server =(localdb)\\MSSQLLocalDB; Database=DevHub; Trusted_Connection=True; TrustServerCertificate=True;";

        // Method to insert the email template into the database
        public async Task InitializeRegistrationEmailTemplateAsync()
        {
            // SQL query to check if the template already exists
            string checkQuery = @"SELECT COUNT(1) FROM EmailTemplates WHERE TemplateName = @TemplateName";

            // SQL query to insert the template into the database
            string insertQuery = @"INSERT INTO EmailTemplates (TemplateName, TemplateSubject, TemplateBody)
                           VALUES (@TemplateName, @TemplateSubject, @TemplateBody)";

            // Email template details
            string templateName = "RegistrationWelcome";
            string subject = "Welcome to DevHub";
            string body = @"
                            <!DOCTYPE html>
                            <html lang=""en"">
                            <head>
                                <meta charset=""UTF-8"">
                                <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
                                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                                <style>
                                    /* General styling for the email */
                                    body {
                                        font-family: Arial, sans-serif;
                                        margin: 0;
                                        padding: 0;
                                        color: white; /* White Text */
                                    }
                                    table {
                                        padding: auto;
                                        width: 100%;
                                        max-width: 600px;
                                        margin: 0 auto;
                                        background-color: #ffffff;
                                        border-spacing: 0;
                                        height: 100vh;
                                    }
                                    td {
                                        padding: 20px;
                                        text-align: center;
                                        vertical-align: top;
                                    }
                                    h1, h2 {
                                        color: white;
                                        font-size: 24px;
                                    }
                                    p {
                                        color: white;
                                        font-size: 16px;
                                    }
                                    .button {
                                        display: inline-block;
                                        padding: 12px 24px;
                                        background-color: #4CAF50;
                                        color: white;
                                        text-decoration: none;
                                        border-radius: 5px;
                                    }
                                    .footer {
                                        font-size: 12px;
                                        color: #bbbbbb;
                                    }
                                    .registration-details {
                                        text-align: left;
                                        padding: 20px;
                                        background-color: #0e2963;
                                        border-radius: 8px;
                                    }
                                    .registration-details p {
                                        margin: 10px 0;
                                        font-size: 16px;
                                    }
                                    .registration-details strong {
                                        color: white;
                                    }
                                </style>
                                <title>Welcome to DevHub</title>
                            </head>
                            <body>
                                <table style=""background-color: #0d1e4c;"">
                                    <tr>
                                        <td>
                                            <h1>Welcome to DevHub, {{FirstName}}!</h1>
                                            <p>We're excited to welcome you to the DevHub Institute!</p>
                                            <div class=""registration-details"">
                                                <h2>Your Registration Details:</h2>
                                                <p><strong>Name:</strong> {{FirstName}} {{LastName}}</p>
                                                <p><strong>NIC Number:</strong> {{NICNumber}}</p>
                                                <p><strong>Password:</strong> {{Password}}</p>
                                            </div>
                                            <p><a href=""http://localhost:4200/login"" class=""button"">Login to DevHub</a></p>
                                            <p class=""footer"">Best regards,<br>The DevHub Team</p>
                                        </td>
                                    </tr>
                                </table>
                            </body>
                            </html>";

            // Create a new SqlConnection using the provided connection string
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    // Open the database connection
                    await connection.OpenAsync();

                    // First, check if the template already exists
                    using (var checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@TemplateName", templateName);

                        var exists = (int)await checkCommand.ExecuteScalarAsync() > 0;

                        if (exists)
                        {
                            Console.WriteLine("Template already exists.");
                            return; // Exit the method if the template exists
                        }
                    }

                    // Create a SqlCommand to execute the insert query
                    using (var command = new SqlCommand(insertQuery, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@TemplateName", templateName);
                        command.Parameters.AddWithValue("@TemplateSubject", subject);
                        command.Parameters.AddWithValue("@TemplateBody", body);
                        //command.Parameters.AddWithValue("@DateCreated", DateTime.Now);

                        // Execute the insert query asynchronously
                        await command.ExecuteNonQueryAsync();
                        Console.WriteLine("Template inserted successfully.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions (e.g., connection errors, query errors)
                    Console.WriteLine($"Error inserting email template: {ex.Message}");
                }
            }
        }

    }
}
