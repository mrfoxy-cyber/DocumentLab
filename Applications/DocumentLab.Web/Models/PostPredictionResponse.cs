namespace DocumentLab.Web.Models
{

  public enum PostPredictionResult
  {
    Success,
    Failed_InternalError
  }

  public class PostPredictionResponse
  {
    public PostPredictionResult Result { get; set; }
    public string Message { get; set; }
  }
}