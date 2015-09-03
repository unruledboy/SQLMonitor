using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

namespace Xnlab.SQLMon.Common
{
    public enum ObjectModes
    {
        None,
        Databases,
        Objects,
        Table,
        Sp,
        View,
        Function,
        Assembly,
        Trigger,
        Job,
        Server,
        Index
    }

    internal enum WorkModes
    {
        Summary = 0,
        Objects = 1,
        Activities = 2,
        Performance = 3,
        Analysis = 4,
        Alerts = 5,
        Histories = 6,
        Options = 7,
        Query = 8,
        TableData = 9,
        UserPerformance = 10
    }

    class Utils
    {
        internal const int EmptyIndex = -1;
        internal const string FileExtenionSql = ".sql";
        internal const string SizeKb = "KB";
        internal const string SizeMb = "MB";
        internal const string SizeGb = "GB";
        internal const int Size1K = 1024;
        internal const string TimeMs = "ms";
        internal const string MultiCommentStart = "/*";
        internal const string MultiCommentEnd = "*/";
        internal const string SingleCommentStart = "--";

        internal static T CloneObject<T>(T objectInstance)
        {
            var bFormatter = new BinaryFormatter();
            var stream = new MemoryStream();
            bFormatter.Serialize(stream, objectInstance);
            stream.Seek(0, SeekOrigin.Begin);
            return (T)bFormatter.Deserialize(stream);
        }

        internal static void SetDragDropContent(TextBox editor, DragEventArgs e)
        {
            var result = GetDragDropContent(e);
            if (!string.IsNullOrEmpty(result))
                editor.Text = result;
        }

        internal static string GetDragDropContent(DragEventArgs e)
        {
            string result = null;
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
                result = e.Data.GetData(DataFormats.StringFormat).ToString();
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = e.Data.GetData(DataFormats.FileDrop) as string[];
                result = File.ReadAllText(files.First());
            }
            return result;
        }

        internal static void HandleContentDragEnter(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat)
                || e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        internal static string FormatSize(long size)
        {
            if (size > Size1K * Size1K * Size1K)
                return Convert.ToInt32(size / Size1K / Size1K / Size1K) + " " + SizeGb;
            else if (size > Size1K * Size1K)
                return Convert.ToInt32(size / Size1K / Size1K) + " " + SizeMb;
            else if (size > Size1K)
                return Convert.ToInt32(size / Size1K) + " " + SizeKb;
            else
                return size + " B";
        }

        internal static void SetTextBoxStyle(TextEditorControl editor)
        {
            editor.ShowEOLMarkers = false;
            editor.ShowHRuler = false;
            editor.ShowInvalidLines = false;
            editor.ShowSpaces = false;
            editor.ShowTabs = false;
            editor.ShowMatchingBracket = true;
            editor.AllowCaretBeyondEOL = false;
            editor.ShowVRuler = false;
            editor.ImeMode = ImeMode.On;
            editor.SetHighlighting("TSQL");
        }

        internal static void SelectText(TextEditorControl editor, string text)
        {
            var offset = editor.Text.IndexOf(text);
            var endOffset = offset + text.Length;
            editor.ActiveTextAreaControl.TextArea.Caret.Position = editor.ActiveTextAreaControl.TextArea.Document.OffsetToPosition(endOffset);
            editor.ActiveTextAreaControl.TextArea.SelectionManager.ClearSelection();
            var document = editor.ActiveTextAreaControl.TextArea.Document;
            var selection = new DefaultSelection(document, document.OffsetToPosition(offset), document.OffsetToPosition(endOffset));
            editor.ActiveTextAreaControl.TextArea.SelectionManager.SetSelection(selection);
        }

        internal static void Split(string content, string splitter, out string key, out string value)
        {
            var index = content.IndexOf(splitter);
            if (index != -1)
            {
                key = content.Substring(0, index);
                value = content.Substring(index + 1).Replace(@"\r\n", "\r\n");
            }
            else
            {
                key = string.Empty;
                value = string.Empty;
            }
        }
    }
}
