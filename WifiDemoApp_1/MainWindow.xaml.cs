﻿#region Using

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

#endregion

namespace WifiDemoApp_1
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly SolidColorBrush _off = new SolidColorBrush(Color.FromRgb(160, 160, 160));
        private readonly SolidColorBrush _on = new SolidColorBrush(Color.FromRgb(130, 190, 125));

        public MainWindow()
        {
            InitializeComponent();
            CreateDispatcherTimer();
        }

        private bool _iseditenabled = false;
        public bool IsEnabledItem2
        {
            get
            {
                return _iseditenabled;
                //return !string.IsNullOrEmpty(WirelessPasswordInput?.Text) &&
                //       !string.IsNullOrEmpty(NetworkNameInput?.Text);
            }
            set { _iseditenabled = value; }
        }

        private void CreateDispatcherTimer()
        {
            var timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal,
                delegate
                {
                    DateTimeTextBlock.Text = DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss");
                    IsEnabledItem2 = true;
                }, Dispatcher);
        }

        private void ToggleButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OutputOfaProgram.Text = "";


            if (SwitchUserControlButton.ToogledBooleanPublic == false)
            {
                RunCmd("netsh wlan stop hostednetwork");
                LightEllipse.Fill = _off;
                DisplayTextBlock.Text = "OFF";
            }
            else
            {
                if (string.IsNullOrEmpty(WirelessPasswordInput.Text) ||
                    string.IsNullOrEmpty(NetworkNameInput.Text))
                {
                    Console.WriteLine(Properties.Resources.Invalid);
                    SwitchUserControlButton.ToogledBooleanPublic = false;
                    return;
                }

                LightEllipse.Fill = _on;
                DisplayTextBlock.Text = "ON";

                RunCmd("netsh wlan set hostednetwork mode=allow ssid=Ivan_Hotspot key=izagar564");
                RunCmd("netsh wlan start hostednetwork");
            }
        }

        /// <summary>
        ///     Metoda koja pokrece windows wifi
        /// </summary>
        private void RunCmd(string command)
        {
            //netsh wlan set hostednetwork mode = allow ssid = Ivan_Hotspot key = izagar564
            //netsh wlan start hostednetwork

            // Start the child process.
            var process = new Process
            {
                StartInfo =
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = "cmd.exe",
                    Arguments = $"/c call {command}",
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };
            // Redirect the output stream of the child process.
            // Do not create the black window.

            process.OutputDataReceived += OutputHandler;
            process.ErrorDataReceived += OutputHandler;

            process.Start();
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            //odjavi se s pretplata nakon što si sve pročitao
            process.OutputDataReceived -= OutputHandler;
            process.ErrorDataReceived -= OutputHandler;
        }


        private void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //This is a common problem with people getting started.Whenever you 
            //update your UI elements from a thread other than the main thread, 
            //you need to use:

            //Using Dispatcher.Invoke is a synchronous call, so it is effectively 
            //the same as doing this on the UI thread, because the call to Invoke 
            //    blocks until the UI performs the requested task. If you're doing
            //    that too much in a short time span you're effectively blocking the 
            //    UI thread.
            Dispatcher.BeginInvoke(new Action(() => { OutputOfaProgram.Text += outLine.Data + "\n"; }),
                DispatcherPriority.ApplicationIdle);
        }

        private void Test_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // e.CanExecute = (txtEditor != null) && (txtEditor.SelectionLength > 0);
        }

        private void Test_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //txtEditor.Cut();
        }

        private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void WirelessPasswordInput_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        #region Network section

        private void OpenNetworkConnections_OnClick(object sender, RoutedEventArgs e)
        {
            var startInfo = new ProcessStartInfo("NCPA.cpl") {UseShellExecute = true};

            Process.Start(startInfo);
        }

        #endregion

        #region Main section

        //TODO: this potentiali whont be event used
        private void NewConfiguration_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("NOT IMPLEMENTED YET", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void OpenConfiguration_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("NOT IMPLEMENTED YET", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SaveConfiguration_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("NOT IMPLEMENTED YET", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        #region About section

        private void AboutAuthor_OnClick(object sender, RoutedEventArgs e)
        {
            //TODO: napraviti ovo kao custom window gdje je sve lijepo poredano
            MessageBox.Show("This section is about author", "About Author", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void AboutProgram_OnClick(object sender, RoutedEventArgs e)
        {
            //TODO: napraviti ovo kao next next tutorial
            MessageBox.Show("This section is about how to use this program", "About Program", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        #endregion
    }
}