using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WPF_SimpleCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); 

        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
            if (e.Handled)
            {
                MessageBox.Show("Must enter a number.", "Error");
            }
        }

        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void AmountCheck(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((bool)rb_one.IsChecked)
                {
                    grd_one.Visibility = Visibility.Visible;
                    grd_two.Visibility = Visibility.Collapsed;
                    grd_three.Visibility = Visibility.Collapsed;

                }
                else if ((bool)rb_two.IsChecked)
                {
                    grd_one.Visibility = Visibility.Visible;
                    grd_two.Visibility = Visibility.Visible;
                    grd_three.Visibility = Visibility.Collapsed;
                }
                else if ((bool)rb_three.IsChecked)
                {
                    grd_one.Visibility = Visibility.Visible;
                    grd_two.Visibility = Visibility.Visible;
                    grd_three.Visibility = Visibility.Visible;
                }
            }
            catch (Exception) {}   
        }

        private void butt_exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void butt_help_Click(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen<Help>())
            {
                Help help = new Help();
                help.Show();
            }
        }

        private void butt_calculate_Click(object sender, RoutedEventArgs e)
        {
            if (!IsWindowOpen<Solution>())
            {
                try
                {
                    string message = "";
                    int item1Cost, item1Quantity, item2Cost, item2Quantity, item3Cost, item3Quantity;
                    int item1Total = 0, item2Total = 0, item3Total = 0;
                    int grandTotal, grandTotalPerPerson;
                    bool pass = true;

                    ResetLabelColors();

                    if ((bool)rb_three.IsChecked)
                    {
                        //item three calcs
                        if (!int.TryParse(tb_price3.Text, out item3Cost))
                        {
                            lbl_price3.Foreground = Brushes.Red;
                            tb_price3.Focus();
                            message += "Please enter cost of item three.\n";
                            pass = false;
                        }
                        if (!int.TryParse(tb_quantity3.Text, out item3Quantity))
                        {
                            lbl_quantity3.Foreground = Brushes.Red;
                            tb_quantity3.Focus();
                            message += "Please enter quantity of item three.\n";
                            pass = false;
                        }

                        item3Total = item3Cost * item3Quantity;
                        item3Total -= (int)(item3Total * (int.Parse(tb_discount3.Text) / 100));
                    }

                    if ((bool)rb_two.IsChecked || (bool)rb_three.IsChecked)
                    {
                        //item two calcs
                        if (!int.TryParse(tb_price2.Text, out item2Cost))
                        {
                            lbl_price2.Foreground = Brushes.Red;
                            tb_price2.Focus();
                            message += "Please enter cost of item two.\n";
                            pass = false;
                        }
                        if (!int.TryParse(tb_quantity2.Text, out item2Quantity))
                        {
                            lbl_quantity2.Foreground = Brushes.Red;
                            tb_quantity2.Focus();
                            message += "Please enter quantity of item two.\n";
                            pass = false;
                        }

                        item2Total = item2Cost * item2Quantity;
                        item2Total -= (int)(item2Total * (int.Parse(tb_discount2.Text) / 100));
                    }

                    //item one calcs
                    if (!int.TryParse(tb_price1.Text, out item1Cost))
                    {
                        lbl_price1.Foreground = Brushes.Red;
                        tb_price1.Focus();
                        message += "Please enter cost of item one.\n";
                        pass = false;
                    }
                    if (!int.TryParse(tb_quantity1.Text, out item1Quantity))
                    {
                        lbl_quantity1.Foreground = Brushes.Red;
                        tb_quantity1.Focus();
                        message += "Please enter quantity of item one.\n";
                        pass = false;
                    }

                    item1Total = item1Cost * item1Quantity;
                    item1Total -= (int)(item1Total * (int.Parse(tb_discount1.Text) / 100));

                    if (pass)
                    {
                        //Final Math
                        grandTotal = item1Total + item2Total + item3Total;
                        grandTotalPerPerson = grandTotal / int.Parse(cb_roomates.Text);

                        Totals totals = new Totals { GrandTotal = grandTotal, GrandTotalPerPerson = grandTotalPerPerson };

                        //print to solution window
                        if (!IsWindowOpen<Solution>())
                        {
                            Solution s = new Solution(totals);
                            s.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show(message, "Error");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please enter a number in each box.","Error");
                }
                
            }
        }

        private void ResetLabelColors()
        {
            lbl_price1.Foreground = Brushes.Black;
            lbl_price2.Foreground = Brushes.Black;
            lbl_price3.Foreground = Brushes.Black;

            lbl_quantity1.Foreground = Brushes.Black;
            lbl_quantity2.Foreground = Brushes.Black;
            lbl_quantity3.Foreground = Brushes.Black;

            lbl_discount1.Foreground = Brushes.Black;
            lbl_discount2.Foreground = Brushes.Black;
            lbl_discount3.Foreground = Brushes.Black;
        }

        private void butt_reset_Click(object sender, RoutedEventArgs e)
        {
            ResetLabelColors();

            tb_price1.Text = "0";
            tb_price2.Text = "0";
            tb_price3.Text = "0";

            tb_quantity1.Text = "0";
            tb_quantity2.Text = "0";
            tb_quantity3.Text = "0";

            tb_discount1.Text = "0";
            tb_discount2.Text = "0";
            tb_discount3.Text = "0";

            cb_roomates.SelectedIndex = 0;

            rb_one.IsChecked = true;
        }
    }
}
