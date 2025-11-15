using MediaPlayer.BLL.Services;
using MediaPlayer.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;

namespace MediaPlayer.WPF
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        private SongService _songService = new();
        private ObservableCollection<Song> songs = new ObservableCollection<Song>();
        private Song currentSong;
        private DispatcherTimer timer;

        public Home()
        {
            InitializeComponent();

            // Load songs
            LoadSongs();
            lvSongs.ItemsSource = songs;
        }

        private void LoadSongs()
        {
            songs.Clear();

            try
            {
                var songFromDb = _songService.GetAllSongs();
                foreach (var song in songFromDb)
                {
                    songs.Add(song);
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Error loading songs: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUploadMusic_Click(object sender, RoutedEventArgs e)
        {
            UploadMusicDialog uploadMusicDialog = new UploadMusicDialog();
            uploadMusicDialog.ShowDialog();
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lvSongs_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void progressBar_MouseDown(object sender, RoutedEventArgs e)
        {

        }
    }
}
