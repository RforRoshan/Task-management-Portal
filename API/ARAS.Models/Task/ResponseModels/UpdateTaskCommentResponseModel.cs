
namespace ARAS.Models.Task.ResponseModels
{
    public class UpdateTaskCommentResponseModel
    {
        public bool IsPreviouslyEdited { get; set; }
        public DateTimeOffset? CommentEditedOn { get; set; }
    }
}
