using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace Xnlab.SQLMon.Logic
{
    #region Serialization
    //http://codebetter.com/blogs/gregyoung/archive/2008/08/24/fast-serialization.aspx
    internal class CustomBinaryFormatter : IFormatter
    {
        private SerializationBinder _mBinder;
        private StreamingContext _mStreamingContext;
        private ISurrogateSelector _mSurrogateSelector;
        private readonly MemoryStream _mWriteStream;
        private MemoryStream _mIndexWriteStream;
        private readonly MemoryStream _mReadStream;
        private readonly BinaryWriter _mWriter;
        private readonly BinaryReader _mReader;
        private readonly Dictionary<Type, int> _mByType = new Dictionary<Type, int>();
        private readonly Dictionary<int, Type> _mById = new Dictionary<int, Type>();
        private const int SizeLength = 8;
        private readonly byte[] _mLengthBuffer = new byte[SizeLength];
        private readonly byte[] _mCopyBuffer;
        private readonly Stream _indexStream = null;
        private readonly Stream _serializationStream = null;
        private long _count = 0;
        private long _pending = 0;
        private const int ChunckSize = 5000;

        public CustomBinaryFormatter(Stream indexStream)
            : this(indexStream, null)
        {
        }

        ~CustomBinaryFormatter()
        {
            Close();
        }

        public Stream DataStream
        {
            get { return _serializationStream; }
        }

        public Stream IndexStream
        {
            get { return _indexStream; }
        }

        public CustomBinaryFormatter(Stream indexStream, Stream serializationStream)
        {
            _mCopyBuffer = new byte[SizeLength * 1000000];
            _mWriteStream = new MemoryStream(100000);
            _mReadStream = new MemoryStream(100000);
            _mWriter = new BinaryWriter(_mWriteStream);
            _mReader = new BinaryReader(_mReadStream);
            _mIndexWriteStream = new MemoryStream(ChunckSize * SizeLength);
            this._indexStream = indexStream;
            this._serializationStream = serializationStream;
            if (indexStream != null)
            {
                bool indexReady;
                if (indexStream.Length >= SizeLength)
                    if (indexStream.Read(_mLengthBuffer, 0, SizeLength) == SizeLength)
                    {
                        _count = BitConverter.ToInt64(_mLengthBuffer, 0);
                        indexReady = true;
                    }
                    else
                        indexReady = false;
                else
                    indexReady = false;
                if (!indexReady)
                {
                    indexStream.Position = 0;
                    indexStream.Write(BitConverter.GetBytes(0L), 0, SizeLength);
                }
                indexStream.Seek(indexStream.Length, SeekOrigin.Begin);
            }
        }

        public void Flush()
        {
            if (_indexStream != null)
            {
                if (_pending % ChunckSize != 0)
                {
                    var buffer = _mIndexWriteStream.ToArray();
                    _indexStream.Write(buffer, 0, buffer.Length);
                    _mIndexWriteStream = new MemoryStream(ChunckSize * SizeLength);
                    _pending = 0;
                }
                _indexStream.Position = 0;
                _indexStream.Write(BitConverter.GetBytes(_count), 0, SizeLength);
            }
        }

        public void Close()
        {
            if (_indexStream != null)
                _indexStream.Close();
            if (_serializationStream != null)
                _serializationStream.Close();
        }

        public void Register<T>(int typeId) where T : ICustomBinarySerializable
        {
            _mById.Add(typeId, typeof(T));
            _mByType.Add(typeof(T), typeId);
        }

        public void MoveTo(long index)
        {
            MoveTo(index, true);
        }

        public void MoveTo(long index, bool relocate)
        {
            if (_indexStream != null && _serializationStream != null)
            {
                if (index >= 0 && index * SizeLength <= (_indexStream.Length - SizeLength))
                {
                    var pos = _indexStream.Position;
                    _indexStream.Position = SizeLength + index * SizeLength;
                    if (_indexStream.Read(_mLengthBuffer, 0, SizeLength) == SizeLength)
                    {
                        _serializationStream.Seek(BitConverter.ToInt64(_mLengthBuffer, 0), SeekOrigin.Begin);
                    }
                    if (relocate)
                        _indexStream.Position = pos;
                }
            }
        }

        public void MoveToEnd()
        {
            _indexStream.Seek(0, SeekOrigin.End);
            _serializationStream.Seek(0, SeekOrigin.End);
        }

        public long Count
        {
            get { return _count; }
        }

        public object Deserialize(Stream serializationStream)
        {
            return null;
        }

        public T Deserialize<T>(bool full)
        {
            if (_serializationStream.Read(_mLengthBuffer, 0, SizeLength) != SizeLength)
                //throw new SerializationException("Could not read length from the stream.");
                return default(T);
            var length = BitConverter.ToInt32(_mLengthBuffer, 0);
            //TODO make this support partial reads from stream
            if (_serializationStream.Read(_mCopyBuffer, 0, length) != length)
                throw new SerializationException("Could not read " + length + " bytes from the stream.");
            _mReadStream.Seek(0L, SeekOrigin.Begin);
            _mReadStream.Write(_mCopyBuffer, 0, length);
            _mReadStream.Seek(0L, SeekOrigin.Begin);
            var typeid = _mReader.ReadInt32();
            Type t;
            if (!_mById.TryGetValue(typeid, out t))
                throw new SerializationException("TypeId " + typeid + " is not a registerred type id");
            var obj = FormatterServices.GetUninitializedObject(t);
            var deserialize = (ICustomBinarySerializable)obj;
            deserialize.SetDataFrom(_mReader, full);
            if (_mReadStream.Position != length)
                throw new SerializationException("object of type " + t + " did not read its entire buffer during deserialization. This is most likely an inbalance between the writes and the reads of the object.");
            return (T)deserialize;
        }

        public void Serialize(Stream serializationStream, object graph)
        {

        }

        public void Serialize<T>(T graph)
        {
            Serialize<T>(graph, false);
        }

        public void Serialize<T>(T graph, bool isUpdate)
        {
            int key;
            if (!_mByType.TryGetValue(graph.GetType(), out key))
                throw new SerializationException(graph.GetType() + " has not been registered with the serializer");
            var c = (ICustomBinarySerializable)graph; //this will always work due to generic constraint on the Register
            _mWriteStream.Seek(0L, SeekOrigin.Begin);
            _mWriter.Write((int)key);
            c.WriteDataTo(_mWriter);
            if (_indexStream != null && !isUpdate)
            {
                _count++;
                _pending++;
                if (_pending % ChunckSize == 0)
                {
                    var buffer = _mIndexWriteStream.ToArray();
                    _indexStream.Write(buffer, 0, buffer.Length);
                    _mIndexWriteStream = new MemoryStream(ChunckSize * SizeLength);
                    _pending = 0;
                }
                _mIndexWriteStream.Write(BitConverter.GetBytes(_serializationStream.Position), 0, SizeLength);
            }
            _serializationStream.Write(BitConverter.GetBytes(_mWriteStream.Position), 0, SizeLength);
            _serializationStream.Write(_mWriteStream.GetBuffer(), 0, (int)_mWriteStream.Position);
        }

        public ISurrogateSelector SurrogateSelector
        {
            get { return _mSurrogateSelector; }
            set { _mSurrogateSelector = value; }
        }

        public SerializationBinder Binder
        {
            get { return _mBinder; }
            set { _mBinder = value; }
        }

        public StreamingContext Context
        {
            get { return _mStreamingContext; }
            set { _mStreamingContext = value; }
        }
    }

    public interface ICustomBinarySerializable
    {
        void WriteDataTo(BinaryWriter writer);
        void SetDataFrom(BinaryReader reader, bool full);
    }
    #endregion
}
