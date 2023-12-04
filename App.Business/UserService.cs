using App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using App.Data;
using App.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Business
{
    public interface IUserService
    {
        Task<ServiceResult<UserDto>> GetUserByIdAsync(int id);
        Task<ServiceResult<UserDto>> GetUserByUsernameAsync(string username);
        Task<ServiceResult> CreateUser(string username, string password,string name,string surname, string address);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<UserService> _logger;
        private readonly IHashService _hashService;

        public UserService(AppDbContext dbContext,
            ILogger<UserService> logger,
            IHashService hashService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _hashService = hashService;
        }

        public async Task<ServiceResult> CreateUser(string username, string password,string name,string surname, string address)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return ServiceResult.Fail("Username and password could not be empty or white space.", StatusCodes.Status400BadRequest);
                }
                if (await _dbContext.Users.AnyAsync(x => x.Username == username))
                {
                    return ServiceResult.Fail("User exists", StatusCodes.Status409Conflict);
                }

                var hashingResult = _hashService.HashString(password);
                if (!hashingResult.IsSuccess)
                {
                    return hashingResult;
                }

                var userEntity = new UserEntity
                {
                    Username = username,
                    PasswordHash = hashingResult.Data,
                    Name = name,
                    SurName = surname,
                    Address = address,
                    CreatedAt = DateTime.Now
                   
                    
                };

                _dbContext.Users.Add(userEntity);
                await _dbContext.SaveChangesAsync();

                return ServiceResult.Success(userEntity, StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return ServiceResult.Fail("Error creating user", StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ServiceResult<UserDto>> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _dbContext.Users
                    .Select(x => new UserDto
                    {
                        Id = x.Id,
                        Username = x.Username,
                        PasswordHash = x.PasswordHash
                    })
                    .SingleOrDefaultAsync(x => x.Id == id);
                if (user is null)
                {
                    return ServiceResult.Fail<UserDto>("User not found", StatusCodes.Status404NotFound);
                }

                return ServiceResult.Success(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by id");
                return ServiceResult.Fail<UserDto>("Error getting user by id", StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ServiceResult<UserDto>> GetUserByUsernameAsync(string username)
        {
            try
            {
                var user = await _dbContext.Users
                    .Select(x => new UserDto
                    {
                        Id = x.Id,
                        Username = x.Username,
                        PasswordHash = x.PasswordHash
                    }).SingleOrDefaultAsync(x => x.Username == username);
                if (user is null)
                {
                    return ServiceResult.Fail<UserDto>("User not found", StatusCodes.Status404NotFound);
                }

                return ServiceResult.Success(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by username");
                return ServiceResult.Fail<UserDto>("Error getting user by username", StatusCodes.Status500InternalServerError);
            }
        }
    }
}
