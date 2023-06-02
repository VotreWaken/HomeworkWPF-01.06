using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
using System.Xml.Linq;
using WpfApp11.Controls;


namespace WpfApp11
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Controller
        public Controller controller_;

        // Fails Input Count
        int Failsvalue;

        // Correct Input Count
        int CorrectValue;

        // Timer That Observe Correct Input Count And Update Display Every 1 Second
        System.Windows.Threading.DispatcherTimer CorrectInputDispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        // Timer That Observe The End of the Game, At the end of which displays Result ( Every 20 Seconds )
        System.Windows.Threading.DispatcherTimer ResultGameDispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        // Click Start Button Boolean Variable Using To Requests Data Base Words
        bool start;
        public MainWindow()
        {
            InitializeComponent();
            controller_ = new Controller();
            StartCorrectInputTimerWithSettings();
        }

        // Button Start Click
        private void StartBtnClick(object sender, RoutedEventArgs e)
        {
            if (start == true)
            {
                return;
            }

            controller_.AddEasyWords();
            if (SliderDifficulty.Value == 1)
            {
                controller_.AddEasyWords();
            }
            else if (SliderDifficulty.Value == 2)
            {
                controller_.AddMediumWords();
            }
            else if (SliderDifficulty.Value == 3)
            {
                controller_.AddHardWords();
            }
            start = true;
            List<string> a = controller_.GetAllWords();
            foreach (var item in a)
            {
                TBContent.Text += item + " ";
            }
            StartResultGameTimerWithSettings();
        }

        // Button Stop Click
        private void StopBtnClick(object sender, RoutedEventArgs e)
        {
            start = false;
            TBContent.Text = string.Empty;
            CorrectValue = 0;
            LBFailsNumber.Content = '0';
            Failsvalue = 0;
        }

        // Fill Words From Data Controller
        private List<string> FillWords()
        {
            controller_.ShuffleWords();
            return controller_.GetAllWords();
        }

        // Keyboard Input Buttons Click
        private void InputBtnClick(object sender, RoutedEventArgs e)
        {
            Button a = (Button)sender;
            try
            {
                if (TBContent.Text[0].ToString().ToUpper() == a.Content.ToString().ToUpper())
                {
                    TBContent.Text = TBContent.Text.Substring(1);
                    CorrectValue++;
                }
                else
                {
                    if (a.Content.ToString() == "Space" && TBContent.Text[0] == ' ')
                    {
                        TBContent.Text = TBContent.Text.Substring(1);
                        CorrectValue++;
                        return;
                    }
                    Failsvalue++;
                    LBFailsNumber.Content = Failsvalue;
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        // Handle KeyDown Event To Main Form
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            window.KeyDown += MainWindow_KeyDown;
        }

        // Override Default PreviewKeyDown Event On Main Form
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Space)
                {
                    e.Handled = true;
                    if (TBContent.Text[0] == ' ')
                    {
                        TBContent.Text = TBContent.Text.Substring(1);
                    }
                    else
                    {
                        Failsvalue++;
                        LBFailsNumber.Content = Failsvalue;
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        // Timer Correct Input Dispatcher Timer Tick
        private void CorrectInputDispatcherTimerTick(object sender, EventArgs e)
        {
            LBSpeedNumber.Content = (CorrectValue);
        }
        // Timer Result Game Dispatcher Timer Tick
        private void ResultGameDispatcherTimerTick(object sender, EventArgs e)
        {
            start = false;
            MessageBox.Show($"Congratulations, Chars/ Min: {CorrectValue}");
            ResultGameDispatcherTimer.Stop();
            TBContent.Text = string.Empty;
            CorrectValue = 0;
            LBFailsNumber.Content = '0';
            Failsvalue = 0;
        }

        // Start Correct Input Timer With Settings
        private void StartCorrectInputTimerWithSettings()
        {
            CorrectInputDispatcherTimer.Tick += new EventHandler(CorrectInputDispatcherTimerTick);
            CorrectInputDispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            CorrectInputDispatcherTimer.Start();
        }

        // Start Result Game Timer With Settings
        private void StartResultGameTimerWithSettings()
        {
            ResultGameDispatcherTimer.Tick += new EventHandler(ResultGameDispatcherTimerTick);
            ResultGameDispatcherTimer.Interval = new TimeSpan(0, 0, 20);
            ResultGameDispatcherTimer.Start();
        }

        // KeyDown Event To Handle Keyboard Input On Main Form
        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show("You Entered: " + e.Key);
            try
            {
                if (CBCaseSensetive.IsChecked == false)
                {
                    if ((Keyboard.GetKeyStates(Key.CapsLock) & KeyStates.Toggled) == KeyStates.Toggled)
                    {
                        Zero.Background = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        Zero.Background = new SolidColorBrush(Colors.Gray);
                    }

                    if (TBContent.Text[0].ToString().ToUpper() == e.Key.ToString())
                    {
                        TBContent.Text = TBContent.Text.Substring(1);
                        CorrectValue++;
                    }
                    else
                    {
                        if (e.Key == Key.Capital)
                        {
                            return;
                        }
                        Failsvalue++;
                        LBFailsNumber.Content = Failsvalue;
                    }
                }
                else
                {

                    if ((Keyboard.GetKeyStates(Key.CapsLock) & KeyStates.Toggled) == KeyStates.Toggled)
                    {
                        Zero.Background = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        Zero.Background = new SolidColorBrush(Colors.Gray);
                    }

                    if (TBContent.Text[0].ToString() == ((Keyboard.GetKeyStates(Key.CapsLock) & KeyStates.Toggled) == KeyStates.Toggled ? e.Key.ToString().ToUpper() : e.Key.ToString().ToLower()))
                    {
                        TBContent.Text = TBContent.Text.Substring(1);
                        CorrectValue++;
                    }
                    else
                    {
                        if (e.Key == Key.Capital)
                        {
                            return;
                        }
                        Failsvalue++;
                        LBFailsNumber.Content = Failsvalue;
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        // Slider Value Changed 
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = e.OriginalSource as Slider;

            if (slider != null)
            {
                DifficultyValue.Content = slider.Value.ToString();
            }
        }

        // Text Box Content Text Changed 
        private void TBContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TBContent.Text == string.Empty && start == true)
            {
                List<string> a = FillWords();
                foreach (var item in a)
                {
                    TBContent.Text += item + " ";
                }
            }
        }
    }
}
