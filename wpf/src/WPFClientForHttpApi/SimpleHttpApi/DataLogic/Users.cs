namespace SimpleHttpApi.DataLogic
{
	using Models;
	using SimpleHttpApi.Extensions;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public static class Users
	{
		public static string LibraryName => "users";

		private static IEnumerable<User> _users = new List<User>()
		{
			new User(1, "User 1", new DateTime(2010, 1, 1), 1.23M, true),
			new User(2, "User 2", new DateTime(2013, 12, 31), 2.34M, false),
			new User(3, "User 3", new DateTime(2018, 3, 18), 4.56789M, true),
		};

		public static DataListModel<User> GetUsers()
		{
			MethodBase method = MethodBase.GetCurrentMethod();
			return new DataListModel<User>(method.DeclaringType.Name, method.Name, _users);
		}

		public static DataListModel<User> GetUser(string parameter)
		{
			var user = GetUserById(parameter);
			if (user != null)
			{
				List<User> result = new List<User>();
				result.Add(user);

				return MakeDataArray<User>(MethodBase.GetCurrentMethod(), result);
			}

			return null;
		}

		public static DataListModel<DataResultModel<object>> IsPowerUser(string parameter)
		{
			bool result = false;

			var user = GetUserById(parameter);
			if (user != null)
			{
				result = user.IsPower;
			}

			return MakeDataResult<object>(MethodBase.GetCurrentMethod(), result);
		}

		public static DataListModel<DataResultModel<object>> GetUserProperty(string parameter, string property)
		{
			object result = null;

			var user = GetUserById(parameter);
			if (user != null)
			{
				result = user.GetPropertyValue(property);
			}
			else
			{
				throw new ArgumentException($"User not found. Parameter = '{parameter}'");
			}

			return MakeDataResult(MethodBase.GetCurrentMethod(), result);
		}

		#region Helpers

		private static DataListModel<DataResultModel<T>> MakeDataResult<T>(string library, string method, T result)
		{
			List<DataResultModel<T>> output = new List<DataResultModel<T>>();
			output.Add(new DataResultModel<T>(result));
			return new DataListModel<DataResultModel<T>>(library, method, output);
		}

		private static DataListModel<T> MakeDataArray<T>(string library, string method, List<T> result) =>
			new DataListModel<T>(library, method, result);

		private static DataListModel<DataResultModel<T>> MakeDataResult<T>(MethodBase method, T result) =>
			MakeDataResult<T>(method.DeclaringType.Name, method.Name, result);

		private static DataListModel<T> MakeDataArray<T>(MethodBase method, List<T> result) =>
			MakeDataArray<T>(method.DeclaringType.Name, method.Name, result);

		private static User GetUserById(string parameter)
		{
			User result = null;

			int id = -1;
			Int32.TryParse(parameter, out id);

			if (id > 0)
			{
				if (_users.Any(u => u.Id == id))
				{
					result = _users.Where(u => u.Id == id).FirstOrDefault();
				}
			}

			return result;
		}

		#endregion //Helpers
	}
}