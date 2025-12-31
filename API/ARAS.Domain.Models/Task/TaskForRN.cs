using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Domain.Models.Task
{
    public class TaskForRN
    {
        public Guid TaskUniqueId { get; set; }
        public string Jira { get; set; }
        public string FeatureName { get; set; }
        public string FixVersion { get; set; }
        public string RNComments { get; set; }
        public string FirstName { get; set; }
        public bool IsAlive { get; set; }
    }
}
