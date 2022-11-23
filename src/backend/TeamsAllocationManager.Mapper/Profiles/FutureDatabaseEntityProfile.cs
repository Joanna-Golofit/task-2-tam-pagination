using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using TeamsAllocationManager.Domain.Enums;
using TeamsAllocationManager.Domain.Models;
using TeamsAllocationManager.Integrations.FutureDatabase.Enums;
using TeamsAllocationManager.Integrations.FutureDatabase.Models;

namespace TeamsAllocationManager.Mapper.Profiles;

public class FutureDatabaseEntityProfile : Profile
{
	public FutureDatabaseEntityProfile()
	{
		CreateMap<User, EmployeeEntity>()
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName))
			.ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.LastName))
			.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
			.ForMember(dest => dest.UserLogin, opt => opt.MapFrom(src => src.DomainUserLogin))
			.ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id))
			.ForAllOtherMembers(opt => opt.Ignore());

		CreateMap<Group, ProjectEntity>()
			.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.ToDate))
			.ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id))
			.ForAllOtherMembers(opt => opt.Ignore());
	}
}
