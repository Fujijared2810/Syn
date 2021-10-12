using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Synapse_X_Remake.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using static Synapse_X_Remake.InputBox;
using Path = System.IO.Path;

namespace Synapse_X_Remake.Controls
{
    public partial class ScriptTab : UserControl
    {
        private string directory;

        public ScriptTab()
        {
            InitializeComponent();
            DataContext = this;
        }
        private TextEditor CreateNewEditor(string content)
        {
            TextEditor editor = new()
            {
                Foreground = Brushes.WhiteSmoke,
                Background = Brushes.Transparent,
                FontFamily = new FontFamily("Segoe UI"),
                Margin = new Thickness(1),
                ShowLineNumbers = true,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                Text = content
            };
            editor.Options.EnableEmailHyperlinks = false;
            editor.Options.EnableHyperlinks = false;
            Stream xshd_stream = File.OpenRead(Environment.CurrentDirectory + @"\bin\" + "lua.xshd");
            XmlTextReader xshd_reader = new(xshd_stream);
            editor.SyntaxHighlighting = HighlightingLoader.Load(xshd_reader, HighlightingManager.Instance);

            xshd_reader.Close();
            xshd_stream.Close();

            editor.TextChanged += delegate
            {
                int offset = editor.CaretOffset;
                DocumentLine line = editor.Document.GetLineByOffset(offset);
                editor.ScrollToLine(line.LineNumber);
            };

            return editor;
        }
        public void AddTab(string file)
        {
            FileInfo info = new(file);
            var content = File.ReadAllText(file);

            var editor = CreateNewEditor(content);

            TabItem item = new()
            {
                Header = info.Name,
                Content = editor,
                Tag = file
            };

            StackPanel panel = new() { Orientation = Orientation.Horizontal };
            TextBlock text = new()
            {
                Name = "ScriptName",
                Text = info.Name,
                Foreground = (SolidColorBrush)TryFindResource("PrimaryTextColor"),
                FontSize = 12
            };

            Button editButton = new()
            {
                Name = "EditButton",
                Content = "\xE70F",
                FontSize = 8,
                Width = 12,
                Height = 12,
                Margin = new Thickness(5, 0, 0, 0),
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                Visibility = Visibility.Collapsed,
                Style = (Style)TryFindResource("BlankButton")
            };

            Button removeButton = new()
            {
                Name = "RemoveButton",
                Content = "\xE8BB",
                FontSize = 8,
                Width = 12,
                Height = 12,
                Margin = new Thickness(5, 0, 0, 0),
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                Visibility = Visibility.Collapsed,
                Style = (Style)TryFindResource("BlankButton")
            };

            Settings.Default.PropertyChanged += delegate (object _, PropertyChangedEventArgs args)
            {
                if (args.PropertyName == "Theme")
                {
                    text.Foreground = removeButton.Foreground = editButton.Foreground = (SolidColorBrush)TryFindResource("PrimaryTextColor");
                }
            };

            panel.Children.Add(text);
            panel.Children.Add(editButton);
            panel.Children.Add(removeButton);

            editButton.Click += delegate
            {
                InputDialog input = new("File's name", "Rename your file");
                if (input.ShowDialog() != InputDialogResult.Enter) return;
                var fileName = Path.Combine(directory, input.Text);

                if (File.Exists(fileName))
                {
                    MessageBox.Show("ERROR: File already exists!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                EditTab(item, text, fileName);
            };

            removeButton.Click += delegate
            {
                if (Tab.Items.Count == 1) return;
                RemoveTab(item);
            };

            item.Header = panel;
            item.Height = 24;

            Tab.Items.Add(item);

            DispatcherTimer timer = new()
            {
                Interval = TimeSpan.FromSeconds(5)
            };

            timer.Tick += delegate
            {
                Save(item);
            };

            Tab.SelectionChanged += delegate
            {
                if (item.IsSelected)
                {
                    editButton.Visibility = Visibility.Visible;
                    removeButton.Visibility = Visibility.Visible;
                }

                else
                {
                    editButton.Visibility = Visibility.Collapsed;
                    removeButton.Visibility = Visibility.Collapsed;

                    Save(item);
                }

                timer.IsEnabled = item.IsSelected;
            };
        }
        public void CreateTab(string fileName, string content = "")
        {
            fileName = Path.Combine(directory, fileName);
            if (File.Exists(fileName))
            {
                MessageBox.Show("File already exists!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            File.WriteAllText(fileName, content);
            AddTab(fileName);
        }

        public void RemoveTab(TabItem item)
        {
            string file = (string)item.Tag;
            if (File.Exists(file))
                File.Delete(file);

            Tab.Items.Remove(item);
        }

        public void EditTab(TabItem item, TextBlock textBlock, string name)
        {
            string oldFileName = (string)item.Tag;
            FileInfo newFileInfo = new(name);

            if (File.Exists(oldFileName))
            {
                File.Move(oldFileName, name);
                textBlock.Text = newFileInfo.Name;
                item.Tag = name;
            }

            else
                Tab.Items.Remove(item);
        }

        public void RefreshAllTabs(bool SaveAll = true)
        {
            if (SaveAll) SaveAllTabs();
            string[] files = Directory.GetFiles(directory);
            if (files.Length == 0)
            {
                File.WriteAllText(Path.Combine(directory, "Script 1"), "");
                files = Directory.GetFiles(directory);
            }

            Tab.Items.Clear();
            foreach (string file in files)
                AddTab(file);
            Tab.SelectedIndex = 0;
        }

        public void Save(TabItem item)
        {
            var file = (string)item.Tag;
            if (!File.Exists(file)) return;
            var editor = (TextEditor)item.Content;
            File.WriteAllText(file, editor.Text);

            Debug.WriteLine("Saved " + file);
        }

        public void SaveAllTabs()
        {
            foreach (TabItem item in Tab.Items)
            {
                Save(item);
            }
        }

        public string Text
        {
            get
            {
                string returnText = "";
                if (Tab.Items.Count > 0)
                    returnText = ((TextEditor)Tab.SelectedContent).Text;
                return returnText;
            }

            set
            {
                if (Tab.Items.Count > 0)
                    ((TextEditor)Tab.SelectedContent).Text = value;
            }
        }

        public void InitializeDirectory(string dir)
        {
            directory = dir;
            RefreshAllTabs(false);
        }

        private ICommand _clickCommand;

        public ICommand ClickCommand
        {
            get { return _clickCommand ??= new CommandHandler(MyAction, () => CanExecute); }
        }

        public bool CanExecute =>
            // check if executing is allowed, i.e., validate, check if a process is running, etc. 
            true;

        public void MyAction()
        {
            InputDialog input = new("File's name", "Input your file's name here.");
            if (input.ShowDialog() == InputDialogResult.Enter)
            {
                CreateTab(input.Text);
            }
        }

        public class CommandHandler : ICommand
        {
            private readonly Action _action;
            private readonly Func<bool> _canExecute;

            /// <summary>
            /// Creates instance of the command handler
            /// </summary>
            /// <param name="action">Action to be executed by the command</param>
            /// <param name="canExecute">A bolean property to containing current permissions to execute the command</param>
            public CommandHandler(Action action, Func<bool> canExecute)
            {
                _action = action;
                _canExecute = canExecute;
            }

            /// <summary>
            /// Wires CanExecuteChanged event 
            /// </summary>
            public event EventHandler CanExecuteChanged
            {
                add => CommandManager.RequerySuggested += value;
                remove => CommandManager.RequerySuggested -= value;
            }

            /// <summary>
            /// Forcess checking if execute is allowed
            /// </summary>
            /// <param name="parameter"></param>
            /// <returns></returns>
            public bool CanExecute(object parameter)
            {
                return _canExecute.Invoke();
            }

            public void Execute(object parameter)
            {
                _action();
            }
        }
    }
}
