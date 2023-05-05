using Demo.CosmoDB.Comman;
using Demo.CosmoDB.Comman.Abstraction;
using Demo.CosmoDB.Models;
using Demo.CosmoDB.Repository.Abstraction;
using Demo.CosmoDB.Services.Abstraction;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Demo.CosmoDB.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly ICacheService<User> _cacheService;
        private readonly IAppSettings _appSettings;
        public UserService(IUserRepository userRepository, ILogger<UserService> logger, ICacheService<User> cacheService, IOptions<AppSettings> appSettings)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _appSettings = appSettings.Value;
        }

        public Task AddAsync(User item)
        {
            try
            {
                _logger.LogInformation($"{nameof(UserService)}.{nameof(AddAsync)} started!");
                // item.Password = BCrypt.HashPassword(item.Password);
                return _userRepository.AddAsync(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UserService)}.{nameof(AddAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }

        public async Task DeleteAsync(string id)
        {
            try
            {
                _logger.LogInformation($"{nameof(UserService)}.{nameof(DeleteAsync)} started!");
                await _cacheService.RemoveAsync(id, async () =>
                {
                    await _userRepository.DeleteAsync(id);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UserService)}.{nameof(DeleteAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }

        public async Task<User> GetAsync(string id)
        {
            try
            {
                _logger.LogInformation($"{nameof(UserService)}.{nameof(GetAsync)} started!");
                return await _cacheService.GetOrAddAsync(id, async () =>
                {
                    return await _userRepository.GetAsync(id);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UserService)}.{nameof(GetAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }

        public async Task UpdateAsync(string id, User item)
        {
            try
            {
                _logger.LogInformation($"{nameof(UserService)}.{nameof(UpdateAsync)} started!");
                await _cacheService.RemoveAsync(id, async () =>
                {
                    await _userRepository.UpdateAsync(id, item);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UserService)}.{nameof(UpdateAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }
        public async Task<List<User>> GetAsync()
        {
            try
            {
                _logger.LogInformation($"{nameof(UserService)}.{nameof(GetAsync)} started!");
                return await _userRepository.GetAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{nameof(UserService)}.{nameof(GetAsync)} getting an error: {ex.Message}!");
                throw;
            }
        }
        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var users = await this.GetAsync();
            var user = users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }
        private string GenerateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
