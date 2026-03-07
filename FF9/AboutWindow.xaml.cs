using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace FF9
{
    /// <summary>
    /// AboutWindow.xaml の相互作用ロジック
    /// </summary>
   [ExcludeFromCodeCoverage]
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

		private void LabelHP_MouseDown(object sender, MouseButtonEventArgs e)
		{
            Process.Start(new ProcessStartInfo
            {
                FileName = "http://turtleinsect.php.xdomain.jp/",
                UseShellExecute = true
            });
		}
	}
}
