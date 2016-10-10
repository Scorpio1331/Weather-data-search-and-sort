using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather_Data_Search_and_Sort
{
    class WeatherData
    {
        #region "Variables + Properties"
        private string _stationName;
        private IDictionary<string, int> _daysOfAirFrost;
        private IDictionary<string, float> _totalRainfall;
        private IDictionary<string, float> _totalSunshineDuration;
        private IDictionary<string, float> _dailyTemperatureMaximum;
        private IDictionary<string, float> _dailyTemperatureMinimum;
        public string StationName
        {
            get
            {
                return _stationName;
            }

            set
            {
                _stationName = value;
            }
        }
        public IDictionary<string, int> DaysOfAirFrost
        {
            get
            {
                return _daysOfAirFrost;
            }

            set
            {
                _daysOfAirFrost = value;
            }
        }
        public IDictionary<string, float> TotalRainfall
        {
            get
            {
                return _totalRainfall;
            }

            set
            {
                _totalRainfall = value;
            }
        }
        public IDictionary<string, float> TotalSunshineDuration
        {
            get
            {
                return _totalSunshineDuration;
            }

            set
            {
                _totalSunshineDuration = value;
            }
        }
        public IDictionary<string, float> DailyTemperatureMaximum
        {
            get
            {
                return _dailyTemperatureMaximum;
            }

            set
            {
                _dailyTemperatureMaximum = value;
            }
        }
        public IDictionary<string, float> DailyTemperatureMinimum
        {
            get
            {
                return _dailyTemperatureMinimum;
            }

            set
            {
                _dailyTemperatureMinimum = value;
            }
        }
        #endregion

        

    }
}
