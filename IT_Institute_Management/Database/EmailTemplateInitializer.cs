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


        public async Task InitializeAccountUnlockEmailTemplateAsync()
        {
            // SQL query to check if the template already exists
            string checkQuery = @"SELECT COUNT(1) FROM EmailTemplates WHERE TemplateName = @TemplateName";

            // SQL query to insert the template into the database
            string insertQuery = @"INSERT INTO EmailTemplates (TemplateName, TemplateSubject, TemplateBody)
                            VALUES (@TemplateName, @TemplateSubject, @TemplateBody)";

            // Email template details for Account Unlock Successfully
            string templateName = "AccountUnlockSuccessfully";
            string subject = "Student Account Unlock Successfully";
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
            .unlock-details {
                text-align: left;
                padding: 20px;
                background-color: #0e2963;
                border-radius: 8px;
            }
            .unlock-details p {
                margin: 10px 0;
                font-size: 16px;
            }
            .unlock-details strong {
                color: white;
            }

            /* Media Query for smaller screens */
            @media only screen and (max-width: 600px) {
                table {
                    width: 100% !important;
                }
                td {
                    padding: 10px !important;
                }
                h1 {
                    font-size: 22px !important;
                }
                h2 {
                    font-size: 20px !important;
                }
                p {
                    font-size: 14px !important;
                }
                .button {
                    padding: 10px 20px !important;
                    font-size: 14px !important;
                }
                .unlock-details {
                    padding: 15px !important;
                }
            }

            /* Media Query for very small screens */
            @media only screen and (max-width: 400px) {
                h1 {
                    font-size: 20px !important;
                }
                h2 {
                    font-size: 18px !important;
                }
                p {
                    font-size: 12px !important;
                }
                .button {
                    padding: 8px 16px !important;
                    font-size: 12px !important;
                }
                .unlock-details {
                    padding: 10px !important;
                }
            }
        </style>
        <title>Student Account Unlock Successfully</title>
    </head>
    <body>
        <table style=""background-color: #0d1e4c;"">
            <tr>
                <td>
                    <h1>Student Account Unlock Successful</h1>
                    <p>Dear {{FirstName}},</p>
                    <p>We are happy to inform you that your account has been successfully unlocked.</p>
                    
                    <div class=""unlock-details"">
                        <h2>Your Account Details:</h2>
                        <p><strong>Name:</strong> {{FirstName}} {{LastName}}</p>
                        <p><strong>NIC Number:</strong> {{NICNumber}}</p>
                    </div>

                    <p>You can now log in to your DevHub account using your credentials.</p>
                    
                    <p><a href=""http://localhost:4200/login"" class=""button"">Login to DevHub</a></p>
                    
                    <p class=""footer"">Best regards,<br>The DevHub Team</p>
                    <p class=""footer"">If you have any questions, please contact us at support@devhub.com.</p>
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


        public async Task InitializeAccountLockedEmailTemplateAsync()
        {
            // SQL query to check if the template already exists
            string checkQuery = @"SELECT COUNT(1) FROM EmailTemplates WHERE TemplateName = @TemplateName";

            // SQL query to insert the template into the database
            string insertQuery = @"INSERT INTO EmailTemplates (TemplateName, TemplateSubject, TemplateBody)
                            VALUES (@TemplateName, @TemplateSubject, @TemplateBody)";

            // Email template details for Account Locked by Admin
            string templateName = "AccountLockedByAdmin";
            string subject = "Your Account Has Been Locked by Admin";
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
            .lock-details {
                text-align: left;
                padding: 20px;
                background-color: #0e2963;
                border-radius: 8px;
            }
            .lock-details p {
                margin: 10px 0;
                font-size: 16px;
            }
            .lock-details strong {
                color: white;
            }

            /* Media Query for smaller screens */
            @media only screen and (max-width: 600px) {
                table {
                    width: 100% !important;
                }
                td {
                    padding: 10px !important;
                }
                h1 {
                    font-size: 22px !important;
                }
                h2 {
                    font-size: 20px !important;
                }
                p {
                    font-size: 14px !important;
                }
                .button {
                    padding: 10px 20px !important;
                    font-size: 14px !important;
                }
                .lock-details {
                    padding: 15px !important;
                }
            }

            /* Media Query for very small screens */
            @media only screen and (max-width: 400px) {
                h1 {
                    font-size: 20px !important;
                }
                h2 {
                    font-size: 18px !important;
                }
                p {
                    font-size: 12px !important;
                }
                .button {
                    padding: 8px 16px !important;
                    font-size: 12px !important;
                }
                .lock-details {
                    padding: 10px !important;
                }
            }
        </style>
        <title>Account Locked by Admin</title>
    </head>
    <body>
        <table style=""background-color: #0d1e4c;"">
            <tr>
                <td>
                    <h1>Your Account Has Been Locked by Admin</h1>
                    <p>Dear {{FirstName}},</p>
                    <p>We regret to inform you that your DevHub account has been locked by our admin team. This action may have been taken due to suspicious activity or policy violations.</p>
                    
                    <div class=""lock-details"">
                        <h2>Account Details:</h2>
                        <p><strong>Name:</strong> {{FirstName}} {{LastName}}</p>
                        <p><strong>NIC Number:</strong> {{NICNumber}}</p>
                    </div>

                    <p>If you believe this action was taken in error or need further clarification, please contact our support team immediately.</p>

                    <p><a href=""http://localhost:4200/contact-support"" class=""button"">Contact Support</a></p>
                    
                    <p class=""footer"">Best regards,<br>The DevHub Team</p>
                    <p class=""footer"">If you have any questions, please contact us at support@devhub.com.</p>
                    <!-- <p class=""footer"">To unsubscribe, click <a href=""http://localhost:4200/unsubscribe"">here</a>.</p> -->
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


        public async Task InitializeAccountLockedFailedLoginEmailTemplateAsync()
        {
            // SQL query to check if the template already exists
            string checkQuery = @"SELECT COUNT(1) FROM EmailTemplates WHERE TemplateName = @TemplateName";

            // SQL query to insert the template into the database
            string insertQuery = @"INSERT INTO EmailTemplates (TemplateName, TemplateSubject, TemplateBody)
                            VALUES (@TemplateName, @TemplateSubject, @TemplateBody)";

            // Email template details for Account Locked Due to Multiple Failed Login Attempts
            string templateName = "AccountLockedFailedLogin";
            string subject = "Your Account Has Been Locked Due to Multiple Failed Login Attempts";
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
            .lock-details {
                text-align: left;
                padding: 20px;
                background-color: #0e2963;
                border-radius: 8px;
            }
            .lock-details p {
                margin: 10px 0;
                font-size: 16px;
            }
            .lock-details strong {
                color: white;
            }

            /* Media Query for smaller screens */
            @media only screen and (max-width: 600px) {
                table {
                    width: 100% !important;
                }
                td {
                    padding: 10px !important;
                }
                h1 {
                    font-size: 22px !important;
                }
                h2 {
                    font-size: 20px !important;
                }
                p {
                    font-size: 14px !important;
                }
                .button {
                    padding: 10px 20px !important;
                    font-size: 14px !important;
                }
                .lock-details {
                    padding: 15px !important;
                }
            }

            /* Media Query for very small screens */
            @media only screen and (max-width: 400px) {
                h1 {
                    font-size: 20px !important;
                }
                h2 {
                    font-size: 18px !important;
                }
                p {
                    font-size: 12px !important;
                }
                .button {
                    padding: 8px 16px !important;
                    font-size: 12px !important;
                }
                .lock-details {
                    padding: 10px !important;
                }
            }
        </style>
        <title>Account Locked Due to Multiple Failed Login Attempts</title>
    </head>
    <body>
        <table style=""background-color: #0d1e4c;"">
            <tr>
                <td>
                    <h1>Your Account Has Been Locked Due to Multiple Failed Login Attempts</h1>
                    <p>Dear {{LastName}},</p>
                    <p>Your account has been automatically locked due to multiple unsuccessful login attempts. This is a security measure to protect your account from unauthorized access.</p>
                    
                    <div class=""lock-details"">
                        <h2>Account Details:</h2>
                        <p><strong>Name:</strong> {{FirstName}} {{LastName}}</p>
                        <p><strong>NIC Number:</strong> {{NICNumber}}</p>
                    </div>

                    <p>If you believe this lock was triggered in error, please reach out to our admin team to have your account unlocked.</p>
                    
                    <p><a href=""http://localhost:4200/contact-admin"" class=""button"">Contact Admin Team</a></p>

                    <p class=""footer"">Best regards,<br>The DevHub Team</p>
                    <p class=""footer"">If you have any questions, please contact us at support@devhub.com.</p>
                    <!-- <p class=""footer"">To unsubscribe, click <a href=""http://localhost:4200/unsubscribe"">here</a>.</p> -->
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


        public async Task InitializePasswordUpdatedEmailTemplateAsync()
        {
            // SQL query to check if the template already exists
            string checkQuery = @"SELECT COUNT(1) FROM EmailTemplates WHERE TemplateName = @TemplateName";

            // SQL query to insert the template into the database
            string insertQuery = @"INSERT INTO EmailTemplates (TemplateName, TemplateSubject, TemplateBody)
                            VALUES (@TemplateName, @TemplateSubject, @TemplateBody)";

            // Email template details for Password Updated Successfully
            string templateName = "PasswordUpdatedSuccessfully";
            string subject = "Password Updated Successfully";
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
            .confirmation-details {
                text-align: left;
                padding: 20px;
                background-color: #0e2963;
                border-radius: 8px;
            }
            .confirmation-details p {
                margin: 10px 0;
                font-size: 16px;
            }
            .confirmation-details strong {
                color: white;
            }

            /* Media Query for smaller screens */
            @media only screen and (max-width: 600px) {
                table {
                    width: 100% !important;
                }
                td {
                    padding: 10px !important;
                }
                h1 {
                    font-size: 22px !important;
                }
                h2 {
                    font-size: 20px !important;
                }
                p {
                    font-size: 14px !important;
                }
                .button {
                    padding: 10px 20px !important;
                    font-size: 14px !important;
                }
                .confirmation-details {
                    padding: 15px !important;
                }
            }

            /* Media Query for very small screens */
            @media only screen and (max-width: 400px) {
                h1 {
                    font-size: 20px !important;
                }
                h2 {
                    font-size: 18px !important;
                }
                p {
                    font-size: 12px !important;
                }
                .button {
                    padding: 8px 16px !important;
                    font-size: 12px !important;
                }
                .confirmation-details {
                    padding: 10px !important;
                }
            }
        </style>
        <title>Password Updated Successfully</title>
    </head>
    <body>
        <table style=""background-color: #0d1e4c;"">
            <tr>
                <td>
                    <h1>Password Updated Successfully</h1>
                    <p>Dear {{LastName}},</p>
                    <p>We’re happy to inform you that your password has been successfully updated for your DevHub account.</p>
                    
                    <div class=""confirmation-details"">
                        <h2>Account Details:</h2>
                        <p><strong>Name:</strong> {{FirstName}} {{LastName}}</p>
                        <p><strong>NIC Number:</strong> {{NICNumber}}</p>
                    </div>

                    <p>If you did not make this change, please contact our support team immediately.</p>
                    
                    <p><a href=""http://localhost:4200/login"" class=""button"">Login to DevHub</a></p>
                    
                    <p class=""footer"">Best regards,<br>The DevHub Team</p>
                    <p class=""footer"">If you need help, please contact us at devhubinstitue@gmail.com.</p>
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


        public async Task InitializeNewCourseEmailTemplateAsync()
        {
            // SQL query to check if the template already exists
            string checkQuery = @"SELECT COUNT(1) FROM EmailTemplates WHERE TemplateName = @TemplateName";

            // SQL query to insert the template into the database
            string insertQuery = @"INSERT INTO EmailTemplates (TemplateName, TemplateSubject, TemplateBody)
                            VALUES (@TemplateName, @TemplateSubject, @TemplateBody)";

            // Email template details for New Course Offering
            string templateName = "NewCourseOffering";
            string subject = "We Are Offering a New Course!";
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
            .course-details {
                text-align: left;
                padding: 20px;
                background-color: #0e2963;
                border-radius: 8px;
            }
            .course-details p {
                margin: 10px 0;
                font-size: 16px;
            }
            .course-details strong {
                color: white;
            }

            /* Media Query for smaller screens */
            @media only screen and (max-width: 600px) {
                table {
                    width: 100% !important;
                }
                td {
                    padding: 10px !important;
                }
                h1 {
                    font-size: 22px !important;
                }
                h2 {
                    font-size: 20px !important;
                }
                p {
                    font-size: 14px !important;
                }
                .button {
                    padding: 10px 20px !important;
                    font-size: 14px !important;
                }
                .course-details {
                    padding: 15px !important;
                }
            }

            /* Media Query for very small screens */
            @media only screen and (max-width: 400px) {
                h1 {
                    font-size: 20px !important;
                }
                h2 {
                    font-size: 18px !important;
                }
                p {
                    font-size: 12px !important;
                }
                .button {
                    padding: 8px 16px !important;
                    font-size: 12px !important;
                }
                .course-details {
                    padding: 10px !important;
                }
            }
        </style>
        <title>New Course Offering</title>
    </head>
    <body>
        <table style=""background-color: #0d1e4c;"">
            <tr>
                <td>
                    <h1>We Are Offering a New Course!</h1>
                    <p>We are excited to announce the launch of a new course that we think you'll love. Here are the details:</p>
                    
                    <div class=""course-details"">
                        <h2>Course Details:</h2>
                        <p><strong>Course Name:</strong> {{CourseName}}</p>
                        <p><strong>Duration:</strong> {{Duration}}</p>
                        <p><strong>Fees:</strong> {{Fees}}</p>
                        <p><strong>Level:</strong> {{Level}}</p>
                    </div>

                    <p>If you're interested in enrolling, get started!</p>

                    <p class=""footer"">Best regards,<br>The DevHub Team</p>
                    <p class=""footer"">For more information, visit our website at <a href=""http://localhost:4200"" style=""color: #4CAF50;"">DevHub</a>.</p>
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


        public async Task InitializeCourseUpdateEmailTemplateAsync()
        {
            // SQL query to check if the template already exists
            string checkQuery = @"SELECT COUNT(1) FROM EmailTemplates WHERE TemplateName = @TemplateName";

            // SQL query to insert the template into the database
            string insertQuery = @"INSERT INTO EmailTemplates (TemplateName, TemplateSubject, TemplateBody)
                            VALUES (@TemplateName, @TemplateSubject, @TemplateBody)";

            // Email template details for Course Update Notification
            string templateName = "CourseUpdateNotification";
            string subject = "Important Update: Course Information";
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
            .course-details {
                text-align: left;
                padding: 20px;
                background-color: #0e2963;
                border-radius: 8px;
            }
            .course-details p {
                margin: 10px 0;
                font-size: 16px;
            }
            .course-details strong {
                color: white;
            }

            /* Media Query for smaller screens */
            @media only screen and (max-width: 600px) {
                table {
                    width: 100% !important;
                }
                td {
                    padding: 10px !important;
                }
                h1 {
                    font-size: 22px !important;
                }
                h2 {
                    font-size: 20px !important;
                }
                p {
                    font-size: 14px !important;
                }
                .button {
                    padding: 10px 20px !important;
                    font-size: 14px !important;
                }
                .course-details {
                    padding: 15px !important;
                }
            }

            /* Media Query for very small screens */
            @media only screen and (max-width: 400px) {
                h1 {
                    font-size: 20px !important;
                }
                h2 {
                    font-size: 18px !important;
                }
                p {
                    font-size: 12px !important;
                }
                .button {
                    padding: 8px 16px !important;
                    font-size: 12px !important;
                }
                .course-details {
                    padding: 10px !important;
                }
            }
        </style>
        <title>Course Update Notification</title>
    </head>
    <body>
        <table style=""background-color: #0d1e4c;"">
            <tr>
                <td>
                    <h1>Important Update: Course Information</h1>
                    <p>We have made some important updates to an existing course. Here are the new details:</p>
                    
                    <div class=""course-details"">
                        <h2>Updated Course Details:</h2>
                        <p><strong>Course Name:</strong> {{CourseName}}</p>
                        <p><strong>Duration:</strong> {{Duration}}</p>
                        <p><strong>Fees:</strong> {{Fees}}</p>
                        <p><strong>Level:</strong> {{Level}}</p>
                    </div>

                    <p>If you're interested in continuing or enrolling in this updated course.</p>
                    
                    <p class=""footer"">Best regards,<br>The DevHub Team</p>
                    <p class=""footer"">For more information, visit our website at <a href=""http://localhost:4200"" style=""color: #4CAF50;"">DevHub</a>.</p>
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


        public async Task InitializeEnrollmentConfirmationEmailTemplateAsync()
        {
            // SQL query to check if the template already exists
            string checkQuery = @"SELECT COUNT(1) FROM EmailTemplates WHERE TemplateName = @TemplateName";

            // SQL query to insert the template into the database
            string insertQuery = @"INSERT INTO EmailTemplates (TemplateName, TemplateSubject, TemplateBody)
                            VALUES (@TemplateName, @TemplateSubject, @TemplateBody)";

            // Email template details for Enrollment Confirmation
            string templateName = "EnrollmentConfirmation";
            string subject = "Congratulations,You’ve Successfully Enrolled";
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
            .enrollment-details {
                text-align: left;
                padding: 20px;
                background-color: #0e2963;
                border-radius: 8px;
            }
            .enrollment-details p {
                margin: 10px 0;
                font-size: 16px;
            }
            .enrollment-details strong {
                color: white;
            }

            /* Media Query for smaller screens */
            @media only screen and (max-width: 600px) {
                table {
                    width: 100% !important;
                }
                td {
                    padding: 10px !important;
                }
                h1 {
                    font-size: 22px !important;
                }
                h2 {
                    font-size: 20px !important;
                }
                p {
                    font-size: 14px !important;
                }
                .button {
                    padding: 10px 20px !important;
                    font-size: 14px !important;
                }
                .enrollment-details {
                    padding: 15px !important;
                }
            }

            /* Media Query for very small screens */
            @media only screen and (max-width: 400px) {
                h1 {
                    font-size: 20px !important;
                }
                h2 {
                    font-size: 18px !important;
                }
                p {
                    font-size: 12px !important;
                }
                .button {
                    padding: 8px 16px !important;
                    font-size: 12px !important;
                }
                .enrollment-details {
                    padding: 10px !important;
                }
            }
        </style>
        <title>Enrollment Confirmation</title>
    </head>
    <body>
        <table style=""background-color: #0d1e4c;"">
            <tr>
                <td>
                    <h1>Congratulations, {{LastName}}! You’ve Successfully Enrolled</h1>
                    <p>Welcome to the DevHub Institute! We're excited to confirm your enrollment in the course.</p>
                    
                    <div class=""enrollment-details"">
                        <h2>Your Enrollment Details:</h2>
                        <p><strong>Course Name:</strong> {{CourseName}}</p>
                        <p><strong>Duration:</strong> {{Duration}}</p>
                        <p><strong>Start Date:</strong> {{StartDate}}</p>
                        <p><strong>Level:</strong> {{Level}}</p>
                        <p><strong>Fees:</strong> {{Fees}}</p>
                        <p><strong>Payment plan:</strong> {{Payment plan}}</p>
                    </div>

                    <p>We are excited to have you as part of our learning community.</p>

                    <p class=""footer"">Best regards,<br>The DevHub Team</p>
                    <p class=""footer"">For more information, visit our website at <a href=""http://localhost:4200"" style=""color: #4CAF50;"">DevHub</a>.</p>
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


        public async Task InitializeEnrollmentCompleteEmailTemplateAsync()
        {
            // SQL query to check if the template already exists
            string checkQuery = @"SELECT COUNT(1) FROM EmailTemplates WHERE TemplateName = @TemplateName";

            // SQL query to insert the template into the database
            string insertQuery = @"INSERT INTO EmailTemplates (TemplateName, TemplateSubject, TemplateBody)
                            VALUES (@TemplateName, @TemplateSubject, @TemplateBody)";

            // Email template details for Enrollment Complete
            string templateName = "EnrollmentComplete";
            string subject = "Congratulations, {{FirstName}}! Enrollment Complete";
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
            .completion-details {
                text-align: left;
                padding: 20px;
                background-color: #0e2963;
                border-radius: 8px;
            }
            .completion-details p {
                margin: 10px 0;
                font-size: 16px;
            }
            .completion-details strong {
                color: white;
            }

            /* Media Query for smaller screens */
            @media only screen and (max-width: 600px) {
                table {
                    width: 100% !important;
                }
                td {
                    padding: 10px !important;
                }
                h1 {
                    font-size: 22px !important;
                }
                h2 {
                    font-size: 20px !important;
                }
                p {
                    font-size: 14px !important;
                }
                .button {
                    padding: 10px 20px !important;
                    font-size: 14px !important;
                }
                .completion-details {
                    padding: 15px !important;
                }
            }

            /* Media Query for very small screens */
            @media only screen and (max-width: 400px) {
                h1 {
                    font-size: 20px !important;
                }
                h2 {
                    font-size: 18px !important;
                }
                p {
                    font-size: 12px !important;
                }
                .button {
                    padding: 8px 16px !important;
                    font-size: 12px !important;
                }
                .completion-details {
                    padding: 10px !important;
                }
            }
        </style>
        <title>Enrollment Complete</title>
    </head>
    <body>
        <table style=""background-color: #0d1e4c;"">
            <tr>
                <td>
                    <h1>Congratulations, {{FirstName}}! Enrollment Complete</h1>
                    <p>We are thrilled to inform you that your enrollment process is now complete!</p>
                    
                    <div class=""completion-details"">
                        <h2>Your Enrollment Details:</h2>
                        <p><strong>Course Name:</strong> {{CourseName}}</p>
                        <p><strong>Start Date:</strong> {{StartDate}}</p>
                        <p><strong>Level:</strong> {{Level}}</p>
                        <p><strong>Duration:</strong> {{Duration}}</p>
                    </div>

                    <p>You can now begin your learning journey with us. To get started, please visit the student portal:</p>

                    <p><a href=""http://localhost:4200/login"" class=""button"">Access Your Course</a></p>

                    <p class=""footer"">Best regards,<br>The DevHub Team</p>
                    <p class=""footer"">For more information, visit our website at <a href=""http://localhost:4200"" style=""color: #4CAF50;"">DevHub</a>.</p>
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


        public async Task InitializePaymentConfirmedEmailTemplateAsync()
        {
            string checkQuery = @"SELECT COUNT(1) FROM EmailTemplates WHERE TemplateName = @TemplateName";
            string insertQuery = @"INSERT INTO EmailTemplates (TemplateName, TemplateSubject, TemplateBody)
                            VALUES (@TemplateName, @TemplateSubject, @TemplateBody)";

            string templateName = "PaymentConfirmed";
            string subject = "Payment Confirmed for {{CourseName}}!";
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
            .payment-summary {
                text-align: left;
                padding: 20px;
                background-color: #0e2963;
                border-radius: 8px;
            }
            .payment-summary p {
                margin: 10px 0;
                font-size: 16px;
            }
            .payment-summary strong {
                color: white;
            }

            /* Media Query for smaller screens */
            @media only screen and (max-width: 600px) {
                table {
                    width: 100% !important;
                }
                td {
                    padding: 10px !important;
                }
                h1 {
                    font-size: 22px !important;
                }
                h2 {
                    font-size: 20px !important;
                }
                p {
                    font-size: 14px !important;
                }
                .button {
                    padding: 10px 20px !important;
                    font-size: 14px !important;
                }
                .payment-summary {
                    padding: 15px !important;
                }
            }

            /* Media Query for very small screens */
            @media only screen and (max-width: 400px) {
                h1 {
                    font-size: 20px !important;
                }
                h2 {
                    font-size: 18px !important;
                }
                p {
                    font-size: 12px !important;
                }
                .button {
                    padding: 8px 16px !important;
                    font-size: 12px !important;
                }
                .payment-summary {
                    padding: 10px !important;
                }
            }
        </style>
        <title>Payment Confirmed</title>
    </head>
    <body>
        <table style=""background-color: #0d1e4c;"">
            <tr>
                <td>
                    <h1>Payment Confirmed for {{CourseName}}!</h1>
                    <p>Dear {{FirstName}},</p>
                    <p>Thank you for your payment! We're excited to have you onboard for the course <strong>{{CourseName}}</strong>.</p>
                    
                    <div class=""payment-summary"">
                        <h2>Payment Summary:</h2>
                        <p><strong>Course Name:</strong> {{CourseName}}</p>
                        <p><strong>Level:</strong> {{Level}}</p>
                        <p><strong>Amount Paid:</strong> {{AmountPaid}}</p>
                        <p><strong>Payment Method:</strong> {{PaymentMethod}}</p>
                    </div>

                    <p>Your enrollment is now fully confirmed. Get ready to embark on an exciting learning journey with us!</p>

                    <p><a href=""http://localhost:4200/login"" class=""button"">Access Your Course</a></p>

                    <p class=""footer"">Warm regards,<br>The DevHub Team</p>
                    <p class=""footer"">For more information, visit our website at <a href=""http://localhost:4200"" style=""color: #4CAF50;"">DevHub</a>.</p>
                </td>
            </tr>
        </table>
    </body>
    </html>";

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    using (var checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@TemplateName", templateName);
                        var exists = (int)await checkCommand.ExecuteScalarAsync() > 0;
                        if (exists) return;
                    }

                    using (var command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@TemplateName", templateName);
                        command.Parameters.AddWithValue("@TemplateSubject", subject);
                        command.Parameters.AddWithValue("@TemplateBody", body);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }


        public async Task InitializePaymentReminderEmailTemplateAsync()
        {
            string checkQuery = @"SELECT COUNT(1) FROM EmailTemplates WHERE TemplateName = @TemplateName";
            string insertQuery = @"INSERT INTO EmailTemplates (TemplateName, TemplateSubject, TemplateBody)
                            VALUES (@TemplateName, @TemplateSubject, @TemplateBody)";

            string templateName = "PaymentReminder";
            string subject = "Reminder: Payment Due for {{CourseName}}!";
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
                background-color: #FF6347;
                color: white;
                text-decoration: none;
                border-radius: 5px;
            }
            .footer {
                font-size: 12px;
                color: #bbbbbb;
            }
            .payment-details {
                text-align: left;
                padding: 20px;
                background-color: #0e2963;
                border-radius: 8px;
            }
            .payment-details p {
                margin: 10px 0;
                font-size: 16px;
            }
            .payment-details strong {
                color: white;
            }

            /* Media Query for smaller screens */
            @media only screen and (max-width: 600px) {
                table {
                    width: 100% !important;
                }
                td {
                    padding: 10px !important;
                }
                h1 {
                    font-size: 22px !important;
                }
                h2 {
                    font-size: 20px !important;
                }
                p {
                    font-size: 14px !important;
                }
                .button {
                    padding: 10px 20px !important;
                    font-size: 14px !important;
                }
                .payment-details {
                    padding: 15px !important;
                }
            }

            /* Media Query for very small screens */
            @media only screen and (max-width: 400px) {
                h1 {
                    font-size: 20px !important;
                }
                h2 {
                    font-size: 18px !important;
                }
                p {
                    font-size: 12px !important;
                }
                .button {
                    padding: 8px 16px !important;
                    font-size: 12px !important;
                }
                .payment-details {
                    padding: 10px !important;
                }
            }
        </style>
        <title>Payment Reminder</title>
    </head>
    <body>
        <table style=""background-color: #0d1e4c;"">
            <tr>
                <td>
                    <h1>Reminder: Payment Due for {{CourseName}}!</h1>
                    <p>Dear {{FirstName}},</p>
                    <p>This is a friendly reminder that your payment for the course <strong>{{CourseName}}</strong> is still pending. We want to make sure you don’t miss out on your learning opportunity.</p>
                    
                    <div class=""payment-details"">
                        <h2>Course Details:</h2>
                        <p><strong>Course Name:</strong> {{CourseName}}</p>
                        <p><strong>Level:</strong> {{Level}}</p>
                        <p><strong>Amount Due:</strong> {{AmountDue}}</p>
                        <p><strong>Due Date:</strong> {{DueDate}}</p>
                    </div>

                    <p>Please make the payment as soon as possible to secure your place in the course. To complete your payment, contact our admin team</p>

                    <p>If you have any questions or need assistance with the payment process, feel free to contact our support team.</p>

                    <p class=""footer"">Best regards,<br>The DevHub Team</p>
                    <p class=""footer"">For more information, visit our website at <a href=""http://localhost:4200"" style=""color: #FF6347;"">DevHub</a>.</p>
                </td>
            </tr>
        </table>
    </body>
    </html>";

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    using (var checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@TemplateName", templateName);
                        var exists = (int)await checkCommand.ExecuteScalarAsync() > 0;
                        if (exists) return;
                    }

                    using (var command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@TemplateName", templateName);
                        command.Parameters.AddWithValue("@TemplateSubject", subject);
                        command.Parameters.AddWithValue("@TemplateBody", body);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }


        public async Task InitializeAccountUnlockWithNewPasswordEmailTemplateAsync()
        {
            string checkQuery = @"SELECT COUNT(1) FROM EmailTemplates WHERE TemplateName = @TemplateName";
            string insertQuery = @"INSERT INTO EmailTemplates (TemplateName, TemplateSubject, TemplateBody)
                            VALUES (@TemplateName, @TemplateSubject, @TemplateBody)";

            string templateName = "AccountUnlockAndPasswordUpdate";
            string subject = "Account Unlocked and Password Updated Successfully";
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
            .details {
                text-align: left;
                padding: 20px;
                background-color: #0e2963;
                border-radius: 8px;
            }
            .details p {
                margin: 10px 0;
                font-size: 16px;
            }
            .details strong {
                color: white;
            }

            /* Media Query for smaller screens */
            @media only screen and (max-width: 600px) {
                table {
                    width: 100% !important;
                }
                td {
                    padding: 10px !important;
                }
                h1 {
                    font-size: 22px !important;
                }
                h2 {
                    font-size: 20px !important;
                }
                p {
                    font-size: 14px !important;
                }
                .button {
                    padding: 10px 20px !important;
                    font-size: 14px !important;
                }
                .details {
                    padding: 15px !important;
                }
            }

            /* Media Query for very small screens */
            @media only screen and (max-width: 400px) {
                h1 {
                    font-size: 20px !important;
                }
                h2 {
                    font-size: 18px !important;
                }
                p {
                    font-size: 12px !important;
                }
                .button {
                    padding: 8px 16px !important;
                    font-size: 12px !important;
                }
                .details {
                    padding: 10px !important;
                }
            }
        </style>
        <title>Account Unlock and Password Update Confirmation</title>
    </head>
    <body>
        <table style=""background-color: #0d1e4c;"">
            <tr>
                <td>
                    <h1>Account Unlocked and Password Updated Successfully</h1>
                    <p>Dear {{LastName}},</p>
                    
                    <p>We are happy to inform you that your account has been successfully unlocked and your password has been updated.</p>
                    
                    <div class=""details"">
                        <h2>Account Details:</h2>
                        <p><strong>Name:</strong> {{FirstName}} {{LastName}}</p>
                        <p><strong>NIC Number:</strong> {{NICNumber}}</p>
                        <p><strong>Account Status:</strong> Unlocked</p>
                        <p><strong>Password Update:</strong> Successful</p>
                    </div>

                    <p>If you did not initiate this action, please contact our support team immediately.</p>
                    
                    <p>You can now log in to your account using your new password.</p>

                    <p><a href=""http://localhost:4200/login"" class=""button"">Login Now</a></p>

                    <p class=""footer"">Best regards,<br>The DevHub Team</p>
                    <p class=""footer"">For more information, visit our website at <a href=""http://localhost:4200"" style=""color: #4CAF50;"">DevHub</a>.</p>
                </td>
            </tr>
        </table>
    </body>
    </html>";

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    using (var checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@TemplateName", templateName);
                        var exists = (int)await checkCommand.ExecuteScalarAsync() > 0;
                        if (exists) return; // Exit if the template already exists
                    }

                    using (var command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@TemplateName", templateName);
                        command.Parameters.AddWithValue("@TemplateSubject", subject);
                        command.Parameters.AddWithValue("@TemplateBody", body);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }


        public async Task InitializeEmailTemplates()
        {
            await InitializeAccountUnlockWithNewPasswordEmailTemplateAsync();
            await InitializePaymentReminderEmailTemplateAsync();
            await InitializePaymentConfirmedEmailTemplateAsync();
            await InitializeEnrollmentCompleteEmailTemplateAsync();
            await InitializeEnrollmentConfirmationEmailTemplateAsync();
            await InitializeCourseUpdateEmailTemplateAsync();
            await InitializeNewCourseEmailTemplateAsync();
            await InitializePasswordUpdatedEmailTemplateAsync();
            await InitializeAccountLockedFailedLoginEmailTemplateAsync();
            await InitializeAccountLockedEmailTemplateAsync();
            await InitializeAccountUnlockEmailTemplateAsync();
            await InitializeRegistrationEmailTemplateAsync();

        }
    }
}
