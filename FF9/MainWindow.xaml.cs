using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FF9.Card;
using FF9.PartyInventory;
using Microsoft.Win32;

namespace FF9
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
    [ExcludeFromCodeCoverage]
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void MenuItemFileOpen_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new OpenFileDialog();
			if (dlg.ShowDialog() == false) return;
			DataContext = new DataContext(dlg.FileName);
		}

		private void MenuItemFileSave_Click(object sender, RoutedEventArgs e)
		{
			var dlg = new SaveFileDialog();
			if (dlg.ShowDialog() == false) return;
			(DataContext as DataContext)?.Save(dlg.FileName);
		}

		private void MenuItemAbout_Click(object sender, RoutedEventArgs e)
		{
			new AboutWindow().ShowDialog();
		}

		private void ButtonItemChoice_Click(object sender, RoutedEventArgs e)
		{
			Item item = (sender as Button)?.DataContext as Item;

			ChoiceWindow window = new ChoiceWindow();
			window.ID = Convert.ToUInt32(item.id);
			window.ShowDialog();
			item.id = window.ID.ToString();
		}

		private void ButtonCardChoice_Click(object sender, RoutedEventArgs e)
		{
			MiniGameCard card = (sender as Button)?.DataContext as MiniGameCard;

			ChoiceWindow window = new ChoiceWindow();
			window.Type = ChoiceWindow.eType.eCard;
			window.ID = Convert.ToUInt32(card.id);
			window.ShowDialog();
			card.id = window.ID.ToString();
		}

		private void ButtonGivePerfectCards_Click(object sender, RoutedEventArgs e)
		{
			FF9.DataContext context = DataContext as FF9.DataContext;			

			var cards = context.GetCardsSection();
			
			if (cards != null)
			{
				cards.Clear();
			}

			cards.AddRange(CardCollectionFactory.CreatePerfectCollection());
			context.SetCardsSection(cards);
			CollectionViewSource.GetDefaultView(cards)?.Refresh();
			
			base.DataContext = null;
			base.DataContext = context;
		}

		private void ButtonAbilityChoice_Click(object sender, RoutedEventArgs e)
		{

		}
		
		private void ButtonGiveAllItem_Click(object sender, RoutedEventArgs e)
		{
            FF9.DataContext context = DataContext as FF9.DataContext;
			var items = context.GetItemsSection();
            if ( items != null)
            {
				items.Clear();
            }
			items.AddRange(ItemFactory.CreateFullInventory());
			context.SetItemsSection(items);

			DataContext = null;
			DataContext = context;
        }
	}
}
