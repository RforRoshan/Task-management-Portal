using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Models.Task.RequestModels
{
    public class GetTaskLogByIdRequestModel
    {
        public Guid TaskUniqueId { get; set; }
    }
}
