using System;
using System.Collections.Generic;
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

namespace WPF_SimpleCalculator
{
    /// <summary>
    /// Interaction logic for Solution.xaml
    /// </summary>
    public partial class Solution : Window
    {
        public Solution(Totals totals)
        {
            InitializeComponent();
            tb_totalCost.Text = "$"+totals.GrandTotal.ToString();
            tb_costPerPerson.Text = "$"+totals.GrandTotalPerPerson.ToString();
        }

        private void butt_close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
