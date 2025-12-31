using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ARAS.Models.Task.RequestModels
{
    public class UpdateTaskRequestModel
    {
        public Guid TaskUniqueId { get; set; }
        public long UserId { get; set; }
        [StringLength(100)]
        public string UserName { get; set; }
        [StringLength(100)]
        public string Catagory { get; set; }
        public string TaskName { get; set; }
        [StringLength(50)]
        public string SubProject { get; set; }
        [StringLength(100)]
        public string Network { get; set; }
        [StringLength(100)]
        public string Status { get; set; }
        
        public bool ItemToDiscuss { get; set; }
        public string MyComments { get; set; }
        [StringLength(100)]
        public string Jira { get; set; }
        public Guid RNGuidId { get; set; }
        public string FeatureName { get; set; }
        [StringLength(100)]
        public string FixVersion { get; set; }
        public string RNComments { get; set; }
        public string MailTitled { get; set; }
    }
}
