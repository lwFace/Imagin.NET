namespace Paint
{
    public struct FileFormat
    {
        string extension;
        public string Extension
        {
            get
            {
                return extension;
            }
        }

        bool readable;
        public bool Readable
        {
            get
            {
                return readable;
            }
        }

        bool writable;
        public bool Writable
        {
            get
            {
                return writable;
            }
        }

        public FileFormat(string Extension, bool Readable, bool Writable)
        {
            extension = Extension;
            readable = Readable;
            writable = Writable;
        }
    }
}