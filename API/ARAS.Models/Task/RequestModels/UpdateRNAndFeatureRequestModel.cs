using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ARAS.Models.Task.RequestModels
{
    public class UpdateRNAndFeatureRequestModel
    {
        public Guid GuidId { get; set; }
        public string RN { get; set; }

    }
}
