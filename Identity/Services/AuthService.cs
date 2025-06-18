using Identity.Models;
using Identity.Models.Request;
using Identity.Services.Abstractions;
using Identity.Tools;
using IdentityDatabase.Models;

namespace Identity.Services
{
    public class AuthService : IAuthService
    {
        //private readonly IDataRepository _dataRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ILogger<AuthService> logger)
        {
            //_dataRepository = dataRepository;
            _logger = logger;
        }

        public async Task<AuthResult> RegisterAsync(RegisterRequest request)
        {
            // Проверка существующего пользователя
            var existingUser = await _dataRepository.GetUserByUsernameAsync(request.Username);
            if (existingUser != null)
                return AuthResult.FailedResult("Пользователь с таким именем уже существует");
            
            var existingEmail = await _dataRepository.GetUserByEmailAsync(request.Email);
            if (existingEmail != null)
                return AuthResult.FailedResult("Пользователь с таким email уже существует");

            // Создание пользователя с хэшированным паролем
            var salt = HashingHelper.GenerateSalt();
            var hashedPassword = HashingHelper.HashPassword(request.Password, salt);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = hashedPassword,
                Salt = salt,
                IsLocked = false,
                FailedLoginAttempts = 0
            };

            await _dataRepository.CreateUserAsync(user);
            _logger.LogInformation($"Пользователь {request.Username} успешно зарегистрирован");

            return AuthResult.SuccessResult("Пользователь успешно зарегистрирован");
        }

        public async Task<AuthResult> LoginAsync(LoginRequest request)
        {
            var user = await _dataRepository.GetUserByUsernameAsync(request.Username);
            
            if (user == null)
                return AuthResult.FailedResult("Неверные учетные данные");
            
            if (user.IsLocked)
                return AuthResult.FailedResult("Аккаунт заблокирован");

             Проверка пароля
            if (!HashingHelper.VerifyPassword(request.Password, user.PasswordHash, user.Salt))
            {
                // Увеличиваем счетчик неудачных попыток
                user.FailedLoginAttempts++;
                if (user.FailedLoginAttempts >= 5)
                {
                    user.IsLocked = true;
                    _logger.LogWarning($"Аккаунт {user.Username} заблокирован после 5 неудачных попыток входа");
                }
                await _dataRepository.UpdateUserAsync(user);
                return AuthResult.FailedResult("Неверные учетные данные");
            }
            
            // Успешный вход
            user.FailedLoginAttempts = 0;
            user.LastLoginDate = DateTime.UtcNow;
            await _dataRepository.UpdateUserAsync(user);
            
            _logger.LogInformation($"Пользователь {user.Username} успешно вошел в систему");
            return AuthResult.SuccessResult("Успешный вход", user);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _dataRepository.GetUserByIdAsync(userId);
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(int userId)
        {
            return await _dataRepository.GetUserRolesAsync(userId);
        }

        public async Task<IEnumerable<string>> GetUserPermissionsAsync(int userId)
        {
            return await _dataRepository.GetUserPermissionsAsync(userId);
        }
    }
}
