namespace ARAS.Models.Task.RequestModels
{
    public class AddTaskCommentRequestModel
    {

        public Guid TaskUniqueId { get; set; }
        public string Comment { get; set; }
        public string ProjectKey { get; set; }

    }
}

