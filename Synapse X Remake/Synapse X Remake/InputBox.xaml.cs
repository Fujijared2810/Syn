using System;
using System.Collections.Generic;
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

namespace Synapse_X_Remake
{
    public partial class InputBox : Window
    {
        public enum InputDialogResult
        {
            Enter,
            Cancel
        }
        public class InputDialog
        {
            public string Name;
            public string Description;
            public string Text;

            private readonly InputBox dialog = new();

            public InputDialog(string name, string desc = "")
            {
                Name = name;
                Description = desc;
            }

            public InputDialogResult ShowDialog()
            {
                InputDialogResult result = InputDialogResult.Cancel;
                dialog.TitleLabel.Content = Name;
                dialog.DescriptionLabel.Content = Description;
                dialog.Title = Name;

                dialog.CloseButton.Click += delegate
                {
                    dialog.Close();
                };

                dialog.EnterBtutton.Click += delegate
                {
                    if (string.IsNullOrWhiteSpace(dialog.InputTextBox.Text))
                    {
                        MessageBox.Show("Please input text.", "Poltergeist A+ V2", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Text = dialog.InputTextBox.Text;
                    result = InputDialogResult.Enter;
                    dialog.Close();
                };

                dialog.ShowDialog();
                return result;
            }
        }
        public InputBox()
        {
            InitializeComponent();
        }
    }
}
