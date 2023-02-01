using Dapper;
using DAPPERASPNETCORE.Context;
using DAPPERASPNETCORE.Contracts;
using DAPPERASPNETCORE.Dto;
using DAPPERASPNETCORE.Entity;
using System.Data;

namespace DAPPERASPNETCORE.Repository {
    public class CompanyRepository : ICompanyRepository {
        private readonly DapperContext _context;
        public CompanyRepository(DapperContext context) {
            _context = context;
        }
        // dynamic parameters vs simply pasing model
        public async Task<Company> CreateCompany(CompanyForCreationDto company) {
            var query = $"insert into Companies ({nameof(Company.Name)}, {nameof(Company.Address)}, {nameof(Company.Country)}) values (@{nameof(Company.Name)}, @{nameof(Company.Address)}, @{nameof(Company.Country)});" +
                "select cast(scope_identity() as int)";
            
            using var connection = _context.CreateConnection();
            var parmeters = new DynamicParameters();
            parmeters.Add(nameof(Company.Name), company.Name, DbType.String);
            parmeters.Add(nameof(Company.Address), company.Address, DbType.String);
            parmeters.Add(nameof(Company.Country), company.Country, DbType.String);
            var id = await connection.QuerySingleAsync<int>(query, parmeters);
            var createdCompany = new Company() {
                Id = id,
                Name = company.Name,
                Address = company.Address,
                Country = company.Country,
            };
            return createdCompany;
        }

        public async Task DeleteCompany(int id) {
            var query = "delete from Companies where Id = @Id";

            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, new {id});
        }

        public async Task<IEnumerable<Company>> GetCompanies() {
            var query = "select * from Companies";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Company>(query);
        }

        public async Task<Company> GetCompany(int id) {
            var query = "select * from Companies where Id = @Id";

            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Company>(query, new {id});
        }

        public async Task UpdateCompany(int id, CompanyForUpdateDto company) {
            var query = $"update Companies set {nameof(Company.Name)} = @{nameof(Company.Name)}, {nameof(Company.Address)} = @{nameof(Company.Address)}, {nameof(Company.Country)} = @{nameof(Company.Country)} where Id = @Id";
            
            using var connection = _context.CreateConnection();
            var parmeters = new DynamicParameters();
            parmeters.Add(nameof(Company.Id), id, DbType.Int32);
            parmeters.Add(nameof(Company.Name), company.Name, DbType.String);
            parmeters.Add(nameof(Company.Address), company.Address, DbType.String);
            parmeters.Add(nameof(Company.Country), company.Country, DbType.String);
            await connection.ExecuteAsync(query, parmeters);
        }
    }
}
