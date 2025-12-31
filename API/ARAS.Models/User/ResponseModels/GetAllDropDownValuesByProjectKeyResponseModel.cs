using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARAS.Infrastructure.DBModels.YourApp.DomainModels;

namespace ARAS.Models.User.ResponseModels
{
    public class GetAllDropDownValuesByProjectKeyResponseModel
    {
        public List<DropDownValueModel> DropDownValueModels { get; set; }

    }
}
