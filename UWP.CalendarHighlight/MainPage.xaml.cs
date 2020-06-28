using Microsoft.Toolkit.Uwp.UI.Extensions;
using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace UWP.CalendarHighlight
{
    public sealed partial class MainPage : Page
    {
        private List<DateTimeOffset> _highlightedDates = new List<DateTimeOffset>();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _highlightedDates.Add(Picker.Date.Date);
            UpdateCalendar();
        }

private void UpdateCalendar()
{
    var displayedDays = Calendar.FindDescendants<CalendarViewDayItem>();
    foreach (var displayedDay in displayedDays)
    {
        if (_highlightedDates.Contains(displayedDay.Date.Date))
        {
            HighlightDay(displayedDay);
        }
        else
        {
            UnHighlightDay(displayedDay);
        }
    }
}

        private void Calendar_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
        {
            if (_highlightedDates.Contains(args.Item.Date))
            {
                HighlightDay(args.Item);
            }
            else
            {
                UnHighlightDay(args.Item);
            }
        }

        private static void HighlightDay(CalendarViewDayItem displayedDay)
        {
            displayedDay.Background = new SolidColorBrush(Colors.Red);
        }

        private static void UnHighlightDay(CalendarViewDayItem displayedDay)
        {
            displayedDay.Background = new SolidColorBrush(Colors.Transparent);
        }
    }
}
