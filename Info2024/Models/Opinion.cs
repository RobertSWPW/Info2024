using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Info2024.Models
{
	public class Opinion
	{
		public Opinion()
		{
			Comment = string.Empty;
			AddedDate = DateTime.Now;
			Id = string.Empty;
		}

		public int OpinionId { get; set; }

		[Required(ErrorMessage = "Proszę podać treść komentarza.")]
		[Display(Name = "Treść komentarza:")]
		[DataType(DataType.MultilineText)]
		[MinLength(3, ErrorMessage = "Komentarz musi mieć co najmniej 3 znaki")]
		[MaxLength(1000, ErrorMessage = "Komentarz nie może przekraczać 1000 znaków")]
		public string Comment { get; set; }

		[Required]
		[Display(Name = "Data dodania:")]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
		public DateTime AddedDate { get; set; }

		[Required(ErrorMessage = "Proszę wybrać ocenę")]
		[Display(Name = "Ocena tekstu:")]
		[Range(1, 5, ErrorMessage = "Proszę wybrać ocenę od 1 do 5")]
		public Rating Rating { get; set; }

		[Required]
		[Display(Name = "Komentowany tekst:")]
		public int TextId { get; set; }
		//Komentowany tekst
		[ForeignKey("TextId")]
		public Text? Text { get; set; }

		[Required]
		[Display(Name = "Autor komentarza:")]
		public string Id { get; set; }
		//Autor komentarza
		[ForeignKey("Id")]
		public AppUser? User { get; set; }
	}

	public enum Rating
	{
		[Display(Name = "Nieprzydatny")]
		Useless = 1,

		[Display(Name = "Słaby")]
		Poor = 2,

		[Display(Name = "Przeciętny")]
		Average = 3,

		[Display(Name = "Dobry")]
		Good = 4,

		[Display(Name = "Świetny")]
		Excellent = 5
	}
}
