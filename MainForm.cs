using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;




namespace Textual_Affective_Properties_Analyzer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            StopListLabel.Text = "Words to Omit\r\n(Case Insensitive)";

            
                       
            //set up our encoding dropdown list
            foreach (var encoding in Encoding.GetEncodings())
            {
                EncodingDropdown.Items.Add(encoding.Name);
            }
            EncodingDropdown.SelectedIndex = EncodingDropdown.FindStringExact(Encoding.Default.BodyName);

            

        }


        




        //taken from here, then modified slightly:
        //http://stackoverflow.com/a/17546909
        private static DialogResult ShowInputDialog(ref string input)
        {
            System.Drawing.Size size = new System.Drawing.Size(200, 70);
            Form inputBox = new Form();

            inputBox.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            inputBox.StartPosition = FormStartPosition.CenterParent;
            inputBox.ClientSize = size;
            inputBox.Text = "Input New Variable Name:";
            inputBox.MaximizeBox = false;
            inputBox.MinimizeBox = false;
            inputBox.TopMost = true;


            System.Windows.Forms.TextBox textBox = new TextBox();
            textBox.Size = new System.Drawing.Size(size.Width - 10, 23);
            textBox.Location = new System.Drawing.Point(5, 5);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new System.Drawing.Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new System.Drawing.Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new System.Drawing.Point(size.Width - 80, 39);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }





        private void NormDataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (NormDataGrid.CurrentCell.ColumnIndex != 0)
            {
                MessageBox.Show("Cell contents must be numeric.\r\nPress 'Esc' to revert change.", "Data Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //make sure that there's *something* in the grid view
            if (NormDataGrid.Rows.Count == 0) return;

            DisableAllStuff();

            FilenameLabel.Text = "Validating norm table...";

            int NumChars = 0;
            int NumWords = 0;

            //we pre-allocate the hashset using the method described here:
            //https://stackoverflow.com/a/23071206
            //...and here
            //https://stackoverflow.com/a/11557205


            List<string> PreallocationList = Enumerable.Range(0, NormDataGrid.Rows.Count).Select(n => n.ToString()).ToList();
            HashSet<string> Entries = new HashSet<string>();
            
            //make sure that we don't have any duplicates and that all types match our list
            Entries = new HashSet<string>(PreallocationList);
            PreallocationList.Clear();
            PreallocationList.TrimExcess();

            Entries.Clear();

            


            string[] RecognizedTypes = new string[] { "word", "char" };
            bool NonRecognizedType = false;
            bool Duplicates = false;
            int ProblemRow = 0;
            string ProblemSymbol = "";

            object[] blank_comparators = new object[] { "", null, DBNull.Value };

            for (int i = 0; i < NormDataGrid.Rows.Count - 1; i++)
            {

                if ((i + 1) % 1000 == 0) {
                    FilenameLabel.Text = "Validating norm table, Row #" + (i + 1).ToString();
                    FilenameLabel.Update();
                    FilenameLabel.Refresh();
                    Application.DoEvents();
                }



                if ((NormDataGrid.Rows[i].Cells[1].Value.ToString() == "char") && (!blank_comparators.Contains(NormDataGrid.Rows[i].Cells[0].Value)))
                    {

                        string Entry = NormDataGrid.Rows[i].Cells[0].Value.ToString() + NormDataGrid.Rows[i].Cells[1].Value.ToString().ToLower();

                        //if we find a duplicate entry, we offer the user the chance to skip over that entry
                        //essentially, what we do is blank out the entry and move on
                        if (Entries.Contains(Entry))
                        {
                            ProblemRow = i + 1;
                            ProblemSymbol = NormDataGrid.Rows[i].Cells[0].Value.ToString();

                            if (MessageBox.Show("There appear to be a duplicate entry\r\nin your list of symbols.\r\n\r\nSee Row #" + ProblemRow.ToString() + " (" + ProblemSymbol + ")\r\n\r\n" + "Would you like to ignore this entry?", "Duplicate Symbols", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                            {
                                NormDataGrid.Rows[i].Cells[0].Value = "";
                                continue;

                            }
                            else
                            {
                                Duplicates = true;
                                break;
                            }
                          }

                        //if we've passed the duplicate check, then we'll go ahead and add to our list
                        Entries.Add(Entry);
                        NumChars++;

                     }
                else
                {
                    if (!blank_comparators.Contains(NormDataGrid.Rows[i].Cells[0].Value)) { 
                        if (WordCaseSensitiveCheckbox.Checked)
                        {
                            string Entry = NormDataGrid.Rows[i].Cells[0].Value.ToString() + NormDataGrid.Rows[i].Cells[1].Value.ToString().ToLower();
                            //if we find a duplicate entry, we offer the user the chance to skip over that entry
                            //essentially, what we do is blank out the entry and move on
                            if (Entries.Contains(Entry))
                            {
                                ProblemRow = i + 1;
                                ProblemSymbol = NormDataGrid.Rows[i].Cells[0].Value.ToString();

                                if (MessageBox.Show("There appear to be a duplicate entry\r\nin your list of symbols.\r\n\r\nSee Row #" + ProblemRow.ToString() + " (" + ProblemSymbol + ")\r\n\r\n" + "Would you like to ignore this entry?", "Duplicate Symbols", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                                {
                                    NormDataGrid.Rows[i].Cells[0].Value = "";
                                    continue;

                                }
                                else
                                {
                                    Duplicates = true;
                                    break;
                                }
                            }
                            Entries.Add(Entry);
                            NumWords++;
                        }
                        else
                        {
                            string Entry = NormDataGrid.Rows[i].Cells[0].Value.ToString().ToLower() + NormDataGrid.Rows[i].Cells[1].Value.ToString().ToLower();

                            //if we find a duplicate entry, we offer the user the chance to skip over that entry
                            //essentially, what we do is blank out the entry and move on
                            if (Entries.Contains(Entry))
                            {
                                ProblemRow = i + 1;
                                ProblemSymbol = NormDataGrid.Rows[i].Cells[0].Value.ToString();

                                if (MessageBox.Show("There appear to be a duplicate entry\r\nin your list of symbols.\r\n\r\nSee Row #" + ProblemRow.ToString() + " (" + ProblemSymbol + ")\r\n\r\n" + "Would you like to ignore this entry?", "Duplicate Symbols", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                                {
                                    NormDataGrid.Rows[i].Cells[0].Value = "";
                                    continue;

                                }
                                else
                                {
                                    Duplicates = true;
                                    break;
                                }
                            }
                            Entries.Add(Entry);
                            NumWords++;
                        }
                    }

                }
                
                if (RecognizedTypes.Contains(NormDataGrid.Rows[i].Cells[1].Value.ToString().Trim().ToLower()) == false){
                    NonRecognizedType = true;
                    break;
                }
            }

            
            FilenameLabel.Text = "Waiting to analyze texts...";

            //this was temporary code that was used to figure out where
            //duplicates were when there was some debugging happening
            //Dictionary<string, int> counts = Entries.GroupBy(x => x)
            //                          .ToDictionary(g => g.Key,
            //                                        g => g.Count());
            //using (StreamWriter file = new StreamWriter("myfile.txt"))
            //    foreach (var entry in counts)
            //        file.WriteLine("[{0} {1}]", entry.Key, entry.Value);




            //this is what we do if there are issues passed to us here
            if (Duplicates) 
            {
                EnableAllStuff();
                return;
            } else if (NonRecognizedType == true)
            {
                MessageBox.Show("There appears to be an unrecognized\r\nsymbol type in your norm list.", "Unrecognized Type", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EnableAllStuff();
                return;
            } else if ((NormDataGrid.Columns[0].Name.ToString().ToLower() != "symbol") || (NormDataGrid.Columns[1].Name.ToString().ToLower() != "type"))
            {
                MessageBox.Show("Your first two columns should be \"Symbol\"\r\nand \"Type\", respectively.", "Unrecognized Columns", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EnableAllStuff();
                return;
            }

            Entries.Clear();
            Entries.TrimExcess();

            FolderBrowser.Description = "Please choose the location of your .txt files";
            FolderBrowser.ShowDialog();
            string TextFileFolder = FolderBrowser.SelectedPath.ToString();

            if (TextFileFolder != "")
            {

                saveFileDialog.FileName = "TAPA Output.csv";

                saveFileDialog.InitialDirectory = TextFileFolder;
                
                if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                {
                    return;
                }

                string OutputFileLocation = saveFileDialog.FileName;

                if (OutputFileLocation != "")
                {
                    
                    BgWorker.RunWorkerAsync(new string[] { TextFileFolder, OutputFileLocation, NumChars.ToString(), NumWords.ToString() });
                }
                
            }
            else
            {
                EnableAllStuff();
            }
        }












        //this is where the magic happens
        //this is where the magic happens
        //this is where the magic happens
        //this is where the magic happens
        //this is where the magic happens
        private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            //determines whether we're going to go case insensitive or not
            bool IgnoreCase = true;

            //the very first thing that we want to do is set up our function word lists
            List<string> FunctionWordWildcardList = new List<string>();
            List<string> FunctionWordsToHash = new List<string>();

            string[] OriginalFunctionWordList = FunctionWordTextBox.Text.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);


            OriginalFunctionWordList = OriginalFunctionWordList.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            foreach (string Word in OriginalFunctionWordList)
            {
                string WordToParse = Word.Trim();

                if (IgnoreCase) WordToParse = WordToParse.ToLower();

                if (WordToParse.Contains('*'))
                {
                    FunctionWordWildcardList.Add(WordToParse.Replace("*", ""));
                }
                else
                {
                    FunctionWordsToHash.Add(WordToParse);
                }

            }

            //remove duplicates
            FunctionWordWildcardList = FunctionWordWildcardList.Distinct().ToList();
            FunctionWordsToHash = FunctionWordsToHash.Distinct().ToList();

            HashSet<string> HashedFuncWords = new HashSet<string>(FunctionWordsToHash);
            string[] FunctionWordWildCards = FunctionWordWildcardList.ToArray();

            FunctionWordsToHash = null;
            FunctionWordWildcardList = null;
            OriginalFunctionWordList = null;
            char[] PunctuationToRemove = { };


            const double Ignored_Value = -9786543210.9876543210;


            //sets our encoding
            Encoding SelectedEncoding = null;
            this.Invoke((MethodInvoker)delegate ()
            {
                SelectedEncoding = Encoding.GetEncoding(EncodingDropdown.SelectedItem.ToString());
                if (WordCaseSensitiveCheckbox.Checked == true) IgnoreCase = false;
                char[] PunctuationToRemoveTemp = PunctuationBox.Text.ToArray();
                PunctuationToRemove = PunctuationToRemoveTemp;
            });

            //report what we're working on
            FilenameLabel.Invoke((MethodInvoker)delegate
            {
                FilenameLabel.Text = "Finalizing user-defined norm data...";
            });

            
            

            //creates our character coding dictionary
            //the argument[2] parameter takes the "NumChars" variable and tells us how big we want our
            //dictionary to be. This will pre-allocate information for us
            Dictionary<char, Dictionary<string, double>> CharDict = new Dictionary<char, Dictionary<string, double>>(Convert.ToInt32(((string[])e.Argument)[2]));
            //this creates our word-level coding dictionary
            //the argument here is the same as above, but for the "NumWords" variable
            Dictionary<string, Dictionary<string, double>> WordDict = new Dictionary<string, Dictionary<string, double>>(Convert.ToInt32(((string[])e.Argument)[3]));



            //this loads everything up into our dictionaries
            this.Invoke((MethodInvoker)delegate ()
            {

                object[] blank_comparator = new object[] { "", null, DBNull.Value };

                for (int row = 0; row < NormDataGrid.RowCount - 1; row++)
                {

                    if ((row + 1) % 1000 == 0)
                    {
                        FilenameLabel.Text = "Finalizing user-defined norm data... Row #" + (row + 1).ToString();
                        Application.DoEvents();
                    }

                    //add the character itself to the character dictionary
                    if ((!blank_comparator.Contains( NormDataGrid.Rows[row].Cells[0].Value)) && (NormDataGrid.Rows[row].Cells[1].Value.ToString().Trim().ToLower() == "char"))
                    {
                        char Character = NormDataGrid.Rows[row].Cells[0].Value.ToString()[0];
                        CharDict.Add(Character, new Dictionary<string, double>());
                        for (int col = 2; col < NormDataGrid.ColumnCount; col++)
                        {
                            if (!blank_comparator.Contains(NormDataGrid.Rows[row].Cells[col].Value))
                            { 
                                CharDict[Character].Add(NormDataGrid.Columns[col].Name, Convert.ToDouble(NormDataGrid.Rows[row].Cells[col].Value));
                            }
                            else
                            {
                                CharDict[Character].Add(NormDataGrid.Columns[col].Name, Ignored_Value);
                            }
                        }
                    }
                    else if ((!blank_comparator.Contains(NormDataGrid.Rows[row].Cells[0].Value)) && (NormDataGrid.Rows[row].Cells[1].Value.ToString().Trim().ToLower() == "word"))
                    {
                        string Word = NormDataGrid.Rows[row].Cells[0].Value.ToString().Trim();
                        if (IgnoreCase) Word = Word.ToLower();
                        WordDict.Add(Word, new Dictionary<string, double>());
                        for (int col = 2; col < NormDataGrid.ColumnCount; col++)
                        {
                            if (!blank_comparator.Contains(NormDataGrid.Rows[row].Cells[col].Value))
                            {
                                WordDict[Word].Add(NormDataGrid.Columns[col].Name, Convert.ToDouble(NormDataGrid.Rows[row].Cells[col].Value));
                            }
                            else
                            {
                                WordDict[Word].Add(NormDataGrid.Columns[col].Name, Ignored_Value);
                            }
                        }

                    }

                }
            });




            //now that we've built our character coding dictionary, we need to build a list of keys
            //that refers to each of the variables that we're going to code for. we also want a complete
            //list of all of the characters that we'll be coding for
            List<string> SymbolPropertyList = new List<string>();
            List<char> CharListList = new List<char>();

            this.Invoke((MethodInvoker)delegate ()
            {

                for (int col = 2; col < NormDataGrid.ColumnCount; col++)
                {
                    SymbolPropertyList.Add(NormDataGrid.Columns[col].Name);
                }

                for (int row = 0; row < NormDataGrid.RowCount; row++)
                {
                    if ((NormDataGrid.Rows[row].Cells[0].Value != null) && (NormDataGrid.Rows[row].Cells[1].Value.ToString().Trim().ToLower() == "char"))
                    {
                        CharListList.Add(NormDataGrid.Rows[row].Cells[0].Value.ToString()[0]);
                    }
                    
                }
                
                });

            string[] SymbolProperties = SymbolPropertyList.ToArray();
            char[] CharList = CharListList.ToArray();
            SymbolPropertyList = null;
            CharListList = null;
            


            //now we've set up 2 different arrays:
            //CharList is an array of each character that we'll be coding for
            //CharProperties is an array of each property that will be coded for each character
            //now that this information is extracted, we can set up a new dictionary for each
            //text file to keep our data coded in a nice and orderly fashion, both in terms of
            //referencing hashed values in the dictionary, as well as for writing the output to a CSV





            //get the list of files
            var SearchDepth = SearchOption.TopDirectoryOnly;
            if (ScanSubfolderCheckbox.Checked)
            {
                SearchDepth = SearchOption.AllDirectories;
            }
            var files = Directory.EnumerateFiles(((string[])e.Argument)[0], "*.txt", SearchDepth);

            //check and see if we want to include this information in the output
            //we do this here so that we only have to reach out to the UI once instead of repeatedly below
            var IncludeCountsAndSumsInOutput = CountsAndSumsCheckbox.Checked;



            try
            {
            short HeaderVars = 8;
            using (StreamWriter outputFile = new StreamWriter(((string[])e.Argument)[1]))
            {

                string HeaderString = "\"Filename\",\"WC\",\"WC_WordsRemoved\",\"CharCountTotal\",\"CharCountCoded\",\"WordsWithCodedChars\",\"AvgWordLength\",\"DictWords\"";

                    //set up the header data for character output
                    if (CharDict.Count() > 0)
                    {
                        //we only want this output if it is checked
                        if (IncludeCountsAndSumsInOutput) { 
                            foreach (string property in SymbolProperties)
                            {
                                HeaderString += ",\"Char_" + property + "_TotalCount\"";
                            }
                        }

                        foreach (string property in SymbolProperties)
                        {
                            HeaderString += ",\"Char_" + property + "_TotalAvg\"";
                        }

                        //we only want this output if it is checked
                        if (IncludeCountsAndSumsInOutput)
                        {
                            foreach (string property in SymbolProperties)
                            {
                                HeaderString += ",\"Char_" + property + "_TotalSum\"";
                            }
                        }

                        //we only want this output if it is checked
                        if (IncludeCountsAndSumsInOutput)
                        {
                            foreach (string property in SymbolProperties)
                            {
                                HeaderString += ",\"Char_" + property + "_WordCount\"";
                            }
                        }

                        foreach (string property in SymbolProperties)
                        {
                            HeaderString += ",\"Char_" + property + "_WordAvg\"";
                        }

                        //we only want this output if it is checked
                        if (IncludeCountsAndSumsInOutput)
                        {
                            foreach (string property in SymbolProperties)
                            {
                                HeaderString += ",\"Char_" + property + "_WordSum\"";
                            }
                        }

                    }

                    if (WordDict.Count() > 0)
                    {

                        //set up the header data for word output
                        //set up the header data for word output

                        //we only want this output if it is checked
                        if (IncludeCountsAndSumsInOutput)
                        {
                            foreach (string property in SymbolProperties)
                            {
                                HeaderString += ",\"Word_" + property + "_Count\"";
                            }
                        }


                        foreach (string property in SymbolProperties)
                        {
                            HeaderString += ",\"Word_" + property + "_Avg\"";
                        }

                        //we only want this output if it is checked
                        if (IncludeCountsAndSumsInOutput)
                        {
                            foreach (string property in SymbolProperties)
                            {
                                HeaderString += ",\"Word_" + property + "_Sum\"";
                            }
                        }
                    }

                //pull together the header data
                foreach (char c in CharList)
                {
                    HeaderString += ",\"Char_" + c + "_Freq\"";
                }

                outputFile.WriteLine(HeaderString);













                //start looping through each file
                foreach (string fileName in files)
                {

                    //set up our variables to report
                    string Filename_Clean = Path.GetFileName(fileName);
                    long TotalNumberOfWords = 0;
                    long TotalNumberOfDictWords = 0;
                    long TotalNumberOfCharacters = 0;
                    long TotalNumberOfCodedCharacters = 0;
                    long TotalNumberOfWordsWithCodedCharacters = 0;
                    long AverageWordLength = 0;
                    

                    //report what we're working on
                    FilenameLabel.Invoke((MethodInvoker)delegate
                    {
                        FilenameLabel.Text = "Analyzing: " + Filename_Clean;
                    });
                    

                    //read in the text
                    string readText = File.ReadAllText(fileName, SelectedEncoding);
                    //clean up linebreaks and tabs

                    

                    //remove all the junk punctuation when measuring word-level stuff
                    string readText_CharClean = readText;
                    for(int i=0; i < PunctuationToRemove.Length; i++)
                    {
                        readText_CharClean = readText_CharClean.Replace(PunctuationToRemove[i], ' ');
                    }
                    readText_CharClean = readText_CharClean.Replace('\r', ' ');
                    readText_CharClean = readText_CharClean.Replace('\n', ' ');


                    //splits everything out into words
                    string[] Words = readText_CharClean.Trim().Split(' ');
                    readText_CharClean = null;
                    Words = Words.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                        for (int i = 0; i < Words.Length; i++) AverageWordLength += Words[i].Length;


                            TotalNumberOfWords = Words.Length;
                    long TotalnumberofwordsClean = TotalNumberOfWords;
                    TotalNumberOfCharacters = readText.Length;


                    // this is for tracking the number of words that are being captured for EACH
                    //symbol property, since each symbol may occur for some properties but not others
                    ulong[] CharsPerProperty = new ulong[SymbolProperties.Length];
                    ulong[] WordsWithCodedCharsPerProperty = new ulong[SymbolProperties.Length];
                    ulong[] WordsPerProperty = new ulong[SymbolProperties.Length];
                        for (int i = 0; i < SymbolProperties.Length; i++)
                        {
                            CharsPerProperty[i] = 0;
                            WordsWithCodedCharsPerProperty[i] = 0;
                            WordsPerProperty[i] = 0;
                        }




                    //assuming that we have at least one word, we'll code the texts from there
                    if (TotalNumberOfWords > 0)
                    {



                     //  _____ _                          _               _____          _ _             
                     // / ____| |                        | |             / ____|        | (_)            
                     //| |    | |__   __ _ _ __ __ _  ___| |_ ___ _ __  | |     ___   __| |_ _ __   __ _ 
                     //| |    | '_ \ / _` | '__/ _` |/ __| __/ _ \ '__| | |    / _ \ / _` | | '_ \ / _` |
                     //| |____| | | | (_| | | | (_| | (__| ||  __/ |    | |___| (_) | (_| | | | | | (_| |
                     // \_____|_| |_|\__,_|_|  \__,_|\___|\__\___|_|     \_____\___/ \__,_|_|_| |_|\__, |
                     //                                                                             __/ |
                     //                                                                            |___/

                        //set up 2 new dictionaries to track all of the data
                        Dictionary<char, ulong> CharCounts = new Dictionary<char, ulong>();
                        Dictionary<string, double> CharPropertySums = new Dictionary<string, double>();
                        Dictionary<string, double> WordCharPropertySums = new Dictionary<string, double>();

                        foreach (char c in CharList)
                        {
                            CharCounts.Add(c, 0);
                        }

                        foreach (string p in SymbolProperties)
                        {
                            CharPropertySums.Add(p, 0);
                        }




                            //if we have words that we want to omit from character-level analyses,
                            //then we start figuring that out here
                        if ((HashedFuncWords.Count > 0 ) | (FunctionWordWildCards.Count() > 0))
                        {
                            //pull in the whole text, replacing all whitespace with just a single space
                            readText_CharClean = System.Text.RegularExpressions.Regex.Replace(readText, @"\s+", " ");
                            //we're splitting this way to keep double spaces, etc.
                            string[] Words_Clean = readText_CharClean.Split(new string[] { " " }, StringSplitOptions.None);
                            string[] Words_Cleaner = Enumerable.Repeat("", Words_Clean.Length).ToArray();

                            for (int i = 0; i < Words_Clean.Length; i++)
                            {
                                    for (int j = 0; j < PunctuationToRemove.Length; j++)
                                    {

                                        //for each character that we're removing from words, we want to check to
                                        //see how many times it occurs in each word, then set it aside in "Words_Cleaner"
                                        //this is actually pretty complicated logic and involves a lot of handing things back
                                        //and forth between arrays, so the code here might not be incredibly obvious
                                        int charcount = 0;
                                        char CharToCheck = PunctuationToRemove[j];
                                        for (int k = 0; k < Words_Clean[i].Length; k++)
                                        {
                                            if (Words_Clean[i][k] == CharToCheck) charcount++;
                                        }

                                        //Words_Cleaner will now consist of only those characters that we want to remove from *whole words*
                                        //for our word-level analyses, but that we want to retain as basic information for 
                                        //our "character level" analyses
                                        if (charcount > 0) { 
                                            //this actually has to be fixed
                                            Words_Cleaner[i] += new String(PunctuationToRemove[j], charcount);
                                            Words_Clean[i] = Words_Clean[i].Replace(PunctuationToRemove[j].ToString(), "");
                                        }
                                        
                                    }
                                }

                            //now, we're left with 2 arrays:
                            //Words_Clean, which has each word with the specified characters removed, and
                            //Words_Cleaner, which only contains those specific characters from each word

                            //now, what we want to do is loop through each of the "clean" words to determine whether
                            //it is in our function word list. If it is, then what we want our final result to be is
                            //just the leftover characters. If it isn't, then we'll leave it as is
                            //we also set up *yet another* array for comparators, because the word omissions should
                            //be case-insensitive, but we don't want to lose case information *in general*
                            //tricky, I know
                            string[] WordComparators = Words_Clean.Select(s => s.ToLowerInvariant()).ToArray();
                            
                            for (int i = 0; i < Words_Clean.Length; i++)
                                {
                                    if (HashedFuncWords.Contains(WordComparators[i]))
                                        {
                                            Words_Clean[i] = Words_Cleaner[i];
                                            continue;
                                        }
                        
                                    for (int k = 0; k < FunctionWordWildCards.Length; k++)
                                        {
                                            if (WordComparators[i].StartsWith(FunctionWordWildCards[k]))
                                            {
                                                Words_Clean[i] = Words_Cleaner[i];
                                                continue;
                                            }
                                        }
                                }


                             //now, what we should be left with in Words_Clean are just the characters that we
                             //would normally omit from word-level analyses, as well as the words that aren't in
                             //the function word list
                             // let's start out in moving forward by cleaning out the leftover garbage
                             Words_Cleaner = null;
                            //then, we want to *reassemble* the original string that we were going to do
                            //character-level analyses on by 
                            readText = String.Join(" ", Words_Clean);

                            readText_CharClean = readText;
                            //now, lastly, we build our words list *from* the scrubbed text. This will strip punctuation *back out*
                            //yet again, because in the "words clean" array, there are going to be some items that are just punctuation
                            for (int i = 0; i < PunctuationToRemove.Length; i++)
                            {
                                readText_CharClean = readText_CharClean.Replace(PunctuationToRemove[i], ' ');
                            }


                            //splits everything out into words
                            Words = readText_CharClean.Trim().Split(' ');
                            readText_CharClean = null;
                            Words = Words.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                            TotalnumberofwordsClean = Words.Length;
                            Words_Clean = null;
                        }
                        








                      if(CharDict.Count > 0) { 

                            //loop through each character in the text
                            for (int i = 0; i < readText.Length; i++)
                            {
                         
                                //if the character is recognized in the dictionary...
                                if (CharDict.ContainsKey(readText[i]))
                                {
                                    char c = readText[i];

                                    TotalNumberOfCodedCharacters++;

                                    CharCounts[c]++;

                                    for (int j = 0; j < SymbolProperties.Length; j++)
                                    {
                                        //add in the properties being coded from the overarching character dictionary
                                        if (CharDict[c][SymbolProperties[j]] != Ignored_Value)
                                        {
                                            CharPropertySums[SymbolProperties[j]] += CharDict[c][SymbolProperties[j]];
                                            CharsPerProperty[j]++;
                                        }
                                    }
                                }
                            }





                            //do the same thing now, but at the word level (STILL LOOKING AT CHARACTERS, NOT WORD CODING)
                            //set up 2 new dictionaries to track all of the data
                                foreach (string p in SymbolProperties)
                            {
                                WordCharPropertySums.Add(p, 0);
                            }
                            //get the average word length
                            for (int i = 0; i < Words.Length; i++)
                            {
                                AverageWordLength += Words[i].Length;
                                bool WordCodedCharacters = false;
                                bool[] WordCodedSymbols = new bool[SymbolProperties.Length];
                                for (int j = 0; j < SymbolProperties.Length; j++) WordCodedSymbols[j] = false;

                                //create a new key to know which properties to code each word along
                                int[] RecognizedCharacters_per_Property = new int[SymbolProperties.Length];
                                //set them all to 0
                                for (int k = 0; k < SymbolProperties.Length; k++) RecognizedCharacters_per_Property[k] = 0;

                                //loop through each character in the word to first determine the count
                                //of total characters being counted
                                for (int j = 0; j < Words[i].Length; j++)
                                {
                                    if (CharDict.ContainsKey(Words[i][j])){ 
                                        for (int k = 0; k < SymbolProperties.Length; k++)
                                        {
                                            //add in the properties being coded from the overarching character dictionary
                                            if (CharDict[Words[i][j]][SymbolProperties[k]] != Ignored_Value)
                                            {
                                                  RecognizedCharacters_per_Property[k]++;
                                                  WordCodedCharacters = true;
                                                  WordCodedSymbols[k] = true;
                                            }
                                          }
                                        }


                                    }



                                for (int j = 0; j < Words[i].Length; j++)
                                {
                                    char c = Words[i][j];
                                    //if the character is recognized in the dictionary...
                                    if (CharDict.ContainsKey(c))
                                    {
                                        if (WordCodedCharacters)
                                        {
                                            for (int k = 0; k < SymbolProperties.Length; k++)
                                            {
                                                    //add in the properties being coded from the overarching character dictionary
                                                    if (CharDict[c][SymbolProperties[k]] != Ignored_Value)
                                                    {
                                                        WordCharPropertySums[SymbolProperties[k]] += CharDict[c][SymbolProperties[k]] / (double)RecognizedCharacters_per_Property[k];
                                                    }
                                            }
                                        }
                                    }
                                }

                                //for each word, if there was as least one symbol coded, we increment
                                if (WordCodedCharacters) TotalNumberOfWordsWithCodedCharacters++;
                                //do the same for each property for each word
                                for (int j = 0; j < SymbolProperties.Length; j++) if (WordCodedSymbols[j]) WordsWithCodedCharsPerProperty[j]++;

                            }
 
                        }









                            //__          __           _    _____          _ _             
                            //\ \        / /          | |  / ____|        | (_)            
                            // \ \  /\  / /__  _ __ __| | | |     ___   __| |_ _ __   __ _ 
                            //  \ \/  \/ / _ \| '__/ _` | | |    / _ \ / _` | | '_ \ / _` |
                            //   \  /\  / (_) | | | (_| | | |___| (_) | (_| | | | | | (_| |
                            //    \/  \/ \___/|_|  \__,_|  \_____\___/ \__,_|_|_| |_|\__, |
                            //                                                        __/ |
                            //                                                       |___/


                        //NOW, we move on to coding all of the words from the Word Dictionary
                        Dictionary<string, double> WordPropertySums = new Dictionary<string, double>();
                        //only run this if we're actually coding for words
                        if (WordDict.Count > 0)
                        {
                            //convert to lower case if we're ignoring case
                            if (IgnoreCase) Words = Words.Select(s => s.ToLowerInvariant()).ToArray();

                            //set up a dictionary to track each of the properties for the words
                            
                            foreach (string p in SymbolProperties)
                            {
                                WordPropertySums.Add(p, 0);
                            }

                            for (int i = 0; i < Words.Length; i++)
                            {

                                if (WordDict.ContainsKey(Words[i]))
                                {
                                    TotalNumberOfDictWords++;
                              
                                    for (int j = 0; j < SymbolProperties.Length; j++)
                                    {
                                         //add in the properties being coded from the overarching word dictionary
                                        if (WordDict[Words[i]][SymbolProperties[j]] != Ignored_Value)
                                        {
                                            WordPropertySums[SymbolProperties[j]] += WordDict[Words[i]][SymbolProperties[j]];
                                            WordsPerProperty[j]++;
                                        }
                                    }
                                }
                            }

                        }











                            //  ____        _               _   
                            // / __ \      | |             | |  
                            //| |  | |_   _| |_ _ __  _   _| |_ 
                            //| |  | | | | | __| '_ \| | | | __|
                            //| |__| | |_| | |_| |_) | |_| | |_ 
                            // \____/ \__,_|\__| .__/ \__,_|\__|
                            //                 | |              
                            //                 |_|       




                            //this is for when we're writing the output
                            ushort NumberOfOutputColumns = 0;
                            if (CharDict.Count > 0) {
                                if (IncludeCountsAndSumsInOutput)
                                {
                                    NumberOfOutputColumns += 6;
                                } else
                                {
                                    NumberOfOutputColumns += 2;
                                }

                            }

                            if (WordDict.Count > 0)
                            {
                                if (IncludeCountsAndSumsInOutput)
                                {
                                    NumberOfOutputColumns += 3;
                                }
                                else
                                {
                                    NumberOfOutputColumns += 1;
                                }
                            }




                        string[] OutputString = new string[HeaderVars + (SymbolProperties.Length * NumberOfOutputColumns) + CharList.Length];
                            for (int i = 0; i < OutputString.Length; i++) OutputString[i] = "";
                            OutputString[0] = '"' + Filename_Clean + '"';
                            OutputString[1] = TotalNumberOfWords.ToString();
                            OutputString[2] = TotalnumberofwordsClean.ToString();
                            OutputString[3] = TotalNumberOfCharacters.ToString();
                            OutputString[4] = TotalNumberOfCodedCharacters.ToString();
                            OutputString[5] = TotalNumberOfWordsWithCodedCharacters.ToString();
                            OutputString[6] = Math.Round(AverageWordLength / (double)TotalNumberOfWords, 5).ToString();
                            OutputString[7] = TotalNumberOfDictWords.ToString();

                        int PropCount;
                        ushort OutputPositionIncrementer = 0;


                        //we only provide character output if we coded characters
                        if (CharDict.Count > 0)
                            {

                                if (IncludeCountsAndSumsInOutput)
                                {
                                    PropCount = SymbolProperties.Count() * OutputPositionIncrementer;
                                    if (CharList.Length > 0)
                                    {
                                        //add in the properties being coded from the overarching character dictionary (counts)
                                        for (int i = 0; i < SymbolProperties.Length; i++)
                                        {
                                            OutputString[i + HeaderVars + PropCount] = CharsPerProperty[i].ToString();
                                        }
                                        OutputPositionIncrementer++;
                                    }
                                }


                                PropCount = SymbolProperties.Count() * OutputPositionIncrementer;
                                if (CharList.Length > 0)
                                {
                                    //add in the properties being coded from the overarching character dictionary (averages)
                                    for (int i = 0; i < SymbolProperties.Length; i++)
                                    {
                                        if (CharsPerProperty[i] > 0) OutputString[i + HeaderVars + PropCount] = Math.Round(CharPropertySums[SymbolProperties[i]] / (double)CharsPerProperty[i], 5).ToString();
                                    }
                                    OutputPositionIncrementer++;
                                }


                                if (IncludeCountsAndSumsInOutput)
                                {
                                    //raw sums of each variable
                                    PropCount = SymbolProperties.Length * OutputPositionIncrementer;
                                    for (int i = 0; i < SymbolProperties.Length; i++)
                                    {
                                        if (CharsPerProperty[i] > 0) OutputString[i + HeaderVars + PropCount] = CharPropertySums[SymbolProperties[i]].ToString();
                                    }
                                    OutputPositionIncrementer++;
                                }




                                //now we do the same things, but for the word level

                                if (IncludeCountsAndSumsInOutput)
                                {
                                    PropCount = SymbolProperties.Count() * OutputPositionIncrementer;
                                    if (CharList.Length > 0)
                                    {
                                        //add in the properties being coded from the overarching character dictionary
                                        for (int i = 0; i < SymbolProperties.Length; i++)
                                        {
                                            OutputString[i + HeaderVars + PropCount] = WordsWithCodedCharsPerProperty[i].ToString();
                                        }
                                        OutputPositionIncrementer++;
                                    }
                                }


                                    //this is the average at the word level
                                    PropCount = SymbolProperties.Length * OutputPositionIncrementer;
                                    for (int i = 0; i < SymbolProperties.Length; i++)
                                    {
                                        if (WordsWithCodedCharsPerProperty[i] > 0) OutputString[i + HeaderVars + PropCount] = Math.Round(WordCharPropertySums[SymbolProperties[i]] / (double)WordsWithCodedCharsPerProperty[i], 5).ToString();
                                    }
                                    OutputPositionIncrementer++;




                                    if (IncludeCountsAndSumsInOutput)
                                    {
                                        //this is the sum at the word level
                                        PropCount = SymbolProperties.Length * OutputPositionIncrementer;
                                        for (int i = 0; i < SymbolProperties.Length; i++)
                                        {
                                            if (WordsWithCodedCharsPerProperty[i] > 0) OutputString[i + HeaderVars + PropCount] = WordCharPropertySums[SymbolProperties[i]].ToString();
                                        }
                                        OutputPositionIncrementer++;
                                    }


                                }
                        

                            //again, we only write this stuff if we're actually coding for any words
                            if (WordDict.Count() > 0)
                            {
                                //this is where we'll do the same thing as above, but for word-level measures


                                if (IncludeCountsAndSumsInOutput)
                                {
                                    //now we do the same things, but for the word level
                                    PropCount = SymbolProperties.Count() * OutputPositionIncrementer;

                                    //add in the properties being coded from the overarching character dictionary
                                    for (int i = 0; i < SymbolProperties.Length; i++)
                                    {
                                        OutputString[i + HeaderVars + PropCount] = WordsPerProperty[i].ToString();
                                    }
                                    OutputPositionIncrementer++;
                                }




                                    PropCount = SymbolProperties.Length * OutputPositionIncrementer;
                                    for (int i = 0; i < SymbolProperties.Length; i++)
                                    {
                                        if (WordsPerProperty[i] > 0) OutputString[i + HeaderVars + PropCount] = Math.Round(WordPropertySums[SymbolProperties[i]] / (double)WordsPerProperty[i], 5).ToString();
                                    }
                                    OutputPositionIncrementer++;


                                if (IncludeCountsAndSumsInOutput)
                                {
                                    PropCount = SymbolProperties.Length * OutputPositionIncrementer;
                                    for (int i = 0; i < SymbolProperties.Length; i++)
                                    {
                                        if (WordsPerProperty[i] > 0) OutputString[i + HeaderVars + PropCount] = WordPropertySums[SymbolProperties[i]].ToString();
                                    }
                                    OutputPositionIncrementer++;
                                }
                                
                            }

                        //add in the counts for each character
                        PropCount = SymbolProperties.Length * OutputPositionIncrementer;
                            for (int i = 0; i < CharList.Length; i++)
                            {
                            OutputString[i + HeaderVars + PropCount] = CharCounts[CharList[i]].ToString();
                            }
                            OutputPositionIncrementer++;


                        outputFile.WriteLine(String.Join(",", OutputString));


                    }
                    else
                    {
                        outputFile.WriteLine('"' + Filename_Clean + '"' + "," + TotalNumberOfWords.ToString());
                    }


                    if (BgWorker.CancellationPending) break;

               }

            }


            }
            catch
            {
                MessageBox.Show("TAPA could not open your output file\r\nfor writing. Is the file open in another application?", "Analysis Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }












        }


        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableAllStuff();
            FilenameLabel.Text = "Finished!";
            MessageBox.Show("TAPA has finished analyzing your texts.", "Analysis Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }




        private void AddColumnButton_Click(object sender, EventArgs e)
        {

            string NewColName = "VariableName";
            ShowInputDialog(ref NewColName);

            if (NewColName == "VariableName")
            {
                return;
            }

            bool AlreadyExisting = false;
            foreach (DataGridViewColumn col in NormDataGrid.Columns)
            {
                if (NewColName == col.Name)
                {
                    MessageBox.Show("This variable name is already in use.", "Duplicate Variable", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }

            if (AlreadyExisting == false)
            {
                NormDataGrid.Columns.Add(NewColName, NewColName);
                NormDataGrid.Columns[NormDataGrid.Columns.Count - 1].ValueType = typeof(string);
            }


        }

        private void NormDataGrid_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            for (int i = 1; i < NormDataGrid.Columns.Count; i++)
            {
                NormDataGrid.Rows[NormDataGrid.Rows.Count - 2].Cells[i].Value = 0;
            }
        }




        private void LoadNormsButton_Click(object sender, EventArgs e)
        {

            FilenameLabel.Text = "Scanning external norms file... please wait...";

            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                FilenameLabel.Text = "Waiting to analyze texts...";
                return;
            }
            
            if (openFileDialog.FileName != null)
            {
                //old way of loading was to read the whole file
                //LoadNorms(File.ReadAllText(openFileDialog.FileName, Encoding.GetEncoding(EncodingDropdown.SelectedItem.ToString())).Trim().Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None));

                //new way of loading is to just pass the info to the reader and let it do it line by line
                LoadNorms(openFileDialog.FileName);

            }

        }






        private void LoadNorms(string Filename)
        {

            DisableAllStuff();

            //this sets us up so that we know the total number of lines we want to get
            var lineCount = 0;
          
            using (var reader = new StreamReader(Filename, Encoding.GetEncoding(EncodingDropdown.SelectedItem.ToString())))
                {
                    while (reader.ReadLine() != null)
                    {
                        lineCount++;
                        if (lineCount % 1000 == 0)
                        {
                            FilenameLabel.Text = "Scanning external norms file... please wait... scanning row #" + lineCount.ToString();
                            FilenameLabel.Update();
                            FilenameLabel.Refresh();
                            Application.DoEvents();
                        }
                    }
                }
                       

            TAPA.NormLoadingWindow LoadingWindow = new TAPA.NormLoadingWindow();
            LoadingWindow.ProgressBarMaxSet = lineCount;
            LoadingWindow.Show();

            //using a data table to update the grid -- this is
            //far, far faster than working directly on the grid itself
            DataTable dt = new DataTable();

            double retNum;
            int numcols = 0;
            NormDataGrid.Columns.Clear();

            try
            {


                using (var reader = new StreamReader(Filename, Encoding.GetEncoding(EncodingDropdown.SelectedItem.ToString())))
                {

                    uint rownum = 0;

                    String InputFileText;
                    while ((InputFileText = reader.ReadLine()) != null)
                    {
                        if (((rownum + 1) % 250 == 0) | (rownum + 1 == lineCount))
                        {
                            FilenameLabel.Text = "Loading norms file row " + (rownum + 1).ToString() + " of " + lineCount.ToString();
                            LoadingWindow.ProgressValueUpdate = rownum + 1;
                            FilenameLabel.Update();
                            FilenameLabel.Refresh();
                            Application.DoEvents();
                        }


                        //the first row of the norms tells us the column names and headers
                        if (rownum == 0)
                        {
                            string[] HeaderRow = InputFileText.Split('\t');
                            numcols = HeaderRow.Length;
                            for (uint j = 0; j < HeaderRow.Length; j++)
                            {
                                dt.Columns.Add(HeaderRow[j]);
                            }
                            dt.Columns[0].DataType = typeof(string);
                            dt.Columns[1].DataType = typeof(string);
                            for (int j = 2; j < dt.Columns.Count; j++)
                            {
                                dt.Columns[j].DataType = typeof(double);
                                dt.Columns[j].AllowDBNull = true;
                            }

                        }
                        else if (!String.IsNullOrEmpty(InputFileText))
                        {
                            string[] RowToAdd = InputFileText.Split('\t');

                            //make sure that blank spots get replaced with a 0
                            for (int j = 0; j < RowToAdd.Length; j++)
                            {

                                if (String.IsNullOrEmpty(RowToAdd[j])) RowToAdd[j] = "";
                                if ((j > 1) && (double.TryParse(RowToAdd[j], out retNum) == false)) RowToAdd[j] = "";

                                //make sure that there's leading/trailing space
                                RowToAdd[j] = RowToAdd[j].Trim();
                            }

                            
                            //in the off chance that there's a screw-up, this will try to output
                            //the row in question into a file
                            if (RowToAdd.Length > numcols)
                            {
                                try
                                {
                                    using (StreamWriter file = new StreamWriter("error_row.txt"))
                                        file.Write(String.Join("\t", RowToAdd));
                                }
                                catch
                                {

                                }
                            }

                            //we have to go in and make sure that blank cells are actually blank
                            //we do this by transferring the incoming string to a new "object" array
                            //which, since it lacks a type, allows us to mix strings with DBNull values
                            object[] array_to_add = new object[RowToAdd.Length];

                            for(int j = 0; j < RowToAdd.Length; j++)
                            {
                                if (!string.IsNullOrWhiteSpace(RowToAdd[j]))
                                { 
                                    array_to_add[j] = RowToAdd[j];
                                }
                                else
                                {
                                    array_to_add[j] = DBNull.Value;
                                }
                            }

                            //finally, we actually add in the row here
                            dt.Rows.Add(array_to_add);
                        }

                        rownum += 1;

                    }

                }





                FilenameLabel.Text = "Updating norm grid...";
                Application.DoEvents();
                NormDataGrid.DataSource = dt;
            }
            catch (Exception ex)
            {

                FilenameLabel.Text = "Updating norm grid...";
                Application.DoEvents();
                NormDataGrid.DataSource = dt;
                MessageBox.Show("There was an error reading your file.", "File Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                LoadingWindow.Close();
                FilenameLabel.Text = "Norm loading process complete.";
                EnableAllStuff();
            }
        }


        //we use this only when loading the default norms, as the default norms
        //are included as a resource within the application. this prevents us from
        //having to do a bunch of workaround logic in the main "LoadNorms" section
        //to handle turning the resource into a specific type of filestream or,
        //alternatively, exporting the resource file to disk then loading it
        //back in from there
        private void LoadDefaultNorms(string[] InputFileText)
        {
            DisableAllStuff();
            TAPA.NormLoadingWindow LoadingWindow = new TAPA.NormLoadingWindow();
            LoadingWindow.ProgressBarMaxSet = InputFileText.Length;
            LoadingWindow.Show();
            //using a data table to update the grid -- this is
            //far, far faster than working directly on the grid itself
            DataTable dt = new DataTable();
            try
            {
                double retNum;
                NormDataGrid.Columns.Clear();
                int maxlen = InputFileText.Length;
                for (uint i = 0; i < maxlen; i++)
                {
                    if (((i + 1) % 250 == 0) | (i + 1 == maxlen))
                    {
                        FilenameLabel.Text = "Loading norms row " + (i + 1).ToString() + " of " + InputFileText.Length.ToString();
                        LoadingWindow.ProgressValueUpdate = i + 1;
                        FilenameLabel.Update();
                        FilenameLabel.Refresh();
                        Application.DoEvents();
                    }
                    //the first row of the norms tells us the column names and headers
                    if (i == 0)
                    {
                        string[] HeaderRow = InputFileText[i].Split('\t');
                        for (uint j = 0; j < HeaderRow.Length; j++)
                        {
                            dt.Columns.Add(HeaderRow[j]);
                        }
                        dt.Columns[0].DataType = typeof(string);
                        dt.Columns[1].DataType = typeof(string);
                        for (int j = 2; j < dt.Columns.Count; j++)
                        {
                            dt.Columns[j].DataType = typeof(double);
                            dt.Columns[j].AllowDBNull = true;
                        }
                    }
                    else
                    {
                        string[] RowToAdd = InputFileText[i].Split('\t');
                        //make sure that blank spots get replaced with a 0
                        for (int j = 0; j < RowToAdd.Length; j++)
                        {
                            if (String.IsNullOrEmpty(RowToAdd[j])) RowToAdd[j] = "";
                            if ((j > 1) && (double.TryParse(RowToAdd[j], out retNum) == false)) RowToAdd[j] = "";
                            //make sure that there's leading/trailing space
                            RowToAdd[j] = RowToAdd[j].Trim();
                        }
                        object[] array_to_add = new object[RowToAdd.Length];

                        for (int j = 0; j < RowToAdd.Length; j++)
                        {
                            if (!string.IsNullOrWhiteSpace(RowToAdd[j]))
                            {
                                array_to_add[j] = RowToAdd[j];
                            }
                            else
                            {
                                array_to_add[j] = DBNull.Value;
                            }
                        }
                        dt.Rows.Add(array_to_add);
                    }
                    InputFileText[i] = null;
                }
                InputFileText = null;
                FilenameLabel.Text = "Updating norm grid...";
                Application.DoEvents();
                NormDataGrid.DataSource = dt;
            }
            catch
            {
                MessageBox.Show("There was an error reading your file.", "File Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                LoadingWindow.Close();
                FilenameLabel.Text = "Norm loading process complete.";
                EnableAllStuff();
            }
        }






        private void DisableAllStuff()
        {
            StartButton.Enabled = false;
            ScanSubfolderCheckbox.Enabled = false;
            EncodingDropdown.Enabled = false;
            NormDataGrid.Enabled = false;
            PunctuationBox.Enabled = false;
            AddColumnButton.Enabled = false;
            LoadNormsButton.Enabled = false;
            FunctionWordTextBox.Enabled = false;
            StopListButton.Enabled = false;
            WordCaseSensitiveCheckbox.Enabled = false;
            CountsAndSumsCheckbox.Enabled = false;
            NormDataGrid.DefaultCellStyle.BackColor = Color.Gray;
        }

        private void EnableAllStuff()
        {
            StartButton.Enabled = true;
            ScanSubfolderCheckbox.Enabled = true;
            EncodingDropdown.Enabled = true;
            NormDataGrid.Enabled = true;
            PunctuationBox.Enabled = true;
            AddColumnButton.Enabled = true;
            LoadNormsButton.Enabled = true;
            WordCaseSensitiveCheckbox.Enabled = true;
            FunctionWordTextBox.Enabled = true;
            StopListButton.Enabled = true;
            CountsAndSumsCheckbox.Enabled = true;
            NormDataGrid.DefaultCellStyle.BackColor = Color.White;
        }






        private void StopListButton_Click(object sender, EventArgs e)
        {
            //load up our function word list
            FunctionWordTextBox.Text = TAPA.Properties.Resources.function_word_list.ToString();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Would you like to load the default norms?", "Load defaults?", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes) {
                LoadDefaultNorms(TAPA.Properties.Resources.Ratings_Warriner_et_al.ToString().Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None));
                FilenameLabel.Text = "Default norms are loaded and TAPA is ready to use.";
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (BgWorker.IsBusy)
            {
                BgWorker.CancelAsync();
                FilenameLabel.Text = "Cancelling analysis...";
            }
        }

    }
    
}
