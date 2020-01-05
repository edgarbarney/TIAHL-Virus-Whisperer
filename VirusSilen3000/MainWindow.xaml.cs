using System;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media;
using NAudio.Wave;

using Timec = System.Windows.Forms.Timer;
using Mbox = System.Windows.MessageBox;

namespace VirusSilen3000
{
    public partial class MainWindow : Window
    {
        FolderBrowserDialog fbd = new FolderBrowserDialog();
        string directory = null;
        bool stopprogressbar;
        Timec timeci = new Timec();
        bool hizlihallet = false;
        Random rnd = new Random();
        string caption = "Virus Whisperer";
        public MainWindow()
        {
            InitializeComponent();
            timeci.Tick += new EventHandler(timer_Tick);
            timeci.Interval = (1000) * (1);
            timeci.Enabled = true;
            timeci.Start();
            Random rnd = new Random();
            int song = rnd.Next(1, 3);
            MemoryStream mp3file = new MemoryStream(Properties.Resources.ChiptuneTest4);
            Mp3FileReader mp3reader = new Mp3FileReader(mp3file);
            var waveOut = new WaveOut();
            waveOut.Init(mp3reader);
            waveOut.Play();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            System.Windows.Application.Current.Shutdown();
        }
        private void Shutdown_Click (object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (stopprogressbar)
                {
                    MainPBar.IsIndeterminate = false;
                    MainPBar.Value = 100f;
                    timeci.Stop();
                    string messageBoxText = "Viruses are no more. Have a nice day :)";
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Warning;
                    MessageBoxResult result = Mbox.Show(messageBoxText, caption, button, icon);
                    switch (result)
                    {
                        case MessageBoxResult.OK:
                            System.Windows.Application.Current.Shutdown();
                            break;
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    var exmexcal = new ExceptionLogger();
                    exmexcal.ExcepionMessageCalc(ex);
                }));
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //string messageBoxText = "Temizleyici seçtiğiniz klasördeki virüsleri temizleyip gizli dosyaları görünürleştirecek ve bütün kısayolları silecektir. Programın yan etkileri de olabilir. Devam etmek istediğinizden emin misiniz?";
            string messageBoxText = "This program will remove all shortcuts and will unhide hidden files. This can cause some side effects. Do you want to cuntinue anyway?";
            MessageBoxButton button = MessageBoxButton.OKCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    MessageBoxResult result = Mbox.Show(messageBoxText, caption, button, icon);
                    switch (result)
                    {
                        case MessageBoxResult.OK:
                            directory = fbd.SelectedPath;
                            MainPBar.IsIndeterminate = true;
                            DirSelBut.IsEnabled = false;
                            ChekBox.IsEnabled = false;
                            Task.Factory.StartNew(() => Clear(this));
                            break;
                        case MessageBoxResult.Cancel:
                            break;
                    }
                }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            hizlihallet = true;
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            hizlihallet = false;
        }
        private void Clear(MainWindow mwin)
        {
            try
            {
                string[] files = Directory.GetFiles(directory);
                string[] dirs = Directory.GetDirectories(directory);
                string[] empty = new String[] { "Virus Deleted BY FranticDreamer", "", "Virüs FranticDreamer tarafından temizlendi" };
                int leng = 0;
                int leng2 = 0;
                foreach (string item2 in dirs)
                {
                    FileInfo f = new FileInfo(item2);
                    if (item2.Contains("System Volume Information") || item2.Contains("$RECYCLE.BIN"))
                    {
                        Debug.WriteLine("System Volume Info or Recycle Bin temp files detected");
                    }
                    else
                    {
                        File.SetAttributes(item2, File.GetAttributes(item2) & ~(FileAttributes.Hidden | FileAttributes.System | FileAttributes.ReadOnly));
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            StatusLabel.Foreground = Brushes.Green;
                            StatusLabel.Content = ("Status: Recovering Folder; " + item2);
                        }));
                        //Dispatcher.BeginInvoke(new Action(() =>
                        //{
                        //    MainPBar.Value = ((leng2 / dirs.Length) * 50);
                        //}));
                    }
                    leng2++;
                    if (!hizlihallet)
                    {
                        Thread.Sleep(rnd.Next(0, 750));
                    }
                }
                foreach (string item in files)
                {
                    FileInfo f = new FileInfo(item);
                    if (item.Contains(".vbs") || item.Contains("autorun.inf"))
                    {
                        var attr = File.GetAttributes(item);
                        File.SetAttributes(item, File.GetAttributes(item) & ~(FileAttributes.Hidden | FileAttributes.System | FileAttributes.ReadOnly));
                        File.Delete(item);
                        File.WriteAllLines(item, empty);
                        Debug.WriteLine("Virus is killed! : '" + item + "'");
                        File.SetAttributes(item, attr | FileAttributes.System | FileAttributes.ReadOnly | FileAttributes.Hidden);
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            StatusLabel.Foreground = Brushes.Red;
                            StatusLabel.Content = ("Status: Killing Virus; " + item);
                        }));
                        if (!hizlihallet)
                        {
                            Thread.Sleep(rnd.Next(0, 750));
                        }
                    }
                    else if (item.Contains(".lnk"))
                    {
                        File.Delete(item);
                        if (!hizlihallet)
                        {
                            Thread.Sleep(rnd.Next(0, 750));
                        }
                    }
                    else
                    {
                        File.SetAttributes(item, File.GetAttributes(item) & ~(FileAttributes.Hidden | FileAttributes.System | FileAttributes.ReadOnly));
                        Debug.WriteLine(item);
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            StatusLabel.Foreground = Brushes.Green;
                            StatusLabel.Content = ("Status: Recovering File; " + item);
                        }));
                        if (!hizlihallet)
                        {
                            Thread.Sleep(rnd.Next(0, 750));
                        }
                    }
                    //Dispatcher.BeginInvoke(new Action(() =>
                    //{
                    //    MainPBar.Value = (((leng / dirs.Length) * 50) + 50);
                    //}));
                    leng++;
                }
                stopprogressbar = true;
            }
            catch (Exception ex)
            {
                var exmexcal = new ExceptionLogger();
                exmexcal.ExcepionMessageCalc(ex);
            }
        }
    }
}
