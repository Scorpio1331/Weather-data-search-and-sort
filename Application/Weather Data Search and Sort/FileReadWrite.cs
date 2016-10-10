using  System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;


namespace Weather_Data_Search_and_Sort
{

    static class FileReadWrite
    {
        /// <summary>
        /// Function to open a file containing Years data then return dictionary filled with years
        /// </summary>
        /// <param name="filePath">filepath of the file to import data from</param>
        /// <returns>Dictionary of imported years</returns>
        public static IDictionary<int, string> ImportYears(string filePath)
        {
            try
            {
                if (!File.Exists(filePath)) //Check if file exists, if not throw exception
                    throw new FileNotFoundException();
                IDictionary<int, string> years = new Dictionary<int, string>();
                using (StreamReader dataReader = new StreamReader(filePath)) //Initialize dictionary to store data and use streamreader to open file and import data
                {
                    string prevYear = "";
                    int yearCounter = -1;
                    while (!dataReader.EndOfStream) //While not end of stream read data
                    {
                        string year = dataReader.ReadLine();
                        if (year != prevYear)
                            yearCounter += 1;
                        if (!years.ContainsKey(yearCounter)) //If dictionary doesnt already contain key for the same year then add year
                        {
                            years.Add(yearCounter, year);
                        }

                        prevYear = year;
                    }
                }
                return years;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed (Error: {0}) ", ex.Message); //If error occurrs tell user importing data failed and why
            }
            return null;
        }

