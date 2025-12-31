

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ARAS.Business.Utility;
using ARAS.Infrastructure.DBModels;
using ARAS.Infrastructure.DBModels.YourApp.DomainModels;
using ARAS.Infrastructure.Repository;
using ARAS.Models.Task.ResponseModels;
using ARAS.Models.User.RequestModels;
using ARAS.Models.User.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static System.Net.WebRequestMethods;

namespace ARAS.Business.Services
{
    public class UserServices: IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserServices(IUserRepository userRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<LoginResponseModel>> Login(LoginRequestModel requestModel)
        {
            ApiResult<LoginResponseModel> responseModel = new ApiResult<LoginResponseModel>();
            LoginResponseModel data = new LoginResponseModel();
            data.Token = "invalid email or passward";

            if (requestModel.Source == "Web")
            {
                requestModel.Password = EncryptionService.Decrypt(requestModel.Password);
            }

            User user = await _userRepository.GetUserByEmail(requestModel.EmailId);
            if (user != null)
            {
                if (VerifyPassword(requestModel.Password, user.PasswordHash, user.PasswordSalt))
                {

                    if (!user.IsEmailConfirmed)
                    {
                        data.IsFirstTimeLogin = true;
                        try
                        {
                            PasswordReset passwordReset = new PasswordReset();
                            passwordReset.UserId = user.UserId;
                            passwordReset.Email = user.Email;
                            passwordReset.OTP = new Random().Next(1000001, 10000000);
                            passwordReset.ValidityDateTime = DateTime.Now.AddMinutes(10);
                            passwordReset = await _userRepository.AddPasswordReset(passwordReset);

                            data.Token = EncryptionService.URLEncrypt(requestModel.EmailId + "|OTP|" + passwordReset.OTP.ToString());
                        }
                        catch
                        {
                            responseModel.ResponseData = null;
                            responseModel.ErrorCode = "";
                            responseModel.Message = "Something went wrong during first login.";
                            responseModel.Status = false;
                        }

                    }
                    else
                    {

                        #region JWT

                        var claims = new[]
                        {
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("Email", user.Email),
                        new Claim("FirstName", user.FirstName),
                        new Claim("MiddleName", user.MiddleName),
                        new Claim("LastName", user.LastName),
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            issuer: _configuration["Jwt:Issuer"],
                            audience: _configuration["Jwt:Issuer"],
                            claims: claims,
                            expires: DateTime.UtcNow.AddDays(1),
                            signingCredentials: creds);

                        string finaltoken = new JwtSecurityTokenHandler().WriteToken(token);
                        #endregion JWT

                        data.Token = finaltoken;
                    }

                    responseModel.ResponseData = data;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "";
                    responseModel.Status = true;

                }
                else
                {
                    responseModel.ResponseData = null;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "Invalid credentials. Please try again.";
                    responseModel.Status = false;
                }
            }
            else
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Invalid User. Please try again with correct user.";
                responseModel.Status = false;
            }
            return responseModel;
        }
        public async Task<ApiResult<FirstTimeUserResponseModel>> FirstTimeUser(FirstTimeUserRequestModel requestModel)
        {
            ApiResult<FirstTimeUserResponseModel> responseModel = new ApiResult<FirstTimeUserResponseModel>();
            FirstTimeUserResponseModel data = new FirstTimeUserResponseModel();
            data.Token = "";
            responseModel.ResponseData = null;
            responseModel.ErrorCode = "";
            responseModel.Message = "Something went wrong with link.";
            responseModel.Status = false;
            try
            {
                string decryptedOTP = EncryptionService.URLDecrypt(requestModel.Token);
                if (decryptedOTP.Contains("|OTP|", StringComparison.Ordinal))
                {
                    string email = string.Empty;
                    int otp = 0;

                    List<string> emailOTP = decryptedOTP.Split("|OTP|", StringSplitOptions.None).ToList();

                    if (emailOTP.Count == 2)
                    {
                        email = emailOTP[0];
                        int.TryParse(emailOTP[1], out otp); // safely parse OTP

                        PasswordReset passwordReset = new PasswordReset();
                        passwordReset = await _userRepository.GetPasswordReset(email, otp);
                        if (passwordReset != null)
                        {

                            if (requestModel.Source == "Web")
                            {
                                requestModel.Password = EncryptionService.Decrypt(requestModel.Password);
                            }


                            User user = await _userRepository.GetUserByEmail(email);
                            if (user != null)
                            {
                                (string hash, string salt) = HashPassword(requestModel.Password);
                                user.PasswordHash = hash;
                                user.PasswordSalt = salt;
                                user = await _userRepository.UpdateUser(user);


                                passwordReset.IsVerified = true;
                                passwordReset = await _userRepository.UpdatePasswordReset(passwordReset);

                                user.IsEmailConfirmed = true;
                                user = await _userRepository.UpdateUser(user);


                                #region JWT

                                var claims = new[]{
                                new Claim("UserId", user.UserId.ToString()),
                                new Claim("Email", user.Email),
                                new Claim("FirstName", user.FirstName),
                                new Claim("MiddleName", user.MiddleName),
                                new Claim("LastName", user.LastName),
                            };

                                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                                var token = new JwtSecurityToken(
                                    issuer: _configuration["Jwt:Issuer"],
                                    audience: _configuration["Jwt:Issuer"],
                                    claims: claims,
                                    expires: DateTime.UtcNow.AddDays(1),
                                    signingCredentials: creds);

                                string finaltoken = new JwtSecurityTokenHandler().WriteToken(token);
                                #endregion JWT

                                data.Token = finaltoken;
                                responseModel.ResponseData = data;
                                responseModel.ErrorCode = "";
                                responseModel.Message = "";
                                responseModel.Status = true;
                            }

                        }
                    }
                }


            }
            catch (Exception ex)
            {
            }
            return responseModel;
        }
        public async Task<ApiResult<RegisterResponseModel>> Register(RegisterRequestModel requestModel)
        {
            ApiResult<RegisterResponseModel> responseModel = new ApiResult<RegisterResponseModel>();

            RegisterResponseModel data = new RegisterResponseModel();
            data.Status = false;
            User user = await _userRepository.GetUserByEmail(requestModel.Email);
            if (user == null)
            {
                user = new User();

                #region Password Generate
                //const string validPasswordString = "ABCDE456FGH@JKL564MNPQRSTUVWXYZab@cdefghkmnpqrstuvwxyz23456789!@#$";
                //Random random = new Random();
                //string generatedPassword = new string(Enumerable.Repeat(validPasswordString, 8).Select(s => s[random.Next(s.Length)]).ToArray());

                Random random = new Random();
                string generatedPassword = random.Next(111111111, 999999999).ToString();

                #endregion Password Generate

                //generatedPassword = "cpp"; user.IsEmailConfirmed = true;

                (user.PasswordHash, user.PasswordSalt) = HashPassword(generatedPassword);
                user.Email = requestModel.Email;
                user.Username = requestModel.Email;
                user.FirstName = requestModel.FirstName;
                user.MiddleName = requestModel.MiddleName;
                user.LastName = requestModel.LastName;
                user.EmployeeNumber = requestModel.EmployeeNumber;

                User userRegister = await _userRepository.AddUser(user);

                if (userRegister != null)
                {
                    string userId = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
                    foreach (AccessRegisterModel accessDetail in requestModel.AccessDetails) {
                        UserRole userRole = new UserRole();
                        userRole.UserId = userRegister.UserId;
                        userRole.ProjectId = accessDetail.ProjectId;
                        userRole.RoleId = accessDetail.RoleId;
                        await _userRepository.AddUserRole(userRole);

                        ResourceName resourceName = new ResourceName();
                        resourceName.UserName = userRegister.Username;
                        resourceName.UserId = userRegister.UserId;
                        resourceName.Name = userRegister.FirstName;
                        resourceName.LastName = userRegister.LastName;
                        resourceName.ProjectId = accessDetail.ProjectId;

                        if (userId != null) 
                        {
                            resourceName.AddedByUserId = long.Parse(userId);
                        }
                        await _userRepository.AddResourceName(resourceName);
                    }
                    data.Status = true;
                    data.Message = $"User register successfully, please check {requestModel.Email} for credentials.";
                    responseModel.ResponseData = data;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "";
                    responseModel.Status = true;

                    string subject = $"ARAS Account Created for {user.FirstName}";
                    string body = MailUtility.GetRegisterEmailBody(user.FirstName, user.Email, generatedPassword);
                    if (_configuration["MailEnable"] != "Y")
                    {
                        user.Email = "roshan.sahu@roshan.com";
                    }
                    MailUtility.SendEmail(user.Email, subject, body);
                }

            }
            else
            {
                data.Message = "user already exist.";
                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "User already exist.";
                responseModel.Status = false;
            }
            return responseModel;
        }
        public async Task<ApiResult<ChangePasswardResponseModel>> ChangePassward(ChangePasswardRequestModel requestModel)
        {
            ApiResult<ChangePasswardResponseModel> responseModel = new ApiResult<ChangePasswardResponseModel>();
            ChangePasswardResponseModel data = new ChangePasswardResponseModel();
            responseModel.Status = false;

            string email = _httpContextAccessor.HttpContext?.User?.FindFirst("Email")?.Value;
            if (email != null)
            {
                User user = await _userRepository.GetUserByEmail(email);
                if (requestModel.Source == "Web")
                {
                    requestModel.NewPassword = EncryptionService.Decrypt(requestModel.NewPassword);
                    requestModel.OldPassword = EncryptionService.Decrypt(requestModel.OldPassword);
                }
                if (VerifyPassword(requestModel.OldPassword, user.PasswordHash, user.PasswordSalt))
                {
                    (string hash, string salt) = HashPassword(requestModel.NewPassword);
                    user.PasswordHash = hash;
                    user.PasswordSalt = salt;
                    user = await _userRepository.UpdateUser(user);
                    data.Status = true;
                    if (data.Status)
                    {
                        string subject = $"ARAS Password Change for {user.FirstName}";
                        string body = MailUtility.GetChangePasswardEmailBody(user.FirstName, user.Email);
                        if (_configuration["MailEnable"] != "Y")
                        {
                            user.Email = "roshan.sahu@roshan.com";
                        }
                        MailUtility.SendEmail(user.Email, subject, body);
                    }
                    responseModel.ResponseData = data;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "Passward change successfully.";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.ResponseData = null;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "Old Password is not correct, please try again.";
                    responseModel.Status = false;
                }
            }
            else
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Email not found in header";
                responseModel.Status = false;
            }

