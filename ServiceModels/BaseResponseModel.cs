namespace ServiceModels
{
    public class BaseResponseModel
    {
        public int HttpStatusCode { get; protected set; }
        public string Message { get; protected set; }
        public BaseResponseModel(int httpStatusCode, string message)
        {
            HttpStatusCode = httpStatusCode;
            Message = message;
        }
    }
}
