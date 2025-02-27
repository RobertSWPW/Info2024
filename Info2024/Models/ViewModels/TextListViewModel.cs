﻿namespace Info2024.Models.ViewModels
{
	public class TextListViewModel
	{
		public TextListViewModel(int pageSize = 5)
		{
			PageSize = pageSize;
		}
		public int TextCount { get; set; }
		public int PageSize { get; set; }
		public int PageNumber { get; set; }
		public int PageCount => (int)Math.Ceiling((decimal)TextCount / PageSize);
		public int? Category { get; set; }
		public string? Author { get; set; }
		public string? Phrase { get; set; }
		public string? Keyword { get; set; }
	}
}
