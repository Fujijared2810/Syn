using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MultiAPI;
using Newtonsoft.Json;
using Synapse_X_Remake.Interface;
using Synapse_X_Remake.Static;

namespace Synapse_X_Remake
{
    public partial class LoadWindow : Window
    {
        
        #region animations
        private TimeSpan duration { get; set; } = TimeSpan.FromSeconds(1);
        private IEasingFunction Smooth { get; set; } = new QuarticEase { EasingMode = EasingMode.EaseInOut };

        private IEasingFunction ease { get; set; } = new QuarticEase { EasingMode = EasingMode.EaseInOut };

        TimeSpan halfsecond = TimeSpan.FromMilliseconds(500);

        TimeSpan second = TimeSpan.FromSeconds(1);

        public void FadeIn(DependencyObject element)
        {
            DoubleAnimation fadeAnimation = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = duration,
                EasingFunction = ease
            };

            Storyboard.SetTarget(fadeAnimation, element);
            Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath(OpacityProperty));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(fadeAnimation);
            //storyboard.FillBehavior = FillBehavior.Stop;
            storyboard.Begin();
        }

        public void FadeOut(DependencyObject element)
        {
            DoubleAnimation fadeAnimation = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = duration,
                EasingFunction = ease
            };

            Storyboard.SetTarget(fadeAnimation, element);
            Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath(OpacityProperty));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(fadeAnimation);
            //storyboard.FillBehavior = FillBehavior.Stop;
            storyboard.Begin();
        }

        public void Shift(DependencyObject element, Thickness from, Thickness to)
        {
            ThicknessAnimation shiftAnimation = new ThicknessAnimation()
            {
                From = from,
                To = to,
                Duration = duration,
                EasingFunction = ease
            };

            Storyboard.SetTarget(shiftAnimation, element);
            Storyboard.SetTargetProperty(shiftAnimation, new PropertyPath(MarginProperty));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(shiftAnimation);
            //storyboard.FillBehavior = FillBehavior.Stop;
            storyboard.Begin();
        }
        public void LoaderResize()
        {
            DoubleAnimation danimationX = new DoubleAnimation();
            danimationX.From = MainBorder.Width;
            danimationX.To = 418.203;
            danimationX.Duration = second;
            danimationX.EasingFunction = Smooth;
            MainBorder.BeginAnimation(WidthProperty, danimationX);

            DoubleAnimation danimationY = new DoubleAnimation();
            danimationY.From = MainBorder.Height;
            danimationY.To = 116.685;
            danimationY.Duration = second;
            danimationY.EasingFunction = Smooth;
            MainBorder.BeginAnimation(HeightProperty, danimationY);
        }
        public void LoaderResizeClose()
        {
            DoubleAnimation danimationX = new DoubleAnimation();
            danimationX.From = MainBorder.Width;
            danimationX.To = 0;
            danimationX.Duration = second;
            danimationX.EasingFunction = Smooth;
            MainBorder.BeginAnimation(WidthProperty, danimationX);

            DoubleAnimation danimationY = new DoubleAnimation();
            danimationY.From = MainBorder.Height;
            danimationY.To = 0;
            danimationY.Duration = second;
            danimationY.EasingFunction = Smooth;
            MainBorder.BeginAnimation(HeightProperty, danimationY);
        }
        public void DragGridResize()
        {
            DoubleAnimation danimationX = new DoubleAnimation();
            danimationX.From = DragBorder.Width;
            danimationX.To = 418.203;
            danimationX.Duration = second;
            danimationX.EasingFunction = Smooth;
            DragBorder.BeginAnimation(WidthProperty, danimationX);
        }
        #endregion;

        WeAreDevsAPI WeAreDevsAPII = new();

        OxygenUAPI OxygenUAPII = new();

        ElectronAPI ElectronAPII = new();

        FluxusAPI FluxusAPII = new();

        public LoadWindow()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            if (Directory.GetCurrentDirectory().Contains(System.IO.Path.GetTempPath()))
            {
                MessageBox.Show("Please extract this to a vaild directory, Do NOT launch Poltergeist A+ V2 through a zip file.", "Poltergeist A+ V2", MessageBoxButton.OK, MessageBoxImage.Hand);
                Close();
            }
            InitializeComponent();
            StatusBox.Visibility = Visibility.Hidden;
            ProgressBox.Visibility = Visibility.Hidden;
            DragGrid.Visibility = Visibility.Hidden;
            MainGrid.Visibility = Visibility.Hidden;
            LoadWorker.DoWork += LoadWorker_DoWork;
        }

        public void SetStatusText(string Status, int Percentage)
        {
            Dispatcher.Invoke(() =>
            {
                StatusBox.Content = Status;
                ProgressBox.Value = Percentage;
            });
        }

        WebClient wc = new();

        private async void LoadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SetStatusText("Checking update...", 25);
            await Task.Delay(2000);
            bool flag = false;
            bool num = true;
            if (num)
            {
                SetStatusText("Downloading Dlls...", 50);
                KrnlAPI.Load();
                WeAreDevsAPI.DownloadLatestVersion();
                OxygenUAPI.IntializeUpdateOxygenU();
                ElectronAPI.DownloadDLL();
                FluxusAPII.InitializeFluxus();
            }
            if (!File.Exists("ICSharpCode.AvalonEdit.dll"))
            {
                SetStatusText("Downloading Avalon", 85);
                wc.DownloadString("https://cdn.discordapp.com/attachments/856792157741121566/894801787024982026/ICSharpCode.AvalonEdit.dll");
                wc.DownloadFile("https://cdn.discordapp.com/attachments/856792157741121566/894801787024982026/ICSharpCode.AvalonEdit.dll", "ICSharpCode.AvalonEdit.dll");
                wc.Dispose();
                await Task.Delay(1000);
            }
            if (!num && !flag)
            {
                SetStatusText("Ready to Launch!", 100);
            }
            else
            {
                SetStatusText("Ready to Launch!", 100);
            }
            Thread.Sleep(1000);
            base.Dispatcher.Invoke(async delegate
            {
                FadeOut(MainGrid);
                await Task.Delay(650);
                LoaderResizeClose();
                MainGrid.Visibility = Visibility.Hidden;
                await Task.Delay(1000);
                MainWindow Exploit = new MainWindow();
                Exploit.Show();
                Hide();
            });
        }

        public static BackgroundWorker LoadWorker = new();

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DataInterface.Exists("options"))
            {
                Globals.Options = new Data.Options
                {
                    AutoAttach = false,
                    TopMost = false,
                    Opacity = false,
                    UnlockFPS = false,
                    krnl = false,
                    OxygenU = false,
                    Electron = false,
                    WeAreDevs = false,
                    Fluxus = false,
                };
                DataInterface.Save("options", new Data.OptionsHolder
                {
                    Version = Data.OptionsVersion,
                    Data = JsonConvert.SerializeObject(Globals.Options)
                });
            }
            else
            {
                try
                {
                    var Read = DataInterface.Read<Data.OptionsHolder>("options");
                    if (Read.Version != Data.OptionsVersion)
                    {
                        Globals.Options = new Data.Options
                        {
                            AutoAttach = false,
                            TopMost = false,
                            Opacity = false,
                            UnlockFPS = false,
                            krnl = false,
                            OxygenU = false,
                            Electron = false,
                            WeAreDevs = false,
                            Fluxus = false,
                        };
                        DataInterface.Save("options", new Data.OptionsHolder
                        {
                            Version = Data.OptionsVersion,
                            Data = JsonConvert.SerializeObject(Globals.Options)
                        });
                    }
                    else
                    {
                        Globals.Options = JsonConvert.DeserializeObject<Data.Options>(Read.Data);
                    }
                }
                catch (Exception)
                {
                    Globals.Options = new Data.Options
                    {
                        AutoAttach = false,
                        TopMost = false,
                        Opacity = false,
                        UnlockFPS = false,
                        krnl = false,
                        OxygenU = false,
                        Electron = false,
                        WeAreDevs = false,
                        Fluxus = false,
                    };
                    DataInterface.Save("options", new Data.OptionsHolder
                    {
                        Version = Data.OptionsVersion,
                        Data = JsonConvert.SerializeObject(Globals.Options)
                    });
                }
            }
            LoaderResize();
            await Task.Delay(1000);
            MainGrid.Visibility = Visibility.Visible;
            FadeIn(MainGrid);
            DragGridResize();
            await Task.Delay(1000);
            DragGrid.Visibility = Visibility.Visible;
            FadeIn(DragGrid);
            StatusBox.Visibility = Visibility.Visible;
            FadeIn(StatusBox);
            ProgressBox.Visibility = Visibility.Visible;
            FadeIn(ProgressBox);
            await Task.Delay(1000);
            if (!Directory.Exists("bin"))
            {
                Directory.CreateDirectory("bin");
            }
            if (!Directory.Exists("bin\\Tabs"))
            {
                Directory.CreateDirectory("bin\\Tabs");
            }
            if (!Directory.Exists("scripts"))
            {
                Directory.CreateDirectory("scripts");
            }
            if (!Directory.Exists("workspace"))
            {
                Directory.CreateDirectory("workspace");
            }
            if (!Directory.Exists("autoexec"))
            {
                Directory.CreateDirectory("autoexec");
            }
            LoadWorker.RunWorkerAsync();
        }
    }
}
