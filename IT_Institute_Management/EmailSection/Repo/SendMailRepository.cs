using IT_Institute_Management.Database;
using IT_Institute_Management.EmailSection.Models;
using IT_Institute_Management.EmailSerivice;

namespace IT_Institute_Management.EmailSection.Repo
{
    public class SendMailRepository(InstituteDbContext _instituteDb)
    {
        public async Task<EmailTemplate> GetTemplate(string TemplateName)
        {
            var template = _instituteDb.EmailTemplates.Where(x => x.TemplateName == TemplateName).FirstOrDefault();
            return template;
        }
    }
}
