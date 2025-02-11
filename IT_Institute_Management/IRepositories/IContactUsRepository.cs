﻿using IT_Institute_Management.Entity;

namespace IT_Institute_Management.IRepositories
{
    public interface IContactUsRepository
    {
        Task<IEnumerable<ContactUs>> GetAllAsync();
        Task<ContactUs> GetByIdAsync(Guid id);
        Task AddAsync(ContactUs contactUs);
        Task UpdateAsync(ContactUs contactUs);
        Task DeleteAsync(Guid id);
        Task<ContactUs> GetByEmail(string email);

    }
}
