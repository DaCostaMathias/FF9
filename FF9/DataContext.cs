using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FF9
{
	class DataContext
	{
		public RootObject Json { get; set; }
		public AppInfo Info { get; private set; } = AppInfo.Info;

		public DataContext() { }
		public DataContext(String filename)
		{
			if (!System.IO.File.Exists(filename)) return;

			String text = System.IO.File.ReadAllText(filename);
			Json = JsonConvert.DeserializeObject<RootObject>(text);
		}

		public void Save(String filename)
		{
			if (Json == null) return;

			String text = JsonConvert.SerializeObject(Json);
			text = text.Replace(",", ", ");
			text = text.Replace("[{", "[ {");
			text = text.Replace("}]", "} ]");
			text = text.Replace("[\"", "[ \"");
			text = text.Replace("\"]", "\" ]");
			System.IO.File.WriteAllText(filename, text);
		}

		public List<MiniGameCard> GetCardsSection()
		{
			if (Json?.Data?.__invalid_name__30000_MiniGame == null) return new List<MiniGameCard>();

			return Json.Data.__invalid_name__30000_MiniGame.MiniGameCard;
		}

		public bool SetCardsSection(List<MiniGameCard> cards)
		{
			if (Json?.Data?.__invalid_name__30000_MiniGame == null) return false;
			Json.Data.__invalid_name__30000_MiniGame.MiniGameCard = cards;
			return true;

		}
	}
}
