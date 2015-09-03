using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Windows.Forms;
using Xnlab.SQLMon.Common;
using Xnlab.SQLMon.Logic;

namespace Xnlab.SQLMon.UI
{
    public partial class UserTableData : UserControl, ICancelable
    {
        private readonly string _table;
        private readonly ServerInfo _server;
        private bool _isRunning = false;
        private Thread _thread = null;
        private bool _hasPrimaryKey = false;
        private string _primaryKey = null;

        public UserTableData()
        {
            InitializeComponent();
        }

        public UserTableData(ServerInfo server, string table)
            : this()
        {
            rtbSQL.Font = Monitor.Instance.SetFont();
            _server = server;
            _table = table;
            var sql = "SELECT TOP 100 * FROM " + _table;
            Utils.SetTextBoxStyle(rtbSQL);
            rtbSQL.Text = sql;
            Execute();
        }

        ~UserTableData()
        {
            Cancel();
        }

        public void Cancel()
        {
            try
            {
                if (_isRunning && _thread != null)
                    _thread.Abort();
                _isRunning = false;
            }
            catch (Exception)
            {
            }
        }

        public bool IsRunning
        {
            get { return _isRunning; }
        }

        private void SetCommand(bool cancel)
        {
            _isRunning = cancel;
            Monitor.Instance.SetExecute(cancel);
        }

        public string Key
        {
            get { return _server.Server + "." + _server.Database + "." + _table; }
        }

        public void Execute()
        {
            if (!_isRunning)
            {
                using (new DisposableState(this, Monitor.Instance.Commands))
                {
                    _thread = new Thread(StartQuery);
                    _thread.Start(rtbSQL.Text);
                }
            }
            else
            {
                if (_thread != null)
                    _thread.Abort();
                _isRunning = false;
                SetCommand(false);
            }
        }

        private void StartQuery(object state)
        {
            try
            {
                SetCommand(true);
                var data = SqlHelper.Query((string)state, Monitor.Instance.CurrentServerInfo);
                if (data != null)
                    data.TableName = _table;

                string schemaName;
                var tableName = QueryEngine.ParseObjectName(_table, out schemaName);
                var sql = string.Format(@"SELECT  COL_NAME(ic.OBJECT_ID,ic.column_id)
FROM    sys.indexes AS i INNER JOIN 
        sys.index_columns AS ic ON  i.OBJECT_ID = ic.OBJECT_ID
                                AND i.index_id = ic.index_id
WHERE OBJECT_NAME(ic.OBJECT_ID) = '{0}' AND i.is_primary_key = 1", tableName);
                var result = SqlHelper.ExecuteScalar(sql, Monitor.Instance.CurrentServerInfo);
                _primaryKey = result != DBNull.Value ? Convert.ToString(result) : string.Empty;
                _hasPrimaryKey = !string.IsNullOrEmpty(_primaryKey);
                this.Invoke(() =>
                    {
                        dgvData.DataSource = data;
                        dgvData.ReadOnly = !_hasPrimaryKey;
                    });
            }
            catch (Exception ex)
            {
                ShowMessage(ex is ThreadAbortException ? "Query cancelled." : ex.Message);
            }
            finally
            {
                SetCommand(false);
            }
        }

        private void OnDataGridRowLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (_hasPrimaryKey)
            {
                var data = dgvData.DataSource as DataTable;
                if (data != null)
                    UpdateQueryData(data);
            }
        }

        private void StartUpdate(DataTable UserData)
        {
            try
            {
                SetCommand(true);
                //dgvData.AllowUserToAddRows = false;
                var userData = (DataTable)UserData;
                userData = userData.GetChanges();
                if (userData != null)
                {
                    using (var connection = SqlHelper.CreateNewConnection(Monitor.Instance.CurrentServerInfo))
                    {
                        connection.Open();
                        using (var transaction = connection.BeginTransaction())
                        {
                            string schemaName;
                            var tableName = QueryEngine.ParseObjectName(userData.TableName, out schemaName);
                            tableName = QueryEngine.GetObjectName(schemaName, tableName);
                            using (var command = new SqlCommand("SELECT TOP 1 * FROM " + tableName, connection))
                            {
                                command.Transaction = transaction;
                                var adapter = new SqlDataAdapter(command);
                                var builder = new SqlCommandBuilder(adapter);
                                adapter.InsertCommand = builder.GetInsertCommand();
                                adapter.DeleteCommand = builder.GetDeleteCommand();
                                //for (int i = adapter.DeleteCommand.Parameters.Count - 1; i >= 0; i--)
                                //{
                                //    if (adapter.DeleteCommand.Parameters[i].SourceColumn != primaryKey)
                                //        adapter.DeleteCommand.Parameters.RemoveAt(i);
                                //}
                                adapter.UpdateCommand = builder.GetUpdateCommand();
                                //for (int i = adapter.UpdateCommand.Parameters.Count - 1; i >= 0; i--)
                                //{
                                //    if (adapter.UpdateCommand.Parameters[i].SourceColumn != primaryKey)
                                //        adapter.UpdateCommand.Parameters.RemoveAt(i);
                                //}
                                adapter.Update(userData);
                                UserData.AcceptChanges();
                            }
                            transaction.Commit();
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
            finally
            {
                SetCommand(false);
                //dgvData.AllowUserToAddRows = true;
            }
        }

        private void ShowMessage(string message)
        {
            this.Invoke(() =>
                {
                    Monitor.Instance.ShowMessage(message);
                });
        }

        private void UpdateQueryData(DataTable userData)
        {
            using (new DisposableState(this, Monitor.Instance.Commands))
            {
                StartUpdate(userData);
            }
        }

    }
}
