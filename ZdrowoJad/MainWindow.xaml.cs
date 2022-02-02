using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using System.Windows.Controls;



namespace ZdrowoJad
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string name = "";
        public SqlConnection conection = new SqlConnection("Server=.;Database=BD1_2020;Integrated Security=True;");

        public MainWindow()

        {
            InitializeComponent();
            ButtonWyczysc_Click(null, null);
            conection.Open();
            var command = new SqlCommand("Select * FROM Pracownicy", conection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ComboBoxImie.Items.Add(reader.GetValue(2).ToString());
            }
            conection.Close();

        }

        private void CheckBoxVat_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("dodano 23% vat.");
            var price = float.Parse(TextBoxSuma.Text);
            if (CheckBoxWysylka.IsChecked == false)
                TextBoxSuma.Text = Math.Round((price * 1.23), 2).ToString();
            else
                TextBoxSuma.Text = Math.Round((((price - 20) * 1.23) + 20), 2).ToString();
        }

        private void CheckBoxVat_Unchecked(object sender, RoutedEventArgs e)
        {
            var price = float.Parse(TextBoxSuma.Text);
            if (CheckBoxWysylka.IsChecked == false)
                TextBoxSuma.Text = (Math.Round(price / 1.23, 2)).ToString();
            else
                TextBoxSuma.Text = (Math.Round(((price - 20) / 1.23), 2) + 20).ToString();
        }


        private void CheckBoxWysylka_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("dodano koszt wysyłki.");
            var price = float.Parse(TextBoxSuma.Text);
            TextBoxSuma.Text = (price + 20).ToString();
        }

        private void CheckBoxWysylka_unchecked(object sender, RoutedEventArgs e)
        {
            var price = float.Parse(TextBoxSuma.Text);
            TextBoxSuma.Text = (price - 20).ToString();
        }

        private void ComboBoxImie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxImie.SelectedItem != null)
            {
                ComboBoxNazwisko.Items.Clear();
                conection.Open();
                var command = new SqlCommand("Select * FROM Pracownicy WHERE Imię = @NAME", conection);
                command.Parameters.AddWithValue("@NAME", ComboBoxImie.SelectedItem);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ComboBoxNazwisko.Items.Add(reader.GetValue(1).ToString());
                }
                ComboBoxNazwisko.IsEnabled = true;
                conection.Close();
            }
        }

        private void ComboBoxNazwisko_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxNazwisko.SelectedItem != null)
            {
                conection.Open();
                var command = new SqlCommand("Select * FROM Klienci", conection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ComboBoxNazwaKlienta.Items.Add(reader.GetValue(1).ToString());
                }
                ComboBoxNazwaKlienta.IsEnabled = true;
                conection.Close();
            }
        }

        private void ComboBoxNazwaKlienta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxNazwaKlienta.SelectedItem != null)
            {
                ComboBoxNazwaProd.Items.Clear();
                TextBoxCenaSzt.Text = "";
                TextBoxDostepne.Text = "";
                TextBoxIlosc.Text = "";
                TextBoxCena.Text = "";
                TextBoxCenaSzt.IsEnabled = false;
                TextBoxDostepne.IsEnabled = false;
                TextBoxIlosc.IsEnabled = false;
                TextBoxCena.IsEnabled = false;
                conection.Open();
                var command = new SqlCommand("Select * FROM Kategorie", conection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ComboBoxKategoria.Items.Add(reader.GetValue(1).ToString());
                }
                ComboBoxKategoria.IsEnabled = true;
                conection.Close();
            }
        }

        private void ComboBoxKategoria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxKategoria.SelectedItem != null)
            {
                ComboBoxNazwaProd.Items.Clear();
                TextBoxCenaSzt.Text = "";
                TextBoxDostepne.Text = "";
                TextBoxIlosc.Text = "";
                TextBoxCena.Text = "";
                TextBoxCenaSzt.IsEnabled = false;
                TextBoxDostepne.IsEnabled = false;
                TextBoxIlosc.IsEnabled = false;
                TextBoxCena.IsEnabled = false;
                ComboBoxNazwaProd.IsEnabled = true;

                conection.Open();
                var command = new SqlCommand("Select * FROM Produkty p, Kategorie k WHERE p.IDkategorii = k.IDkategorii AND k.NazwaKategorii = @KAT", conection);
                command.Parameters.AddWithValue("@KAT", ComboBoxKategoria.SelectedItem);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ComboBoxNazwaProd.Items.Add(reader.GetValue(1).ToString());
                }
                //ComboBoxKategoria.IsEnabled = true;
                conection.Close();
            }
        }

        private void ComboBoxNazwaProd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxNazwaProd.SelectedItem != null)
            {
                ButtonDodaj.IsEnabled = false;
                TextBoxCenaSzt.Clear();
                TextBoxDostepne.Clear();
                TextBoxIlosc.Clear();
                TextBoxCena.Clear();
                conection.Open();
                var command = new SqlCommand("Select * FROM Produkty WHERE NazwaProduktu = @NAME", conection);
                command.Parameters.AddWithValue("@NAME", ComboBoxNazwaProd.SelectedItem);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TextBoxDostepne.Text = reader.GetValue(6).ToString();
                    TextBoxCenaSzt.Text = Math.Round(float.Parse(reader.GetValue(5).ToString()), 2).ToString();
                }
                TextBoxIlosc.IsEnabled = true;
                //ComboBoxKategoria.IsEnabled = true;
                conection.Close();
            }
        }

        private void ButtonWyczysc_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxImie.IsEnabled = true;
            CheckBoxVat.IsChecked = false;
            CheckBoxWysylka.IsChecked = false;
            ButtonDodaj.IsEnabled = false;
            ButtonZatwierdz.IsEnabled = false;
            ListBoxZamówiene.IsEnabled = false;
            ListBoxZamówiene.Items.Clear();
            ComboBoxImie.SelectedItem = null;
            TextBoxCenaSzt.Clear();
            TextBoxDostepne.Clear();
            TextBoxIlosc.Clear();
            TextBoxCena.Clear();
            TextBoxCenaSzt.IsEnabled = false;
            TextBoxDostepne.IsEnabled = false;
            TextBoxIlosc.IsEnabled = false;
            TextBoxCena.IsEnabled = false;
            TextBoxSuma.Text = "0";
            TextBoxSuma.IsEnabled = false;
            ComboBoxNazwisko.Items.Clear();
            ComboBoxKategoria.Items.Clear();
            ComboBoxNazwaProd.Items.Clear();
            ComboBoxNazwaKlienta.Items.Clear();
            ComboBoxNazwisko.IsEnabled = false;
            ComboBoxKategoria.IsEnabled = false;
            ComboBoxNazwaProd.IsEnabled = false;
            ComboBoxNazwaKlienta.IsEnabled = false;
        }

        private void TextBoxIlosc_TextChanged(object sender, TextChangedEventArgs e)
        {
            int number;
            bool success = int.TryParse(TextBoxIlosc.Text, out number);
            if (TextBoxIlosc.Text != null && success == true)
            {
                if (number > int.Parse(TextBoxDostepne.Text))
                {
                    MessageBox.Show("Niewystarczająca ilość towaru w magazynie");
                    TextBoxIlosc.Clear();
                    TextBoxCena.Clear();
                }
                else if (number <= 0)
                {
                    MessageBox.Show("Błędna wartosć");
                    TextBoxIlosc.Clear();
                    TextBoxCena.Clear();
                }
                else
                    TextBoxCena.Text = (Math.Round(number * float.Parse(TextBoxCenaSzt.Text), 2)).ToString("0.00");
                ButtonDodaj.IsEnabled = true;
            }
        }

        private void ButtonDodaj_Click(object sender, RoutedEventArgs e)
        {
            TextBoxDostepne.Text = (int.Parse(TextBoxDostepne.Text) - int.Parse(TextBoxIlosc.Text)).ToString();
            ComboBoxImie.IsEnabled = false;
            ComboBoxNazwisko.IsEnabled = false;
            string element = TextBoxCena.Text + "zł  - " + TextBoxIlosc.Text + " x " + ComboBoxNazwaProd.SelectedItem;
            ListBoxZamówiene.Items.Add(element);
            TextBoxSuma.Text = (float.Parse(TextBoxSuma.Text) + float.Parse(TextBoxCena.Text)).ToString();
            if (TextBoxSuma.Text.ToString() != "0")
                ButtonZatwierdz.IsEnabled = true;
            if (int.Parse(TextBoxDostepne.Text) < int.Parse(TextBoxIlosc.Text))
            {
                MessageBox.Show("Niewystarczająca ilość towaru w magazynie");
                TextBoxIlosc.Clear();
                TextBoxCena.Clear();
            }
        }

        private void ButtonZatwierdz_Click(object sender, RoutedEventArgs e)
        {
            TextWriter txt = new StreamWriter("C:\\Users\\adamj\\Desktop\\rachunek.txt");
            string message = "Jesteś pewien, że chcesz wydrukować rachunek?";
            string caption = "Czy Wydrukować rachunek?";
            var result = MessageBox.Show(message, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);

            // If the no button was pressed ...
            if (result == MessageBoxResult.Yes)
            {
                //wydrukować nagłówek paragonu (dane firmy)
                txt.Write("CryptoBuy.SA\nRenata Bibuła\nSklep internetowy\nul.Chlebowa 25\n45-512 Bogaczew\n");
                //wydrukować dane pracownika
                txt.Write("\nObsługiwał/a:\n{0} {1}\n\n", ComboBoxImie.Text, ComboBoxNazwisko.Text);
                //wydrukować produkty z zamówienia
                txt.Write("Zamówienie:\n");
                foreach (var item in ListBoxZamówiene.Items)
                {
                    txt.Write(item.ToString() + "\n");
                }
                //wydrukować dodanie lub nie dodanie VAT
                //wydrukować dodanie lub nie dodanie przesyłki
                txt.Write("\nUsługi dodatkowe:\n");
                if (CheckBoxVat.IsChecked == true)
                {
                    double vat;
                    if (CheckBoxWysylka.IsChecked == false)
                        vat = Math.Round((double.Parse(TextBoxSuma.Text) * 0.23), 2);
                    else
                        vat = Math.Round(((double.Parse(TextBoxSuma.Text) - 20) * 0.23), 2);
                    txt.Write("23% VAT:\t{0}zł\n", vat);

                }
                if (CheckBoxWysylka.IsChecked == true)
                    txt.Write("Wysyłka:\t20zł\n");
                if (CheckBoxWysylka.IsChecked == false && CheckBoxVat.IsChecked == false)
                    txt.Write("Bark kosztów dodatkowych.\n");
                //wydrukować całkowity koszt zamówienia
                txt.Write("\nSUMA:\t\t{0}\nZapłacono:\t{0}\n", TextBoxSuma.Text);
                //wydrukować datę i dzień zamówienia
                txt.Write("\n" + DateTime.Now.ToString() + "\n\n");
                //wydrukować podziękowanie za zakup
                txt.Write("Dziękujemy za zakupy:)\nZapraszamy Ponownie!");

                message = "Rachunek został wydrukowany!";
                caption = "Sukces!";
                MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
                ButtonWyczysc_Click(null, null);
                txt.Close();
            }
        }
    }
}