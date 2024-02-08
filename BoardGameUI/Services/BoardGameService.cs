using System.Globalization;
using System.Web;
using System.Xml.Linq;
using BoardGameModels;

namespace BoardGameUI.Services
{
    public class BoardGameService
    {
        private readonly HttpClient _httpClient;
        private const string _baseAddress = "https://boardgamegeek.com/xmlapi2/";
        public BoardGameService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BoardGame> GetBoardGameAsync(int id)
        {
            var data1 = await _httpClient.GetStreamAsync(_baseAddress + "/thing?id=" + id + "&stats=1");
            XDocument xmlDoc = XDocument.Load(data1);

            var itemElement = xmlDoc.Element("items")?.Element("item");

            if (itemElement != null)
            {
                BoardGame boardGame = new BoardGame
                {
                    Id = id,
                    Name = itemElement.Element("name")?.Attribute("value")?.Value,
                    Description = HttpUtility.HtmlDecode(itemElement.Element("description")?.Value),
                    Type = Enum.TryParse<ItemType>(itemElement.Attribute("type")?.Value, true, out var type) ? type : ItemType.BoardGame,
                    YearPublished = int.Parse(itemElement.Element("yearpublished")?.Attribute("value")?.Value ?? "0"),
                    MinPlayers = int.Parse(itemElement.Element("minplayers")?.Attribute("value")?.Value ?? "0"),
                    MaxPlayers = int.Parse(itemElement.Element("maxplayers")?.Attribute("value")?.Value ?? "0"),
                    ImageUrl = itemElement.Element("image")?.Value,
                    PlayTime = int.Parse(itemElement.Element("playingtime")?.Attribute("value")?.Value ?? "0"),
                    Rating = float.Parse(itemElement.Element("statistics")?.Element("ratings")?.Element("average")?.Attribute("value")?.Value ?? "0", NumberStyles.Float, CultureInfo.InvariantCulture),
                    Weight = float.Parse(itemElement.Element("statistics")?.Element("ratings")?.Element("averageweight")?.Attribute("value")?.Value ?? "0", NumberStyles.Float, CultureInfo.InvariantCulture),
                };

                return boardGame;
            }

            return new BoardGame();
        }

        public async Task<List<BoardGame>> SearchBoardGames(string search)
        {
            var data1 = await _httpClient.GetStreamAsync(_baseAddress + "/search?query=" + search + "&type=boardgame");
            XDocument xmlDoc = XDocument.Load(data1);

            List<BoardGame> boardGames = xmlDoc
                .Descendants("item")
                .Select(item => new BoardGame
                {
                    Id = int.TryParse(item.Attribute("id")?.Value, out var id) ? id : 0,
                    Type = Enum.TryParse<ItemType>(item.Attribute("type")?.Value, true, out var type) ? type : ItemType.BoardGame,
                    Name = item.Element("name")?.Attribute("value")?.Value,
                    YearPublished = int.TryParse(item.Element("yearpublished")?.Attribute("value")?.Value, out var year) ? year : 0
                })
                .ToList();

            return boardGames;
        }
    }
}