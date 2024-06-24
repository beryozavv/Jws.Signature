using Refit;

namespace ApiResponseSignature.Sender.RefitClients;

public interface IRespondenApi
{
    [Get("/Respondent")]
    Task<ResponseDto> Test();
}