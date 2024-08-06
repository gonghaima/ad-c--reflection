using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityFunctions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class InformationAttribute : Attribute
    {
        public string Description { get; set; }
    }
}