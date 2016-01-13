using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WpfApplication1
{
    public partial class MainWindow : Window
    {
        private String pathToFile = @"C:\Stream\Firma\XWD1\XWD1.bat";
        private bool toContinue = true;
        private bool threadTaken = false;
        public MainWindow()
        {
            InitializeComponent();
            pathTBX.Text = pathToFile;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (!threadTaken)
            {
                toContinue = true;
                pathToFile = System.IO.Path.GetFullPath(pathTBX.Text);
                new Thread(() =>
                {

                    threadTaken = true;
                    Thread.CurrentThread.IsBackground = true;

                    while (toContinue)
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(pathToFile);

                            this.stack_panel.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                            {
                                Label l = new Label();

                            l.Content = "Uruchomiono program: " + pathToFile
                            + "\nO: " + DateTime.Now.ToString("h:mm:ss tt");

                            stack_panel.Children.Add(l);
                            }));

                            Thread.Sleep(180000);
                        }
                        catch (Exception ex)
                        {
                            this.stack_panel.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                            {
                                Label l = new Label();

                                l.Content = ex.Message;


                                stack_panel.Children.Add(l);
                                
                                threadTaken = false;
                                toContinue = false;
                            }));
                        }
                    }
                    threadTaken = false;
                }).Start();
            }
        }

        private void stop_button_Click(object sender, RoutedEventArgs e)
        {
            toContinue = false;
        }

        private void pathTBX_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void browse_button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)       
                pathToFile = dlg.FileName;

        }
    }
}
