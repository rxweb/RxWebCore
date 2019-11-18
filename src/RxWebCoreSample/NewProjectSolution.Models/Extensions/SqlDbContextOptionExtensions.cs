using Microsoft.EntityFrameworkCore.Infrastructure;
using NewProjectSolution.Models.Const;
using System;
using System.Collections.Generic;

namespace NewProjectSolution.Models.Extensions
{
    public static class SqlDbContextOptionExtensions
    {
        public static SqlServerDbContextOptionsBuilder AddConnectionResiliency(this SqlServerDbContextOptionsBuilder options, Dictionary<string, int> resiliencyOptions)
        {
            if (resiliencyOptions.ContainsKey(ApplicationConstants.MAX_RETRY_COUNT) && resiliencyOptions.ContainsKey(ApplicationConstants.MAX_RETRY_DELAY))
                return options.EnableRetryOnFailure(resiliencyOptions[ApplicationConstants.MAX_RETRY_COUNT], TimeSpan.FromMilliseconds(resiliencyOptions[ApplicationConstants.MAX_RETRY_DELAY]), null);
            else if (resiliencyOptions.ContainsKey(ApplicationConstants.MAX_RETRY_COUNT))
                return options.EnableRetryOnFailure(resiliencyOptions[ApplicationConstants.MAX_RETRY_COUNT]);
            else
                return options.EnableRetryOnFailure();
        }
    }
}

