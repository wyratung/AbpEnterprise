//using System;
//using System.Threading.Tasks;
//using Volo.Abp.Domain.Entities;

//namespace AbpEnterprise.Enterprises
//{
//    public class EfCoreEnterpriseTypeRepository : EfCoreRepository<AbpEnterpriseDbContext, EnterpriseType, Guid>, IEnterpriseTypeRepository
//    {
//        public EfCoreEnterpriseTypeRepository(IDbContextProvider<AbpEnterpriseDbContext> dbContextProvider)
//            : base(dbContextProvider)
//        {
//        }

//        public async Task<EnterpriseType> FindByNameAsync(string name)
//        {
//            var dbSet = await GetDbSetAsync();
//            return await dbSet
//                .Include(e => e.Industries) 
//                .FirstOrDefaultAsync(e => e.Name == name);
//        }
//        public async Task<EnterpriseType> GetWithIndustriesAsync(Guid id)
//        {
//            var dbContext = await GetDbContextAsync();
//            return await dbContext.EnterpriseTypes
//                .Include(e => e.Industries)
//                .FirstOrDefaultAsync(e => e.Id == id)
//                ?? throw new EntityNotFoundException(typeof(EnterpriseType), id);
//        }
//    }
//}
//}
