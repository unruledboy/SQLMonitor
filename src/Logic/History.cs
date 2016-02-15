using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Xnlab.SQLMon.Logic
{
    public class PerformanceRecord
    {
        public long Value1 { get; set; }
        public long Value2 { get; set; }
        public long Value3 { get; set; }
        public long Value4 { get; set; }
        public long Value5 { get; set; }
        public long Value6 { get; set; }
        public long Value7 { get; set; }
        public long Value8 { get; set; }
        public long Value9 { get; set; }
        public long Value10 { get; set; }
        public long Value11 { get; set; }
        public long Value12 { get; set; }
        public long Value13 { get; set; }
        public long Value14 { get; set; }
        public long Value15 { get; set; }
        public DateTime Value16 { get; set; }
    }

    internal class HistoryRecord : PerformanceRecord, ICustomBinarySerializable
    {
        public string Date { get; set; }
        public string Key { get; set; }

        internal HistoryRecord(PerformanceRecord record)
        {
            //damn, it's crazy! I wanted to use reflection...
            Value1 = record.Value1;
            Value2 = record.Value2;
            Value3 = record.Value3;
            Value4 = record.Value4;
            Value5 = record.Value5;
            Value6 = record.Value6;
            Value7 = record.Value7;
            Value8 = record.Value8;
            Value9 = record.Value9;
            Value10 = record.Value10;
            Value11 = record.Value11;
            Value12 = record.Value12;
            Value13 = record.Value13;
            Value14 = record.Value14;
        }

        public void WriteDataTo(BinaryWriter writer)
        {
            writer.Write(Date);
            writer.Write(Key);
            writer.Write(Value1);
            writer.Write(Value2);
            writer.Write(Value3);
            writer.Write(Value4);
            writer.Write(Value5);
            writer.Write(Value6);
            writer.Write(Value7);
            writer.Write(Value8);
            writer.Write(Value9);
            writer.Write(Value10);
            writer.Write(Value11);
            writer.Write(Value12);
            writer.Write(Value13);
            writer.Write(Value14);
        }

        public void SetDataFrom(BinaryReader reader, bool full)
        {
            Date = reader.ReadString();
            Key = reader.ReadString();
            Value1 = reader.ReadInt64();
            Value2 = reader.ReadInt64();
            Value3 = reader.ReadInt64();
            Value4 = reader.ReadInt64();
            Value5 = reader.ReadInt64();
            Value6 = reader.ReadInt64();
            Value7 = reader.ReadInt64();
            Value8 = reader.ReadInt64();
            Value9 = reader.ReadInt64();
            Value10 = reader.ReadInt64();
            Value11 = reader.ReadInt64();
            Value12 = reader.ReadInt64();
            Value13 = reader.ReadInt64();
            Value14 = reader.ReadInt64();
        }
    }

    internal class HistoryDate : ICustomBinarySerializable
    {
        public string Date { get; set; }
        public long Index { get; set; }

        public void WriteDataTo(BinaryWriter writer)
        {
            writer.Write(Date);
            writer.Write(Index);
        }

        public void SetDataFrom(BinaryReader reader, bool full)
        {
            Date = reader.ReadString();
            Index = reader.ReadInt64();
        }
    }

    internal enum DateTypes
    {
        Hour = 0,
        Day = 1,
        Week = 2,
        Month = 3,
        Year = 4
    }

    internal class History
    {
        internal const string Separator = "|";
        private const string IndexExtension = ".idx";
        private const string DataExtension = ".dat";
        private const string HistoryFile = "HistoryData";
        private const string DatesFile = "HistoryDates";
        private static readonly object SyncRoot = new object();

        internal static string GetKey(ServerInfo server, bool isServer)
        {
            return (server.Server + (isServer ? string.Empty : "." + server.Database)).ToLower();
        }

        private static string GetFile(bool isHistory, bool isIndex)
        {
            return (isHistory ? HistoryFile : DatesFile) + (isIndex ? IndexExtension : DataExtension);
        }

        internal static void AddRecords(List<HistoryRecord> records)
        {
            lock (SyncRoot)
            {
                using (var dataIndexStream = new FileStream(GetFile(true, true), FileMode.OpenOrCreate))
                {
                    using (var dataContentStream = new FileStream(GetFile(true, false), FileMode.OpenOrCreate))
                    {
                        var historyFormatter = new CustomBinaryFormatter(dataIndexStream, dataContentStream);
                        historyFormatter.Register<HistoryRecord>(1);

                        historyFormatter.MoveToEnd();
                        var historyCount = historyFormatter.Count;
                        records.ForEach(r =>
                        {
                            historyFormatter.Serialize(r);
                        });

                        historyFormatter.Flush();

                        using (var dateIndexStream = new FileStream(GetFile(false, true), FileMode.OpenOrCreate))
                        {
                            using (var dateContentStream = new FileStream(GetFile(false, false), FileMode.OpenOrCreate))
                            {
                                var dateFormatter = new CustomBinaryFormatter(dateIndexStream, dateContentStream);
                                dateFormatter.Register<HistoryDate>(1);

                                var dateCount = dateFormatter.Count;
                                var today = DateTime.Now.Date.AddDays(1);
                                var yesterday = DateTime.Now.Date;
                                long todayIndex = -1;
                                var foundYesterday = false;
                                for (long i = 0; i < dateCount; i++)
                                {
                                    var date = dateFormatter.Deserialize<HistoryDate>(false);
                                    if (DateTime.Parse(date.Date) == today)
                                        todayIndex = i;
                                    if (DateTime.Parse(date.Date) == yesterday)
                                        foundYesterday = true;
                                }
                                if (!foundYesterday)
                                {
                                    dateFormatter.MoveToEnd();
                                    dateFormatter.Serialize(new HistoryDate { Date = yesterday.ToString(), Index = historyCount });
                                }

                                if (todayIndex != -1)
                                {
                                    dateFormatter.MoveTo(todayIndex);
                                    dateFormatter.Serialize(new HistoryDate { Date = today.ToString(), Index = historyFormatter.Count }, true);
                                }
                                else
                                {
                                    dateFormatter.MoveToEnd();
                                    dateFormatter.Serialize(new HistoryDate { Date = today.ToString(), Index = historyFormatter.Count });
                                }

                                dateFormatter.Flush();
                            }
                        }
                    }
                }
            }
        }

        internal static List<HistoryRecord> GetRecords(ServerInfo server, bool isServer, DateTypes dateType, DateTime startDate)
        {
            var records = new List<HistoryRecord>();
            var key = GetKey(server, isServer);
            using (var dateIndexStream = new FileStream(GetFile(false, true), FileMode.OpenOrCreate))
            {
                using (var dateContentStream = new FileStream(GetFile(false, false), FileMode.OpenOrCreate))
                {
                    var dateFormatter = new CustomBinaryFormatter(dateIndexStream, dateContentStream);
                    dateFormatter.Register<HistoryDate>(1);

                    var endDate = DateTime.Now.Date;
                    var samplingSpan = 1;
                    switch (dateType)
                    {
                        case DateTypes.Hour:
                            endDate = startDate.AddDays(1);
                            samplingSpan = 1;
                            break;
                        case DateTypes.Day:
                            endDate = startDate.AddDays(1);
                            samplingSpan = 24;
                            break;
                        case DateTypes.Week:
                            endDate = startDate.AddDays(7);
                            samplingSpan = 7 * 24;
                            break;
                        case DateTypes.Month:
                            endDate = startDate.AddMonths(1);
                            samplingSpan = 31 * 24;
                            break;
                        case DateTypes.Year:
                            endDate = startDate.AddYears(1);
                            samplingSpan = 365 * 24;
                            break;
                        default:
                            break;
                    }
                    var count = dateFormatter.Count;
                    Debug.WriteLine("all date count:" + count);
                    Debug.WriteLine("start date:" + startDate);
                    var dates = new List<HistoryDate>();
                    for (long i = 0; i < count; i++)
                    {
                        var date = dateFormatter.Deserialize<HistoryDate>(false);
                        var dateTime = DateTime.Parse(date.Date);
                        Debug.WriteLine("current date:" + dateTime);
                        if (startDate.Date <= dateTime && dateTime <= endDate)
                            dates.Add(date);
                    }
                    Debug.WriteLine("valid date count:" + dates.Count);
                    if (dates.Count > 0)
                    {
                        var start = dates.Aggregate((d1, d2) => DateTime.Parse(d1.Date) < DateTime.Parse(d2.Date) ? d1 : d2);
                        var end = dates.Aggregate((d1, d2) => DateTime.Parse(d1.Date) > DateTime.Parse(d2.Date) ? d1 : d2);

                        using (var dataIndexStream = new FileStream(GetFile(true, true), FileMode.OpenOrCreate))
                        {
                            using (var dataContentStream = new FileStream(GetFile(true, false), FileMode.OpenOrCreate))
                            {
                                var historyFormatter = new CustomBinaryFormatter(dataIndexStream, dataContentStream);
                                historyFormatter.Register<HistoryRecord>(1);

                                for (var i = start.Index; i < end.Index; i += samplingSpan)
                                {
                                    historyFormatter.MoveTo(i);
                                    var record = historyFormatter.Deserialize<HistoryRecord>(false);
                                    if (record.Key == key)
                                        records.Add(record);
                                }
                                historyFormatter.Close();
                                dataContentStream.Close();
                            }
                            dataIndexStream.Close();
                        }
                    }
                    dateFormatter.Close();
                    dateContentStream.Close();
                }
                dateIndexStream.Close();
            }
            return records;
        }
    }
}