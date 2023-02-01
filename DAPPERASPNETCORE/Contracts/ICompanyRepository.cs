using DAPPERASPNETCORE.Dto;
using DAPPERASPNETCORE.Entity;

namespace DAPPERASPNETCORE.Contracts
{
    public interface ICompanyRepository {
        public Task<IEnumerable<Company>> GetCompanies();
        public Task<Company> GetCompany(int id);
        public Task<Company> CreateCompany(CompanyForCreationDto company);
        public Task DeleteCompany(int id);
        public Task UpdateCompany(int id, CompanyForUpdateDto company);
    }
}
