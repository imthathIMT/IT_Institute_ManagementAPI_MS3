﻿using System;
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

namespace IT_Institute_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
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
        public async Task<IActionResult> AddStudent( StudentRequestDto studentDto)
        {
            try
            {
                await _studentService.AddStudentAsync(studentDto);
                return CreatedAtAction(nameof(GetStudentByNic), new { nic = studentDto.NIC }, studentDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{nic}")]
        public async Task<IActionResult> UpdateStudent(string nic, [FromBody] StudentRequestDto studentDto)
        {
            try
            {
                await _studentService.UpdateStudentAsync(nic, studentDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{nic}")]
        public async Task<IActionResult> DeleteStudent(string nic)
        {
            try
            {
                await _studentService.DeleteStudentAsync(nic);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // PUT: api/students/{nic}/update-password
        [HttpPut("{nic}/update-password")]
        public async Task<IActionResult> UpdatePassword(string nic, UpdatePasswordRequestDto updatePasswordDto)
        {
            try
            {
                // Call service to update password
                await _studentService.UpdatePasswordAsync(nic, updatePasswordDto);

                // Return success response
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


        //sample

        [HttpPost("test")]
        public async void imageUpload(StudentRequestDto studentDto)
        {
            
        }
    }
}
