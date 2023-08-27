using System.Globalization;

namespace SocialMedia.Core.Aplication.Exceptions
{
  public class ApiException : Exception
  {
    public int ErrorCode { get; set; }

    public ApiException() : base() { }

    public ApiException(string message) : base(message) { }

    public ApiException(string message, int errorCode) : base(message)
    {
      ErrorCode = errorCode;
    }

    // This constructor is recommended to implement in order fullfill all they possible scenarios
    //params object[] args // This is a parameter array, it means that you can pass as many arguments as you want
    public ApiException(string message, params object[] args) 
      : base(String.Format(CultureInfo.CurrentCulture, message, args)) { }
  }
}
