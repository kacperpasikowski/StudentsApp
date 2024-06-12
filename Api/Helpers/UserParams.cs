using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace Api.Helpers
{
	public class UserParams
	{
		private const int MaxPageSize = 50;
		public int PageNumber { get; set; } = 1;
		private int _pageSize = 10;
		public int PageSize
		{
			get => _pageSize;
			set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
		}

		public string SortBy { get; set; }
		public string SortOrder { get; set; } = "asc"; 



	}
}