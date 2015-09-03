using System;
using System.Linq;
using System.Windows.Forms;
using Xnlab.SQLMon.Common;

namespace Xnlab.SQLMon.UI
{
    internal class DisposableState : IDisposable
    {
        private readonly Control _win;
        private readonly ToolStripItem[] _elements;

        internal DisposableState(Control win, ToolStripItem[] elements)
        {
            _win = win;
            _elements = elements;
            SetState(false);
        }

        private void SetState(bool state)
        {
            if (_win.IsHandleCreated)
                _win.Invoke(() =>
                    {
                        _win.Cursor = state ? Cursors.Arrow : Cursors.WaitCursor;
                        if (_elements != null)
                            _elements.ToList().ForEach((o) => { o.Enabled = state; o.Invalidate(); });
                    }
                 );
        }

        public void Dispose()
        {
            SetState(true);
        }
    }
}
