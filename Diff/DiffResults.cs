using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using Xnlab.SQLMon.Common;
using Xnlab.SQLMon.Properties;
using Xnlab.SQLMon.UI;

//source: http://www.codeproject.com/KB/recipes/diffengine.aspx

namespace Xnlab.SQLMon.Diff
{
    /// <summary>
    /// Summary description for Results.
    /// </summary>
    public class DiffResults : Form
    {
        private SplitContainer _splitContainer1;
        private ComboBox _cboSources;
        private Label _lblSource;
        private ComboBox _cboDestinations;
        private Label _lblDestination;
        private readonly bool _isLoaded = false;
        private TextEditorControl _rtbSourceScript;
        private TextEditorControl _rtbDestinationScript;
        private Button _cmdCompare;
        private readonly int _previousVersion = 0;
        private readonly int _currentVersion = 0;
        private const string DefaultCompare = "-- PUT YOUR SCRIPT HERE TO COMPARE";
        private List<int> _differences = new List<int>();
        private int _currentIndex = 0;
        private Panel _pnlSearchCommands;
        private Button _cmdDifferenceNext;
        private Button _cmdDifferencePrevious;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly Container _components = null;

        public DiffResults()
        {
            InitializeComponent();

            this.Icon = Icon.FromHandle(Resources.History2.GetHicon());

            Utils.SetTextBoxStyle(_rtbSourceScript);
            Utils.SetTextBoxStyle(_rtbDestinationScript);

            //rtbSourceScript.CurrentLineColor = Color.Blue;
            //rtbDestinationScript.CurrentLineColor = Color.Blue;

        }

        public DiffResults(string sourceScript)
            : this()
        {
            _currentVersion = Utils.EmptyIndex;
            _previousVersion = Utils.EmptyIndex;
            _cboSources.Visible = false;
            _cboDestinations.Visible = false;
            _isLoaded = true;
            TextDiff(sourceScript, false, DefaultCompare, false);
        }

        public DiffResults(int previousVersion, int currentVersion)
            : this()
        {
            _previousVersion = previousVersion;

            if (_previousVersion == Utils.EmptyIndex)
                _cboSources.Visible = false;
            else
            {
                _cboSources.DataSource = Monitor.Instance.GetObjectVersions();
                _cboSources.SelectedValue = _previousVersion;
            }

            _currentVersion = currentVersion;
            if (_currentVersion == Utils.EmptyIndex)
                _cboDestinations.Visible = false;
            else
            {
                _cboDestinations.DataSource = Monitor.Instance.GetObjectVersions();
                _cboDestinations.SelectedValue = _currentVersion;
            }
            _isLoaded = true;
            ShowVersionDiff();
        }

        private void TextDiff(int previousVersion, int currentVersion)
        {
            var sourceScript = previousVersion != Utils.EmptyIndex ? GetVersionScript(previousVersion) : DefaultCompare;
            var destinationScript = currentVersion != Utils.EmptyIndex ? GetVersionScript(currentVersion) : string.Empty;

            TextDiff(sourceScript, false, destinationScript, false);
        }

        private string GetVersionScript(int version)
        {
            if (version == 0)
                return Monitor.Instance.GetObjectScriptTextEx();
            else
                return Monitor.Instance.GetObjectScriptVersionText(version);
        }