        /// <summary>
        /// Function to open a given file and read integer data from file into a dictionary then return the dictionary
        /// </summary>
        /// <param name="filePath">filepath of the file to import data from</param>
        /// <returns>Dictionary of imported data</returns>
        public static IDictionary<string, int> ImportWeatherDataInt(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException();
                IDictionary<string, int> weatherData = new Dictionary<string, int>();
                using (StreamReader dataReader = new StreamReader(filePath))
                {
                    int counter = 0;
                    int yearCounter = 0;
                    while (!dataReader.EndOfStream)
                    {
                        int data;
                        string year = Program.YearsDictionary[yearCounter];
                        if (counter == 0)
                            yearCounter += 1;
                        if (Int32.TryParse(dataReader.ReadLine(), out data) && !weatherData.ContainsKey(year + "__" + Program.Months[counter]))
                        {
                            weatherData.Add(year + "__" + Program.Months[counter], data);
                            counter += 1;
                        }

                        if (counter >= 12)
                            counter = 0;
                    }
                }

                return weatherData;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed (Error: {0}) ", ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Function to open a given file and read float data from file into a dictionary then return the dictionary
        /// </summary>
        /// <param name="filePath">filepath of the file to import data from</param>
        /// <returns>Dictionary of imported data</returns>
        public static IDictionary<string, float> ImportWeatherDataFloat(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException();
                IDictionary<string, float> weatherData = new Dictionary<string, float>();
                using (StreamReader dataReader = new StreamReader(filePath))
                {
                    int counter = 0;
                    int yearCounter = 0;
                    while (!dataReader.EndOfStream)
                    {
                        float data;
                        string year = Program.YearsDictionary[yearCounter];
                        if (counter == 0)
                            yearCounter += 1;
                        if (Single.TryParse(dataReader.ReadLine(), out data) && !weatherData.ContainsKey(year + "__" + Program.Months[counter]))
                        {
                            weatherData.Add(year + "__" + Program.Months[counter], data);
                            counter += 1;
                        }

                        if (counter >= 12)
                            counter = 0;
                    }
                }

                return weatherData;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed (Error: {0}) ", ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Public Function used to read text from a List and write it in a HTML page.
        /// the function checks whether a HTML page with the filepath exists, if it does, it deletes the HTML page,
        /// then using a FileStream to create a new HTML page under the same name and location and using a StreamWriter to write to the HTML page
        /// the function loops through the Array's values and writes them to the HTML page.
        /// it does this until it reaches the end of the dictionary. This process is encapsulated in a try..catch statement to catch any
        /// exceptions and tell the user what happened.
        /// </summary>
        /// /// <param name="headers">array that contains the text that is going to be used as table headers</param>
        /// <param name="results">array that contains the data to be used in the table</param>
        /// <param name="filePath">String used to indicate a location for the HTML page to write to</param>
        public static void HtmlWriter(string[] headers,string[] results, string filePath,string title)
        {
            try //Try..Catch statement to handle any exception that may occur and tell the user what happened
            {
                if (File.Exists(filePath)) //Checks if the HTML page with the filepath exists
                {
                    File.Delete(filePath); //if the HTML page exists, then delete it
                }
                using (FileStream fs = new FileStream(filePath, FileMode.CreateNew)) //Use a FileStream to open the HTML page, 'using' statement automatically disposes of the object
                {
                    using (StreamWriter writer = new StreamWriter(fs)) //Use a StreamReader to write to the file
                    {
                        using (HtmlTextWriter htmlWriter = new HtmlTextWriter(writer)) //use HtmlTextWriter to write to the HTML page
                        {
                            htmlWriter.RenderBeginTag(HtmlTextWriterTag.Html); //Start creating webpage by writing <html> tag

                            htmlWriter.RenderBeginTag(HtmlTextWriterTag.Head); //Write <head> tag
                            htmlRenderTag(htmlWriter, HtmlTextWriterTag.Title, title); //Call function to open and close a <title> tag write the title for the page
                            htmlWriter.WriteLine(htmlWriter.NewLine); //Separate tags by writing an empty line for easier reading of source code

                            string divStyle = "div { border-radius: 15px; margin: 5px 5px 10px 10px;"+
                                            "width: 30%; height: 10%; text-align: center; display: inline-block; overflow: auto; padding: 0px; }"; //Create base style for div elements
                            string tableStyle = "table {border-collapse: collapse;width: 100%;}table, th, td {border: 1px solid black;}"; //Create style for li elements
                            htmlRenderTag(htmlWriter, HtmlTextWriterTag.Style, divStyle + tableStyle); //Call function to open and close a <style> tag and write the div and li styles to it

                            htmlWriter.RenderEndTag(); //Close <head> tag

                            htmlWriter.WriteLine(htmlWriter.NewLine); //Separate tags by writing an empty line for easier reading of source code
                            htmlWriter.RenderBeginTag(HtmlTextWriterTag.Body); //Write <body> tag

                            htmlWriter.AddStyleAttribute("background-color", "#A5CBC3"); //Change style of <div> tag
                            htmlWriter.AddStyleAttribute("overflow", "hidden");
                            htmlWriter.AddStyleAttribute("display", "block");
                            htmlWriter.RenderBeginTag(HtmlTextWriterTag.Div); //Write <div> tag to contain header

                            htmlWriter.AddStyleAttribute("margin", "5% 0 0 0"); //Add margin to <h2> header tag
                            htmlRenderTag(htmlWriter, HtmlTextWriterTag.H2, title); //Call function to open and close a <h2> tag and write the header to
                            htmlWriter.RenderEndTag(); //Close <div> tag
                            htmlWriter.WriteLine(htmlWriter.NewLine); //Separate tags by writing an empty line for easier reading of source code
  
                            
                            htmlWriter.RenderBeginTag(HtmlTextWriterTag.Table); //Write <table> tag to contain table of data
                            htmlWriter.RenderBeginTag(HtmlTextWriterTag.Thead);
                            htmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr); //Write <tr> tag to contain table header
                            foreach (string header in headers)
                            {
                                htmlRenderTag(htmlWriter,HtmlTextWriterTag.Td, header);
                            }
                            htmlWriter.RenderEndTag(); //close <tr> table header row tag
                            htmlWriter.RenderEndTag();
                            htmlWriter.RenderBeginTag(HtmlTextWriterTag.Tbody);
                            htmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);
                            for (int i = 0; i < results.Length; i++)
                            {
                                htmlRenderTag(htmlWriter, HtmlTextWriterTag.Td, results[i]); //Call function to open and close a <td> tag and write the current text to
                                if ((i % (headers.Length) == (headers.Length -1)))
                                {
                                    htmlWriter.RenderEndTag();
                                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Tr);
                                }
                            }
                            
                            htmlWriter.RenderEndTag(); htmlWriter.RenderEndTag(); htmlWriter.RenderEndTag(); //Close last <tr> tags and <table> tag
                            
                            htmlWriter.RenderEndTag(); htmlWriter.RenderEndTag(); //Close <body> tag and <html> tag
                        }
                            
                    }
                }                
            }
            catch (Exception e) //Catch any exception that may occur
            {
                Console.WriteLine("The process failed because: {0}", e.Message); //Tell the user an exception occurred and what the exception was
                Console.ReadLine(); //Wait for user input before moving on
            }
        }

        /// <summary>
        /// Private function used to open and close a html tag enclose a string inside 
        /// </summary>
        /// <param name="htmlWriter">HtmlTextWriter used to write the tag and string to the file</param>
        /// <param name="tag">The tag to open and close</param>
        /// <param name="text">String containing text to be enclosed in tags</param>
        private static void htmlRenderTag(HtmlTextWriter htmlWriter, HtmlTextWriterTag tag, string text)
        {
            htmlWriter.RenderBeginTag(tag); //Write tag to contain text
            htmlWriter.Write(text); //Write text in-between tags
            htmlWriter.RenderEndTag(); //Close tag
        }
    }
}
