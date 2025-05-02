using TiendaOnline1.Application.Interfaces;
using TiendaOnline1.Core.Database;
using TiendaOnline1.Application.Dtos;
using AutoMapper;
using TiendaOnline1.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TiendaOnline1.Services{       
    public class UserService : IUserServiceDatabase
    {
        private readonly IMapper _mapper;
        private readonly MysqlDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public UserService(MysqlDbContext dbContext, IMapper mapper, IConfiguration configuration){
            _dbContext = dbContext;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new user in the database.
        /// </summary>
        /// <param name="userRequestRegisterDto">The user data to create.</param>
        /// <returns>True if the user is successfully created.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided user data is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the user could not be added to the database.</exception>
        public async Task<bool> CreateUserAsync(UserRequestRegisterDto userRequestRegisterDto)
        {
            if (userRequestRegisterDto == null)
            {
                throw new ArgumentNullException(nameof(userRequestRegisterDto), "The user data cannot be null.");
            }

            if (_dbContext.Users.Any(x => x.Email == userRequestRegisterDto.Email))
            {
                throw new InvalidOperationException("A user with this email already exists.");
            }

            var user = _mapper.Map<User>(userRequestRegisterDto);

            try
            {
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while creating the user.", ex);
            }

            return true;
        }

        
        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>True if the user exists, otherwise false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the provided ID is null or empty.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the user is not found in the database.</exception>
        public async Task<bool> GetUserByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id), "The user ID cannot be null or empty.");
            }

            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException($"No user found with the ID: {id}");
            }

            return true;
        }
        /// <summary>
        /// Generates a JSON Web Token (JWT) for a user based on their login information.
        /// </summary>
        /// <param name="userRequestLoginDto">The login information of the user, including their email.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the generated JWT as a string.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the user is not found in the database or when the JWT configuration is missing.
        /// </exception>
        /// <remarks>
        /// This method retrieves the user from the database using their email, generates a JWT with claims
        /// such as the user's email, role, and a unique identifier (JTI), and signs the token using the configured
        /// secret key. The token is valid for 30 minutes.
        /// </remarks>
        public Task<string> GenerateToken(UserRequestLoginDto userRequestLoginDto)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Email == userRequestLoginDto.Email) ?? throw new InvalidOperationException("User not found.");
            var Claims = new[]{
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: Claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Task.FromResult(tokenString);
        }
    }
}

