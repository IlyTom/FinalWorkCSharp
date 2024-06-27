using AutoMapper;
using Microsoft.EntityFrameworkCore;

using ModelsLibrary.UserModels;
using ModelsLibrary.UserModels.DTO;
using ModelsLibrary.UserModels.Entity;
using ModelsLibrary.UserModels.Response;
using System.Security.Cryptography;
using System.Text;
using UserApi.Abstraction;
using UserApi.Context;

namespace UserApi.Services
{
    public class UserService : IUserService
    {
        private readonly ContextApp _context;
        private readonly IMapper _mapper;
        private readonly UserAccount _account;

        public UserService (ContextApp context, IMapper mapper, UserAccount account)
        {
            _context = context;
            _mapper = mapper;
            _account = account;
        }

        public UserResponse AddAdmin(RegistrationModel model)
        {
            var response = UserResponse.OK();
            using (_context) 
            {
                var admin = _context.Users.Count(x => x.RoleType.Role == UserRole.Administrator);

                if (admin > 0) return UserResponse.AddAdminError();

                var entity = _mapper.Map<UserEntity>(model);

                entity.RoleType = new RoleEntity
                {
                    Role = UserRole.Administrator
                };
                
                _context.Users.Add(entity);
                _context.SaveChanges();
                response.UserId = entity.Id;
            }
            return response;
        }

        public UserResponse AddUser(RegistrationModel model)
        {
            var response = UserResponse.OK();
            using( _context)
            {
                var user = _context.Users.FirstOrDefault(x => x.Email == model.Email.ToLower());
                if (user != null) return UserResponse.UserExist();

                var entity = _mapper.Map<UserEntity>(model);
                entity.RoleType = new RoleEntity { Role = UserRole.User };

                _context.Users.Add(entity);
                _context.SaveChanges();

                response.UserId = entity.Id;
            }

            return response;
        }

        public UserResponse Authentificate(LoginModel model)
        {
            using (_context)
            {
                var user = _context.Users.Include(x => x.RoleType).FirstOrDefault(x => x.Email == model.Email);

                if (user == null) 
                {
                    return UserResponse.UserNotFound();
                }

                if (CheckPassword(user.Salt, model.Password, user.Password))
                {
                    var response = UserResponse.OK();
                    response.Users.Add(_mapper.Map<UserModel>(user));
                    return response;
                }

                return UserResponse.PasswordWrong();
            }
        }

        public UserResponse DeleteUser(Guid? userId, string? email)
        {
            if (_account.Role != UserRole.Administrator) return UserResponse.AccessDenied();

            using (_context)
            {
                var query = _context.Users.Include(x => x.RoleType).AsQueryable();

                if(!string.IsNullOrEmpty(email)) query = query.Where(x => x.Email == email);

                if(userId.HasValue) query = query.Where(x => x.Id == userId);

                var user = query.FirstOrDefault();

                if (user == null) return UserResponse.UserNotFound();

                if (user.RoleType.Role == UserRole.Administrator)
                {
                    return new UserResponse
                    {
                        IsSuccess = false,
                        Errors = new List<ErrorResponse> { new ErrorResponse { Message = "You can't delete an administrator" } }
                    };
                }

                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return UserResponse.OK();
        }

        public UserResponse GetUser(Guid? userId, string? email)
        {
            var user = new UserEntity();

            using (_context)
            {
                var query = _context.Users.Include(x => x.RoleType).AsQueryable();

                if (!string.IsNullOrEmpty(email)) query = query.Where(x => x.Email == email);

                if (userId.HasValue) query = query.Where(x => x.Id == userId);

                user = query.FirstOrDefault();               
            }

            if (user == null) return UserResponse.UserNotFound();

            if (_account.Role == UserRole.Administrator || _account.Id == userId)
            {
                return new UserResponse
                {
                    IsSuccess = true,
                    Users = new List<UserModel> { _mapper.Map<UserModel>(user) }
                };
            }

            return UserResponse.AccessDenied();
        }

        public UserResponse GetUsers()
        {
            var users = new List<UserModel>();

            if (_account.Role != UserRole.Administrator) return UserResponse.AccessDenied();

            using (_context)
            {
                users.AddRange(_context.Users.Include(x => x.RoleType).Select(x => _mapper.Map<UserModel>(x)).ToList());
            }

            return new UserResponse { IsSuccess = true, Users = users };
        }

        private bool CheckPassword(byte[] salt, string password, byte[] dbPassword)
        {
            var data = Encoding.ASCII.GetBytes(password).Concat(salt).ToArray();
            SHA512 shaM = new SHA512Managed();
            var pass = shaM.ComputeHash(data);

            return dbPassword.SequenceEqual(pass);
        }
    }
}
