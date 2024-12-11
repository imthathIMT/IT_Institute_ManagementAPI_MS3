using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IT_Institute_Management.Database;
using IT_Institute_Management.Entity;
using IT_Institute_Management.DTO.RequestDTO;
using IT_Institute_Management.IServices;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace IT_Institute_Management.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;

        public StudentsController(IStudentService studentService, IUserService userService)
        {
            _studentService = studentService;
            _userService = userService;
        }


       


        [HttpGet]
        [Authorize(Roles = "MasterAdmin, Admin")]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                return Ok(students);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("{nic}")]
        public async Task<IActionResult> GetStudentByNic(string nic)
        {
            try
            {
                var student = await _studentService.GetStudentByNicAsync(nic);
                return Ok(student);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Authorize(Roles = "MasterAdmin, Admin")]
        public async Task<IActionResult> AddStudent(StudentRequestDto studentDto)
        {
            try
            {
                if (studentDto == null)
                {
                    return BadRequest("Student data is required.");
                }

                var studentExists = await _userService.CheckUserExistsByNic(studentDto.NIC);
                if (studentExists)
                {
                    return BadRequest(new { message = $"Student with NIC {studentDto.NIC} already exists." });
                }

                await _studentService.AddStudentAsync(studentDto);
                return CreatedAtAction(nameof(GetStudentByNic), new { nic = studentDto.NIC }, studentDto);
            }
            catch (ValidationException validationEx)
            {
                return BadRequest($"Validation failed: {validationEx.Message}");
            }
            catch (DbUpdateException dbEx)
            {
                // Log the database exception for better diagnostics
                return BadRequest($"Database error occurred: {dbEx.Message}. Inner Exception: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                // Log general exceptions
                return BadRequest($"An unexpected error occurred: {ex.Message}. Inner Exception: {ex.InnerException?.Message}");
            }
        }




        [HttpPut("{nic}")]
        [Authorize(Roles = "MasterAdmin, Admin")]
        public async Task<IActionResult> UpdateStudent(string nic, [FromForm] StudentRequestDto studentDto)
        {
            try
            {

                await _studentService.UpdateStudentAsync(nic, studentDto);
                return Ok(new { message = "Student Successfully Updated." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpDelete("{nic}")]
        [Authorize(Roles = "MasterAdmin, Admin")]
        public async Task<IActionResult> DeleteStudent(string nic)
        {
            try
            {
                await _studentService.DeleteStudentAsync(nic);
                return Ok(new { message = "Student Successfully Deleted." });
            }
            catch (Exception ex)
            {
               

               
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest(new { message = errorMessage });
            }
        }




        [HttpPut("{nic}/update-password")]
        public async Task<IActionResult> UpdatePassword(string nic, UpdatePasswordRequestDto updatePasswordDto)
        {
            try
            {

                await _studentService.UpdatePasswordAsync(nic, updatePasswordDto);


                return Ok(new { message = "Password updated successfully." });
            }
            catch (ApplicationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{nic}/lock")]
        [Authorize(Roles = "MasterAdmin, Admin")]
        public async Task<IActionResult> LockStudentAccount(string nic)
        {
            try
            {
                var message = await _studentService.LockAccountAsync(nic);
                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{nic}/unlock")]
        [Authorize(Roles = "MasterAdmin, Admin")]
        public async Task<IActionResult> UnlockStudentAccount(string nic, [FromBody] UnlockAccountDto unlockDto)
        {
            try
            {
                unlockDto.NIC = nic;  // Ensure the NIC matches the path
                var message = await _studentService.UnlockAccountAsync(unlockDto);
                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{nic}/Directunlock")]
        [Authorize(Roles = "MasterAdmin, Admin")]
        public async Task<IActionResult> DirectUnlockAccount(string nic)
        {
            try
            {

                var message = await _studentService.DirectUnlock(nic);
                return Ok(new { message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("profile/{nic}")]
        public async Task<IActionResult> GetStudentProfileByNIC(string nic)
        {
            try
            {
                var studentProfile = await _studentService.GetStudentProfileByNICAsync(nic);
                return Ok(studentProfile);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Student not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("update/{nic}")]
        public async Task<IActionResult> UpdateStudent(string nic, [FromBody] StudentUpdateRequestDto updateDto)
        {
            if (updateDto == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var updatedStudent = await _studentService.UpdateStudentAsync(nic, updateDto);
                return Ok(updatedStudent);
            }
            catch (Exception ex)
            {
                // Log the exception details (not shown here)
                return StatusCode(500, new { message = ex.Message });
            }
        }



        [HttpPut("{nic}/update-image")]
        public async Task<IActionResult> UpdateStudentProfileImage(string nic, IFormFile image)
        {
            try
            {
                
                if (image == null || image.Length == 0)
                {
                    return BadRequest("No image file uploaded.");
                }

                
                var message = await _studentService.UpdateStudentImageAsync(nic, image);
                return Ok(new { message });
            }
            catch (Exception ex)
            {
               
                return BadRequest(new { message = ex.Message });
            }
        }



    }
}
