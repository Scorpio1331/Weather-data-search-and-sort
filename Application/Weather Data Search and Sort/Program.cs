using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Weather_Data_Search_and_Sort
{
    class Program
    {

        /// <summary>
        /// Create Dictionary and property to hold years imported from file (Dictionaries have quicker lookup time for lots of items compared to lists)
        /// </summary>
        private static IDictionary<int, string> _yearsDictionary = new Dictionary<int,string>();
        /// <summary>
        /// Create string array to hold months for easy referencing
        /// </summary>
        public static readonly string[] Months = {"December","January","February","March","April","May","June","July","August","September","October","November"};
        /// <summary>
        /// Create constant string to point to weather data files folder
        /// </summary>
        private const string BaseFilePath = @"Weather Data\";
        public static IDictionary<int, string> YearsDictionary
        {
            get
            {
                return _yearsDictionary;
            }

            set
            {
                _yearsDictionary = value;
            }
        }

        /// <summary>
        /// Create variables and properties for each weather weatherStation to hold weather data
        /// </summary>
        private static WeatherData _lerwichData = new WeatherData { StationName = "Lerwich" };
        private static WeatherData _rossOnWyeData = new WeatherData {StationName = "Ross on Wye"};
        public static WeatherData LerwichData
        {
            get { return _lerwichData; }
            private set { _lerwichData = value; }
        }
        public static WeatherData RossOnWyeData
        {
            get { return _rossOnWyeData; }
            private set { _rossOnWyeData = value; }
        }

        /// <summary>
        /// Create variables for outputting data to webpage rather than console if user wishes
        /// </summary>
        private static bool outputWeb = false;
        private static IList<string> _webData = new List<string>();
        private static IList<string> _webDataHeaders = new List<string>();
        public static IList<string> WebData
        {
            get { return _webData; }
            set { _webData = value; }
        }
        public static IList<string> WebDataHeaders
        {
            get
            {
                return _webDataHeaders;
            }

            set
            {
                _webDataHeaders = value;
            }
        }


        static void Main(string[] args)
        {
            Console.Title = "Weather Data searcher/sorter"; //Set applications title

            MainMenu(); //Call procedure to provide user with main menu


            Console.ReadKey(true);   
        }

#region "Menus"

        /// <summary>
        /// Procedure to provide user with main menu
        /// </summary>
        private static void MainMenu()
        {
            bool exit = false;
            do
            { //loop until exit and print menu to console
                Console.WriteLine("******* Welcome to the weather data searcher and sorter *******");
                Console.WriteLine("\nMain Menu:\nPress I to import data\nPress L to go to Lerwich Weather weatherStation menu\nPress R to go to Ross on Wye Weather weatherStation menu" +
                                  "\nPress B to go to Both Weather stations menu\nPress W to output data to a webpage\nPress Q to quit\n");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.I:
                        ImportDataMenu(); //Call procedure to provide user with menu to import data
                        break;
                    case ConsoleKey.L:
                        WeatherStationMenu(LerwichData); //Call procedure to provide user with menu to manipulate lerwich's data
                        break;
                    case ConsoleKey.R:
                        WeatherStationMenu(RossOnWyeData);//Call procedure to provide user with menu to manipulate ross on wye's data
                        break;
                    case ConsoleKey.B:
                        BothWeatherStationsMenu(LerwichData, RossOnWyeData);//Call procedure to provide user with menu to manipulate both stations's data
                        break;
                    case ConsoleKey.W:
                        outputWeb = !outputWeb; //Toggle outputting to web or console
                        break;
                    case ConsoleKey.Q: //Exit loop
                        exit = true;
                        break;
                    default:
                        break;
                }

                Console.Clear();
            } while (!exit);
        }

        /// <summary>
        /// Procedure to provide menu for manipulating both weather stations data to user
        /// </summary>
        /// <param name="weatherStation1">First weather station to retrieve data from</param>
        /// <param name="weatherStation2">Second weather station to retrieve data from</param>
        private static void BothWeatherStationsMenu(WeatherData weatherStation1,WeatherData weatherStation2)
        {
            bool exit = false;
            IList<string> selectedDataListsLerwich = new List<string>();
            IList<string> selectedDataListsRossOnWye = new List<string>();
            do
            { //Loop until exit and print menu to console
                Console.Clear();
                Console.WriteLine($"******* Menu for manipulating {weatherStation1.StationName} & {weatherStation2.StationName}'s weather data *******");
                Console.WriteLine("\nPress L to select data lists\nPress V to view selected data" +
                                  "\nPress A to sort first selected list by ascending order" +
                                   "\nPress D to sort first selected list by descending order\nPress Y to search according to the content of the Year" +
                                   "\nPress M to search according to the content of the Month\nPress S to view statistics" +
                                   "\nPress B to go back\n");
                PrintSelectedDataLists(selectedDataListsLerwich.ToArray());
                PrintSelectedDataLists(selectedDataListsRossOnWye.ToArray()); //Call functions to print selected data dictionaries to console
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.L:
                        ChooseDataLists(selectedDataListsLerwich,weatherStation1.StationName); //Call functions to provide user with menu to select data dictionaries for both stations
                        ChooseDataLists(selectedDataListsRossOnWye,weatherStation2.StationName);
                        break;
                    case ConsoleKey.V:
                        switch (CheckDataListsSelected(selectedDataListsLerwich.ToArray(), selectedDataListsRossOnWye.ToArray())) //Call function to check what data dictionaries have been selected
                        { //and switch on result
                            case -1:
                                ViewSelectedData(selectedDataListsLerwich.ToArray(), weatherStation1); //If only data dictionaries for Lerwich station selected print their data
                                break;
                            case 0:
                                ViewSelectedData(selectedDataListsRossOnWye.ToArray(), weatherStation2);//only data dictionaries for Ross on Wye station selected print their data
                                break;
                            case 1:
                                selectedDataListsLerwich.Add("Lerwich"); //Else add seperating name of Lerwich to split each set of selected dictionaries
                                string[] selectedDataLists = selectedDataListsLerwich.Concat(selectedDataListsRossOnWye).ToArray();
                                selectedDataListsLerwich.Remove("Lerwich"); //Concatenate both lists and call function to view data
                                ViewSelectedData(weatherStation1, selectedDataLists, weatherStation2);
                                break;
                        }
                       
                        break;
                    case ConsoleKey.A:
                        switch (CheckDataListsSelected(selectedDataListsLerwich.ToArray(),selectedDataListsRossOnWye.ToArray()))
                        {
                            case -1:
                                ViewSelectedData(selectedDataListsLerwich.ToArray(), weatherStation1, true); //Call function to view lerwich data in ascending order
                                break;
                            case 0:
                                ViewSelectedData(selectedDataListsRossOnWye.ToArray(), weatherStation2, true); //Call function to view ross on wye data in ascending order
                                break;
                            case 1:
                                selectedDataListsLerwich.Add("Lerwich");
                                string[] selectedDataLists = selectedDataListsLerwich.Concat(selectedDataListsRossOnWye).ToArray();
                                selectedDataListsLerwich.Remove("Lerwich");
                                ViewSelectedData(selectedDataLists, weatherStation1, true, weatherStation2); //Call function to view both stations data in ascending order
                                break;
                        }
                        break;
                    case ConsoleKey.D:
                        switch (CheckDataListsSelected(selectedDataListsLerwich.ToArray(), selectedDataListsRossOnWye.ToArray()))
                        {
                            case -1:
                                ViewSelectedData(selectedDataListsLerwich.ToArray(), weatherStation1, false);
                                break;
                            case 0:
                                ViewSelectedData(selectedDataListsRossOnWye.ToArray(), weatherStation2, false);
                                break;
                            case 1:
                                selectedDataListsLerwich.Add("Lerwich");
                                string[] selectedDataLists = selectedDataListsLerwich.Concat(selectedDataListsRossOnWye).ToArray();
                                selectedDataListsLerwich.Remove("Lerwich");
                                ViewSelectedData(selectedDataLists, weatherStation1, false, weatherStation2);//Call function to view both stations data in descending order
                                break;
                        }
                        
                        break;
                    case ConsoleKey.Y:
                        switch (CheckDataListsSelected(selectedDataListsLerwich.ToArray(), selectedDataListsRossOnWye.ToArray()))
                        {
                            case 1: //If both stations have selected data dictionaries then call function to get year to search for from user and view data from that year
                                selectedDataListsLerwich.Add("Lerwich");
                                string[] selectedDataLists = selectedDataListsLerwich.Concat(selectedDataListsRossOnWye).ToArray();
                                selectedDataListsLerwich.Remove("Lerwich");
                                string year = SearchMenu(weatherStation1.StationName, true, weatherStation2.StationName);
                                if (string.IsNullOrWhiteSpace(year))
                                    break;

                                ViewSelectedData(selectedDataLists, weatherStation1, year, null, weatherStation2);
                                break;
                        }
                        break;
                    case ConsoleKey.M:
                        switch (CheckDataListsSelected(selectedDataListsLerwich.ToArray(), selectedDataListsRossOnWye.ToArray()))
                        {
                            case 1: //If both stations have selected data dictionaries then call function to get month to search for from user and view data from that month
                                selectedDataListsLerwich.Add("Lerwich");
                                string[] selectedDataLists = selectedDataListsLerwich.Concat(selectedDataListsRossOnWye).ToArray();
                                selectedDataListsLerwich.Remove("Lerwich");
                                string month = SearchMenu(weatherStation1.StationName, false, weatherStation2.StationName);
                                if (string.IsNullOrWhiteSpace(month))
                                    break;

                                ViewSelectedData(selectedDataLists, weatherStation1, null, month, weatherStation2);
                                break;
                        }
                        
                        break;
                    case ConsoleKey.S:
                        switch (CheckDataListsSelected(selectedDataListsLerwich.ToArray(),selectedDataListsRossOnWye.ToArray()))
                        {
                            case -1: 
                                StatisticsMenu(selectedDataListsLerwich.ToArray(), weatherStation1); //Call procedure to provide user with statistics menu for first station
                                break;
                            case 0:
                                StatisticsMenu(selectedDataListsRossOnWye.ToArray(), weatherStation2);//Call procedure to provide user with statistics menu for second station
                                break;
                            case 1:
                                selectedDataListsLerwich.Add("Lerwich"); //Call procedure to provide user with statistics menu for both stations
                                string[] selectedDataLists = selectedDataListsLerwich.Concat(selectedDataListsRossOnWye).ToArray();
                                selectedDataListsLerwich.Remove("Lerwich");
                                StatisticsMenu(selectedDataLists, weatherStation1, weatherStation2);
                                break;
                        }
                        break;
                    case ConsoleKey.B: //Exit loop
                        exit = true;
                        break;
                    default:
                        break;
                }
            } while (!exit);
        }

        /// <summary>
        /// Function to check whether two data list contain any values and return a integer to corrospond to the result
        /// </summary>
        /// <param name="dataList1">Array of data dictionaries to check</param>
        /// <param name="dataList2">Array of data dictionaries to check</param>
        /// <returns>Returns -1 if only first datalist contains values, 0 if only second datalist contains value or 1 if both, returns 2 on error</returns>
        private static int CheckDataListsSelected(string[] dataList1,string[] dataList2)
        {
            if (CheckDataListsSelected(dataList1) &&
                !CheckDataListsSelected(dataList2))
                return -1;
            if (!CheckDataListsSelected(dataList1) &&
                CheckDataListsSelected(dataList2))
                return 0;
            if (CheckDataListsSelected(dataList1) &&
                CheckDataListsSelected(dataList2))
                return 1;
            return 2;
        }


        /// <summary>
        /// Provide menu for selected weather station
        /// </summary>
        /// <param name="weatherStation">weather station to provide menu for</param>
        private static void WeatherStationMenu(WeatherData weatherStation)
        {
            bool exit = false;
            IList<string> selectedDataLists = new List<string>();
            do
            {
                Console.Clear(); //loop until exit and print menu to console
                Console.WriteLine($"******* Menu for manipulating {weatherStation.StationName}'s weather data *******");
                Console.WriteLine("\nPress L to select data lists\nPress V to view selected data"+
                                  "\nPress A to sort first selected list by ascending order"+
                                   "\nPress D to sort first selected list by descending order\nPress Y to search according to the content of the Year"+
                                   "\nPress M to search according to the content of the Month\nPress S to view statistics"+
                                   "\nPress B to go back\n");
                PrintSelectedDataLists(selectedDataLists.ToArray()); //print selected data dictionaries to console
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.L:
                        ChooseDataLists(selectedDataLists,weatherStation.StationName); //Call function to select data dictionaries
                        break;
                    case ConsoleKey.V:
                        if (CheckDataListsSelected(selectedDataLists.ToArray())) //Call function to check if user has selected data dictionaries, and view data from dictionaries
                            ViewSelectedData(weatherStation, selectedDataLists.ToArray());
                        break;
                    case ConsoleKey.A:
                        if (CheckDataListsSelected(selectedDataLists.ToArray()))//Call function to check if user has selected data dictionaries
                            ViewSelectedData(selectedDataLists.ToArray(), weatherStation, true); //Call function to view data in ascending order
                        break;
                    case ConsoleKey.D:
                        if (CheckDataListsSelected(selectedDataLists.ToArray()))
                            ViewSelectedData(selectedDataLists.ToArray(), weatherStation, false); //Call function to view data in descending order
                        break;
                    case ConsoleKey.Y:
                        if (!CheckDataListsSelected(selectedDataLists.ToArray())) //If no data dictionaries selected exit switch
                            break;

                        string year = SearchMenu(weatherStation.StationName, true); //Call function to provide searching for year menu to user
                        if (string.IsNullOrWhiteSpace(year)) //If year is empty exit switch
                            break;

                        ViewSelectedData(selectedDataLists.ToArray(), weatherStation,year); //View data for selected year
                        break;
                    case ConsoleKey.M:
                        if (!CheckDataListsSelected(selectedDataLists.ToArray()))
                            break;

                        string month = SearchMenu(weatherStation.StationName, false);//Call function to provide searching for month menu to user
                        if (string.IsNullOrWhiteSpace(month))
                            break;

                        ViewSelectedData(selectedDataLists.ToArray(), weatherStation,null,month);//View data for selected month
                        break;
                    case ConsoleKey.S:
                        if (!CheckDataListsSelected(selectedDataLists.ToArray()))
                            break;
                        StatisticsMenu(selectedDataLists.ToArray(),weatherStation); //Call function to provide statistics menu to user
                        break;
                    case ConsoleKey.B: 
                        exit = true;//Exit loop
                        break;
                    default:
                        break;
                }
            } while (!exit);
        }
        /// <summary>
        /// Function to provide user a menu to enter a year or month to search for
        /// </summary>
        /// <param name="stationName">Name of weather station to retrieve data from</param>
        /// <param name="yearOrMonth">Whether user is searching for year or month</param>
        /// <param name="station2Name">Name of second weather station to retrieve data from</param>
        /// <returns>Year or month being searched for</returns>
        private static string SearchMenu(string stationName, bool yearOrMonth = false, string station2Name = null)
        {
            string yearMonth = "";
            do
            {
                Console.Clear(); //Loop and provide menu, if station2Name is not null or empty add that to menu string
                Console.WriteLine($"******* Menu for searching for a specific {(yearOrMonth == false ? "Months" : "Year")} in { stationName + (!string.IsNullOrWhiteSpace(station2Name) ? " & "+station2Name : "")}'s weather data *******");
                Console.Write($"\n Please enter a {(yearOrMonth == false ? "Month(month name or number)" : "Year")} or nothing to go back: ");
                if (yearOrMonth)
                {
                    string input = Console.ReadLine(); //If searching for a year then read user input
                    if (string.IsNullOrWhiteSpace(input)) //If user input nothing then exit loop
                        break;

                    int year;
                    if (int.TryParse(input, out year)) //If input is parsable into an int 
                    {
                        if (YearsDictionary.Values.ToList().BinarySearch(year.ToString()) >= 0) //Retrieve yearsDictionary values, convert the collection to a list and then use binary search to find if it contains input year
                        {
                            yearMonth = year.ToString();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("There is no data for that year, press any key to try again");
                            Console.ReadKey(true);
                            continue;
                        }

                    }
                }
                else
                {
                    string input = Console.ReadLine(); //Else if searching for a month get user input
                    if (string.IsNullOrWhiteSpace(input))
                        break;

                    int month = 0;
                    if (int.TryParse(input, out month)) //If input is parsable into an int then check if number is between 0 and 12
                    {
                        if (month < 0 || month >= 13) //If number is out of bounds then tell user to try again
                        {
                            Console.WriteLine("That was not a valid month number, press any key to try again");
                            Console.ReadKey(true);
                            continue;
                        }
                        else if (month == 12) //Else if number is 12 then set it to 0 to corrospond to december
                            month = 0;

                        yearMonth = Months[month]; //set yearMonth to corrosponding month
                        break;
                    }
                    else
                    {
                        input = input.Substring(0, 1).ToUpper() + input.Substring(1).ToLower(); //Capitilize first letter and make rest of the characters lower
                        if (Months.Contains(input)) //If months array contains user input then make yearMonth input
                        {
                            yearMonth = input;
                            break;
                        }

                    }
                }

            } while (true);

            return yearMonth;
        }
        /// <summary>
        /// Procedure to provide menu to user to get statistics from selected data dictionaries
        /// </summary>
        /// <param name="selectedDataLists">array of selected data dictionaries</param>
        /// <param name="weatherStation">Weather station to retrieve data from</param>
        /// <param name="weatherStation2">Secondary weather station to retrieve data from</param>
        private static void StatisticsMenu(string[] selectedDataLists, WeatherData weatherStation, WeatherData weatherStation2 = null)
        {
            bool exit = false;
            do //Loop until user exits
            {
                Console.Clear();
                Console.WriteLine("******* Menu for choosing what statistics to view *******"); //Print menu to console
                Console.WriteLine("\nSelect :\nPress M to select Minimum and Maximum\nPress E to select Median\nPress B to go back\n");
                PrintSelectedDataLists(selectedDataLists.ToArray()); //Call procedure to print selected data dictionaries to console
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.M: //If user presses M key then call function to get min/max value's keys and call function to print data from those keys
                        string[] minMaxYearKeys = GetStatisticsYearKeys(selectedDataLists.ToArray(), weatherStation, false, weatherStation2);
                        ViewSelectedData(selectedDataLists.ToArray(), weatherStation, minMaxYearKeys, weatherStation2);
                        break;
                    case ConsoleKey.E: //Or if user press E key then call function to median value's keys and call function to print that data
                        string[] medianYearKeys = GetStatisticsYearKeys(selectedDataLists.ToArray(), weatherStation, true, weatherStation2);
                        ViewSelectedData(selectedDataLists.ToArray(), weatherStation, medianYearKeys, weatherStation2);
                        break;
                    case ConsoleKey.B: //If user press B key then exit loop and procedure
                        exit = true;
                        break;
                    default:
                        break;
                }
            } while (!exit);
        }
