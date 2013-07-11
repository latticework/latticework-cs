using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lw.ComponentModel.DataAnnotations
{
    /// <summary>
    ///     Specifies that this is a TransietKey property of the parent Type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited=true)]
    public sealed class TransientKeyAttribute : Attribute
    {
    }
}
