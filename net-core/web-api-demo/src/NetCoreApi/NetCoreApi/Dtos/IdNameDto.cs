namespace NetCoreApi.Dtos
{
	public class IdNameDto
	{
		public IdNameDto()
		{
		}

		public IdNameDto(int id, string name)
		{
			Id = id;
			Name = name;
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public static IdNameDto Create(int id, string name) => new IdNameDto(id, name);

		public override string ToString() => $"{{ Id = {Id}, Name = '{Name}'}}";
	}
}