#endregion

        /// <summary>
        /// Function to get list of yearKeys for each selected data dictionary's min/max or median value
        /// </summary>
        /// <param name="selectedDataLists">Array of selected data dictionaries</param>
        /// <param name="weatherStation">Weather station selected data dictionary belongs to</param>
        /// <param name="median">Whether to get min/max or median</param>
        /// <param name="weatherStation2">Secondary weather station selected data dictionary belongs to</param>
        /// <returns>Array of year keys to retrieve data</returns>
        private static string[] GetStatisticsYearKeys(string[] selectedDataLists, WeatherData weatherStation, bool median = false,WeatherData weatherStation2 = null)
        {
            IList<string> yearKeys = new List<string>();
            bool useSecondStation = false;
            foreach (string dataList in selectedDataLists)
            {
                if (dataList == "Lerwich") //If datalist is Lerwich then change to use second weather station
                {
                    useSecondStation = true;
                    continue;
                }
                    
                if (!useSecondStation)
                    AddYearKeys(yearKeys,dataList, weatherStation,median); //Call function to get year keys
                else
                    AddYearKeys(yearKeys, dataList, weatherStation2, median);
            }
            return yearKeys.ToArray();
            }

        /// <summary>
        /// Function to get list of yearkeys for each year and month that the selected data dictionary's min and max or median value is contained
        /// </summary>
        /// <param name="yearKeys">List of keys for the data dictionaries values</param>
        /// <param name="dataList">selected data dictionary to get keys for</param>
        /// <param name="weatherStation">Weather station selected data dictionary belongs to</param>
        /// <param name="median">Whether to get min/max or median</param>
        private static void AddYearKeys(IList<string> yearKeys,string dataList,WeatherData weatherStation, bool median = false)
        {
            if (!median) //If not getting median call function for selected data dictionary's min/max value
            {
                switch (dataList)
                {
                    case "Days of Air Frost":
                        Array.ForEach(SortingAlgorithms.GetMaxMin( weatherStation.DaysOfAirFrost), //Get keys for data's min/max values loop through and add each one to list of keys
                            item => yearKeys.Add(item));
                        break;
                    case "Total Rainfall":
                        Array.ForEach(SortingAlgorithms.GetMaxMin( weatherStation.TotalRainfall),
                            item => yearKeys.Add(item));
                        break;
                    case "Total Sunshine Duration":
                        Array.ForEach(SortingAlgorithms.GetMaxMin( weatherStation.TotalSunshineDuration),
                            item => yearKeys.Add(item));
                        break;
                    case "Daily Maximum Temperature":
                        Array.ForEach(SortingAlgorithms.GetMaxMin( weatherStation.DailyTemperatureMaximum),
                            item => yearKeys.Add(item));
                        break;
                    case "Daily Minimum Temperature":
                        Array.ForEach(SortingAlgorithms.GetMaxMin( weatherStation.DailyTemperatureMinimum),
                            item => yearKeys.Add(item));
                        break;
                    default:
                        break;
                }
            }
            else //else call function for selected data dictionary to get median and add each returned key to list of keys
            {
                switch (dataList)
                {
                    case "Days of Air Frost":
                        Array.ForEach(SortingAlgorithms.GetMedian( weatherStation.DaysOfAirFrost),
                            item => yearKeys.Add(item));
                        break;
                    case "Total Rainfall":
                        Array.ForEach(SortingAlgorithms.GetMedian( weatherStation.TotalRainfall),
                            item => yearKeys.Add(item));
                        break;
                    case "Total Sunshine Duration":
                        Array.ForEach(SortingAlgorithms.GetMedian( weatherStation.TotalSunshineDuration),
                            item => yearKeys.Add(item));
                        break;
                    case "Daily Maximum Temperature":
                        Array.ForEach(SortingAlgorithms.GetMedian( weatherStation.DailyTemperatureMaximum),
                            item => yearKeys.Add(item));
                        break;
                    case "Daily Minimum Temperature":
                        Array.ForEach(SortingAlgorithms.GetMedian( weatherStation.DailyTemperatureMinimum),
                            item => yearKeys.Add(item));
                        break;
                    default:
                        break;
                }
            }
        }

