using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Word;

/// <summary>
/// Summary description for WordApplication
/// </summary>
public class WordApplication : IDisposable
{
	 private Microsoft.Office.Interop.Word.Application _application = new Microsoft.Office.Interop.Word.Application();
        private Microsoft.Office.Interop.Word.Document _document = new Microsoft.Office.Interop.Word.Document();
       
      

        public WordApplication()
        {
           
        }

        public void Open(string docFilename)
        {
            string text = System.IO.File.ReadAllText(docFilename);

            // Display the file contents to the console. Variable text is a string.
            System.Console.WriteLine("Contents of WriteText.txt = {0}", text);

            // Example #2
            // Read each line of the file into a string array. Each element
            // of the array is one line of the file.
            string[] lines = System.IO.File.ReadAllLines(@"C:\WriteLines2.txt");

            // Display the file contents by using a foreach loop.
            System.Console.WriteLine("Contents of WriteLines2.txt = ");
            foreach (string line in lines)
            {
                // Use a tab to indent each line of the file.
                Console.WriteLine("\t" + line);
            }

            // Keep the console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();



            object docFilenameAsObject = docFilename;
            _document = _application.Documents.Open(ref docFilenameAsObject);
        }

        public void SaveAsText(string outputTxtFilename)
        {
            object outputTxtFilenameAsObject = outputTxtFilename;
            object formatAsObject = WdSaveFormat.wdFormatText;
            _document.SaveAs(ref outputTxtFilenameAsObject, ref formatAsObject);
        }

        public void Dispose()
        {
            _application.Application.Quit();
        }
    }


