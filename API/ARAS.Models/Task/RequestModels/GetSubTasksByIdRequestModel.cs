using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARAS.Models.Task.RequestModels
{
    public class GetSubTasksByIdRequestModel
    {
        public Guid TaskUniqueId { get; set; }
    }
}
