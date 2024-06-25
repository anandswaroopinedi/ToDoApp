using AutoMapper;
using Azure;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private IConfiguration _config;
        private IUnitOfWork _unitOfWork;
        public UserManager(IUserRepository userRepository, IMapper mapper, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _config = configuration;
            _unitOfWork = unitOfWork;
        }
        public async Task<int> AddUser(UserDto user)
        {
            _unitOfWork.BeginTransaction();
            user.Password = PasswordHashing.HashPassword(user.Password);
            User result = await _unitOfWork.UserRepository.GetByUsername(user.UserName);
            if (result != null)
            {
                return 3;
            }
            else
            {
                _unitOfWork.UserRepository.AddUser(_mapper.Map<User>(user));
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
                return 1;
            }
        }
        public async Task<int> AuthenticateUser(UserDto user)
        {
            user.Password = PasswordHashing.HashPassword(user.Password);
            User result = await _unitOfWork.UserRepository.AuthenticateUser(_mapper.Map<User>(user));
            if (result != null)
            {
                if (user.Password.Equals(result.Password, StringComparison.OrdinalIgnoreCase))
                {
                    return 1;
                }
                else
                {
                    return 3;
                }
            }
            else
            {
                return 4;
            }
        }
        /*public async Task<ApiResponse> AddUser(UserDto user)
        {
            user.Password=PasswordHashing.HashPassword(user.Password);
            return await _userRepository.AddUser(_mapper.Map<User>(user));
        }
        public async Task<ApiResponse> AuthenticateUser(UserDto user)
        {
            user.Password = PasswordHashing.HashPassword(user.Password);
            ApiResponse response = await _userRepository.AuthenticateUser(_mapper.Map<User>(user));
            if(response.StatusCode == 200 && (int)response.result>0)
            {
                response.result = GenerateToken((int)response.result);
            }
            return response;
        }*/
        public async Task<string> GenerateToken(int id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var tokenDescdriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
                ,
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim("Id",id.ToString())
                })
            };
            var token = new JwtSecurityTokenHandler().CreateToken(tokenDescdriptor);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
