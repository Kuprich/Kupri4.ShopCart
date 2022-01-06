using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Mvc.ModelBinding
{
    public  static class ModelStateDictionaryExtensions
    {
        public static void AddModelErrors(this ModelStateDictionary modelStateDictionary, IEnumerable<IdentityError> identityErrors)
        {
            foreach(var error in identityErrors)
            {
                modelStateDictionary.AddModelError("", error.Description);
            }
        }
    }
}