#region "Data List functions"
        /// <summary>
        /// Function to check if the user has selected any data dictionaries and if not tell user to select some
        /// </summary>
        /// <param name="selectedDataLists">string array of selected data dictionaries</param>
        /// <returns>True if user has selected any data dictionaries, if not returns false</returns>
        private static bool CheckDataListsSelected(string[] selectedDataLists)
        {
            if (selectedDataLists.Length <= 0)
            {
                Console.WriteLine("Please select a data list first, press any key to continue");
                Console.ReadKey(true);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Procedure to provide menu to user to select weather stations data dictionaries
        /// </summary>
        /// <param name="selectedDataLists">List of selected data dictionaries passed by reference</param>
        /// <param name="stationName">Name of the weather station to select data dictionaries from</param>
        private static void ChooseDataLists(IList<string> selectedDataLists,string stationName)
        {
            bool exit = false;
            do //Loop until user exits
            {
                Console.Clear(); 
                Console.WriteLine($"******* Menu for choosing {stationName}'s weather data to manipulate *******"); //Print menu to console
                Console.WriteLine("\nSelect :\nPress F to select Days of Air Frost\nPress R to select Total Rainfall\nPress S to select Total Sunshine Duration" +
                                   "\nPress X to select Daily Maximum Temperature\nPress N to select Daily Minimum Temperature"+
                                   "\nPress B to go back\n");
                PrintSelectedDataLists(selectedDataLists.ToArray()); //Call procedure to print selected data dictionaries to console
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.F:
                        AddRemoveDataList("Days of Air Frost", selectedDataLists); //Call function to add/remove selected data dictionary from list
                        break;
                    case ConsoleKey.R:
                        AddRemoveDataList("Total Rainfall", selectedDataLists);
                        break;
                    case ConsoleKey.S:
                        AddRemoveDataList("Total Sunshine Duration", selectedDataLists);
                        break;
                    case ConsoleKey.X:
                        AddRemoveDataList("Daily Maximum Temperature", selectedDataLists);
                        break;
                    case ConsoleKey.N:
                        AddRemoveDataList("Daily Minimum Temperature", selectedDataLists);
                        break;
                    case ConsoleKey.B: //Set exit to true to exit loop
                        exit = true;
                        break;
                    default:
                        break;
                }
            } while (!exit);
            
        }

        /// <summary>
        /// Procedure to loop through selected data dictionaries and print them to console
        /// </summary>
        /// <param name="selectedDataLists">List of selected data dictionaries</param>
        private static void PrintSelectedDataLists(string[] selectedDataLists)
        {
            if (selectedDataLists.Length > 0)
            {
                Console.Write("Selected data lists: ");
                for (var i = 0; i < selectedDataLists.Length - 1; i++)
                {
                    if (selectedDataLists[i] == "Lerwich")
                        continue;
                    Console.Write("{0}, ", selectedDataLists[i]);
                }
                Console.WriteLine(selectedDataLists.Last());
            }
        }

        /// <summary>
        /// Function to add or remove selected data dictionaries from list, returns void because list is passed by reference and is not a copy
        /// </summary>
        /// <param name="dataList">Selected data dictionary to add/remove</param>
        /// <param name="selectedDataLists">List of selected data dictionaries passed by reference</param>
        private static void AddRemoveDataList(string dataList, IList<string> selectedDataLists)
        {
            if (!selectedDataLists.Contains(dataList))
            {
                selectedDataLists.Add(dataList);
            }
            else
            {
                selectedDataLists.Remove(dataList);
            }
        }
#endregion
#region "View Data"

        /// <summary>
        /// Procedure to view data from a selected data dictionaries
        /// </summary>
        /// <param name="weatherStation">Weather station to retrieve data from</param>
        /// <param name="selectedDataList">List of selected data dictionaries to retrieve data from</param>
        /// <param name="weatherStation2">Second weather station to retrieve data from</param>
        private static void ViewSelectedData(WeatherData weatherStation, string[] selectedDataList, WeatherData weatherStation2 = null)
        {
            WebDataHeaders.Clear(); //Clear Web headers list and web data list
            WebData.Clear();
            PrintDataTable(weatherStation, selectedDataList,weatherStation2);
            if (outputWeb) //If outputting to web write data to html then open webpage
            {
                FileReadWrite.HtmlWriter(WebDataHeaders.ToArray(), WebData.ToArray(), "WeatherData.html", "Weather Data");
                Process.Start("WeatherData.html");
            }
            Console.ReadKey(true);
        }

        /// <summary>
        /// Procedure to view data from a searched year or month
        /// </summary>
        /// <param name="weatherStation">Weather station to retrieve data from</param>
        /// <param name="selectedDataList">List of selected data dictionaries to retrieve data from</param>
        /// <param name="year">Optional Year to retrieve data from</param>
        /// <param name="month">Optional Month to retrieve data from</param>
        /// <param name="weatherStation2">Second weather station to retrieve data from</param>
        private static void ViewSelectedData(string[] selectedDataList, WeatherData weatherStation,string year = "", string month = "",WeatherData weatherStation2 = null)
        {
            WebDataHeaders.Clear();
            WebData.Clear();
            if (!string.IsNullOrWhiteSpace(year)) //If year is not empty then call procedure to print data from that year
            {
               PrintDataTable(year, weatherStation, selectedDataList,weatherStation2);
            }
            else if (!string.IsNullOrWhiteSpace(month)) //else if month is not empty then call procedure to print data for that month
            {
                PrintDataTable(Array.IndexOf(Months, month),weatherStation,selectedDataList, weatherStation2);
            }
            if (outputWeb) //If outputting to web write data to html then open webpage
            {
                FileReadWrite.HtmlWriter(WebDataHeaders.ToArray(), WebData.ToArray(), "WeatherData.html", $"Search for {year + month}'s Data");
                Process.Start("WeatherData.html");
            }
            Console.ReadKey(true);
        }

        /// <summary>
        /// Procedure to view data sorted into ascending or descending order
        /// </summary>
        /// <param name="weatherStation">Weather station to retrieve data from</param>
        /// <param name="selectedDataList">List of selected data dictionaries to retrieve data from</param>
        /// <param name="sortAscending">Whether to sort data into ascending order or descending</param>
        /// <param name="weatherStation2">Second weather station to retrieve data from</param>
        private static void ViewSelectedData(string[] selectedDataList, WeatherData weatherStation,bool sortAscending, WeatherData weatherStation2 = null)
        {
            WebDataHeaders.Clear();
            WebData.Clear();
            if (sortAscending == true)
            {
                IList<string> orderedKeysAsc = new List<string>(); //Create list to store ordered keys
                if (weatherStation2 != null)
                {
                    do //If weatherStation2 is not null ask user what station's data to orderby
                    {
                        int input = 0;
                        Console.Write("\nPlease Press 1 to select Lerwich, or Press 2 to select Ross on Wye: ");
                        if (int.TryParse(Console.ReadKey().KeyChar.ToString(), out input) && (input == 1 || input == 2))
                        {
                            if (input == 1)
                                orderedKeysAsc = GetOrderedKeys(selectedDataList[0], weatherStation); //Call function to get ordered keys
                            else
                                orderedKeysAsc = GetOrderedKeys(selectedDataList[selectedDataList.ToList().IndexOf("Lerwich")+1], weatherStation2);
                            break;
                        }
                        
                    } while (true);

                }
                else
                    orderedKeysAsc = GetOrderedKeys(selectedDataList[0], weatherStation);


                PrintDataTable(orderedKeysAsc, weatherStation, selectedDataList,weatherStation2); //Call function to print data table
            }
            else
            {
                IList<string> orderedKeysDesc;
                if (weatherStation2 != null)
                {
                    do
                    {
                        int input;
                        Console.Write("Please Press 1 to select Lerwich, or Press 2 to select Ross on Wye: ");
                        if (int.TryParse(Console.ReadKey().KeyChar.ToString(), out input) && (input == 1 || input == 2))
                        {
                            if (input == 1)
                                orderedKeysDesc = GetOrderedKeys(selectedDataList[0], weatherStation);
                            else
                                orderedKeysDesc = GetOrderedKeys(selectedDataList[selectedDataList.ToList().IndexOf("Lerwich")+1], weatherStation2);
                            break;
                        }

                    } while (true);

                }
                else
                    orderedKeysDesc = GetOrderedKeys(selectedDataList[0], weatherStation);
                orderedKeysDesc = orderedKeysDesc.Reverse().ToList(); //Reverse key list into descending order
                PrintDataTable(orderedKeysDesc, weatherStation, selectedDataList,weatherStation2);
            }
            if (outputWeb) //If outputting to web write data to html then open webpage
            {
                FileReadWrite.HtmlWriter(WebDataHeaders.ToArray(), WebData.ToArray(), "WeatherData.html", "Sorted Data");
                Process.Start("WeatherData.html");
            }
            Console.ReadKey(true);
        }

        /// <summary>
        /// Procedure to view data from data dictionaries from  a list of years
        /// </summary>
        /// <param name="weatherStation">Weather station to retrieve data from</param>
        /// <param name="selectedDataList">List of selected data dictionaries to retrieve data from</param>
        /// <param name="years">List of years to retrieve data for</param>
        /// <param name="weatherStation2">Second weather station to retrieve data from</param>
        private static void ViewSelectedData(string[] selectedDataList, WeatherData weatherStation, string[] years,WeatherData weatherStation2 = null)
        {
            WebDataHeaders.Clear();
            WebData.Clear();
            PrintDataTable(years, weatherStation, selectedDataList,weatherStation2);
            if (outputWeb) //If outputting to web write data to html then open webpage
            {
                FileReadWrite.HtmlWriter(WebDataHeaders.ToArray(), WebData.ToArray(), "WeatherData.html", "Statistics Data");
                Process.Start("WeatherData.html");
            }
                
            Console.ReadKey(true);
        }
#endregion

        /// <summary>
        /// Function to return an ordered string array of keys after sorting the weather stations data dictionary
        /// </summary>
        /// <param name="dataList">Data dictionary to sort</param>
        /// <param name="weatherStation">Weather Station owner of the data dictionary to sort</param>
        /// <returns></returns>
        private static string[] GetOrderedKeys(string dataList, WeatherData weatherStation)
        {
            switch (dataList)
            {
                case "Days of Air Frost":
                    return SortingAlgorithms.SortDictionary( weatherStation.DaysOfAirFrost); //Call function to sort Dictionary and return ordered key string array
                case "Total Rainfall":
                    return SortingAlgorithms.SortDictionary( weatherStation.TotalRainfall);
                case "Total Sunshine Duration":
                    return SortingAlgorithms.SortDictionary( weatherStation.TotalSunshineDuration);
                case "Daily Maximum Temperature":
                    return SortingAlgorithms.SortDictionary( weatherStation.DailyTemperatureMaximum);
                case "Daily Minimum Temperature":
                    return SortingAlgorithms.SortDictionary( weatherStation.DailyTemperatureMinimum);
                default:
                    break;
            }
            return null;
        }


#region "Print Table"

        /// <summary>
        /// Create variables used to create a table format in the console
        /// </summary>
        private const string Row = "|";
        private const int Width = 10;

        /// <summary>
        /// Procedure to create a table header in the console, using a list of selected data dictionaries as headers
        /// </summary>
        /// <param name="stationName">Weather stations name</param>
        /// <param name="selectedDataList">List of selected data dictionaries</param>
        /// <param name="clear">bool to clear the console before printing table</param>
        private static void PrintTableHeader(string stationName,IList<string> selectedDataList,bool clear = true)
        {
            if (clear)
            {
                Console.Clear(); //Clear console to print table
                if (!outputWeb)
                    Console.WriteLine($"*******-{ stationName}'s data-*******");
            }

            PrintTableLine(selectedDataList.Count); //Call function to print table line

            if (!outputWeb)
                Console.Write($"{Row,-3}Year{Row,3}{"Month",8}{Row,5}"); //If not outputting to web write Year and Month headers to console
            else
            {
                WebDataHeaders.Add("Year"); //else add Year and month to web headers list
                WebDataHeaders.Add("Month");
            }
                


            foreach (string dataList in selectedDataList) //Loop through each selected data dictionary
            {
                if (dataList == "Lerwich") //If dataList is Lerwich then skip to next loop
                    continue;
                string dataListInitials = "";
                Array.ForEach(dataList.Split(null), i => dataListInitials += i.Substring(0, i[0] == 'M' ? 3 : 1)); //Split datalist up and retrieve initials
                if (!outputWeb)
                    Console.Write($"{dataListInitials,8}{Row,3}"); //If not outputting to web write dataListInitials to console
                else
                    WebDataHeaders.Add($"{dataListInitials}"); //else add initials to web headers
            }
            if (!outputWeb) //if not outputting to web end console line
                Console.WriteLine();
            PrintTableLine(selectedDataList.Count); //Call function to write table line
        }
        /// <summary>
        /// Procedure to write a dashed line to console for table 
        /// </summary>
        /// <param name="dataListCount"></param>
        private static void PrintTableLine(int dataListCount)
        {
            if (!outputWeb)
                Console.WriteLine(new string('-', (Width * (dataListCount + 3)) - 1));
        }

        /// <summary>
        /// Procedure to print year and month to console, or add to webdata list if outputting to webpage
        /// </summary>
        /// <param name="year">Year to print to console/add to list</param>
        /// <param name="month">Month to print to console/add to list</param>
        private static void PrintYearAndMonth(string year, string month)
        {
            if (!outputWeb)
                Console.Write($"{Row,-3}{year}{Row,3}{month,10}{Row,3}");
            else
            {
                WebData.Add($"{year}");
                WebData.Add($"{month}");
            }
                
        }


        /// <summary>
        /// Procedure to print/add to webdata list the year and months data from each selected data dictionary from the weather stations data dictionaries
        /// </summary>
        /// <param name="selectedDataList">List of selected data dictionaries to retrieve data from</param>
        /// <param name="weatherStation">Weather station to retrieve data from</param>
        /// <param name="year">Year to retrieve data</param>
        /// <param name="month">Month to retrieve data</param>
        /// <param name="weatherStation2">Second weather station to retrieve data from</param>
        private static void PrintData(IList<string> selectedDataList, WeatherData weatherStation, string year, string month,WeatherData weatherStation2 = null)
        {
            IList<string> retrievedDataList = new List<string>();
            bool useStation2 = false;
            foreach (string dataListName in selectedDataList)
            {
                if (dataListName == "Lerwich") //If dataListName is Lerwich then use weather station 2
                {
                    useStation2 = true;
                    continue; //Skip to next iteration
                }
                //Call function to retrieve data from weather stations data dictionaries
                string retrievedData = RetrieveData(dataListName, year + "__" + month, !useStation2 ? weatherStation : weatherStation2); 

                if (string.IsNullOrWhiteSpace(retrievedData)) //If no data was returned exit for loop
                    break;
                else
                    retrievedDataList.Add(retrievedData); //else add data to retrievedDataList
            }
            if (retrievedDataList.Count <= 0) //If datalist is empty exit procedure
                return;
            else
            {
                PrintYearAndMonth(year, month); //Else call procedure to print year and month then loop through datalist
                foreach (string data in retrievedDataList)
                {
                    if (!outputWeb)
                        Console.Write($"{data,8}{Row,3}"); //If not outputting to web print data to console
                    else
                        WebData.Add($"{data}"); //else add to webdata list
                }
                if (!outputWeb)
                    Console.WriteLine();
            }
        }

        /// <summary>
        /// Procedure to print/add to webdata list data from selected dictionaries from a certain month of each year
        /// </summary>
        /// <param name="month">Searched month to retrieve data for</param>
        /// <param name="weatherStation">Weather station to retrieve data from</param>
        /// <param name="selectedDataList">List of selected data dictionaries to retrieve data from</param>
        /// <param name="weatherStation2">Second weather station to retrieve data from</param>
        private static void PrintDataTable(int month, WeatherData weatherStation, IList<string> selectedDataList,WeatherData weatherStation2 = null)
        {
            PrintTableHeader(weatherStation.StationName, selectedDataList);
            foreach (string year in YearsDictionary.Values)
            {
                PrintData(selectedDataList, weatherStation, year, Months[month],weatherStation2);
            }
            PrintTableLine(selectedDataList.Count);
        }

        /// <summary>
        /// Procedure to print/add to webdata list data from selected dictionaries from a certain year
        /// </summary>
        /// <param name="year">Searched year to retrieve data for</param>
        /// <param name="weatherStation">Weather station to retrieve data from</param>
        /// <param name="selectedDataList">List of selected data dictionaries to retrieve data from</param>
        /// <param name="weatherStation2">Second weather station to retrieve data from</param>
        private static void PrintDataTable(string year, WeatherData weatherStation, IList<string> selectedDataList,WeatherData weatherStation2 = null)
        {
            PrintTableHeader(weatherStation.StationName, selectedDataList);
            for (int i = 1; i < Months.Length + 1; i++)
            {
                int month = i >= 12 ? 0 : i;
                PrintData(selectedDataList,weatherStation,year, Months[month],weatherStation2);
                if (month == 0)
                    break;
            }
            PrintTableLine(selectedDataList.Count);

        }

        /// <summary>
        /// Procedure to print/add to webdata list data from selected dictionaries from a list of years
        /// </summary>
        /// <param name="yearsList">List of years to retrieve data for</param>
        /// <param name="weatherStation">Weather station to retrieve data from</param>
        /// <param name="selectedDataList">List of selected data dictionaries to retrieve data from</param>
        /// <param name="weatherStation2">Second weather station to retrieve data from</param>
        private static void PrintDataTable(IList<string> yearsList,WeatherData weatherStation, IList<string> selectedDataList,WeatherData weatherStation2 = null)
        {
            if (weatherStation2 == null)
                PrintTableHeader(weatherStation.StationName, selectedDataList); //If only one weather station print that ones name else join both names
            else
                PrintTableHeader(weatherStation.StationName + " & " + weatherStation2.StationName, selectedDataList);
            int eodCount = 0;
            if (selectedDataList.Contains("Lerwich")) //If list of selected data dictionaries contains Lerwich then set eodCount to 1
                eodCount = 1;
            foreach (string yearMonth in yearsList)
            {
                if (yearMonth == "EoD") //If yearMonth is EoD then print new headers to show user next data dictionaries statistical data
                {
                    PrintTableLine(selectedDataList.Count);
                    if (eodCount < selectedDataList.Count - 1)
                        PrintTableHeader(weatherStation.StationName, selectedDataList, false);
                    eodCount += 1;
                    continue;
                } 
                string year = yearMonth.Split(new string[] { "__" }, StringSplitOptions.RemoveEmptyEntries)[0]; //split yearMonth into year and month then call procedure to print data
                string month = yearMonth.Split(new string[] {"__"}, StringSplitOptions.RemoveEmptyEntries)[1];
                PrintData(selectedDataList, weatherStation, year, month,weatherStation2);
            }
            if (eodCount < selectedDataList.Count) //If eodCount is below data dictionary count then print line
                PrintTableLine(selectedDataList.Count);
        }

        /// <summary>
        /// Procedure to print all data contained in selected data dictionaries to console/add to webdata list
        /// </summary>
        /// <param name="weatherStation">Weather station to retrieve data from</param>
        /// <param name="selectedDataList">List of selected data dictionaries to retrieve data from</param>
        /// <param name="weatherStation2">Second weather station to retrieve data from</param>
        private static void PrintDataTable(WeatherData weatherStation,IList<string>selectedDataList,WeatherData weatherStation2 = null )
        {
            if (weatherStation2 == null) 
                PrintTableHeader(weatherStation.StationName,selectedDataList);
            else
                PrintTableHeader(weatherStation.StationName + " & " + weatherStation2.StationName, selectedDataList);
            foreach (string year in YearsDictionary.Values)
            {
                
                for (int i = 1; i < Months.Length+1; i++) //Loop through each year contained in yearsDictionary and then loop through each month
                {
                    
                    int month = i >= 12 ? 0 : i; //If i is equal or greater than 12 set i to 0 to get december
                    
                    PrintData(selectedDataList, weatherStation, year, Months[month], weatherStation2 ?? null);
                    if (month == 0) //if i is 0 then exit loop
                        break;
                }
            }
            PrintTableLine(selectedDataList.Count);
           
        }

#endregion
        
        /// <summary>
        /// Function to retrieve data from a weatherstations data dictionaries if the dictionary contains a year__month key then return that keys value 
        /// </summary>
        /// <param name="dataList">What data dictionary to attempt to retrieve data from</param>
        /// <param name="yearMonth">The year__month key used to retrieve a value</param>
        /// <param name="weatherStation">The weather weatherStation to retrieve data from</param>
        /// <returns>Empty string if key is not valid, or data in string format if key returns a value</returns>
        private static string RetrieveData(string dataList, string yearMonth, WeatherData weatherStation)
        {
            int iData;
            float fData;
            switch (dataList)
            {
                case "Days of Air Frost":
                    if (weatherStation.DaysOfAirFrost.TryGetValue(yearMonth, out iData))//If dictionary contains key equal to yearMonth then return keys value else return empty string
                        return iData.ToString();
                    break;
                case "Total Rainfall":
                    if (weatherStation.TotalRainfall.TryGetValue(yearMonth, out fData)) 
                        return fData.ToString();
                    break;
                case "Total Sunshine Duration":
                    if (weatherStation.TotalSunshineDuration.TryGetValue(yearMonth, out fData)) 
                        return fData.ToString();
                    break;
                case "Daily Maximum Temperature":
                    if (weatherStation.DailyTemperatureMaximum.TryGetValue(yearMonth, out fData)) 
                        return fData.ToString();
                    break;
                case "Daily Minimum Temperature":
                    if(weatherStation.DailyTemperatureMinimum.TryGetValue(yearMonth, out fData))
                        return fData.ToString();
                    break;
                default:
                    break;
            }
            return "";
        }

#region "Import Data"

        /// <summary>
        /// Procedure to provide user with menu to import data from
        /// </summary>
        private static void ImportDataMenu()
        {
            Console.Clear();
            Console.WriteLine("******* Importing data menu *******");
            Console.WriteLine("\nPress Y to import year data\nPress L to import Lerwich data\nPress R to import Ross on Wye data\nPress A to import all data\nPress B to go back\n");
            bool exit = false;
            do
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Y:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Importing Year data: "); Console.ForegroundColor = ConsoleColor.Red;
                        YearsDictionary = FileReadWrite.ImportYears(BaseFilePath + @"Year.txt");
                        if (YearsDictionary != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Successful");
                        }
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case ConsoleKey.L:
                        if (CheckIfYearsDataImported)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Please import Year data first\n");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.ReadKey(true);
                            break;
                        }
                        LerwichData = ImportWeatherStationsData(LerwichData, BaseFilePath + "WS1");
                        break;
                    case ConsoleKey.R:
                        if (CheckIfYearsDataImported)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Please import Year data first\n");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.ReadKey(true);
                            break;
                        }
                        RossOnWyeData = ImportWeatherStationsData(RossOnWyeData, BaseFilePath + "WS2");
                        break;
                    case ConsoleKey.A:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Importing Year data: "); Console.ForegroundColor = ConsoleColor.Red;
                        YearsDictionary = FileReadWrite.ImportYears(BaseFilePath + @"Year.txt");
                        if (YearsDictionary != null)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Successful");
                        }
                        Console.ForegroundColor = ConsoleColor.Gray;
                        if (CheckIfYearsDataImported)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Please import Year data first\n");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.ReadKey(true);
                            break;
                        }
                        LerwichData = ImportWeatherStationsData(LerwichData, BaseFilePath + "WS1");
                        RossOnWyeData = ImportWeatherStationsData(RossOnWyeData, BaseFilePath + "WS2");
                        break;
                    case ConsoleKey.B:
                        exit = true;
                        break;
                    default:
                        break;
                }
            } while (!exit);
        }

        /// <summary>
        /// Function to check if Years dictionary contains any values
        /// </summary>
        private static bool CheckIfYearsDataImported => (YearsDictionary == null || YearsDictionary.Count <= 0);


        /// <summary>
        /// Function to import weather data from files into appropriate dictionaries and return stored data
        /// </summary>
        /// <param name="weatherStation">Weather weatherStation to import data for</param>
        /// <param name="weatherStationFiles">Name of weatherstations preceding file name</param>
        /// <returns>WeatherData class with data stored in appropriate dictionaries</returns>
        private static WeatherData ImportWeatherStationsData(WeatherData weatherStation, string weatherStationFiles)
        {
            Console.WriteLine("\n******* Importing Data for weather weatherStation: {0} *******", weatherStation.StationName);

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Importing Days of Air Frost data: "); Console.ForegroundColor = ConsoleColor.Red; //Notify user what data the program is importing and change console foreground colour
            weatherStation.DaysOfAirFrost = FileReadWrite.ImportWeatherDataInt(weatherStationFiles + @"_AF.txt"); //Call function to import data
            if (weatherStation.DaysOfAirFrost != null) //If importing data didnt fail change console foreground colour and tell user importing succeeded
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successful");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Importing TotalRainfall data: "); Console.ForegroundColor = ConsoleColor.Red;
            weatherStation.TotalRainfall = FileReadWrite.ImportWeatherDataFloat(weatherStationFiles + @"_Rain.txt");
            if (weatherStation.TotalRainfall != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successful");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Importing TotalSunshineDuration data: "); Console.ForegroundColor = ConsoleColor.Red;
            weatherStation.TotalSunshineDuration = FileReadWrite.ImportWeatherDataFloat(weatherStationFiles + @"_Sun.txt");
            if (weatherStation.TotalSunshineDuration != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successful");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Importing DailyTemperatureMaximum data: "); Console.ForegroundColor = ConsoleColor.Red;
            weatherStation.DailyTemperatureMaximum = FileReadWrite.ImportWeatherDataFloat(weatherStationFiles + @"_TMax.txt");
            if (weatherStation.DailyTemperatureMaximum != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successful");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Importing DailyTemperatureMinimum data: "); Console.ForegroundColor = ConsoleColor.Red;
            weatherStation.DailyTemperatureMinimum = FileReadWrite.ImportWeatherDataFloat(weatherStationFiles + @"_TMin.txt");
            if (weatherStation.DailyTemperatureMinimum != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successful");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            
            Console.WriteLine("******* Imported data for weather weatherStation: {0} *******", weatherStation.StationName);

            return weatherStation;
        }


 
#endregion
    }
}
