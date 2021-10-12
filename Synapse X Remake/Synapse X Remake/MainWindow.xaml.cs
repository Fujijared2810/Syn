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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;
using MultiAPI;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Synapse_X_Remake.Static;
using Synapse_X_Remake.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Synapse_X_Remake
{
    public partial class MainWindow : Window
    {
        WeAreDevsAPI WeAreDevsAPI = new();

        ElectronAPI ElectronAPI = new();

        OxygenUAPI OxygenUAPI = new();

        FluxusAPI FluxusAPI = new();

        private System.Windows.Forms.Timer timer;

        public void ListBoxRefresh()
        {
            this.ScriptBox.Items.Clear();
            foreach (FileInfo fileInfo in new DirectoryInfo("./scripts").GetFiles("*.txt"))
            {
                this.ScriptBox.Items.Add(fileInfo.Name);
            }
            foreach (FileInfo fileInfo2 in new DirectoryInfo("./scripts").GetFiles("*.lua"))
            {
                this.ScriptBox.Items.Add(fileInfo2.Name);
            }

            FileSystemWatcher files = new FileSystemWatcher();
        }

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
            danimationX.To = 801;
            danimationX.Duration = second;
            danimationX.EasingFunction = Smooth;
            MainBorder.BeginAnimation(WidthProperty, danimationX);

            DoubleAnimation danimationY = new DoubleAnimation();
            danimationY.From = MainBorder.Height;
            danimationY.To = 355;
            danimationY.Duration = second;
            danimationY.EasingFunction = Smooth;
            MainBorder.BeginAnimation(HeightProperty, danimationY);
        }
        public void ScriptHubResize()
        {
            DoubleAnimation danimationX = new DoubleAnimation();
            danimationX.From = MainBorder.Width;
            danimationX.To = 439;
            danimationX.Duration = second;
            danimationX.EasingFunction = Smooth;
            MainBorder.BeginAnimation(WidthProperty, danimationX);

            DoubleAnimation danimationY = new DoubleAnimation();
            danimationY.From = MainBorder.Height;
            danimationY.To = 336;
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
        #endregion;
        public MainWindow()
        {
            InitializeComponent();
            ScriptEditor.InitializeDirectory("bin\\Tabs");
            Settingss.Visibility = Visibility.Hidden;
            FadeOut(Settings_Grid);
            GridMian.Visibility = Visibility.Hidden;
            Scripthub_Grid.Visibility = Visibility.Hidden;

            timer = new System.Windows.Forms.Timer();
            timer.Tick += AttachedTimer;
            timer.Interval = 100;
            timer.Start();

            timer1 = new();
            timer1.Tick += KrnlAutoAttachTime;
            timer1.Interval = 2000;
            timer1.Start();

            timer2 = new();
            timer2.Tick += ElectronAutoAttachTime;
            timer2.Interval = 2000;
            timer2.Start();

            timer3 = new();
            timer3.Tick += OxygenUAutoAttachTime;
            timer3.Interval = 2000;
            timer3.Start();

            timer4 = new();
            timer4.Tick += WearedevsAutoAttachTime;
            timer4.Interval = 2000;
            timer4.Start();

            timer5 = new();
            timer5.Tick += FluxusAutoAttachTime;
            timer5.Interval = 2000;
            timer5.Start();
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool WaitNamedPipe(string pipe, int timeout = 10);

        public bool Exists(string Name)
        {
            return WaitNamedPipe("\\\\.\\pipe\\" + Name);
        }

        private void AttachedTimer(object sender, EventArgs e)
        {
            if (krnlbox.IsChecked == true)
            {
                if (KrnlAPI.IsKrnlAttach()== true)
                {
                    TitleBox.Content = "Synapse X - v1.0.0 (Attached!)";
                }
                else
                {
                    TitleBox.Content = "Synapse X - v1.0.0";
                }
            }
            else if (electronbox.IsChecked == true)
            {
                if (ElectronAPI.IsElectronAttach() == true)
                {
                    TitleBox.Content = "Synapse X - v1.0.0 (Attached!)";
                }
                else
                {
                    TitleBox.Content = "Synapse X - v1.0.0";
                }
            }
            else if (oxygenubox.IsChecked == true)
            {
                if (OxygenUAPI.isOXygenUAttached() == true)
                {
                    TitleBox.Content = "Synapse X - v1.0.0 (Attached!)";
                }
                else
                {
                    TitleBox.Content = "Synapse X - v1.0.0";
                }
            }
            else if (Wearedevsbox.IsChecked == true)
            {
                if (WeAreDevsAPI.IsWeAreDevsAttach() == true)
                {
                    TitleBox.Content = "Synapse X - v1.0.0 (Attached!)";
                }
                else
                {
                    TitleBox.Content = "Synapse X - v1.0.0";
                }
            }
            else if (FluxusBox.IsChecked == true)
            {
                if (FluxusAPI.IsFluxusAttached() == true)
                {
                    TitleBox.Content = "Synapse X - v1.0.0 (Attached!)";
                }
                else
                {
                    TitleBox.Content = "Synapse X - v1.0.0";
                }
            }
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            var OpenDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Script Files (*.lua, *.txt)|*.lua;*.txt",
                Title = "Synapse X - Open File",
                FileName = ""
            };

            if (OpenDialog.ShowDialog() != true) return;
            var script = File.ReadAllText(OpenDialog.FileName);
            var messageBox = MessageBox.Show("Open in a new tab?", "Synapse X", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBox == MessageBoxResult.Yes)
                ScriptEditor.CreateTab(Path.GetFileName(OpenDialog.FileName), script);
            else
                ScriptEditor.Text = script;
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            var SaveDialog = new Microsoft.Win32.SaveFileDialog { Title = "Synapse X - Save File", Filter = "Script Files (*.lua, *.txt)|*.lua;*.txt", FileName = "" };

            SaveDialog.FileOk += (o, args) =>
            {
                File.WriteAllText(SaveDialog.FileName, ScriptEditor.Text);
            };

            SaveDialog.ShowDialog();
        }

        private async void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (Settingss.Visibility == Visibility.Hidden)
            {
                FadeIn(Settingss);
                Shift(Settingss, Settingss.Margin, new Thickness(673, 34.6, 0, 0));
                await Task.Delay(450);
                Settingss.Visibility = Visibility.Visible;
                await Task.Delay(1000);
                FadeIn(Settings_Grid);
            }
            else
            {
                FadeOut(Settings_Grid);
                await Task.Delay(1000);
                FadeOut(Settingss);
                Shift(Settingss, Settingss.Margin, new Thickness(827, 35, -150, 0));
                await Task.Delay(1000);
                Settingss.Visibility = Visibility.Hidden;
            }
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            if (krnlbox.IsChecked == true)
            {
                KrnlAPI.ExecuteKrnl(ScriptEditor.Text);
            }
            else if (electronbox.IsChecked == true)
            {
                ElectronAPI.ExecuteElectron(ScriptEditor.Text);
            }
            else if (oxygenubox.IsChecked == true)
            {
                OxygenUAPI.ExecuteOxygenU(ScriptEditor.Text);
            }
            else if (Wearedevsbox.IsChecked == true)
            {
                WeAreDevsAPI.ExecuteWeAreDevs(ScriptEditor.Text);
            }
            else if (FluxusBox.IsChecked == true)
            {
                FluxusAPI.RunScript(ScriptEditor.Text);
            }
            else
            {
                MessageBox.Show("Select an API first before executing!", "Synapse X", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExecuteFileButton_Click(object sender, RoutedEventArgs e)
        {
            var OpenDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Script Files (*.lua, *.txt)|*.lua;*.txt",
                Title = "Synapse X - Execute File",
                FileName = ""
            };

            if (OpenDialog.ShowDialog() != true) return;

            try
            {
                if (krnlbox.IsChecked == true)
                {
                    KrnlAPI.ExecuteKrnl(File.ReadAllText(OpenDialog.FileName));
                }
                else if (electronbox.IsChecked == true)
                {
                    ElectronAPI.ExecuteElectron(File.ReadAllText(OpenDialog.FileName));
                }
                else if (oxygenubox.IsChecked == true)
                {
                    OxygenUAPI.ExecuteOxygenU(File.ReadAllText(OpenDialog.FileName));
                }
                else if (Wearedevsbox.IsChecked == true)
                {
                    WeAreDevsAPI.ExecuteWeAreDevs(File.ReadAllText(OpenDialog.FileName));
                }
                else if (FluxusBox.IsChecked == true)
                {
                    FluxusAPI.RunScript(File.ReadAllText(OpenDialog.FileName));
                }
                else
                {
                    MessageBox.Show("Select an API first before executing!", "Synapse X", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to read file. Check if it is accessible.", "Poltergeist A+ V2");
            }
        }

        private void AttachButton_Click(object sender, RoutedEventArgs e)
        {
            if (krnlbox.IsChecked == true)
            {
                KrnlAPI.InjectKrnl();
            }
            else if (electronbox.IsChecked == true)
            {
                ElectronAPI.Attachelectron();
            }
            else if (oxygenubox.IsChecked == true)
            {
                OxygenUAPI.Attach();
            }
            else if (Wearedevsbox.IsChecked == true)
            {
                WeAreDevsAPI.LaunchWeAreDevs();
            }
            else if (FluxusBox.IsChecked == true)
            {
                FluxusAPI.Attach();
            }
            else
            {
                MessageBox.Show("Select an API first before Injecting!", "Synapse X", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        #region AutoAttachCode
        private System.Windows.Forms.Timer timer1;

        private System.Windows.Forms.Timer timer2;

        private System.Windows.Forms.Timer timer3;

        private System.Windows.Forms.Timer timer4;

        private System.Windows.Forms.Timer timer5;

        private bool KrnlAutoAttach;
        private bool KrnlAutoAttach2;

        private async void KrnlAutoAttachTime(object sender, EventArgs e)
        {
            Process[] roblox = Process.GetProcessesByName("RobloxPlayerBeta"); //detect roblox
            if (roblox.Length > 0 && KrnlAutoAttach && KrnlAutoAttach2)
            {
                await Task.Delay(3000);//the time roblox loads for on most machines(increase if you think its too early)
                if (roblox.Length > 0 && KrnlAutoAttach && KrnlAutoAttach2) //double check if you closed it
                {
                    KrnlAPI.InjectKrnl();
                    KrnlAutoAttach = false; //lock the loop until look below
                }
            }
            if (roblox.Length == 0 && KrnlAutoAttach2) //when roblox is closed
            {
                KrnlAutoAttach = true; //unlock the loop
            }
        }

        private bool ElectronAutoAttach;
        private bool ElectronAutoAttach2;

        private async void ElectronAutoAttachTime(object sender, EventArgs e)
        {
            Process[] roblox = Process.GetProcessesByName("RobloxPlayerBeta"); //detect roblox
            if (roblox.Length > 0 && ElectronAutoAttach && ElectronAutoAttach2)
            {
                await Task.Delay(3000);//the time roblox loads for on most machines(increase if you think its too early)
                if (roblox.Length > 0 && ElectronAutoAttach && ElectronAutoAttach2) //double check if you closed it
                {
                    ElectronAPI.Attachelectron();
                    ElectronAutoAttach = false; //lock the loop until look below
                }
            }
            if (roblox.Length == 0 && ElectronAutoAttach2) //when roblox is closed
            {
                ElectronAutoAttach = true; //unlock the loop
            }
        }

        private bool OxygenUAutoAttach;
        private bool OxygenUAutoAttach2;

        private async void OxygenUAutoAttachTime(object sender, EventArgs e)
        {
            Process[] roblox = Process.GetProcessesByName("RobloxPlayerBeta"); //detect roblox
            if (roblox.Length > 0 && OxygenUAutoAttach && OxygenUAutoAttach2)
            {
                await Task.Delay(3000);//the time roblox loads for on most machines(increase if you think its too early)
                if (roblox.Length > 0 && OxygenUAutoAttach && OxygenUAutoAttach2) //double check if you closed it
                {
                    OxygenUAPI.Attach();
                    OxygenUAutoAttach = false; //lock the loop until look below
                }
            }
            if (roblox.Length == 0 && OxygenUAutoAttach2) //when roblox is closed
            {
                OxygenUAutoAttach = true; //unlock the loop
            }
        }

        private bool WearedevsAutoAttach;
        private bool WearedevsAutoAttach2;

        private async void WearedevsAutoAttachTime(object sender, EventArgs e)
        {
            Process[] roblox = Process.GetProcessesByName("RobloxPlayerBeta"); //detect roblox
            if (roblox.Length > 0 && WearedevsAutoAttach && WearedevsAutoAttach2)
            {
                await Task.Delay(3000);//the time roblox loads for on most machines(increase if you think its too early)
                if (roblox.Length > 0 && WearedevsAutoAttach && WearedevsAutoAttach2) //double check if you closed it
                {
                    WeAreDevsAPI.LaunchWeAreDevs();
                    WearedevsAutoAttach = false; //lock the loop until look below
                }
            }
            if (roblox.Length == 0 && WearedevsAutoAttach2) //when roblox is closed
            {
                WearedevsAutoAttach = true; //unlock the loop
            }
        }

        private bool FluxussAutoAttach;
        private bool FluxusAutoAttach2;
        private async void FluxusAutoAttachTime(object sender, EventArgs e)
        {
            Process[] roblox = Process.GetProcessesByName("RobloxPlayerBeta"); //detect roblox
            if (roblox.Length > 0 && FluxussAutoAttach && FluxusAutoAttach2)
            {
                await Task.Delay(3000);//the time roblox loads for on most machines(increase if you think its too early)
                if (roblox.Length > 0 && FluxussAutoAttach && FluxusAutoAttach2) //double check if you closed it
                {
                    FluxusAPI.Attach();
                    FluxussAutoAttach = false; //lock the loop until look below
                }
            }
            if (roblox.Length == 0 && FluxusAutoAttach2) //when roblox is closed
            {
                FluxussAutoAttach = true; //unlock the loop
            }
        }
        #endregion

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ScriptEditor.Text = "";
        }

        private void AutoAttachBox_Checked(object sender, RoutedEventArgs e)
        {
            if (krnlbox.IsChecked == true)
            {
                KrnlAutoAttach = true;
                KrnlAutoAttach2 = true;
            }
            else if (electronbox.IsChecked == true)
            {
                ElectronAutoAttach = true;
                ElectronAutoAttach2 = true;
            }
            else if (oxygenubox.IsChecked == true)
            {
                OxygenUAutoAttach = true;
                OxygenUAutoAttach2 = true;
            }
            else if (Wearedevsbox.IsChecked == true)
            {
                WearedevsAutoAttach = true;
                WearedevsAutoAttach2 = true;
            }
            else if (FluxusBox.IsChecked == true)
            {
                FluxussAutoAttach = true;
                FluxusAutoAttach2 = true;
            }
        }

        private void AutoAttachBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (krnlbox.IsChecked == true)
            {
                KrnlAutoAttach = false;
                KrnlAutoAttach2 = false;
            }
            else if (electronbox.IsChecked == true)
            {
                ElectronAutoAttach = false;
                ElectronAutoAttach2 = false;
            }
            else if (oxygenubox.IsChecked == true)
            {
                OxygenUAutoAttach = false;
                OxygenUAutoAttach2 = false;
            }
            else if (Wearedevsbox.IsChecked == true)
            {
                WearedevsAutoAttach = false;
                WearedevsAutoAttach2 = false;
            }
            else if (FluxusBox.IsChecked == true)
            {
                FluxussAutoAttach = false;
                FluxusAutoAttach2 = false;
            }
        }

        private void TopBord_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private List<string> collection = new List<string>();

        public string jsonLink = "https://raw.githubusercontent.com/Fujijared2810/SynapseX/main/SynapseScriptHub.json";

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ScriptPictureBox.Stretch = Stretch.Fill;
            JToken jToken = JObject.Parse(new WebClient().DownloadString(jsonLink))["scriptHub"];
            foreach (JToken item in jToken.Children())
            {
                ScriptBox1.Items.Add(string.Format("{0}", jToken[item.ToObject<JProperty>().Name]!["Name"]));
                collection.Add(jToken[item.ToObject<JProperty>().Name]!["Name"]!.ToString());
            }
            AutoAttachBox.IsChecked = Globals.Options.AutoAttach;
            FPSBox.IsChecked = Globals.Options.UnlockFPS;
            Topmostbox.IsChecked = Globals.Options.TopMost;
            OpacityBox.IsChecked = Globals.Options.Opacity;
            krnlbox.IsChecked = Globals.Options.krnl;
            oxygenubox.IsChecked = Globals.Options.OxygenU;
            electronbox.IsChecked = Globals.Options.Electron;
            Wearedevsbox.IsChecked = Globals.Options.WeAreDevs;
            FluxusBox.IsChecked = Globals.Options.Fluxus;
            await Task.Delay(1000);
            LoaderResize();
            await Task.Delay(750);
            GridMian.Visibility = Visibility.Visible;
            FadeIn(GridMian);
        }

        private void MiniButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private async void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            FadeOut(GridMian);
            await Task.Delay(750);
            LoaderResizeClose();
            await Task.Delay(850);
            Globals.Options = new Data.Options
            {
                AutoAttach = AutoAttachBox.IsChecked.Value,
                UnlockFPS = FPSBox.IsChecked.Value,
                TopMost = Topmostbox.IsChecked.Value,
                Opacity = OpacityBox.IsChecked.Value,
                krnl = krnlbox.IsChecked.Value,
                OxygenU = oxygenubox.IsChecked.Value,
                Electron = electronbox.IsChecked.Value,
                WeAreDevs = Wearedevsbox.IsChecked.Value,
                Fluxus = FluxusBox.IsChecked.Value,
            };
            DataInterface.Save("options", new Data.OptionsHolder
            {
                Version = Data.OptionsVersion,
                Data = JsonConvert.SerializeObject(Globals.Options)
            });
            Environment.Exit(0);
        }

        private async void ScriptHubButton_Click(object sender, RoutedEventArgs e)
        {
            ScriptHubButton.Content = "Starting...";
            await Task.Delay(850);
            FadeOut(GridMian);
            await Task.Delay(1000);
            ScriptHubResize();
            await Task.Delay(850);
            Scripthub_Grid.Visibility = Visibility.Visible;
            FadeIn(Scripthub_Grid);
            ScriptHubButton.Content = "Script Hub";
        }

        private void TopBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ScriptBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            JToken jToken = JObject.Parse(new WebClient().DownloadString(jsonLink))["scriptHub"];
            foreach (JToken item in jToken.Children())
            {
                try
                {
                    if (ScriptBox1.SelectedItem == null)
                    {
                        return;
                    }
                    if (jToken[item.ToObject<JProperty>().Name]!["Name"]!.ToString() == ScriptBox1.SelectedItem.ToString())
                    {
                        DescriptionBox.Text = jToken[item.ToObject<JProperty>().Name]!["Description"]!.ToString();
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(jToken[item.ToObject<JProperty>().Name]!["Picture"]!.ToString(), UriKind.Absolute);
                        bitmapImage.EndInit();
                        ScriptPictureBox.Source = bitmapImage;
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        private void ExecuteButton1_Click(object sender, RoutedEventArgs e)
        {
            if (ScriptBox1.SelectedItem == null)
            {
                return;
            }
            if (krnlbox.IsChecked == true)
            {
                JToken jToken = JObject.Parse(new WebClient().DownloadString(jsonLink))["scriptHub"];
                foreach (JToken item in jToken.Children())
                {
                    if (jToken[item.ToObject<JProperty>().Name]!["Name"]!.ToString() == ScriptBox1.SelectedItem.ToString())
                    {
                        Task.Delay(150);
                        KrnlAPI.ExecuteKrnl(new WebClient().DownloadString(jToken[item.ToObject<JProperty>().Name]!["source"]!.ToString()));
                    }
                }
            }
            else if (electronbox.IsChecked == true)
            {
                JToken jToken = JObject.Parse(new WebClient().DownloadString(jsonLink))["scriptHub"];
                foreach (JToken item in jToken.Children())
                {
                    if (jToken[item.ToObject<JProperty>().Name]!["Name"]!.ToString() == ScriptBox1.SelectedItem.ToString())
                    {
                        Task.Delay(150);
                        ElectronAPI.ExecuteElectron(new WebClient().DownloadString(jToken[item.ToObject<JProperty>().Name]!["source"]!.ToString()));
                    }
                }
            }
            else if (oxygenubox.IsChecked == true)
            {
                JToken jToken = JObject.Parse(new WebClient().DownloadString(jsonLink))["scriptHub"];
                foreach (JToken item in jToken.Children())
                {
                    if (jToken[item.ToObject<JProperty>().Name]!["Name"]!.ToString() == ScriptBox1.SelectedItem.ToString())
                    {
                        Task.Delay(150);
                        OxygenUAPI.ExecuteOxygenU(new WebClient().DownloadString(jToken[item.ToObject<JProperty>().Name]!["source"]!.ToString()));
                    }
                }
            }
            else if (Wearedevsbox.IsChecked == true)
            {
                JToken jToken = JObject.Parse(new WebClient().DownloadString(jsonLink))["scriptHub"];
                foreach (JToken item in jToken.Children())
                {
                    if (jToken[item.ToObject<JProperty>().Name]!["Name"]!.ToString() == ScriptBox1.SelectedItem.ToString())
                    {
                        Task.Delay(150);
                        WeAreDevsAPI.ExecuteWeAreDevs(new WebClient().DownloadString(jToken[item.ToObject<JProperty>().Name]!["source"]!.ToString()));
                    }
                }
            }
            else if (FluxusBox.IsChecked == true)
            {
                JToken jToken = JObject.Parse(new WebClient().DownloadString(jsonLink))["scriptHub"];
                foreach (JToken item in jToken.Children())
                {
                    if (jToken[item.ToObject<JProperty>().Name]!["Name"]!.ToString() == ScriptBox1.SelectedItem.ToString())
                    {
                        Task.Delay(150);
                        FluxusAPI.RunScript(new WebClient().DownloadString(jToken[item.ToObject<JProperty>().Name]!["source"]!.ToString()));
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an API Before using Script Hub!", "Synapse X", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }

        private async void CloseButton1_Click(object sender, RoutedEventArgs e)
        {
            CloseButton1.Content = "Closing...";
            await Task.Delay(850);
            FadeOut(Scripthub_Grid);
            await Task.Delay(1000);
            LoaderResize();
            await Task.Delay(850);
            FadeIn(GridMian);
            Scripthub_Grid.Visibility = Visibility.Hidden;
            CloseButton1.Content = "Close";
        }

        private void MiniButton1_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ScriptBox_Initialized(object sender, EventArgs e)
        {
            Directory.CreateDirectory("./scripts");

            void LoadListBoxFile(string directory, IEnumerable<string> fileTypes)
            {
                ScriptBox.Items.Clear();

                var directoryInfo = new DirectoryInfo(directory);
                foreach (var fileType in fileTypes)
                {
                    foreach (var fileInfo in directoryInfo.GetFiles(fileType))
                    {
                        var newItem = new ListBoxItem
                        {
                            Content = fileInfo.Name,
                            Tag = fileInfo.FullName,
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Stretch
                        };

                        newItem.Selected += (_, _) =>
                        {
                            ScriptEditor.CreateTab(
                                fileInfo.Name,
                                File.ReadAllText(fileInfo.FullName)
                            );

                            ScriptBox.SelectedIndex = -1;
                        };

                        ScriptBox.Items.Add(newItem);
                    }
                }
            }

            void ReloadListBox() => Dispatcher.Invoke(() => LoadListBoxFile("./scripts", new[] { "*.txt", "*.lua" }));

            FileSystemWatcher watcher = new()
            {
                Path = "./scripts",
                NotifyFilter =
                    NotifyFilters.Attributes
                    | NotifyFilters.CreationTime
                    | NotifyFilters.DirectoryName
                    | NotifyFilters.FileName
                    | NotifyFilters.LastAccess
                    | NotifyFilters.LastWrite
                    | NotifyFilters.Security
                    | NotifyFilters.Size
            };

            watcher.Created += (_, _) => ReloadListBox();
            watcher.Deleted += (_, _) => ReloadListBox();
            watcher.Renamed += (_, _) => ReloadListBox();
            watcher.Changed += (_, _) => ReloadListBox();

            watcher.Filter = "*.*";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            ReloadListBox();
        }
    }
}
