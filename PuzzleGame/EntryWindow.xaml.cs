using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

namespace PuzzleGame
{
    /// <summary>
    /// Interaction logic for EntryWindow.xaml
    /// </summary>
    public partial class EntryWindow : Window
    {
        public EntryWindow()
        {
            InitializeComponent();
        }

        private void BtnEasy_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            int easyTime = 80;
            try
            {
                using (Stream stream = File.Open("Time.bin", FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, easyTime);
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Error, can't serialize to file.");
            }
            mw.ShowDialog();



            //try
            //{
            //    using (Stream stream = File.Open("Movies.bin", FileMode.Open))
            //    {
            //        BinaryFormatter bin = new BinaryFormatter();
            //        {
            //            List<Movie> movies = (List<Movie>)bin.Deserialize(stream);
            //            foreach (Movie item in movies)
            //            {
            //                listOfAllMovies.Add(item);
            //                lbMovies.Items.Add(item.getMovieName());
            //                lbProjectionMovies.Items.Add(item.getMovieName());
            //            }
            //        }
            //    }
            //}
            //catch (IOException)
            //{
            //    MessageBox.Show("Error, deserilizing from the list");
            //}
        }
    }
}
