//using Shouldly;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Volo.Abp;
//using Volo.Abp.Modularity;
//using Xunit;

//namespace AbpEnterprise.Enterprises
//{
//    public abstract class EnterpriseTypeAddressAppServiceTests<TStartupModule> : AbpEnterpriseApplicationTestBase<TStartupModule>
//     where TStartupModule : IAbpModule
//    {
//        private readonly IEnterpriseTypeAddressAppService _appService;

//        public EnterpriseTypeAddressAppServiceTests()
//        {
//            _appService = GetRequiredService<IEnterpriseTypeAddressAppService>();
//        }

//        [Fact]
//        public async Task Should_Create_Address()
//        {
//            // Arrange
//            var input = new CreateUpdateEnterpriseTypeAddressDto
//            {
//                Name = "Silicon Valley Office",
//                Street = "1 Hacker Way",
//                City = "Menlo Park",
//                State = "CA",
//                Country = "USA",
//                ZipCode = "94025",
//                ContactPhone = "+1-650-543-4800",
//                ContactEmail = "contact@siliconvalley.com",
//                IsActive = true
//            };

//            // Act
//            var result = await _appService.CreateAsync(input);

//            // Assert
//            result.ShouldNotBeNull();
//            result.Name.ShouldBe(input.Name);
//            result.Street.ShouldBe(input.Street);
//            result.City.ShouldBe(input.City);
//            result.State.ShouldBe(input.State);
//            result.Country.ShouldBe(input.Country);
//            result.ZipCode.ShouldBe(input.ZipCode);
//            result.ContactPhone.ShouldBe(input.ContactPhone);
//            result.ContactEmail.ShouldBe(input.ContactEmail);
//            result.IsActive.ShouldBe(input.IsActive);
//            result.FullAddress.ShouldNotBeNullOrEmpty();
//        }

//        [Fact]
//        public async Task Should_Get_List_Of_Addresses()
//        {
//            // Arrange
//            await CreateSampleAddressesAsync();

//            // Act
//            var result = await _appService.GetListAsync(new GetEnterpriseTypeAddressesInput());

//            // Assert
//            result.ShouldNotBeNull();
//            result.Items.Count.ShouldBeGreaterThan(0);
//        }

//        [Fact]
//        public async Task Should_Filter_Addresses_By_Name()
//        {
//            // Arrange
//            await CreateSampleAddressesAsync();
//            var input = new GetEnterpriseTypeAddressesInput
//            {
//                Filter = "Silicon"
//            };

//            // Act
//            var result = await _appService.GetListAsync(input);

//            // Assert
//            result.Items.ShouldAllBe(x =>
//                x.Name.Contains("Silicon") ||
//                x.Street.Contains("Silicon") ||
//                x.City.Contains("Silicon") ||
//                x.Country.Contains("Silicon"));
//        }

//        [Fact]
//        public async Task Should_Update_Address()
//        {
//            // Arrange
//            var address = await CreateSampleAddressAsync();
//            var updateInput = new CreateUpdateEnterpriseTypeAddressDto
//            {
//                Name = "Updated " + address.Name,
//                Street = "Updated Street",
//                City = address.City,
//                State = address.State,
//                Country = address.Country,
//                ZipCode = address.ZipCode,
//                ContactPhone = "+1-555-9999",
//                ContactEmail = "updated@example.com",
//                IsActive = false
//            };

//            // Act
//            var result = await _appService.UpdateAsync(address.Id, updateInput);

//            // Assert
//            result.Name.ShouldBe(updateInput.Name);
//            result.Street.ShouldBe(updateInput.Street);
//            result.ContactPhone.ShouldBe(updateInput.ContactPhone);
//            result.ContactEmail.ShouldBe(updateInput.ContactEmail);
//            result.IsActive.ShouldBe(updateInput.IsActive);
//        }

//        [Fact]
//        public async Task Should_Check_Can_Delete_Address()
//        {
//            // Arrange
//            var address = await CreateSampleAddressAsync();

//            // Act
//            var canDelete = await _appService.CanDeleteAsync(address.Id);

//            // Assert
//            canDelete.ShouldBeTrue(); // No mappings exist, so should be deletable
//        }

//        [Fact]
//        public async Task Should_Throw_Exception_For_Duplicate_Location()
//        {
//            // Arrange
//            await CreateSampleAddressAsync("First Address", "123 Main St", "New York", "USA");

//            var duplicateInput = new CreateUpdateEnterpriseTypeAddressDto
//            {
//                Name = "Second Address", // Different name
//                Street = "123 Main St", // Same location details
//                City = "New York",
//                State = "NY",
//                Country = "USA",
//                ZipCode = "10001",
//                IsActive = true
//            };

//            // Act & Assert
//            await Should.ThrowAsync<UserFriendlyException>(
//                async () => await _appService.CreateAsync(duplicateInput));
//        }

//        private async Task<EnterpriseTypeAddressDto> CreateSampleAddressAsync(
//            string name = null,
//            string street = null,
//            string city = null,
//            string country = null)
//        {
//            return await _appService.CreateAsync(new CreateUpdateEnterpriseTypeAddressDto
//            {
//                Name = name ?? "Test Address " + Guid.NewGuid().ToString("N")[..8],
//                Street = street ?? "123 Test Street",
//                City = city ?? "Test City",
//                State = "Test State",
//                Country = country ?? "Test Country",
//                ZipCode = "12345",
//                ContactPhone = "+1-555-0123",
//                ContactEmail = "test@example.com",
//                IsActive = true
//            });
//        }

//        private async Task CreateSampleAddressesAsync()
//        {
//            await _appService.CreateAsync(new CreateUpdateEnterpriseTypeAddressDto
//            {
//                Name = "Silicon Valley Office",
//                Street = "1 Hacker Way",
//                City = "Menlo Park",
//                State = "CA",
//                Country = "USA",
//                ZipCode = "94025",
//                ContactPhone = "+1-650-543-4800",
//                ContactEmail = "contact@siliconvalley.com",
//                IsActive = true
//            });

//            await _appService.CreateAsync(new CreateUpdateEnterpriseTypeAddressDto
//            {
//                Name = "New York Office",
//                Street = "350 Fifth Avenue",
//                City = "New York",
//                State = "NY",
//                Country = "USA",
//                ZipCode = "10118",
//                ContactPhone = "+1-212-736-3100",
//                ContactEmail = "contact@newyork.com",
//                IsActive = true
//            });
//        }
//    }
//}
