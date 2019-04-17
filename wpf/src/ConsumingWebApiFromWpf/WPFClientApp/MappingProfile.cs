namespace WPFClientApp
{
	using AutoMapper;
	using WPFClientApp.Dtos;
	using WPFClientApp.Models;

	class MappingProfile : Profile
	{
		public MappingProfile()
		{
			Mapper.CreateMap<Product, ProductModel>();
			Mapper.CreateMap<ProductModel, Product>();
		}
	}
}
