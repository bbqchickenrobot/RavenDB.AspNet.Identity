using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace RavenDB.AspNet.Identity
{
    public abstract class BaseRole : IRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Role : BaseRole
    {
        
    }
}
