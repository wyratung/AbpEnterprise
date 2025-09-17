using AbpEnterprise.Enterpries;
using AbpEnterprise.Enterprises.DomainServices;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace AbpEnterprise.Enterprises
{
    public abstract class EnterpriseIndustryService_Tests<TStartupModule> : AbpEnterpriseApplicationTestBase<TStartupModule>
     where TStartupModule : IAbpModule
    {
        private readonly IEnterpriseIndustryAppService enterpriseIndustryAppService;

        public EnterpriseIndustryService_Tests()
        {
            enterpriseIndustryAppService = GetRequiredService<IEnterpriseIndustryAppService>();
        }
        //write code test cased for method CreateAsync
        [Fact]
        public async Task Should_Create_EnterpriseIndustry()
        {
            var input = new CreateEnterpriseIndustryDto
            {
                Name = "Test Industry",
                Code = "TEST",
                Description = "This is a test industry"
            };
            var result = await enterpriseIndustryAppService.CreateAsync(input);
            result.ShouldNotBeNull();
            result.Name.ShouldBe(input.Name);
            result.Code.ShouldBe(input.Code);
            result.Description.ShouldBe(input.Description);
            result.IsActive.ShouldBeTrue();
        }

        //write code test cased for method UpdateAsync
        [Fact]
        public async Task Should_Update_EnterpriseIndustry()
        {
            var input = new CreateEnterpriseIndustryDto
            {
                Name = "Test Industry",
                Code = "TEST",
                Description = "This is a test industry"
            };
            var created = await enterpriseIndustryAppService.CreateAsync(input);
            var updateInput = new UpdateEnterpriseIndustryDto
            {
                Name = "Updated Industry",
                Description = "This is an updated test industry",
            };
            var updated = await enterpriseIndustryAppService.UpdateAsync(created.Id, updateInput);
            updated.ShouldNotBeNull();
            updated.Name.ShouldBe(updateInput.Name);
            updated.Description.ShouldBe(updateInput.Description);
        }
        //write code test cased for method GetWithDetailsAsync
        [Fact]
        public async Task Should_Get_EnterpriseIndustry_With_Details()
        {
            var input = new CreateEnterpriseIndustryDto
            {
                Name = "Test Industry",
                Code = "TEST",
                Description = "This is a test industry"
            };
            var created = await enterpriseIndustryAppService.CreateAsync(input);
            var result = await enterpriseIndustryAppService.GetWithDetailsAsync(created.Id);
            result.ShouldNotBeNull();
            result.Name.ShouldBe(input.Name);
            result.Code.ShouldBe(input.Code);
            result.Description.ShouldBe(input.Description);
            result.IsActive.ShouldBeTrue();
            result.EnterpriseTypes.ShouldBeEmpty();
        }
        //write code test cased for method GetActiveListAsync
        //[Fact]
        //public async Task Should_Get_Active_EnterpriseIndustries()
        //{
        //    var input1 = new CreateEnterpriseIndustryDto
        //    {
        //        Name = "Active Industry 1",
        //        Code = "ACTIVE1",
        //        Description = "This is an active industry"
        //    };
        //    var input2 = new CreateEnterpriseIndustryDto
        //    {
        //        Name = "Active Industry 2",
        //        Code = "ACTIVE2",
        //        Description = "This is another active industry"
        //    };
        //    var input3 = new CreateEnterpriseIndustryDto
        //    {
        //        Name = "Inactive Industry",
        //        Code = "INACTIVE",
        //        Description = "This is an inactive industry"
        //    };
        //    var active1 = await enterpriseIndustryAppService.CreateAsync(input1);
        //    var active2 = await enterpriseIndustryAppService.CreateAsync(input2);
        //    var inactive = await enterpriseIndustryAppService.CreateAsync(input3);
        //    await enterpriseIndustryAppService.DeactivateAsync(inactive.Id);
        //    var result = await enterpriseIndustryAppService.GetActiveListAsync();
        //    result.ShouldNotBeNull();
        //    result.Count.ShouldBe(2);
        //    result.Any(e => e.Id == active1.Id).ShouldBeTrue();
        //    result.Any(e => e.Id == active2.Id).ShouldBeTrue();
        //    result.Any(e => e.Id == inactive.Id).ShouldBeFalse();
        //}
    }
}
