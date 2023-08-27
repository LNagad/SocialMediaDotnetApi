using SocialMedia.Core.Aplication.DTOs.CustomEntities;

namespace SocialMedia.Core.Aplication.Wrappers
{
    public class Response<T> where T : class
  {
    public bool Succeeded { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }
    public T Data { get; set; }
    public Metadata? Meta { get; set; }


    public Response() { }

    public Response(T data, string? message = null) // everything went well
    {
      Succeeded = true;
      Message = message ?? "";
      Data = data;
    }

    public Response(T data, string? message = null, Metadata? meta = null) // everything went well
    {
      Succeeded = true;
      Message = message ?? "";
      Data = data;
      Meta = meta;
    }

    public Response(string? message) // something went wrong
    {
      Succeeded = false;
      Message = message;
    }

  }
}
