namespace Vidly.App_Start
{
	using AutoMapper;
	using Vidly.Dtos;
	using Vidly.Models;

	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			Mapper.CreateMap<Customer, CustomerDto>();
			Mapper.CreateMap<CustomerDto, Customer>();
		}
	}
}