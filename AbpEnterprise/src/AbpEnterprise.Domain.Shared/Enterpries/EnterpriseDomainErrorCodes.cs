using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpEnterprise.Enterpries
{
    public static class EnterpriseDomainErrorCodes
    {
        public const string EnterpriseIndustryCodeAlreadyExists = "Enterprise:001";
        public const string EnterpriseTypeCodeAlreadyExists = "Enterprise:002";
        public const string CannotDeactivateIndustryWithActiveTypes = "Enterprise:003";
        public const string EnterpriseIndustryNotFound = "Enterprise:004";
        public const string EnterpriseTypeNotFound = "Enterprise:005";
    }
}
