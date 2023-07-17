using System.IO;

namespace system
{
    class io
    {

        FileStream Stream = null;
        string fname = "";
        string mode = "";

        public void init(string file, string mode_)
        {
            fname = file;
            mode = mode_;
            openFile();
        }

        public string readAllText()
        {
            if (Stream == null)
                openFile();
            byte[] array = new byte[Stream.Length];
            Stream.Read(array, 0, array.Length);
            return System.Text.Encoding.UTF8.GetString(array);
        }

        public void openFile()
        {
            if (mode.Equals("r"))
                Stream = File.OpenRead(fname);
            else if (mode.Equals("w"))
                Stream = File.OpenWrite(fname);
            else if (mode.Equals("rw"))
                Stream = File.Open(fname, FileMode.OpenOrCreate);
        }

        public byte[] read(int count)
        {
            if (Stream == null)
                openFile();
            byte[] buffer = new byte[count];
            Stream.Read(buffer, 0, count);
            return buffer;
        }

        public void close()
        {
            if (Stream != null)
                Stream.Close();
        }

        public long getLength()
        {
            if (Stream == null)
                openFile();
            return Stream.Length;
        }

        public void writeText(string line, bool flush) {
            if (Stream == null)
                openFile();
            byte[] array = System.Text.Encoding.UTF8.GetBytes(line);
            Stream.Write(array);
            if (flush)
                Stream.Flush();
        }

        public int readByte() {
            return Stream.ReadByte();
        }

        public void write(byte[] array, bool flush) {
            if (Stream == null)
                openFile();
            Stream.Write(array);
            if (flush)
                Stream.Flush();
        }

        public void flush() {
            Stream.Flush();
        }

        public void writeByte(byte byt, bool flush) {
            if (Stream == null)
                openFile();
            Stream.WriteByte(byt);
            if (flush)
                Stream.Flush();
        }

        public void seek(int offset, string origin) {
            if (Stream == null)
                openFile();
            if (origin.Equals("c"))
                Stream.Seek(offset, SeekOrigin.Current);
            else if (origin.Equals("b"))
                Stream.Seek(offset, SeekOrigin.Begin);
            else if (origin.Equals("e"))
                Stream.Seek(offset, SeekOrigin.End);
        }
    }
}
