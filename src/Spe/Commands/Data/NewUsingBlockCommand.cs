﻿using System;
using System.Management.Automation;

namespace Spe.Commands.Data
{
    [Cmdlet(VerbsCommon.New, "UsingBlock")]
    public class NewUsingBlockCommand : BaseCommand
    {
        [Parameter(Mandatory = true, Position = 0)]
        public IDisposable InputObject { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public ScriptBlock ScriptBlock { get; set; }

        protected override void ProcessRecord()
        {
            using (InputObject)
            {
                WriteObject(ScriptBlock.Invoke());
            }
        }
    }
}