using MySolution.Models.Common;
using MySolution.Models.System.Users;

namespace MySolution.ApiIntergration
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Login(LoginRequest request);
        Task<ApiResult<PageResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request);
        Task<ApiResult<bool>> Register(RegisterRequest request);
        Task<ApiResult<bool>> Update(UpdateRequest request);
        Task<ApiResult<UserViewModel>> GetById(string Id);
        Task<ApiResult<bool>> Delete(UserDeleteRequest request);
        Task<ApiResult<bool>> ForgetPassword(ForgetPasswordRequest request);
        Task<ApiResult<bool>> ChangePassword(ChangePasswordRequest request);
    }
}