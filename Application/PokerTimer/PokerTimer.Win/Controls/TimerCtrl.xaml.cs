using System;
using System.ComponentModel;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using PokerTimer.BusinessObjects;

namespace PokerTimer.Win.Controls
{
    public partial class TimerCtrl : UserControl, INotifyPropertyChanged
    {
        # region Properities

        private string _timerText;
        public string TimerText
        {
            get
            {
                return _timerText;
            }
            set
            {
                _timerText = value;
                Refresh();
            }
        }

        private int _levelTime;
        public int LevelTimeMinutes
        {
            get
            {
                return _levelTime;
            }
            set
            {
                _seconds = 1;
                _levelTime = value;
                SetText(_levelTime * 60);
                Refresh();
            }
        }

        private DispatcherTimer Timer = new DispatcherTimer();
        SpeechSynthesizer Synthesizer = new SpeechSynthesizer();

        # region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void PropertyChange(Property property)
        {
            PropertyChanged.Raise(this, property.ToString());
        }

        private enum Property
        {
            NotSet = 0,
            TimerText
        }

        private void Refresh()
        {
            PropertyChange(Property.TimerText);
        }

        # endregion

        # endregion

        public event RoutedEventHandler LevelCompleted;

        public TimerCtrl()
        {
            InitializeComponent();
            DataContext = this;

            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += Timer_Tick;

            Synthesizer.Volume = 100;  // 0...100
            Synthesizer.Rate = 3;     // -10...10
        }

        private int _seconds;

        private void Timer_Tick(object sender, EventArgs e)
        {
            var totalSecondCount = LevelTimeMinutes * 60 - _seconds;

            SetText(totalSecondCount);

            if (totalSecondCount == 1)
            {
                if (LevelCompleted != null)
                    LevelCompleted(this, new RoutedEventArgs());
            }
            else
            {
                _seconds++;
            }
        }

        public void ToggleStartStop()
        {
            if (Timer.IsEnabled)
                Timer.IsEnabled = false;
            else
                Timer.IsEnabled = true;

            Refresh();
        }

        private void SetText(int seconds)
        {
            TimerText = string.Format("{0:00} : {1:00}", seconds / 60, seconds % 60);
        }
    }
}