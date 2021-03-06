﻿using FluentValidation;
using System;

namespace Esquio.UI.Api.Shared.Models.Permissions.List
{
    public class ListPermissionRequestValidator
         : AbstractValidator<ListPermissionRequest>
    {
        public ListPermissionRequestValidator()
        {
            this.RuleFor(rf => rf.PageIndex)
                .GreaterThanOrEqualTo(0)
                .LessThan(Int32.MaxValue);

            this.RuleFor(rf => rf.PageCount)
                .GreaterThan(0)
                .LessThan(100);
        }
    }
}
