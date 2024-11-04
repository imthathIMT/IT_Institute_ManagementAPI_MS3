﻿using System.ComponentModel.DataAnnotations;

namespace IT_Institute_Management.Entity
{
    public class Student
    {
        [Key]
        [Required(ErrorMessage = "NIC is required.")]
        [RegularExpression(@"^(?!.*[^0-9VXZ]).{9}$|^(?!.*[^0-9]).{12}$",
            ErrorMessage = "NIC must be either 9 digits followed by V/Z/X or 12 digits.")]
        public string NIC { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^07\d{8}$",
            ErrorMessage = "Phone number must be exactly 10 digits and start with 07.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and contain at least 1 uppercase letter, 1 lowercase letter, and 1 special character.")]
        public string Password { get; set; }

        public string? ImagePath { get; set; }

        public bool Status { get; set; }  // Indicates if the account is locked (true) or unlocked (false)

        public Address? Address { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}