            return responseModel;
        }


        public async Task<ApiResult<GetProjectAccessResponseModel>> GetProjectAccess(GetProjectAccessRequestModel requestModel)
        {
            ApiResult<GetProjectAccessResponseModel> responseModel = new ApiResult<GetProjectAccessResponseModel>();
            GetProjectAccessResponseModel data = new GetProjectAccessResponseModel();
           
            try
            {
                string userId = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
                if (userId != null)
                {
                    //List<ProjectAccess> projectAccess = await _userRepository.ExecuteStoredProcedureAsync<ProjectAccess>($"EXEC GetProjectAccess {userId}");
                    List<ProjectAccess> projectAccess = await _userRepository.GetProjectAccess(long.Parse(userId));
                    data.ProjectAccess = projectAccess;
                    responseModel.ResponseData = data;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "";
                    responseModel.Status = true;
                }
                else
                {
                    responseModel.ResponseData = null;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "UserId not found in header";
                    responseModel.Status = false;
                }
            }
            catch (Exception ex)
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong.";
                responseModel.Status = false;
            }
            return responseModel;
        }


        public async Task<ApiResult<GetDashBoardDataResponseModel>> GetDashBoardData(GetDashBoardDataRequestModel requestModel)
        {
            ApiResult<GetDashBoardDataResponseModel> responseModel = new ApiResult<GetDashBoardDataResponseModel>();
            GetDashBoardDataResponseModel data = new GetDashBoardDataResponseModel();

            try
            {
                string userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
                string email = _httpContextAccessor.HttpContext?.User?.FindFirst("Email")?.Value;
                if (userIdStr != null && email != null)
                {
                    long userId = long.Parse(userIdStr);
                    Project project = await _userRepository.ProjectByKey(requestModel.ProjectKey);
                    UserRole userRoles = await _userRepository.GetUserRolesByUserAndProjectId(userId, project.ProjectId);

                    if (userRoles != null)
                    {
                        User user = await _userRepository.GetUserByEmail(email);

                        data.Username = user.Username;
                        data.FirstName = user.FirstName;
                        data.LastName = user.LastName;
                        data.Email = user.Email;
                        data.MiddleName = user.MiddleName;
                        data.UserRoleId = userRoles.UserRoleId;
                        data.RoleId = userRoles.RoleId;
                        data.UserId = userId;
                        data.ProjectId = project.ProjectId;

                        responseModel.ResponseData = data;
                        responseModel.ErrorCode = "";
                        responseModel.Message = "";
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.ResponseData = null;
                        responseModel.ErrorCode = "";
                        responseModel.Message = $"User Does not have permission to access the {requestModel.ProjectKey} dashboard";
                        responseModel.Status = false;
                    }
                }
                else
                {
                    responseModel.ResponseData = null;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "UserId or Email not found in header";
                    responseModel.Status = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<ForgotPasswardLinkGenerateResponseModel>> ForgotPasswardLinkGenerate(ForgotPasswardLinkGenerateRequestModel requestModel)
        {
            ApiResult<ForgotPasswardLinkGenerateResponseModel> responseModel = new ApiResult<ForgotPasswardLinkGenerateResponseModel>();
            ForgotPasswardLinkGenerateResponseModel data = new ForgotPasswardLinkGenerateResponseModel();
            responseModel.ResponseData = null;
            responseModel.ErrorCode = "";
            responseModel.Message = "Something went wrong.";
            responseModel.Status = false;
            data.IsEmailExist = false;
            User user = await _userRepository.GetUserByEmail(requestModel.EmailId);
            if (user != null)
            {
                try
                {
                    PasswordReset passwordReset = new PasswordReset();
                    passwordReset.UserId = user.UserId;
                    passwordReset.Email = user.Email;
                    passwordReset.OTP = new Random().Next(1000001, 10000000);
                    passwordReset.ValidityDateTime = DateTime.Now.AddMinutes(10);
                    passwordReset = await _userRepository.AddPasswordReset(passwordReset);

                    string encriptedOTP = EncryptionService.URLEncrypt(requestModel.EmailId + "|OTP|" +  passwordReset.OTP.ToString());

                    string link = requestModel.LinkPrefix + "/" + encriptedOTP;

                    //send otp on email code
                    string subject = $"ARAS Password Reset OTP for {user.FirstName}";
                    string body = MailUtility.GetOtpEmailBody(user.FirstName, link);
                    if (_configuration["MailEnable"] != "Y")
                    {
                        user.Email = "roshan.sahu@roshan.com";
                    }
                    MailUtility.SendEmail(user.Email, subject, body);

                    data.IsEmailExist = true;
                    responseModel.ResponseData = data;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "";
                    responseModel.Status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    responseModel.ResponseData = null;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "Something went wrong.";
                    responseModel.Status = false;
                }

            }
            else
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = $"User does not exists.";
                responseModel.Status = false;
            }
            return responseModel;
        }

        public async Task<ApiResult<ForgotPasswardResponseModel>> ForgotPasswardReset(ForgotPasswardRequestModel requestModel)
        {
            ApiResult<ForgotPasswardResponseModel> responseModel = new ApiResult<ForgotPasswardResponseModel>();
            ForgotPasswardResponseModel data = new ForgotPasswardResponseModel();

            responseModel.ResponseData = null;
            responseModel.ErrorCode = "";
            responseModel.Message = "Something went wrong.";
            responseModel.Status = false;
            string decryptedOTP = EncryptionService.URLDecrypt(requestModel.Token);
            if (decryptedOTP.Contains("|OTP|", StringComparison.Ordinal))
            {
                string email = string.Empty;
                int otp = 0;

                List<string> emailOTP = decryptedOTP.Split("|OTP|", StringSplitOptions.None).ToList();

                if (emailOTP.Count == 2)
                {
                    email = emailOTP[0];
                    int.TryParse(emailOTP[1], out otp); // safely parse OTP

                    PasswordReset passwordReset = new PasswordReset();
                    passwordReset = await _userRepository.GetPasswordReset(email, otp);
                    if (passwordReset != null)
                    {

                        if (requestModel.Source == "Web")
                        {
                            requestModel.Password = EncryptionService.Decrypt(requestModel.Password);
                        }


                        User user = await _userRepository.GetUserByEmail(email);
                        if (user != null)
                        {
                            (string hash, string salt) = HashPassword(requestModel.Password);
                            user.PasswordHash = hash;
                            user.PasswordSalt = salt;
                            user = await _userRepository.UpdateUser(user);


                            passwordReset.IsVerified = true;
                            passwordReset = await _userRepository.UpdatePasswordReset(passwordReset);

                            data.Status = true;
                            responseModel.ResponseData = data;
                            responseModel.ErrorCode = "";
                            responseModel.Message = "";
                            responseModel.Status = true;
                            if (responseModel.Status)
                            {
                                string subject = $"ARAS Password Change for {user.FirstName}";
                                string body = MailUtility.GetChangePasswardEmailBody(user.FirstName, email);
                                if (_configuration["MailEnable"] != "Y")
                                {
                                    user.Email = "roshan.sahu@roshan.com";
                                }
                                MailUtility.SendEmail(user.Email, subject, body);
                            }
                        }
                        else
                        {
                            responseModel.ResponseData = null;
                            responseModel.ErrorCode = "";
                            responseModel.Message = $"User not exist.";
                            responseModel.Status = false;
                        }

                    }
                    else
                    {
                        responseModel.ResponseData = null;
                        responseModel.ErrorCode = "";
                        responseModel.Message = $"Reset password link had expire or already used.";
                        responseModel.Status = false;
                    }

                }
                else
                {
                    responseModel.ResponseData = null;
                    responseModel.ErrorCode = "";
                    responseModel.Message = $"Not a valid reset password link.";
                    responseModel.Status = false;
                }
            }
            else
            {
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = $"Not a valid reset password link.";
                responseModel.Status = false;
            }
            return responseModel;
        }
        public async Task<ApiResult<GetAllRoleByAccessResponseModel>> GetAllRoleByAccess(GetAllRoleByAccessRequestModel requestModel)
        {
            ApiResult<GetAllRoleByAccessResponseModel> responseModel = new ApiResult<GetAllRoleByAccessResponseModel>();
            GetAllRoleByAccessResponseModel data = new GetAllRoleByAccessResponseModel();
            List<RoleModel> roleModels = new List<RoleModel>();

            try
            {
                string userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
                string email = _httpContextAccessor.HttpContext?.User?.FindFirst("Email")?.Value;
                if (userIdStr != null && email != null)
                {
                    long userId = long.Parse(userIdStr);
                    Project project = await _userRepository.ProjectByKey(requestModel.ProjectKey);
                    UserRole userRole = await _userRepository.GetUserRolesByUserAndProjectId(userId, project.ProjectId);

                    if (userRole != null)
                    {
                        List<Role> roles = await _userRepository.GetAllRolesLessID(userRole.RoleId);

                        foreach (Role role in roles)
                        {
                            RoleModel roleModel = new RoleModel();
                            roleModel.RoleId = role.RoleId;
                            roleModel.RoleName = role.RoleName;

                            roleModels.Add(roleModel);
                        }
                        data.Roles = roleModels;
                        responseModel.ResponseData = data;
                        responseModel.ErrorCode = "";
                        responseModel.Message = "";
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.ResponseData = null;
                        responseModel.ErrorCode = "";
                        responseModel.Message = $"User Does not have permission to access the {requestModel.ProjectKey} dashboard";
                        responseModel.Status = false;
                    }
                }
                else
                {
                    responseModel.ResponseData = null;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "UserId or Email not found in header";
                    responseModel.Status = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong.";
                responseModel.Status = false;
            }
            return responseModel;
        }
        public async Task<ApiResult<GetAllUserResponseModel>> GetAllUser(GetAllUserRequestModel requestModel)
        {
            ApiResult<GetAllUserResponseModel> responseModel = new ApiResult<GetAllUserResponseModel>();
            GetAllUserResponseModel data = new GetAllUserResponseModel();
            List<GetAllUserModel> userIdNames = new List<GetAllUserModel>();

            try
            {
                string userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
                if (userIdStr != null)
                {
                    long userId = long.Parse(userIdStr);
                    Project project = await _userRepository.ProjectByKey(requestModel.ProjectKey);
                    UserRole userRole = await _userRepository.GetUserRolesByUserAndProjectId(userId, project.ProjectId);


                    if (userRole != null && userRole.RoleId < 104)
                    {
                        bool isAdmin = false;
                        if(userRole.RoleId == 101)
                        {
                            isAdmin = true;
                        }

                        List<UserIdNameModel> userIdNameModels = await _userRepository.GetUserIdName(isAdmin);

                        foreach (UserIdNameModel userIdNameModel in userIdNameModels)
                        {
                            if(userIdNameModel.UserId != userId)
                            {
                                GetAllUserModel userIdName = new GetAllUserModel();
                                userIdName.UserId = userIdNameModel.UserId;
                                userIdName.UserName = char.ToUpper(userIdNameModel.FirstName[0]) + userIdNameModel.FirstName.Substring(1) +
                                    " " + char.ToUpper(userIdNameModel.LastName[0]) + userIdNameModel.LastName.Substring(1);

                                userIdNames.Add(userIdName);
                            }
                        }
                        data.userIdNames = userIdNames;
                        responseModel.ResponseData = data;
                        responseModel.ErrorCode = "";
                        responseModel.Message = "";
                        responseModel.Status = true;
                    }
                    else
                    {
                        responseModel.ResponseData = null;
                        responseModel.ErrorCode = "";
                        responseModel.Message = $"User Does not have permission to access this.";
                        responseModel.Status = false;
                    }
                }
                else
                {
                    responseModel.ResponseData = null;
                    responseModel.ErrorCode = "";
                    responseModel.Message = "UserId or Email not found in header";
                    responseModel.Status = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong.";
                responseModel.Status = false;
            }
            return responseModel;
        }
        public async Task<ApiResult<GetProjectAccessByIdResponseModel>> GetProjectAccessById(GetProjectAccessByIdRequestModel requestModel)
        {
            ApiResult<GetProjectAccessByIdResponseModel> responseModel = new ApiResult<GetProjectAccessByIdResponseModel>();
            GetProjectAccessByIdResponseModel data = new GetProjectAccessByIdResponseModel();
            List<ProjectAndRoleModel> projectAndRoles = new List<ProjectAndRoleModel>();

            try
            {
                List<UserRole> userRoles = await _userRepository.GetUserRolesByUserId(requestModel.UserId);
                foreach (UserRole userRole in userRoles)
                {
                    ProjectAndRoleModel projectAndRole = new ProjectAndRoleModel();
                    projectAndRole.ProjectId = userRole.ProjectId;
                    projectAndRole.RoleId = userRole.RoleId;

                    projectAndRoles.Add(projectAndRole);
                }
                data.ProjectAndRoles = projectAndRoles;
                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong.";
                responseModel.Status = false;
            }
            return responseModel;
        }
        public async Task<ApiResult<AlterUserAccessResponseModel>> AlterUserAccess(AlterUserAccessRequestModel requestModel)
        {
            ApiResult<AlterUserAccessResponseModel> responseModel = new ApiResult<AlterUserAccessResponseModel>();
            AlterUserAccessResponseModel data = new AlterUserAccessResponseModel();
            data.Status = false;      

            try
            {
                User user = null;
                foreach (UserAccessModel userAccessModel in requestModel.UserAccess)
                {
                    UserRole userRole = await _userRepository.GetUserRolesByUserAndProjectId(requestModel.UserId, userAccessModel.ProjectId);
                    if (userAccessModel.AcessHave)
                    {
                        if (userRole != null)
                        {
                            userRole.RoleId = userAccessModel.RoleId;
                            await _userRepository.UpdateUserRole(userRole);
                        }
                        else
                        {
                            UserRole userRole1 = new UserRole();
                            userRole1.UserId = requestModel.UserId;
                            userRole1.ProjectId = userAccessModel.ProjectId;
                            userRole1.RoleId = userAccessModel.RoleId;
                            await _userRepository.AddUserRole(userRole1);


                            ResourceName resourceName = await _userRepository.GetResourceNameByUserId(requestModel.UserId, userAccessModel.ProjectId);
                            if (resourceName == null)
                            {
                                string addedUserId = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;

                                if(user == null)
                                {
                                    user = await _userRepository.GetUserById(requestModel.UserId);
                                }
                                ResourceName resourceName1 = new ResourceName();
                                resourceName1.UserName = user.Username;
                                resourceName1.UserId = requestModel.UserId;
                                resourceName1.Name = user.FirstName;
                                resourceName1.LastName = user.LastName;
                                resourceName1.ProjectId = userAccessModel.ProjectId;
                                if (addedUserId != null)
                                {
                                    resourceName1.AddedByUserId = long.Parse(addedUserId);
                                }

                                await _userRepository.AddResourceName(resourceName1);
                            }
                            
                        }
                    }
                    else
                    {
                        if (userRole != null)
                        {
                            await _userRepository.DeleteUserRole(userRole.UserRoleId);  
                        }
                    }
                }
                data.Status = true;
                responseModel.ResponseData = data;
                responseModel.ErrorCode = "";
                responseModel.Message = "";
                responseModel.Status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                responseModel.ResponseData = null;
                responseModel.ErrorCode = "";
                responseModel.Message = "Something went wrong.";
                responseModel.Status = false;
            }
            return responseModel;
        }
        private static (string hash, string salt) HashPassword(string password)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(16);
            string salt = Convert.ToBase64String(saltBytes);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100000, HashAlgorithmName.SHA256);
            byte[] hashBytes = pbkdf2.GetBytes(32);
            string hash = Convert.ToBase64String(hashBytes);

            return (hash, salt);
        }
        private static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            using var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 100000, HashAlgorithmName.SHA256);
            byte[] hashBytes = pbkdf2.GetBytes(32);
            string computedHash = Convert.ToBase64String(hashBytes);

            return computedHash == storedHash;
        }

    }
}
