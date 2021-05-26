using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Notes
{
    public enum MatchSource
    {
        AllDocuments,
        CurrentDocument,
        Selection
    }

    public class Region 
    {
        public readonly TextDocument Document;

        public readonly int Start;

        public readonly int Length;

        public Region(TextDocument document, int index, int length)
        {
            Document = document;
            Start = index;
            Length = length;
        }

        public override string ToString() => $"Start = {Start}, Length = {Length}";
    }

    public class FindPanel : Panel
    {
        const string DialogTitle = "Find...";

        public TextDocument ActiveDocument => Get.Current<MainViewModel>().ActiveDocument;

        public DocumentCollection Documents => Get.Current<MainViewModel>().Documents;

        bool matchCase = false;
        public bool MatchCase
        {
            get => matchCase;
            set => this.Change(ref matchCase, value);
        }

        bool matchWord = false;
        public bool MatchWord
        {
            get => matchWord;
            set => this.Change(ref matchWord, value);
        }

        string findText = string.Empty;
        public string FindText
        {
            get => findText;
            set => this.Change(ref findText, value);
        }

        string replaceText = string.Empty;
        public string ReplaceText
        {
            get => replaceText;
            set => this.Change(ref replaceText, value ?? string.Empty);
        }

        Region originalSelection = null;
        public Region OriginalSelection
        {
            get => originalSelection;
            set => CurrentSelection = originalSelection = value;
        }

        Region CurrentSelection = null;

        public override string Title => "Find";

        public FindPanel() : base(Resources.Uri(nameof(Notes), "Images/Zoom.png")) { }

        //----------------------------------------------------------------

        void NoMatches() => Dialog.Show(DialogTitle, "No matches...", DialogImage.Exclamation, DialogButtons.Ok);

        //----------------------------------------------------------------

        IEnumerable<Tuple<TextDocument, int>> FindAll()
        {
            switch (Get.Current<Options>().FindMatchSource)
            {
                case MatchSource.CurrentDocument:
                    foreach (var i in FindAll(ActiveDocument))
                        yield return Tuple.Create(ActiveDocument, i);

                    break;

                case MatchSource.AllDocuments:
                    foreach (var i in Documents)
                    {
                        if (Supported(i))
                        {
                            foreach (var j in FindAll((TextDocument)i))
                                yield return Tuple.Create((TextDocument)i, j);
                        }
                    }
                    break;
            }
        }

        IEnumerable<int> FindAll(TextDocument document)
        {
            int start = 0;

            var next = 0;
            while ((next = FindNext(new Region(document, start, findText.Length)).Start) != -1)
            {
                yield return next;
                start = next + FindText.Length;
            }
            yield break;
        }

        //----------------------------------------------------------------

        string Replace(int index, string input)
        {
            if (index < 0)
                return input;

            return $"{input.Substring(0, index)}{replaceText}{input.Substring(index + FindText.Length, input.Length - (index + FindText.Length))}";
        }

        //----------------------------------------------------------------

        bool Can()
        {
            if (!FindText.NullOrEmpty())
            {
                if (Documents.Any<Document>())
                {
                    switch (Get.Current<Options>().FindMatchSource)
                    {
                        case MatchSource.AllDocuments:
                            foreach (var i in Documents)
                            {
                                if (Supported(i))
                                    return true;
                            }
                            return false;

                        case MatchSource.CurrentDocument:
                            return Supported(ActiveDocument);

                        case MatchSource.Selection:
                            return Supported(ActiveDocument) && ActiveDocument.SelectionLength > 0;
                    }
                }
            }
            return false;
        }

        //----------------------------------------------------------------

        bool Supported(Document document) => Supported(document as TextDocument);

        bool Supported(TextDocument document) => document is Note;

        //----------------------------------------------------------------

        void FindReplace(bool direction, bool replace)
        {
            //Determine what the original and current selection should be. 
            //The original selection should be changed externally. 
            //The current selection is always determined internally.
            switch (Get.Current<Options>().FindMatchSource)
            {
                case MatchSource.CurrentDocument:

                    if (OriginalSelection == null || OriginalSelection.Document != ActiveDocument)
                        OriginalSelection = new Region(ActiveDocument, 0, 0);

                    CurrentSelection = CurrentSelection ?? OriginalSelection;
                    break;

                case MatchSource.AllDocuments:

                    if (OriginalSelection == null)
                    {
                        var firstDocument = Documents.FirstOrDefault(i => Supported(i)) as TextDocument;
                        OriginalSelection = new Region(firstDocument, 0, 0);
                    }

                    CurrentSelection = CurrentSelection ?? OriginalSelection;
                    break;

                case MatchSource.Selection:

                    if (OriginalSelection == null || OriginalSelection.Length <= 0)
                    {
                        Dialog.Show(DialogTitle, "Selection cannot have zero length!", DialogImage.Exclamation, DialogButtons.Ok);
                        return;
                    }

                    CurrentSelection = CurrentSelection ?? new Region(OriginalSelection.Document, OriginalSelection.Start, 0);
                    break;
            }

            //If the current selection is invalid in the following ways, we cannot find a new one based on it
            if (CurrentSelection == null || CurrentSelection.Document.Not<TextDocument>() || !Supported(CurrentSelection.Document))
                return;

            //Get a new selection based on the current selection
            switch (direction)
            {
                case true:
                    CurrentSelection = FindNext(CurrentSelection);
                    break;

                case false:
                    CurrentSelection = FindPrevious(CurrentSelection);
                    break;
            }

            //------------- We found a match
            if (CurrentSelection != null)
            {
                //Get the new document in focus (if applicable)
                switch (Get.Current<Options>().FindMatchSource)
                {
                    case MatchSource.CurrentDocument:
                    case MatchSource.Selection:
                        break;

                    case MatchSource.AllDocuments:

                        if (CurrentSelection.Document != ActiveDocument)
                            Get.Current<MainViewModel>().ActiveContent = CurrentSelection.Document;

                        break;
                }
                //Set the new selection in the new document (and/or replace)
                switch (replace)
                {
                    case true:
                        CurrentSelection.Document.Text = Replace(CurrentSelection.Start, CurrentSelection.Document.Text);
                        CurrentSelection.Document.SelectionStart = CurrentSelection.Start;
                        CurrentSelection.Document.SelectionLength = replaceText.Length;
                        break;

                    case false:
                        CurrentSelection.Document.SelectionStart = CurrentSelection.Start;
                        CurrentSelection.Document.SelectionLength = findText.Length;
                        break;
                }
                //Do nothing more...
                return;
            }

            //------------- We didn't find a match...

            //Get the original document in focus (if applicable)
            switch (Get.Current<Options>().FindMatchSource)
            {
                case MatchSource.CurrentDocument:
                case MatchSource.Selection:
                    break;

                case MatchSource.AllDocuments:

                    if (OriginalSelection.Document != ActiveDocument)
                        Get.Current<MainViewModel>().ActiveContent = OriginalSelection.Document;

                    break;
            }

            //Show a dialog
            NoMatches();

            //Set the original selection in the original document
            OriginalSelection.Document.SelectionStart = OriginalSelection.Start;
            OriginalSelection.Document.SelectionLength = 0;
            OriginalSelection.Document.CaretIndex = OriginalSelection.Start;
        }

        //----------------------------------------------------------------

        Region FindNext(Region input)
        {
            switch (Get.Current<Options>().FindMatchSource)
            {
                case MatchSource.CurrentDocument:
                    return NextCurrent(input, true);

                case MatchSource.AllDocuments:
                    return NextAll(input);

                case MatchSource.Selection:
                    break;
            }
            return null;
        }

        Region FindPrevious(Region input)
        {
            switch (Get.Current<Options>().FindMatchSource)
            {
                case MatchSource.CurrentDocument:
                    return PreviousCurrent(input, true);

                case MatchSource.AllDocuments:
                    return PreviousAll(input);

                case MatchSource.Selection:
                    break;
            }
            return null;
        }

        //----------------------------------------------------------------

        /// <summary>
        /// Searches from the end of the current selection to the end of the document, then from the beginning of the same document to the beginning of the current selection.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Region NextCurrent(Region input, bool circular)
        {
            /*
            if (MatchWord)
            {
                var a = document.Text[result + findText.Length];
                var b = document.Text[result - 1];
                if (a.ToString().AlphaNumeric() || b.ToString().AlphaNumeric())
                    result = -1;
            }
            */

            var start = input.Start + input.Length;
            //If we're already at the end of the document
            if (input.Start == input.Document.Text.Length)
            {
                //If we're allowed to circle back
                if (circular)
                {
                    //Circle back now versus later
                    start = 0;
                    circular = false;
                }
            }

            var j = 0;
            for (int i = start, count = input.Document.Text.Length; i < count; i++)
            {
                //Get the next character
                var character = input.Document.Text[i];
                //If we did not match all characters
                if (j < findText.Length)
                {
                    var a = character;
                    var b = findText[j];

                    if (!MatchCase)
                    {
                        a = $"{a}".ToLower()[0];
                        b = $"{b}".ToLower()[0];
                    }

                    if (a == b)
                    {
                        j++;
                    }
                    else
                    {
                        j = 0;
                    }
                }
                //If we did match all characters
                if (j == findText.Length)
                {
                    return new Region(input.Document, i - findText.Length + 1, findText.Length);
                }

                //We didn't match all characters!
                //If we reached the beginning of the original selection
                if (OriginalSelection.Start - 1 >= 0)
                {
                    if (i == OriginalSelection.Start - 1)
                        break;
                }
                else
                {
                    if (i == input.Document.Text.Length - 1)
                    {
                        break;
                    }
                }

                //Circle back!
                if (i == input.Document.Text.Length - 1)
                {
                    if (!circular)
                        break;

                    i = -1;
                    j = 0;
                    count = input.Start;
                }
            }
            return null;
        }

        /// <summary>
        /// Searches from the beginning of the current selection to the beginning of the document, then from the end of the same document to the end of the current selection.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Region PreviousCurrent(Region input, bool circular)
        {
            var start = input.Start - 1;
            //If we're already at the beginning of the document
            if (input.Start == 0)
            {                
                //If we're allowed to circle back
                if (circular)
                {                    
                    //Circle back now versus later
                    start = input.Document.Text.Length - 1;
                    circular = false;
                }
            }

            var j = 0;
            for (int i = start, count = 0; i >= count; i--)
            {
                //Get the next character
                var character = input.Document.Text[i];
                //If we did not match all characters
                if (j >= 0)
                {
                    var a = character;
                    var b = findText[j];

                    if (!MatchCase)
                    {
                        a = $"{a}".ToLower()[0];
                        b = $"{b}".ToLower()[0];
                    }

                    if (a == b)
                    {
                        j--;
                    }
                    else
                    {
                        j = findText.Length - 1;
                    }
                }
                //If we did match all characters
                if (j < 0)
                {
                    return new Region(input.Document, i, findText.Length);
                }

                //We didn't match all characters!
                //If we reached the end of the original selection
                if (i == OriginalSelection.Start + OriginalSelection.Length)
                    break;

                //Circle back!
                if (i == 0)
                {
                    if (!circular)
                        break;

                    i = input.Document.Text.Length;
                    j = findText.Length - 1;
                    count = input.Start + input.Length;
                }
            }
            return null;
        }

        //----------------------------------------------------------------

        /// <summary>
        /// Reevaluate!
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Region NextAll(Region input)
        {
            var document = input.Document;
            while (document != null)
            {
                var result 
                    = document == input.Document 
                    ? NextCurrent(input, false) 
                    : NextCurrent(new Region(document, 0, 0), false);

                if (result != null)
                    return result;

                if (result == null)
                    document = NextDocument(document);

                if (document == input.Document)
                    break;
            }
            return null;
        }

        /// <summary>
        /// Reevaluate!
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Region PreviousAll(Region input)
        {
            var document = input.Document;
            while (document != null)
            {
                var result
                    = document == input.Document
                    ? PreviousCurrent(input, false)
                    : PreviousCurrent(new Region(document, 0, 0), false);

                if (result != null)
                {
                    return result;
                }

                if (result == null)
                    document = PreviousDocument(document);

                if (document == input.Document)
                    break;
            }
            return null;
        }

        //----------------------------------------------------------------

        TextDocument PreviousDocument(TextDocument input)
        {
            bool circular = Documents.IndexOf(input) < Documents.Count - 1;
            for (int i = Documents.IndexOf(input), count = 0; i >= count; i--)
            {
                var document = (TextDocument)Documents[i];
                if (document != input)
                {
                    if (Supported(document))
                    {
                        return document;
                    }
                }
                if (i == 0)
                {
                    i = Documents.Count;
                    count = Documents.IndexOf(input) + 1;
                }
            }
            return null;
        }

        TextDocument NextDocument(TextDocument input)
        {
            bool circular = Documents.IndexOf(input) > 0;
            for (int i = Documents.IndexOf(input), count = Documents.Count; i < count; i++)
            {
                var document = (TextDocument)Documents[i];
                if (document != input)
                {
                    if (Supported(document))
                    {
                        return document;
                    }
                }
                if (circular)
                {
                    if (i == Documents.Count - 1)
                    {
                        i = -1;
                        count = Documents.IndexOf(input);
                    }
                }
            }
            return null;
        }

        //----------------------------------------------------------------

        ICommand findNextCommand;
        public ICommand FindNextCommand
        {
            get
            {
                findNextCommand = findNextCommand ?? new RelayCommand(() => FindReplace(true, false), () => Can());
                return findNextCommand;
            }
        }

        ICommand findPreviousCommand;
        public ICommand FindPreviousCommand
        {
            get
            {
                findPreviousCommand = findPreviousCommand ?? new RelayCommand(() => FindReplace(false, false), () => Can());
                return findPreviousCommand;
            }
        }

        //----------------------------------------------------------------

        ICommand replaceNextCommand;
        public ICommand ReplaceNextCommand
        {
            get
            {
                replaceNextCommand = replaceNextCommand ?? new RelayCommand(() => FindReplace(true, true), () => Can());
                return replaceNextCommand;
            }
        }

        ICommand replacePreviousCommand;
        public ICommand ReplacePreviousCommand
        {
            get
            {
                replacePreviousCommand = replacePreviousCommand ?? new RelayCommand(() => FindReplace(false, true), () => Can());
                return replacePreviousCommand;
            }
        }

        ICommand replaceAllCommand;
        public ICommand ReplaceAllCommand
        {
            get
            {
                replaceAllCommand = replaceAllCommand ?? new RelayCommand(() =>
                {
                    var all = FindAll();
                    if (!all.Empty())
                    {
                        foreach (var i in all)
                            i.Item1.Text = Replace(i.Item2, i.Item1.Text);
                    }
                    else NoMatches();
                }, () => Can());
                return replaceAllCommand;
            }
        }
    }
}