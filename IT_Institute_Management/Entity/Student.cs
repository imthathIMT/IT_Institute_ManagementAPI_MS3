﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IT_Institute_Management.Entity
{
    public class Student
    {
        [Key]
        [Required(ErrorMessage = "NIC is required.")]
        [RegularExpression(@"^\d{9}[vxzVXZ]$|^\d{12}$", ErrorMessage = "Invalid NIC format.")]
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
        [RegularExpression(@"^\+?(\d{1,4})?[\s\-]?\(?\d{1,4}?\)?[\s\-]?\d{1,4}[\s\-]?\d{1,4}[\s\-]?\d{1,4}$",
            ErrorMessage = "Phone number must be in a valid international format. Example format: +44 20 7946 0958.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$",
            ErrorMessage = "Password must be at least 8 characters long and contain at least 1 uppercase letter, 1 lowercase letter, 1 number, and 1 special character.")]
        public string? Password { get; set; }

        public string? ImagePath { get; set; }

        public bool IsLocked { get; set; }  
        public int FailedLoginAttempts { get; set; } = 0; 

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public Address Address { get; set; }

        public ICollection<Notification>? Notification { get; set; }
        //[JsonIgnore]
        public ICollection<Enrollment>? Enrollment { get; set; }

        public virtual SocialMediaLinks? SocialMediaLinks { get; set; }
        public ICollection<StudentMessage>? StudentMessages { get; set; }



    }
}
