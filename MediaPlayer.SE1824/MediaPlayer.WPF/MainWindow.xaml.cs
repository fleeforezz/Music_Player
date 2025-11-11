using Microsoft.Win32;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;

namespace MediaPlayer.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        
       

        private bool IsPlaying = false;
        private bool IsUserDraggingSlider = false;

        private readonly DispatcherTimer Timer = new() { Interval = TimeSpan.FromSeconds(0.1) };
        private readonly OpenFileDialog MediaOpenDialog = new()
        {
            Title = "Open a media file",
            Filter = "Media Files (*.mp3,*.mp4)|*.mp3;*.mp4"
        };

        public MainWindow()
        {
            InitializeComponent();

            Timer.Tick += Timer_Tick;
            Timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (Player.Source != null && Player.NaturalDuration.HasTimeSpan && !IsUserDraggingSlider)
            {
                ProgressSlider.Maximum = Player.NaturalDuration.TimeSpan.TotalSeconds;
                ProgressSlider.Value = Player.Position.TotalSeconds;
            }
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MediaOpenDialog.ShowDialog() == true)
            {
                Player.Source = new Uri(MediaOpenDialog.FileName);
                TitleLbl.Content = System.IO.Path.GetFileName(MediaOpenDialog.FileName);

                Player.Play();
                IsPlaying = true;
            }
        }

        #region Media Controls

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Player?.Source != null)
            {
                Player.Play();
                IsPlaying = true;
            }
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsPlaying)
                Player.Pause();
        }

        private void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsPlaying)
            {
                Player.Stop();
                IsPlaying = false;
            }
        }

        private void ProgressSlider_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            IsUserDraggingSlider = true;
        }

        private void ProgressSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            IsUserDraggingSlider = false;
            Player.Position = TimeSpan.FromSeconds(ProgressSlider.Value);
        }

        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            StatusLbl.Text = TimeSpan.FromSeconds(ProgressSlider.Value).ToString(@"hh\:mm\:ss");
        }

        #endregion
        #region Properties

        private void PropertiesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MediaOpenDialog.FileName != "")
            {
                var tfile = TagLib.File.Create(MediaOpenDialog.FileName);
                StringBuilder sb = new();

                sb.AppendLine("Duration: " + tfile.Properties.Duration.ToString(@"hh\:mm\:ss"));

                if (tfile.Properties.MediaTypes.HasFlag(TagLib.MediaTypes.Audio))
                {
                    sb.AppendLine("Audio bitrate: " + tfile.Properties.AudioBitrate);
                    sb.AppendLine("Audio sample rate: " + tfile.Properties.AudioSampleRate);
                    sb.AppendLine("Audio channels: " + (tfile.Properties.AudioChannels == 1 ? "Mono" : "Stereo"));
                }

                if (tfile.Properties.MediaTypes.HasFlag(TagLib.MediaTypes.Video))
                {
                    sb.AppendLine($"Video resolution: {tfile.Properties.VideoWidth} x {tfile.Properties.VideoHeight}");
                }

                MessageBox.Show(sb.ToString(), "Properties");
            }
        }

        private void Player_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (Player.NaturalVideoWidth > 0 && Player.NaturalVideoHeight > 0)
            {
                // Bước 1: Tính toán không gian hiển thị tối đa an toàn.

                // Lấy kích thước màn hình hiện tại
                double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
                double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;

                // Bước 2: Ước tính chiều cao của các control cố định (TitleLbl, ControlDockPanel)
                // Cần đảm bảo các control này đã được tính toán (Measure) để lấy kích thước mong muốn chính xác
                TitleLbl.Measure(new Size(screenWidth, screenHeight));
                ControlDockPanel.Measure(new Size(screenWidth, screenHeight));

                // Tính tổng chiều cao cố định của các control và Margin xung quanh
                // TitleLbl (Height + 40 Margin T/B) + ControlDockPanel (Height + 40 Margin T/B)
                double fixedLayoutHeight = TitleLbl.DesiredSize.Height + 40 + ControlDockPanel.DesiredSize.Height + 40;

                // Khoảng đệm an toàn thêm cho viền cửa sổ
                double safeMargin = 20;

                // Bước 3: Thiết lập giới hạn kích thước tối đa cho MediaElement.
                // MediaElement sẽ tự động co lại (Stretch="Uniform") nếu độ phân giải gốc của video quá lớn.

                // Chiều rộng tối đa: Chiều rộng màn hình - Margin MediaElement (20*2) - Lề an toàn
                Player.MaxWidth = screenWidth - (20 * 2) - safeMargin;

                // Chiều cao tối đa: Chiều cao màn hình - Tổng chiều cao controls cố định - Lề an toàn
                Player.MaxHeight = screenHeight - fixedLayoutHeight - safeMargin;
            }
        }
    }

    #endregion
}