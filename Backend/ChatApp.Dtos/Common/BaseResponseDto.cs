namespace ChatApp.Dtos.Common
{
    public class BaseResponseDto<T>
    {
        public T Result { get; set; }

        public bool Success { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
