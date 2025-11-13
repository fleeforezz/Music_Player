using MediaPlayer.BLL.Services;
using MediaPlayer.DAL.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace MediaPlayer.WPF
{
    /// <summary>
    /// Interaction logic for HomeScreen.xaml
    /// </summary>
    public partial class HomeScreen : Window
    {
        private SongService _songService = new();
        private ObservableCollection<Song> songs = new ObservableCollection<Song>();
        private Song currentSong;

        public HomeScreen()
        {
            InitializeComponent();

            LoadSongsFromDatabase();
            lvSongs.ItemsSource = songs;
        }

        private void LoadSongsFromDatabase()
        {
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

            LoadSongsFromDatabase();
        }

        private void LvSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvSongs.SelectedItem is Song song)
            {
                currentSong = song;
                txtNowPlaying.Text = song.Title;
                //txtNowPlayingArtist.Text = song.Artist;
            }
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (currentSong != null)
            {
                try
                {
                    if (File.Exists(currentSong.FilePath))
                    {
                        mediaPlayer.Source = new Uri(currentSong.FilePath);
                        mediaPlayer.Play();
                        btnPlay.Content = "#";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Audio file not found!", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a song first!", "No Song Selected",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            btnPlay.Content = "▶";
        }
    }
}
