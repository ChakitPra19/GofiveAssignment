namespace BackendApi.Dtos
{
    public class DeleteUserResponseDto
    {
        public Status Status { get; set; }
        public DeleteUserData Data { get; set; }
    }

    public class Status
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class DeleteUserData
    {
        public bool Result { get; set; }
        public string Message { get; set; }
    }
} 