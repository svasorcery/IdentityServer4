﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Test
{
    public class TestUserProfileService : IProfileService
    {
        protected readonly ILogger Logger;
        protected readonly TestUserStore Users;

        public TestUserProfileService(TestUserStore users, ILogger<TestUserProfileService> logger)
        {
            Users = users;
            Logger = logger;
        }

        public virtual Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            Logger.LogDebug("Get profile called for subject {subject} from client {client} with claim types {claimTypes} via {caller}",
                context.Subject.GetSubjectId(),
                context.Client.ClientName ?? context.Client.ClientId,
                context.RequestedClaimTypes,
                context.Caller);

            if (context.RequestedClaimTypes.Any())
            {
                var user = Users.FindBySubjectId(context.Subject.GetSubjectId());
                context.AddFilteredClaims(user.Claims);
            }

            return Task.FromResult(0);
        }

        public virtual Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;

            return Task.FromResult(0);
        }
    }
}