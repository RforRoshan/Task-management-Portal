
using ARAS.Infrastructure.DBModels.YourApp.DomainModels;

namespace ARAS.Models.Task.ResponseModels
{
    public class GetAllTaskCommentResponseModel
    {
        public IList<TaskCommentModel> TaskComments { get; set; } = [];
    }

    public class TaskCommentModel
    {
        public Guid GuidId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comment { get; set; }
        public DateTimeOffset CommentedOn { get; set; }
        public bool IsPreviouslyEdited { get; set; }
        public bool isSameUserComment { get; set; }
        public DateTimeOffset? CommentEditedOn { get; set; }
    }
}
