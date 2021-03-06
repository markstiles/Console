﻿using System.Data;
using System.Management.Automation;
using Sitecore.Security.Accounts;
using Spe.Core.Extensions;
using Spe.Core.Validation;

namespace Spe.Commands.Security.Accounts
{
    [Cmdlet(VerbsCommon.Remove, "User", DefaultParameterSetName = "Id", SupportsShouldProcess = true)]
    public class RemoveUserCommand : BaseSecurityCommand
    {
        [Alias("Name")]
        [Parameter(Mandatory = true, ValueFromPipeline = true, Position = 0,
            ParameterSetName = "Id")]
        [ValidateNotNullOrEmpty]
        [AutocompleteSet(nameof(UserNames))]
        public AccountIdentity Identity { get; set; }

        [Parameter(Mandatory = true, ValueFromPipeline = true,
            ParameterSetName = "Instance")]
        [ValidateNotNull]
        public User Instance { get; set; }

        protected override void ProcessRecord()
        {
            switch (ParameterSetName)
            {
                case "Instance":
                    WriteVerbose($"Removing user '{Instance.Name}'.");
                    Instance.Delete();
                    break;
                default:
                    if (!this.CanFindAccount(Identity, AccountType.User))
                    {
                        WriteError(typeof(ObjectNotFoundException), $"User '{Identity.Name}' not found.",
                            ErrorIds.AccountNotFound, ErrorCategory.ResourceUnavailable, Identity);
                        return;
                    }

                    var name = Identity?.Name ?? string.Empty;

                    if (!ShouldProcess(name, "Remove user")) return;

                    var user = User.FromName(name, true);
                    WriteVerbose($"Removing user '{user.Name}'.");
                    user.Delete();
                    break;
            }
        }
    }
}