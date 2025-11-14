using MediaPlayer.BLL.Services;
using MediaPlayer.DAL.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for UploadMusicDialog.xaml
    /// </summary>
    public partial class UploadMusicDialog : Window
    {
        private SongService _songService = new();
        private string _selectedAudioPath;
        private string _selectedCoverPath;

        public UploadMusicDialog()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a song title.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTitle.Focus();
                return;
            }

            // Open file dialog to select song file
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Audio Files|*.mp3;*.wav;*.wma;*.m4a;*.flac;*.aac|All Files|*.*",
                Multiselect = false, // Changed to single file selection for clearer UX
                Title = "Select Audio File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string title = txtTitle.Text.Trim();
                string artist = txtArtist.Text.Trim(); 
                string genre = txtGenre.Text.Trim();

                // Save to database
                if (SaveSongToDatabase(title, artist, genre, filePath))
                {
                    DialogResult = true;
                    Close();
                }
            }
        }

        private bool SaveSongToDatabase(string title, string artist, string genre, string filePath)
        {
            try
            {
                string appData = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media");
                Directory.CreateDirectory(appData);

                string songsDir = System.IO.Path.Combine(appData, "Songs");
                string coversDir = System.IO.Path.Combine(appData, "Covers");

                Directory.CreateDirectory(songsDir);
                Directory.CreateDirectory(coversDir);

                // Copy audio file
                string audioDest = System.IO.Path.Combine(songsDir, System.IO.Path.GetFileName(filePath));
                File.Copy(filePath, audioDest, true);

                // Copy cover image if selected
                string coverDest = null;
                if (!string.IsNullOrEmpty(_selectedCoverPath))
                {
                    coverDest = System.IO.Path.Combine(coversDir, System.IO.Path.GetFileName(_selectedCoverPath));
                    File.Copy(_selectedCoverPath, coverDest, true);
                }

                // Validate file exists
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("Selected file does not exist.", "File Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                int? artistId = null;
                if (!string.IsNullOrWhiteSpace(artist))
                    artistId = _songService.GetOrCreateArtist(artist);

                // Get actual duration if possible (you may need to implement this)
                var duration = GetAudioDuration(filePath);

                var createSong = new Song()
                {
                    Title = title,
                    ArtistId = artistId,
                    Genre = genre,
                    Duration = duration,
                    FilePath = audioDest,
                    CoverImagePath = coverDest ?? "",
                };

                _songService.CreateSong(createSong);

                MessageBox.Show($"Song '{title}' uploaded successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving song: {ex.Message}", "Database Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show($"Error saving song: {ex.Message}");
                return false;
            }
        }

        private int GetAudioDuration(string filePath)
        {
            try
            {
                var file = TagLib.File.Create(filePath);
                return (int)file.Properties.Duration.TotalSeconds;
            }
            catch
            {
                return 0;
            }
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Audio Files|*.mp3;*.wav;*.wma;*.m4a;*.flac;*.aac|All Files|*.*",
                Multiselect = false,
                Title = "Select Audio File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedAudioPath = openFileDialog.FileName;
                txtFileName.Text = $"Selected: {System.IO.Path.GetFileName(_selectedAudioPath)}";

                // Auto-fill title if empty
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    txtTitle.Text = System.IO.Path.GetFileNameWithoutExtension(_selectedAudioPath);
                }
            }
        }

        private void BtnBrowseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp",
                Multiselect = false,
                Title = "Select Cover Image"
            };

            if (dlg.ShowDialog() == true)
            {
                _selectedCoverPath = dlg.FileName;

                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_selectedCoverPath);
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                imgCover.Source = bitmap;
            }
        }
    }
}
