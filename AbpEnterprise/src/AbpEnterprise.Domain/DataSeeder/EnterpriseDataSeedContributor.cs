using AbpEnterprise.Enterpries.EnterpriseInterfaces;
using AbpEnterprise.Enterpries.Enum;
using AbpEnterprise.Enterprises;
using AbpEnterprise.Enterprises.DomainServices;
using AbpEnterprise.Enterprises.Interfaces;
using AbpEnterprise.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace AbpEnterprise.DataSeeder
{
    public class EnterpriseDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IEnterpriseIndustryRepository _enterpriseIndustryRepository;
        private readonly IEnterpriseTypeAddressRepository _addressRepository;
        private readonly IEnterpriseTypeAddressMappingRepository _mappingRepository;
        private readonly IdentityUserManager _userManager;
        private readonly IdentityRoleManager _roleManager;
        private readonly IPermissionManager _permissionManager;
        private readonly EnterpriseIndustryManager _enterpriseIndustryManager;
        private readonly EnterpriseTypeAddressManager _addressManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public EnterpriseDataSeedContributor(
            IUnitOfWorkManager unitOfWorkManager,
            IGuidGenerator guidGenerator,
            IEnterpriseIndustryRepository enterpriseIndustryRepository,
            IEnterpriseTypeAddressRepository addressRepository,
            IEnterpriseTypeAddressMappingRepository mappingRepository,
            IdentityUserManager userManager,
            IdentityRoleManager roleManager,
            IPermissionManager permissionManager,
            EnterpriseIndustryManager enterpriseIndustryManager,
            EnterpriseTypeAddressManager addressManager)
        {
            _guidGenerator = guidGenerator;
            _enterpriseIndustryRepository = enterpriseIndustryRepository;
            _addressRepository = addressRepository;
            _mappingRepository = mappingRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _permissionManager = permissionManager;
            _enterpriseIndustryManager = enterpriseIndustryManager;
            _addressManager = addressManager;
            _unitOfWorkManager = unitOfWorkManager;
        }

        //[UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            await SeedRolesAsync();
            await SeedUsersAsync();
            await SeedEnterpriseDataAsync();
        }

        private async Task SeedRolesAsync()
        {
            // Create Admin role with all permissions
            await CreateRoleIfNotExistsAsync("Admin", new[]
            {
                AbpEnterprisePermissions.EnterpriseIndustries.Default,
                AbpEnterprisePermissions.EnterpriseIndustries.Create,
                AbpEnterprisePermissions.EnterpriseIndustries.Edit,
                AbpEnterprisePermissions.EnterpriseIndustries.Delete,

                AbpEnterprisePermissions.EnterpriseTypeAddresses.Default,
                AbpEnterprisePermissions.EnterpriseTypeAddresses.Create,
                AbpEnterprisePermissions.EnterpriseTypeAddresses.Edit,
                AbpEnterprisePermissions.EnterpriseTypeAddresses.Delete,

            });

            // Create Manager role with comprehensive management permissions
            await CreateRoleIfNotExistsAsync("Manager", new[]
            {
                AbpEnterprisePermissions.EnterpriseIndustries.Default,
                AbpEnterprisePermissions.EnterpriseIndustries.Create,
                AbpEnterprisePermissions.EnterpriseIndustries.Edit,

                AbpEnterprisePermissions.EnterpriseTypeAddresses.Default,
                AbpEnterprisePermissions.EnterpriseTypeAddresses.Create,
                AbpEnterprisePermissions.EnterpriseTypeAddresses.Edit,

            });

            // Create ProductManager role (focused on business operations)
            await CreateRoleIfNotExistsAsync("ProductManager", new[]
            {
                AbpEnterprisePermissions.EnterpriseIndustries.Default,
                AbpEnterprisePermissions.EnterpriseIndustries.Create,
                AbpEnterprisePermissions.EnterpriseIndustries.Edit,

                AbpEnterprisePermissions.EnterpriseTypeAddresses.Default,
                AbpEnterprisePermissions.EnterpriseTypeAddresses.Create,
                AbpEnterprisePermissions.EnterpriseTypeAddresses.Edit,
            });

            // Create Editor role (can modify but not delete critical data)
            await CreateRoleIfNotExistsAsync("Editor", new[]
            {
                AbpEnterprisePermissions.EnterpriseIndustries.Default,
                AbpEnterprisePermissions.EnterpriseIndustries.Edit,


                AbpEnterprisePermissions.EnterpriseTypeAddresses.Default,
                AbpEnterprisePermissions.EnterpriseTypeAddresses.Edit,
            });

            // Create Contributor role (can create and edit own content)
            await CreateRoleIfNotExistsAsync("Contributor", new[]
            {
                AbpEnterprisePermissions.EnterpriseIndustries.Default,

                AbpEnterprisePermissions.EnterpriseTypeAddresses.Default,
                AbpEnterprisePermissions.EnterpriseTypeAddresses.Create,

            });

            // Create Viewer role (read-only access)
            await CreateRoleIfNotExistsAsync("Viewer", new[]
            {
                AbpEnterprisePermissions.EnterpriseIndustries.Default,
                AbpEnterprisePermissions.EnterpriseTypeAddresses.Default,
            });
        }

        private async Task CreateRoleIfNotExistsAsync(string roleName, string[] permissions)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                role = new IdentityRole(_guidGenerator.Create(), roleName, tenantId: null)
                {
                    IsDefault = roleName == "Viewer", // Make Viewer the default role
                    IsPublic = true
                };

                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    throw new Exception($"Failed to create role {roleName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }

            // Grant permissions to role
            foreach (var permission in permissions)
            {
                try
                {
                    await _permissionManager.SetForRoleAsync(roleName, permission, true);
                }
                catch (Exception ex)
                {
                    // Log warning but continue
                    Console.WriteLine($"Warning: Failed to set permission {permission} for role {roleName}: {ex.Message}");
                }
            }
        }

        private async Task SeedUsersAsync()
        {
            // Create Super Admin user
            await CreateUserIfNotExistsAsync(
                userName: "superadmin",
                email: "superadmin@yourproject.com",
                password: "SuperAdmin123!",
                roleNames: new[] { "Admin" },
                name: "Super Administrator",
                surname: "System");

            // Create Admin user
            await CreateUserIfNotExistsAsync(
                userName: "admin1",
                email: "admin@yourproject.com",
                password: "Admin123!",
                roleNames: new[] { "Admin" },
                name: "Administrator",
                surname: "User");

            // Create Manager user
            await CreateUserIfNotExistsAsync(
                userName: "manager",
                email: "manager@yourproject.com",
                password: "Manager123!",
                roleNames: new[] { "Manager" },
                name: "System Manager",
                surname: "User");

            // Create Product Manager user
            await CreateUserIfNotExistsAsync(
                userName: "productmanager",
                email: "pm@yourproject.com",
                password: "PM123!",
                roleNames: new[] { "ProductManager" },
                name: "Product Manager",
                surname: "Business");

            // Create Editor user
            await CreateUserIfNotExistsAsync(
                userName: "editor",
                email: "editor@yourproject.com",
                password: "Editor123!",
                roleNames: new[] { "Editor" },
                name: "Content Editor",
                surname: "User");

            // Create Contributor user
            await CreateUserIfNotExistsAsync(
                userName: "contributor",
                email: "contributor@yourproject.com",
                password: "Contributor123!",
                roleNames: new[] { "Contributor" },
                name: "Content Contributor",
                surname: "User");

            // Create Viewer user
            await CreateUserIfNotExistsAsync(
                userName: "viewer",
                email: "viewer@yourproject.com",
                password: "Viewer123!",
                roleNames: new[] { "Viewer" },
                name: "System Viewer",
                surname: "User");

            // Create demo users with multiple roles
            await CreateUserIfNotExistsAsync(
                userName: "demo.manager",
                email: "demo.manager@yourproject.com",
                password: "DemoManager123!",
                roleNames: new[] { "Manager", "ProductManager" },
                name: "Demo Manager",
                surname: "User");

            await CreateUserIfNotExistsAsync(
                userName: "demo.editor",
                email: "demo.editor@yourproject.com",
                password: "DemoEditor123!",
                roleNames: new[] { "Editor", "Contributor" },
                name: "Demo Editor",
                surname: "User");
        }

        private async Task CreateUserIfNotExistsAsync(
            string userName,
            string email,
            string password,
            string[] roleNames,
            string name = null,
            string surname = null)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new IdentityUser(_guidGenerator.Create(), userName, email, tenantId: null)
                {
                    Name = name ?? userName,
                    Surname = surname ?? "User",
                };

                var createResult = await _userManager.CreateAsync(user, password);
                if (!createResult.Succeeded)
                {
                    throw new Exception($"Failed to create user {userName}: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                }

                // Add user to roles
                foreach (var roleName in roleNames)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, roleName);
                    if (!roleResult.Succeeded)
                    {
                        Console.WriteLine($"Warning: Failed to add user {userName} to role {roleName}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }

        private async Task SeedEnterpriseDataAsync()
        {
            // Create Enterprise Industries
            using (var uow = _unitOfWorkManager.Begin())
            {     
                var technologyIndustryId = _guidGenerator.Create();
            var manufacturingIndustryId = _guidGenerator.Create();
            var serviceIndustryId = _guidGenerator.Create();
            var retailIndustryId = _guidGenerator.Create();

            if (!await _enterpriseIndustryRepository.AnyAsync())
            {
                // Technology Industry
                var technologyIndustry = await _enterpriseIndustryManager.CreateAsync(
                    "Technology", "TECH", "Technology and Software Industry");

                // Manufacturing Industry
                var manufacturingIndustry = await _enterpriseIndustryManager.CreateAsync(
                    "Manufacturing", "MFG", "Manufacturing and Production Industry");

                // Service Industry
                var serviceIndustry = await _enterpriseIndustryManager.CreateAsync(
                    "Services", "SVC", "Professional and Business Services");

                // Retail Industry
                var retailIndustry = await _enterpriseIndustryManager.CreateAsync(
                    "Retail", "RTL", "Retail and E-commerce Industry");

                await _enterpriseIndustryRepository.InsertManyAsync(new[]
                {
                    technologyIndustry, manufacturingIndustry, serviceIndustry, retailIndustry
                });

                // Add Enterprise Types to Technology Industry
                var softwareType = await _enterpriseIndustryManager.CreateEnterpriseTypeAsync(
                    technologyIndustry, "Software Development", "SOFT", "Software development companies");
                var consultingType = await _enterpriseIndustryManager.CreateEnterpriseTypeAsync(
                    technologyIndustry, "IT Consulting", "CONS", "IT consulting services");
                var startupType = await _enterpriseIndustryManager.CreateEnterpriseTypeAsync(
                    technologyIndustry, "Tech Startup", "STARTUP", "Technology startups");

                // Add Enterprise Types to Manufacturing Industry  
                var automotiveType = await _enterpriseIndustryManager.CreateEnterpriseTypeAsync(
                    manufacturingIndustry, "Automotive", "AUTO", "Automotive manufacturing");
                var electronicsType = await _enterpriseIndustryManager.CreateEnterpriseTypeAsync(
                    manufacturingIndustry, "Electronics", "ELEC", "Electronics manufacturing");
                var textileType = await _enterpriseIndustryManager.CreateEnterpriseTypeAsync(
                    manufacturingIndustry, "Textile", "TEXT", "Textile and garment manufacturing");

                // Add Enterprise Types to Service Industry
                var financialType = await _enterpriseIndustryManager.CreateEnterpriseTypeAsync(
                    serviceIndustry, "Financial Services", "FINSVCS", "Banking and financial services");
                var legalType = await _enterpriseIndustryManager.CreateEnterpriseTypeAsync(
                    serviceIndustry, "Legal Services", "LEGAL", "Legal and law services");
                var marketingType = await _enterpriseIndustryManager.CreateEnterpriseTypeAsync(
                    serviceIndustry, "Marketing Agency", "MARKET", "Marketing and advertising agencies");

                // Add Enterprise Types to Retail Industry
                var ecommerceType = await _enterpriseIndustryManager.CreateEnterpriseTypeAsync(
                    retailIndustry, "E-commerce", "ECOM", "Online retail platforms");
                var fashionType = await _enterpriseIndustryManager.CreateEnterpriseTypeAsync(
                    retailIndustry, "Fashion Retail", "FASHION", "Fashion and apparel retail");

                await _enterpriseIndustryRepository.UpdateAsync(technologyIndustry);
                await _enterpriseIndustryRepository.UpdateAsync(manufacturingIndustry);
                await _enterpriseIndustryRepository.UpdateAsync(serviceIndustry);
                await _enterpriseIndustryRepository.UpdateAsync(retailIndustry);
            }

                // Create Comprehensive Address Data
                if (!await _addressRepository.AnyAsync())
                {
                    var addresses = new List<EnterpriseTypeAddress>();

                    // Technology hubs
                    addresses.Add(await _addressManager.CreateAsync(
                        "Silicon Valley HQ", "1 Hacker Way", "Menlo Park", "CA", "USA", "94025",
                        "+1-650-543-4800", "contact@siliconvalley.com"));

                    addresses.Add(await _addressManager.CreateAsync(
                        "Seattle Tech Center", "410 Terry Ave N", "Seattle", "WA", "USA", "98109",
                        "+1-206-266-1000", "seattle@techcenter.com"));

                    addresses.Add(await _addressManager.CreateAsync(
                        "Austin Innovation Hub", "500 W 2nd St", "Austin", "TX", "USA", "78701",
                        "+1-512-474-5171", "austin@innovation.com"));

                    // Business centers
                    addresses.Add(await _addressManager.CreateAsync(
                        "New York Financial District", "200 West Street", "New York", "NY", "USA", "10282",
                        "+1-212-619-2000", "ny@financial.com"));

                    addresses.Add(await _addressManager.CreateAsync(
                        "Chicago Business Center", "233 S Wacker Dr", "Chicago", "IL", "USA", "60606",
                        "+1-312-875-8000", "chicago@business.com"));

                    // International offices
                    addresses.Add(await _addressManager.CreateAsync(
                        "London European HQ", "25 Churchill Pl", "London", "England", "UK", "E14 5HU",
                        "+44-20-7418-8000", "london@european.com"));

                    addresses.Add(await _addressManager.CreateAsync(
                        "Tokyo Asia Pacific", "1-1-1 Marunouchi", "Tokyo", "Tokyo", "Japan", "100-6390",
                        "+81-3-6225-1000", "tokyo@asiapacific.com"));

                    addresses.Add(await _addressManager.CreateAsync(
                        "Singapore Regional Office", "8 Marina View", "Singapore", "Singapore", "Singapore", "018960",
                        "+65-6538-0000", "singapore@regional.com"));

                    // Manufacturing facilities
                    addresses.Add(await _addressManager.CreateAsync(
                        "Detroit Manufacturing Plant", "2000 Town Center", "Southfield", "MI", "USA", "48075",
                        "+1-248-948-2000", "detroit@manufacturing.com"));

                    addresses.Add(await _addressManager.CreateAsync(
                        "Shanghai Factory", "1000 Lujiazui Ring Rd", "Shanghai", "Shanghai", "China", "200120",
                        "+86-21-2899-5000", "shanghai@factory.com"));

                    await _addressRepository.InsertManyAsync(addresses);
                    await uow.SaveChangesAsync();
                }
                    // Create comprehensive mappings between enterprise types and addresses
                var industries = await _enterpriseIndustryRepository.GetListAsync();
                var allAddresses = await _addressRepository.GetListAsync();

                foreach (var industry in industries)
                {
                    foreach (var enterpriseType in industry.EnterpriseTypes)
                    {
                        var mappingsToCreate = new List<(EnterpriseTypeAddress address, AddressType type)>();

                        // Define mapping logic based on industry and enterprise type
                        if (industry.Code == "TECH")
                        {
                            if (enterpriseType.Code == "SOFT")
                            {
                                mappingsToCreate.Add((allAddresses[0], AddressType.Headquarters)); // Silicon Valley HQ
                                mappingsToCreate.Add((allAddresses[1], AddressType.Office)); // Seattle
                                mappingsToCreate.Add((allAddresses[2], AddressType.Branch)); // Austin
                                mappingsToCreate.Add((allAddresses[5], AddressType.Office)); // London
                            }
                            else if (enterpriseType.Code == "CONS")
                            {
                                mappingsToCreate.Add((allAddresses[3], AddressType.Headquarters)); // NYC Financial
                                mappingsToCreate.Add((allAddresses[4], AddressType.Office)); // Chicago
                                mappingsToCreate.Add((allAddresses[5], AddressType.Office)); // London
                            }
                            else if (enterpriseType.Code == "STARTUP")
                            {
                                mappingsToCreate.Add((allAddresses[0], AddressType.Office)); // Silicon Valley
                                mappingsToCreate.Add((allAddresses[2], AddressType.Office)); // Austin
                            }
                        }
                        else if (industry.Code == "MFG")
                        {
                            if (enterpriseType.Code == "AUTO")
                            {
                                mappingsToCreate.Add((allAddresses[8], AddressType.Headquarters)); // Detroit
                                mappingsToCreate.Add((allAddresses[9], AddressType.Factory)); // Shanghai
                            }
                            else if (enterpriseType.Code == "ELEC")
                            {
                                mappingsToCreate.Add((allAddresses[7], AddressType.Headquarters)); // Singapore
                                mappingsToCreate.Add((allAddresses[6], AddressType.Office)); // Tokyo
                                mappingsToCreate.Add((allAddresses[9], AddressType.Factory)); // Shanghai
                            }
                        }
                        else if (industry.Code == "SVC")
                        {
                            if (enterpriseType.Code == "FINSVCS")
                            {
                                mappingsToCreate.Add((allAddresses[3], AddressType.Headquarters)); // NYC Financial
                                mappingsToCreate.Add((allAddresses[5], AddressType.Office)); // London
                                mappingsToCreate.Add((allAddresses[7], AddressType.Office)); // Singapore
                            }
                        }
                        else if (industry.Code == "RTL")
                        {
                            if (enterpriseType.Code == "ECOM")
                            {
                                mappingsToCreate.Add((allAddresses[1], AddressType.Headquarters)); // Seattle
                                mappingsToCreate.Add((allAddresses[4], AddressType.Warehouse)); // Chicago
                            }
                        }

                        // Create the mappings
                        var listAddMappings = new List<EnterpriseTypeAddressMapping>();
                        foreach (var (address, addressType) in mappingsToCreate)
                        {
                            try
                            {
                                var mapping = new EnterpriseTypeAddressMapping(
                                    _guidGenerator.Create(),
                                    enterpriseType.Id,
                                    address.Id);
                                listAddMappings.Add(mapping);
                            }
                            catch (Exception ex)
                            {
                                // Log but continue
                                Console.WriteLine($"Warning: Failed to create mapping for {enterpriseType.Name} - {address.Name}: {ex.Message}");
                            }
                        }
                        if (listAddMappings.Any())
                        {
                            await _mappingRepository.InsertManyAsync(listAddMappings);
                        }
                    }
                }
            }
        }
    }
}

