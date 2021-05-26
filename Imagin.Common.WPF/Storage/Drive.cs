using Imagin.Common.Data;
using System.IO;

namespace Imagin.Common.Storage
{
    public sealed class Drive : Container
    {
        long availableFreeSpace;
        [DisplayName("Available free space")]
        [LongFormat(LongFormat.FileSize)]
        [ReadOnly]
        public long AvailableFreeSpace
        {
            get => availableFreeSpace;
            set => this.Change(ref availableFreeSpace, value);
        }

        string format;
        [Hidden]
        public string Format
        {
            get => format;
            set => this.Change(ref format, value);
        }

        long totalSize;
        [DisplayName("Total size")]
        [LongFormat(LongFormat.FileSize)]
        [ReadOnly]
        public long TotalSize
        {
            get => totalSize;
            set => this.Change(ref totalSize, value);
        }

        public Drive(DriveInfo driveInfo) : base(ItemType.Drive, Origin.Local, driveInfo.Name)
        {
            AvailableFreeSpace = driveInfo.AvailableFreeSpace;
            Format = driveInfo.DriveFormat;
            TotalSize = driveInfo.TotalSize;
        }

        public override FileSystemInfo Read() => null;
    }
}