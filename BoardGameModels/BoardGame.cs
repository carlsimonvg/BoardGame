using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace BoardGameModels
{
    public class BoardGame
    { 
        public ItemType Type { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
		public string? Name { get; set; }
        public string? Description { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public string? ImageUrl { get; set; }
        public int YearPublished { get; set; }
        public int PlayTime { get; set; }
        public float Rating { get; set; }
        public float Weight { get; set; }
        public int VideoTotal { get; set; }
    }

    public enum ItemType
    {
        [Display(Name = "Board Game")]
        [XmlEnum("boardgame")]
        BoardGame,

        [Display(Name = "Board Game Expansion")]
        [XmlEnum("boardgameexpansion")]
        BoardGameExpansion
    }
}