        private void ShowDiff(DiffListText source, DiffListText destination, List<DiffResultSpan> diffLines, double seconds)
        {
            _currentIndex = 0;
            _cmdDifferenceNext.Enabled = true;
            _cmdDifferencePrevious.Enabled = true;
            _differences = new List<int>();
            _rtbSourceScript.BeginUpdate();
            _rtbDestinationScript.BeginUpdate();
            var sourceLines = new List<KeyValuePair<string, Color>>();
            var destinationLines = new List<KeyValuePair<string, Color>>();
            int i;

            foreach (var drs in diffLines)
            {
                switch (drs.Status)
                {
                    case DiffResultSpanStatus.DeleteSource:
                        _differences.Add(sourceLines.Count);
                        for (i = 0; i < drs.Length; i++)
                        {
                            sourceLines.Add(new KeyValuePair<string, Color>(((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line, Color.Red));
                            destinationLines.Add(new KeyValuePair<string, Color>(new string(' ', ((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line.Length), Color.Blue));
                        }
                        break;
                    case DiffResultSpanStatus.NoChange:
                        for (i = 0; i < drs.Length; i++)
                        {
                            sourceLines.Add(new KeyValuePair<string, Color>(((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line, Color.Empty));
                            destinationLines.Add(new KeyValuePair<string, Color>(((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line, Color.Empty));
                        }
                        break;
                    case DiffResultSpanStatus.AddDestination:
                        _differences.Add(sourceLines.Count);
                        for (i = 0; i < drs.Length; i++)
                        {
                            sourceLines.Add(new KeyValuePair<string, Color>(new string(' ', ((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line.Length), Color.Blue));
                            destinationLines.Add(new KeyValuePair<string, Color>(((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line, Color.Blue));
                        }
                        break;
                    case DiffResultSpanStatus.Replace:
                        _differences.Add(sourceLines.Count);
                        for (i = 0; i < drs.Length; i++)
                        {
                            sourceLines.Add(new KeyValuePair<string, Color>(((TextLine)source.GetByIndex(drs.SourceIndex + i)).Line, Color.Red));
                            destinationLines.Add(new KeyValuePair<string, Color>(((TextLine)destination.GetByIndex(drs.DestIndex + i)).Line, Color.Green));
                        }
                        break;
                }
            }

            FillScript(sourceLines, _rtbSourceScript);
            FillScript(destinationLines, _rtbDestinationScript);

            _rtbSourceScript.EndUpdate();
            _rtbDestinationScript.EndUpdate();

            this.Text = _differences.Count + " differences.";
        }

        private void FillScript(List<KeyValuePair<string, Color>> lines, TextEditorControl textBox)
        {
            var script = new StringBuilder();
            foreach (var item in lines)
            {
                script.AppendLine(item.Key);
            }
            textBox.Text = script.ToString();
            for (var i = 0; i < lines.Count; i++)
            {
                if (lines[i].Value == Color.Blue
                    || lines[i].Value == Color.Green
                    || lines[i].Value == Color.Red)
                {
                    var marker = new TextMarker(textBox.Document.LineSegmentCollection[i].Offset, textBox.Document.LineSegmentCollection[i].Length, TextMarkerType.SolidBlock, Color.White, lines[i].Value);
                    textBox.Document.MarkerStrategy.AddMarker(marker);
                }
            }
        }

        public void TextDiff(string source, bool sourceIsFile, string destination, bool destinationIsFile)
        {
            using (new DisposableState(this, null))
            {
                DiffListText sLf = null;
                DiffListText dLf = null;
                try
                {
                    sLf = new DiffListText(source, sourceIsFile);
                    dLf = new DiffListText(destination, destinationIsFile);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "File Error");
                    return;
                }

                try
                {
                    double time = 0;
                    var de = new DiffEngine();
                    time = de.ProcessDiff(sLf, dLf, DiffEngineLevel.SlowPerfect);

                    var rep = de.DiffReport();
                    ShowDiff(sLf, dLf, rep, time);
                }
                catch (Exception ex)
                {
                    var tmp = string.Format("{0}{1}{1}***STACK***{1}{2}",
                        ex.Message,
                        Environment.NewLine,
                        ex.StackTrace);
                    MessageBox.Show(tmp, "Compare Error");
                }
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_components != null)
                {
                    _components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._rtbSourceScript = new ICSharpCode.TextEditor.TextEditorControl();
            this._cboSources = new System.Windows.Forms.ComboBox();
            this._lblSource = new System.Windows.Forms.Label();
            this._cmdCompare = new System.Windows.Forms.Button();
            this._rtbDestinationScript = new ICSharpCode.TextEditor.TextEditorControl();
            this._cboDestinations = new System.Windows.Forms.ComboBox();
            this._lblDestination = new System.Windows.Forms.Label();
            this._pnlSearchCommands = new System.Windows.Forms.Panel();
            this._cmdDifferenceNext = new System.Windows.Forms.Button();
            this._cmdDifferencePrevious = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer1)).BeginInit();
            this._splitContainer1.Panel1.SuspendLayout();
            this._splitContainer1.Panel2.SuspendLayout();
            this._splitContainer1.SuspendLayout();
            this._pnlSearchCommands.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this._splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer1.Location = new System.Drawing.Point(0, 0);
            this._splitContainer1.Name = "_splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this._splitContainer1.Panel1.Controls.Add(this._rtbSourceScript);
            this._splitContainer1.Panel1.Controls.Add(this._cboSources);
            this._splitContainer1.Panel1.Controls.Add(this._lblSource);
            // 
            // splitContainer1.Panel2
            // 
            this._splitContainer1.Panel2.Controls.Add(this._cmdCompare);
            this._splitContainer1.Panel2.Controls.Add(this._rtbDestinationScript);
            this._splitContainer1.Panel2.Controls.Add(this._cboDestinations);
            this._splitContainer1.Panel2.Controls.Add(this._lblDestination);
            this._splitContainer1.Size = new System.Drawing.Size(862, 531);
            this._splitContainer1.SplitterDistance = 425;
            this._splitContainer1.TabIndex = 3;
            // 
            // rtbSourceScript
            // 
            this._rtbSourceScript.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._rtbSourceScript.AutoScroll = true;
            this._rtbSourceScript.AutoScrollMinSize = new System.Drawing.Size(25, 15);
            this._rtbSourceScript.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._rtbSourceScript.IsReadOnly = false;
            this._rtbSourceScript.Location = new System.Drawing.Point(6, 34);
            this._rtbSourceScript.Name = "_rtbSourceScript";
            this._rtbSourceScript.Size = new System.Drawing.Size(409, 493);
            this._rtbSourceScript.TabIndex = 5;
            // 
            // cboSources
            // 
            this._cboSources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._cboSources.DisplayMember = "Key";
            this._cboSources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboSources.FormattingEnabled = true;
            this._cboSources.Location = new System.Drawing.Point(90, 9);
            this._cboSources.Name = "_cboSources";
            this._cboSources.Size = new System.Drawing.Size(325, 20);
            this._cboSources.TabIndex = 4;
            this._cboSources.ValueMember = "Value";
            this._cboSources.SelectedIndexChanged += new System.EventHandler(this.OnSourcesSelectedIndexChanged);
            // 
            // lblSource
            // 
            this._lblSource.AutoSize = true;
            this._lblSource.Location = new System.Drawing.Point(4, 11);
            this._lblSource.Name = "_lblSource";
            this._lblSource.Size = new System.Drawing.Size(47, 12);
            this._lblSource.TabIndex = 3;
            this._lblSource.Text = "&Source:";
            // 
            // cmdCompare
            // 
            this._cmdCompare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._cmdCompare.Location = new System.Drawing.Point(347, 9);
            this._cmdCompare.Name = "_cmdCompare";
            this._cmdCompare.Size = new System.Drawing.Size(78, 22);
            this._cmdCompare.TabIndex = 8;
            this._cmdCompare.Text = "&Compare";
            this._cmdCompare.UseVisualStyleBackColor = true;
            this._cmdCompare.Click += new System.EventHandler(this.OnCompareClick);
            // 
            // rtbDestinationScript
            // 
            this._rtbDestinationScript.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._rtbDestinationScript.AutoScroll = true;
            this._rtbDestinationScript.AutoScrollMinSize = new System.Drawing.Size(25, 15);
            this._rtbDestinationScript.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._rtbDestinationScript.IsReadOnly = false;
            this._rtbDestinationScript.Location = new System.Drawing.Point(6, 34);
            this._rtbDestinationScript.Name = "_rtbDestinationScript";
            this._rtbDestinationScript.Size = new System.Drawing.Size(424, 493);
            this._rtbDestinationScript.TabIndex = 7;
            // 
            // cboDestinations
            // 
            this._cboDestinations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._cboDestinations.DisplayMember = "Key";
            this._cboDestinations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cboDestinations.FormattingEnabled = true;
            this._cboDestinations.Location = new System.Drawing.Point(92, 9);
            this._cboDestinations.Name = "_cboDestinations";
            this._cboDestinations.Size = new System.Drawing.Size(247, 20);
            this._cboDestinations.TabIndex = 6;
            this._cboDestinations.ValueMember = "Value";
            this._cboDestinations.SelectedIndexChanged += new System.EventHandler(this.OnDestinationsSelectedIndexChanged);
            // 
            // lblDestination
            // 
            this._lblDestination.AutoSize = true;
            this._lblDestination.Location = new System.Drawing.Point(6, 11);
            this._lblDestination.Name = "_lblDestination";
            this._lblDestination.Size = new System.Drawing.Size(77, 12);
            this._lblDestination.TabIndex = 5;
            this._lblDestination.Text = "&Destination:";
            // 
            // pnlSearchCommands
            // 
            this._pnlSearchCommands.BackColor = System.Drawing.Color.WhiteSmoke;
            this._pnlSearchCommands.Controls.Add(this._cmdDifferenceNext);
            this._pnlSearchCommands.Controls.Add(this._cmdDifferencePrevious);
            this._pnlSearchCommands.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._pnlSearchCommands.Location = new System.Drawing.Point(0, 531);
            this._pnlSearchCommands.Name = "_pnlSearchCommands";
            this._pnlSearchCommands.Size = new System.Drawing.Size(862, 29);
            this._pnlSearchCommands.TabIndex = 5;
            // 
            // cmdDifferenceNext
            // 
            this._cmdDifferenceNext.Location = new System.Drawing.Point(102, 5);
            this._cmdDifferenceNext.Name = "_cmdDifferenceNext";
            this._cmdDifferenceNext.Size = new System.Drawing.Size(90, 21);
            this._cmdDifferenceNext.TabIndex = 4;
            this._cmdDifferenceNext.Text = "&Next";
            this._cmdDifferenceNext.UseVisualStyleBackColor = true;
            this._cmdDifferenceNext.Click += new System.EventHandler(this.OnDifferenceNextClick);
            // 
            // cmdDifferencePrevious
            // 
            this._cmdDifferencePrevious.Enabled = false;
            this._cmdDifferencePrevious.Location = new System.Drawing.Point(5, 4);
            this._cmdDifferencePrevious.Name = "_cmdDifferencePrevious";
            this._cmdDifferencePrevious.Size = new System.Drawing.Size(90, 21);
            this._cmdDifferencePrevious.TabIndex = 3;
            this._cmdDifferencePrevious.Text = "&Previous";
            this._cmdDifferencePrevious.UseVisualStyleBackColor = true;
            this._cmdDifferencePrevious.Click += new System.EventHandler(this.OnDifferencePreviousClick);
            // 
            // DiffResults
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(862, 560);
            this.Controls.Add(this._splitContainer1);
            this.Controls.Add(this._pnlSearchCommands);
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.Name = "DiffResults";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Diff Results";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnFormKeyDown);
            this._splitContainer1.Panel1.ResumeLayout(false);
            this._splitContainer1.Panel1.PerformLayout();
            this._splitContainer1.Panel2.ResumeLayout(false);
            this._splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer1)).EndInit();
            this._splitContainer1.ResumeLayout(false);
            this._pnlSearchCommands.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private void OnSourcesSelectedIndexChanged(object sender, EventArgs e)
        {
            ShowVersionDiff();
        }

        private void ShowVersionDiff()
        {
            if (_isLoaded)
                TextDiff(_previousVersion != Utils.EmptyIndex && _cboSources.SelectedValue != null ? Convert.ToInt32(_cboSources.SelectedValue) : Utils.EmptyIndex
                    , _currentVersion != Utils.EmptyIndex && _cboDestinations.SelectedValue != null ? Convert.ToInt32(_cboDestinations.SelectedValue) : Utils.EmptyIndex);
        }

        private void OnDestinationsSelectedIndexChanged(object sender, EventArgs e)
        {
            ShowVersionDiff();
        }

        //private void OnScriptVisibleRangeChanged(object sender, EventArgs e)
        //{
        //    var box = (sender as FastColoredTextBox);
        //    var vPos = box.VerticalScroll.Value;
        //    var curLine = box.Selection.Start.iLine;

        //    if (box == rtbSourceScript)
        //        UpdateScroll(rtbDestinationScript, vPos, curLine);
        //    else
        //        UpdateScroll(rtbSourceScript, vPos, curLine);
        //}

        //private void UpdateScroll(ICSharpCode.TextEditor.TextEditorControl tb, int vPos, int curLine)
        //{
        //    if (updating > 0)
        //        return;
        //    //
        //    BeginUpdate();
        //    //
        //    if (vPos <= tb.VerticalScroll.Maximum)
        //    {
        //        tb.VerticalScroll.Value = vPos;

        //        //some magic for scroll update (it's no my bug, it's Microsoft :)
        //        tb.AutoScrollMinSize -= new Size(1, 0);
        //        tb.AutoScrollMinSize += new Size(1, 0);
        //    }

        //    if (curLine < tb.LinesCount)
        //        tb.Selection = new Range(tb, 0, curLine, 0, curLine);
        //    //
        //    EndUpdate();
        //}

        //private void BeginUpdate()
        //{
        //    updating++;
        //}

        //private void EndUpdate()
        //{
        //    updating--;
        //}

        private void OnFormKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                ShowDiff();
        }

        private void ShowDiff()
        {
            if (!_cboSources.Visible || !_cboDestinations.Visible)
                TextDiff(_rtbSourceScript.Text, false, _rtbDestinationScript.Text, false);
            else
                ShowVersionDiff();
        }

        private void OnCompareClick(object sender, EventArgs e)
        {
            ShowDiff();
        }

        private void OnDifferenceNextClick(object sender, EventArgs e)
        {
            SetDifferencePosition(true);
        }

        private void SetDifferencePosition(bool isNext)
        {
            if (isNext)
                _currentIndex++;
            else
                _currentIndex--;
            if (_currentIndex >= _differences.Count)
                _currentIndex = _differences.Count - 1;
            if (_currentIndex < 0)
                _currentIndex = 0;
            _cmdDifferenceNext.Enabled = _currentIndex < _differences.Count - 1 && _differences.Count > 0;
            _cmdDifferencePrevious.Enabled = _currentIndex > 0 && _differences.Count > 0;
            if (_currentIndex < _differences.Count)
            {
                _rtbSourceScript.ActiveTextAreaControl.TextArea.Caret.Position = new TextLocation(0, _differences[_currentIndex]);
                _rtbSourceScript.ActiveTextAreaControl.TextArea.ScrollToCaret();
            }
        }

        private void OnDifferencePreviousClick(object sender, EventArgs e)
        {
            SetDifferencePosition(false);
        }
    }
}
