using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependentOperation.Models
{
    public class DependencyModel
    {
        public DependencyModel()
        {

        }
        public DependencyModel(string dependency, string item)
        {
            Dependency = dependency;

            Item = item;

        }
        public string Dependency { get; set; } = string.Empty;
        public string Item { get; set; } = string.Empty;
    }
}
