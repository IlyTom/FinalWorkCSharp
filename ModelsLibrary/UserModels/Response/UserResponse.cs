using ModelsLibrary.UserModels.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLibrary.UserModels.Response
{
    public class UserResponse
    {
        public bool IsSuccess { get; set; }

        public List<ErrorResponse> Errors = new List<ErrorResponse>();
        public List<UserModel> Users = new List<UserModel>();
        public Guid? UserId { get; set; }

        public static UserResponse AddAdminError()
        {
            return new UserResponse
            {
                IsSuccess = false,
                Errors = new List<ErrorResponse> {
                    new ErrorResponse
                    {
                        Message = "The administrator already exists",
                        StatusCode = 409
                    } }
            };
        }

        public static UserResponse UserNotFound()
        {
            return new UserResponse
            {
                IsSuccess = false,
                Errors = new List<ErrorResponse> {
                    new ErrorResponse
                    {
                        Message = "User not found",
                        StatusCode = 404
                    }
                }
            };
        }

        public static UserResponse UserExist()
        {
            return new UserResponse
            {
                IsSuccess = false,
                Errors = new List<ErrorResponse> {
                    new ErrorResponse
                    {
                        Message = "The user already exists",
                        StatusCode = 409
                    }
                }
            };
        }

        public static UserResponse PasswordWrong()
        {
            return new UserResponse
            {
                IsSuccess = false,
                Errors = new List<ErrorResponse> {
                    new ErrorResponse
                    {
                        Message = " Incorrect password",
                    }
                }
            };
        }

        public static UserResponse AccessDenied()
        {
            return new UserResponse
            {
                IsSuccess = false,
                Errors = new List<ErrorResponse> {
                    new ErrorResponse
                    {
                        Message = "Access denied",
                    }
                }
            };
        }

        public static UserResponse OK()
        {
            return new UserResponse
            {
                IsSuccess = true,
            };
        }
    }
}

