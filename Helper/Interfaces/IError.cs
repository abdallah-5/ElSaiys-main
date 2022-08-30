namespace ElSaiys.Helper
{
    public interface IError
    {
        string ErrorCode { get; set; }
        string ErrorMessage { get; set; }
        string ErrorProp { get; set; }

        void LoadError(string errorCode);
    }
}