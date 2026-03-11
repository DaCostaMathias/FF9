using System.Collections.Generic;
using System.Linq;

namespace FF9.Card
{
	public static class CardCollectionFactory
	{
		public static List<MiniGameCard> CreatePerfectCollection()
		{
			const string perfectType = "3"; // Unverified save-file encoding; likely the highest internal card type value.
			const string perfectSide = "0"; // Unverified; public Tetra Master docs do not clearly describe this save field.
			const string perfectStat = "255"; // Public Tetra Master docs describe visible card stats as hex digits, so 0xF is the max.
			const string perfectPoints = "0"; // Unverified save-file encoding; chosen as the maximum byte value.
			const string perfectArrows = "255"; // Likely correct because 0xFF enables all eight arrow bits.

			return AppInfo.Info.Cards
				.Where(card => !string.IsNullOrWhiteSpace(card.Value))
				.OrderBy(card => card.Key)
				.Select(card => new MiniGameCard
				{
					id = card.Key.ToString(),
					type = perfectType,
					side = perfectSide,
					atk = perfectStat,
					pdef = perfectStat,
					mdef = perfectStat,
					cpoint = perfectPoints,
					arrow = perfectArrows
				})
				.ToList();
		}
	}
}
