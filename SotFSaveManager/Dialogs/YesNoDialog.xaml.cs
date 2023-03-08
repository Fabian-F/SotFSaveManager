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

namespace SotFSaveManager.Dialogs
{
    /// <summary>
    /// Interaktionslogik für YesNoDialog.xaml
    /// </summary>
    public partial class YesNoDialog : Window
    {
        public enum ResultType
        {
            Cancel, No, Yes
        }

        private ResultType _result;

        public ResultType Result
        {
            get { return _result; }
            set { _result = value; Close(); }
        }

        public YesNoDialog(string text, string title)
        {
            InitializeComponent();
            label.Text = text;
            lbl_title.Content = title;
            Title = title;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = ResultType.Cancel;
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            Result = ResultType.No;
        }

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            Result = ResultType.Yes;
        }
    }
}
