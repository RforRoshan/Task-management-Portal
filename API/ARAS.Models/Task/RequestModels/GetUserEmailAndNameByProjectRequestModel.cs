using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARAS.Infrastructure.DBModels;

namespace ARAS.Models.Task.RequestModels
{
    public class GetUserEmailAndNameByProjectRequestModel
    {
        public string ProjectName { get; set; }
    }
}
