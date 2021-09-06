using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using VideoLibrary;

namespace AudioTube
{
    public partial class AudioTube : MaterialForm
    {
        public string Audio { get; set; } = "audio.mp3";
        public string Video { get; set; } = "video.mp3";
        public string StoragePlace { get; set; }
        public string URI { get; set; }
        public AudioTube()
        {
            InitializeComponent();
            pictureBox2.Enabled = false;
            pictureBox2.Visible = false;

            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;

            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Red400, Primary.Red500,
                Primary.Red500, Accent.Amber100,
                TextShade.WHITE);
        }
        private void materialSingleLineTextField2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                materialSingleLineTextField2.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        private void PictureBoxLoadingAndButtonConvert(bool state)
        {
            pictureBox2.Enabled = state;
            pictureBox2.Visible = state;
            materialFlatButton1.Enabled = !state;
        }
        private void TextClear()
        {
            materialSingleLineTextField1.Text = String.Empty;
            materialSingleLineTextField2.Text = String.Empty;
        }
        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            PictureBoxLoadingAndButtonConvert(true);
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            URI = materialSingleLineTextField1.Text;
            StoragePlace = materialSingleLineTextField2.Text;

            YouTube youtube = YouTube.Default;
            YouTubeVideo video =  youtube.GetVideo(URI);

            string videoNameCompleted = Path.Combine(StoragePlace, Video);
            string audioNameCompleted = Path.Combine(StoragePlace, Audio);
            File.WriteAllBytes(videoNameCompleted,  video.GetBytes());
            

            var Convert = new NReco.VideoConverter.FFMpegConverter();
            Convert.ConvertMedia(videoNameCompleted, audioNameCompleted, "mp3");
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            PictureBoxLoadingAndButtonConvert(false);
            MessageBox.Show("Download completed");
            Process.Start(new ProcessStartInfo().FileName = Path.Combine(StoragePlace,Audio));
            TextClear();
        }
    }

}
