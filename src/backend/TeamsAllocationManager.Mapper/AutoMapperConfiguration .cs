using AutoMapper;
using TeamsAllocationManager.Mapper.Profiles;

namespace TeamsAllocationManager.Mapper;

public static class AutoMapperConfiguration
{
	public static IMapper Build()
	{
		var mappingConfig = new MapperConfiguration(mc =>
		{
			mc.AddProfile(new EntityDtoProfile());
			mc.AddProfile(new FutureDatabaseEntityProfile());
		});

		return mappingConfig.CreateMapper();
	}
}
