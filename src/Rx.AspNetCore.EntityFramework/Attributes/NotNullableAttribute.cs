using System;

namespace Rx.AspNetCore.EntityFramework.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class NotNullAttribute : Attribute
    {
    }
}
