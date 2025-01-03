using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Info2024.Infrastructure.TagHelpers
{
	[HtmlTargetElement("star-rating")]
	public class RatingTagHelper : TagHelper
	{
		public int RatingCount { get; set; }
		public double? RatingAvg { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "span";
			output.TagMode = TagMode.StartTagAndEndTag;
			output.Attributes.SetAttribute("class", "text-warning");

			var rating = RatingAvg ?? 0;
			var stars = GenerateStars(rating);
			output.PreContent.SetHtmlContent(stars);
		}

		private string GenerateStars(double rating)
		{
			return rating switch
			{
				0 => EmptyStar(5),
				<= 5 when RatingCount == 1 => FullStar((int)rating) + EmptyStar(5 - (int)rating),
				<= 5 => GenerateRatingStars(rating),
				_ => throw new ArgumentException("Rating must be between 0 and 5")
			};
		}

		private string GenerateRatingStars(double rating)
		{
			var roundedRating = Math.Round(rating * 2) / 2; // zaokrągli do najbliższej 0.5
			var fullStars = (int)Math.Floor(roundedRating);
			var hasHalfStar = (roundedRating % 1) != 0;
			var emptyStars = 5 - fullStars - (hasHalfStar ? 1 : 0);

			return FullStar(fullStars) +
						 (hasHalfStar ? HalfStar() : "") +
						 EmptyStar(emptyStars);
		}

		private static string EmptyStar(int count)
		{
			return string.Concat(Enumerable.Repeat("<i class=\"bi-star me-1\"></i>", count));
		}

		private static string HalfStar()
		{
			return "<i class=\"bi-star-half me-1\"></i>";
		}

		private static string FullStar(int count)
		{
			return string.Concat(Enumerable.Repeat("<i class=\"bi-star-fill me-1\"></i>", count));
		}

	}
}
