using AutoMapper;
using System.Security.Principal;

using System.Xml.Linq;
using System.Security.Cryptography;
using System.Text;
using ModelsLibrary.UserModels.Entity;
using ModelsLibrary.UserModels.DTO;
using ModelsLibrary.UserModels;

namespace UserApi.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<UserEntity, UserModel>().ConvertUsing(new EntityToModelConverter());
            CreateMap<UserEntity, UserAccount>(MemberList.Destination);
            CreateMap<RegistrationModel, UserEntity>().ConvertUsing(new RegisterEntityConverter());
        }

        private class EntityToModelConverter : ITypeConverter<UserEntity, UserModel>
        {
            public UserModel Convert(UserEntity source, UserModel target, ResolutionContext context)
            {
                return new UserModel
                {
                    Id = source.Id,
                    Email = source.Email,
                    Name = source.Name,
                    Role = source.RoleType.Role,
                    Surname = source.Surname,

                };
            }
        }

        private class RegisterEntityConverter : ITypeConverter<RegistrationModel, UserEntity> 
        { 
            public UserEntity Convert(RegistrationModel source, UserEntity destination, ResolutionContext context)
            {
                var entity = new UserEntity
                {
                    Id = Guid.NewGuid(),
                    Email = source.Email.ToLower(),
                    Name = source.Name,
                    Surname = source.Surname,
                };
                entity.Salt = new byte[16];
                new Random().NextBytes(entity.Salt);
                var data = Encoding.ASCII.GetBytes(source.Password).Concat(entity.Salt).ToArray();
                SHA512 shaM = new SHA512Managed();
                entity.Password = shaM.ComputeHash(data);

                return entity;
            }
        }


    }
}
