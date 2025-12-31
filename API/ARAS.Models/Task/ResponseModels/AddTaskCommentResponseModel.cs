
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARAS.Models.Task.ResponseModels
{
    public class AddTaskCommentResponseModel
    {
        public Guid GuidId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset CommentedOn { get; set; }

    }
}
