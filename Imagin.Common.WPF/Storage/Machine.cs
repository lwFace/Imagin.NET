using Imagin.Common.Analytics;
using Imagin.Common.Linq;
using Imagin.Common.Native;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using Shell32;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Storage
{
    public static class Machine
    {
        public static IEnumerable<DriveInfo> Drives
        {
            get
            {
                foreach (var i in DriveInfo.GetDrives())
                {
                    if (i.IsReady)
                    {
                        yield return i;
                    }
                }
            }
        }

        public static IEnumerable<string> RemovableDrives
        {
            get
            {
                var drives = DriveInfo.GetDrives();
                if (drives?.Length > 0)
                {
                    foreach (var i in drives)
                    {
                        if (i.DriveType == DriveType.Removable)
                            yield return i.Name;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------

        #region Classes

        #region Desktop

        public static class Desktop
        {
            public enum StretchMode
            {
                Centered,
                Stretched,
                Tiled
            }

            const int SPI_SETDESKWALLPAPER = 20;
            const int SPIF_UPDATEINIFILE = 0x01;
            const int SPIF_SENDWININICHANGE = 0x02;

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

            /// <summary>
            /// Path to current desktop wallpaper.
            /// </summary>
            public static string Background
            {
                get
                {
                    var result = string.Empty;
                    var key = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", false);
                    if (key != null)
                    {
                        result = key.GetValue("WallPaper").ToString();
                        key.Close();
                    }
                    return result;
                }
            }

            public static StretchMode BackgroundStretchMode
            {
                get
                {
                    var Key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

                    var a = Key.GetValue(@"WallpaperStyle").ToString();
                    var b = Key.GetValue(@"TileWallpaper").ToString();

                    if (a == "1" && b == "0")
                        return StretchMode.Centered;

                    if (a == "2" && b == "0")
                        return StretchMode.Stretched;

                    if (a == "1" && b == "1")
                        return StretchMode.Tiled;

                    return StretchMode.Centered;
                }
            }

            static Desktop() { }

            public static bool SetBackground(string path, StretchMode stretchMode = StretchMode.Centered)
            {
                try
                {
                    var key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
                    switch (stretchMode)
                    {
                        case StretchMode.Centered:
                            key.SetValue(@"WallpaperStyle", "1");
                            key.SetValue(@"TileWallpaper", "0");
                            break;
                        case StretchMode.Stretched:
                            key.SetValue(@"WallpaperStyle", "2");
                            key.SetValue(@"TileWallpaper", "0");
                            break;
                        case StretchMode.Tiled:
                            key.SetValue(@"WallpaperStyle", "1");
                            key.SetValue(@"TileWallpaper", "1");
                            break;
                    }
                    SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }

        #endregion

        #region Icon

        public static class Icon
        {
            #region Constants

            const int WM_CLOSE = 0x0010;

            const int SHGFI_ICON = 0x100;

            const int SHGFI_SMALLICON = 0x1;

            const int SHGFI_LARGEICON = 0x0;

            const int SHIL_JUMBO = 0x4;

            const int SHIL_EXTRALARGE = 0x2;

            #endregion

            #region Structs

            struct Pair
            {
                public System.Drawing.Icon Icon
                {
                    get; set;
                }

                public IntPtr HandleToDestroy
                {
                    set; get;
                }
            }

            #endregion

            #region Methods

            #region Imports

            [DllImport("user32")]
            static extern IntPtr SendMessage(IntPtr handle, int Msg, IntPtr wParam, IntPtr lParam);

            /// <summary>
            /// SHGetImageList is not exported correctly in XP.  See KB316931
            /// http://support.microsoft.com/default.aspx?scid=kb;EN-US;Q316931
            /// Apparently (and hopefully) ordinal 727 isn't going to change.
            /// </summary> 
            [DllImport("shell32.dll", EntryPoint = "#727")]
            extern static int SHGetImageList(int iImageList, ref Guid riid, out IImageList ppv);

            /// <summary>
            /// The signature of SHGetFileInfo (located in Shell32.dll)
            /// </summary>
            [DllImport("Shell32.dll")]
            static extern int SHGetFileInfo(string pszPath, int dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, uint uFlags);

            [DllImport("Shell32.dll")]
            static extern int SHGetFileInfo(IntPtr pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, int cbFileInfo, uint uFlags);

            [DllImport("shell32.dll", SetLastError = true)]
            static extern int SHGetSpecialFolderLocation(IntPtr hwndOwner, Int32 nFolder, ref IntPtr ppidl);

            [DllImport("user32")]
            static extern int DestroyIcon(IntPtr hIcon);

            #endregion

            #region Internal

            /*
            static ImageSource SystemIcon(bool Small, ShellApi.CSIDL csidl)
            {
                IntPtr pidlTrash = IntPtr.Zero;
                int hr = SHGetSpecialFolderLocation(IntPtr.Zero, (int)csidl, ref pidlTrash);
                System.Diagnostics.Debug.Assert(hr == 0);

                SHFILEINFO shinfo = new SHFILEINFO();

                uint SHGFI_USEFILEATTRIBUTES = 0x000000010;

                // Get a handle to the large icon
                uint flags;
                uint SHGFI_PIDL = 0x000000008;
                if (!Small)
                {
                    flags = SHGFI_PIDL | SHGFI_ICON | SHGFI_LARGEICON | SHGFI_USEFILEATTRIBUTES;
                }
                else
                {
                    flags = SHGFI_PIDL | SHGFI_ICON | SHGFI_SMALLICON | SHGFI_USEFILEATTRIBUTES;
                }

                var res = SHGetFileInfo(pidlTrash, 0, ref shinfo, Marshal.SizeOf(shinfo), flags);
                System.Diagnostics.Debug.Assert(res != 0);

                var myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon);
                Marshal.FreeCoTaskMem(pidlTrash);
                var bs = myIcon.ToImageSource();
                myIcon.Dispose();
                bs.Freeze(); // importantissimo se no fa memory leak
                DestroyIcon(shinfo.hIcon);
                SendMessage(shinfo.hIcon, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                return bs;

            }
            */

            static ImageSource Extract(string FileName, bool Small, bool checkDisk, bool addOverlay)
            {
                SHFILEINFO shinfo = new SHFILEINFO();

                uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
                uint SHGFI_LINKOVERLAY = 0x000008000;

                uint flags;
                if (Small)
                {
                    flags = SHGFI_ICON | SHGFI_SMALLICON;
                }
                else
                {
                    flags = SHGFI_ICON | SHGFI_LARGEICON;
                }
                if (!checkDisk)
                    flags |= SHGFI_USEFILEATTRIBUTES;
                if (addOverlay)
                    flags |= SHGFI_LINKOVERLAY;

                var res = SHGetFileInfo(FileName, 0, ref shinfo, Marshal.SizeOf(shinfo), flags);
                if (res == 0)
                {
                    throw (new System.IO.FileNotFoundException());
                }

                var myIcon = System.Drawing.Icon.FromHandle(shinfo.hIcon);

                var bs = myIcon.ImageSource();
                myIcon.Dispose();
                bs.Freeze(); // importantissimo se no fa memory leak
                DestroyIcon(shinfo.hIcon);
                SendMessage(shinfo.hIcon, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                return bs;
            }

            static ImageSource ExtractLarge(string FileName, bool jumbo, bool checkDisk)
            {
                try
                {
                    SHFILEINFO shinfo = new SHFILEINFO();

                    uint SHGFI_USEFILEATTRIBUTES = 0x000000010;
                    uint SHGFI_SYSICONINDEX = 0x4000;

                    int FILE_ATTRIBUTE_NORMAL = 0x80;

                    uint flags;
                    flags = SHGFI_SYSICONINDEX;

                    if (!checkDisk)  // This does not seem to work. If I try it, a folder icon is always returned.
                        flags |= SHGFI_USEFILEATTRIBUTES;

                    var res = SHGetFileInfo(FileName, FILE_ATTRIBUTE_NORMAL, ref shinfo, Marshal.SizeOf(shinfo), flags);
                    if (res == 0)
                    {
                        throw (new System.IO.FileNotFoundException());
                    }
                    var iconIndex = shinfo.iIcon;

                    // Get the System IImageList object from the Shell:
                    Guid iidImageList = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");

                    int size = jumbo ? SHIL_JUMBO : SHIL_EXTRALARGE;
                    var hres = SHGetImageList(size, ref iidImageList, out IImageList iml);
                    // writes iml
                    //if (hres == 0)
                    //{
                    //    throw (new System.Exception("Error SHGetImageList"));
                    //}

                    IntPtr hIcon = IntPtr.Zero;
                    int ILD_TRANSPARENT = 1;
                    hres = iml.GetIcon(iconIndex, ILD_TRANSPARENT, ref hIcon);
                    //if (hres == 0)
                    //{
                    //    throw (new System.Exception("Error iml.GetIcon"));
                    //}

                    var myIcon = System.Drawing.Icon.FromHandle(hIcon);
                    var bs = myIcon.ImageSource();
                    myIcon.Dispose();
                    bs.Freeze(); // very important to avoid memory leak
                    DestroyIcon(hIcon);
                    SendMessage(hIcon, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                    return bs;
                }
                catch
                {
                    return null;
                }
            }

            #endregion

            #region Static

            public static ImageSource GetLarge(string Path)
            {
                return ExtractLarge(Path, true, true);
            }

            /*
            public static ImageSource GetSystem(bool Small, ShellApi.CSIDL Kind)
            {
                return SystemIcon(Small, Kind);
            }
            */

            #endregion

            #endregion
        }

        #endregion

        #region Properties

        public static class Properties
        {
            #region Methods

            [DllImport("shell32.dll", SetLastError = true)]
            static extern int SHMultiFileProperties(IDataObject pdtobj, int flags);

            [DllImport("shell32.dll", CharSet = CharSet.Auto)]
            static extern IntPtr ILCreateFromPath(string path);

            [DllImport("shell32.dll", CharSet = CharSet.None)]
            static extern void ILFree(IntPtr pidl);

            [DllImport("shell32.dll", CharSet = CharSet.None)]
            static extern int ILGetSize(IntPtr pidl);

            static MemoryStream GetShellIds(StringCollection paths)
            {
                //Convert list of paths into a list of PIDLs.
                var pos = 0;
                var pidls = new byte[paths.Count][];
                foreach (var filename in paths)
                {
                    //Get PIDL based on name
                    var pidl = ILCreateFromPath(filename);
                    var pidlSize = ILGetSize(pidl);
                    //Copy over to our managed array
                    pidls[pos] = new byte[pidlSize];
                    Marshal.Copy(pidl, pidls[pos++], 0, pidlSize);
                    ILFree(pidl);
                }

                //Determine where in CIDL we will start pumping PIDLs
                var pidlOffset = 4 * (paths.Count + 2);
                //Start the CIDL stream
                var result = new MemoryStream();

                var writer = new BinaryWriter(result);
                writer.Write(paths.Count); //Initialize CIDL witha count of files
                writer.Write(pidlOffset); //Calcualte and write relative offsets of every pidl starting with root

                pidlOffset += 4; //Root is 4 bytes
                foreach (var pidl in pidls)
                {
                    writer.Write(pidlOffset);
                    pidlOffset += pidl.Length;
                }

                //Write the root PIDL (0) followed by all PIDLs
                writer.Write(0);
                foreach (var pidl in pidls)
                    writer.Write(pidl);

                //Stream now contains the CIDL
                return result;
            }

            public static int Show(string Path)
            {
                return Show(new string[] { Path });
            }

            public static int Show(IEnumerable<string> paths)
            {
                var files = new StringCollection();
                foreach (var i in paths)
                    files.Add(i);

                var Data = new DataObject();
                Data.SetFileDropList(files);
                Data.SetData("Preferred DropEffect", new MemoryStream(Array<byte>.New(5, 0, 0, 0)), true);
                Data.SetData("Shell IDList Array", GetShellIds(files), true);

                return SHMultiFileProperties(Data, 0);
            }

            #endregion
        }

        #endregion

        #region RecycleBin

        public static class RecycleBin
        {
            static Shell Shell;

            [DllImport("shell32.dll", CharSet = CharSet.Auto)]
            static extern int SHFileOperation(ref SHFILEOPSTRUCT FileOp);

            static bool DoVerb(FolderItem Item, string Verb)
            {
                foreach (FolderItemVerb FIVerb in Item.Verbs())
                {
                    if (FIVerb.Name.ToUpper().Contains(Verb.ToUpper()))
                    {
                        FIVerb.DoIt();
                        return true;
                    }
                }
                return false;
            }

            static bool Recycle(string path, FileOperationFlags flags)
            {
                try
                {
                    var fs = new SHFILEOPSTRUCT
                    {
                        wFunc = FileOperationType.FO_DELETE,
                        pFrom = path + '\0' + '\0',
                        fFlags = FileOperationFlags.FOF_ALLOWUNDO | flags
                    };
                    SHFileOperation(ref fs);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public static IEnumerable<string> GetContents()
            {
                Shell = new Shell();
                Shell32.Folder RecycleBin = Shell.NameSpace(10);
                foreach (FolderItem2 Entry in RecycleBin.Items())
                    yield return Entry.Path;
                Marshal.FinalReleaseComObject(Shell);
            }

            /// <summary>
            /// Restore item at given path.
            /// </summary>
            /// <param name="Path"></param>
            /// <returns></returns>
            public static bool Restore(string path)
            {
                Shell = new Shell();
                var Recycler = Shell.NameSpace(10);
                for (int i = 0; i < Recycler.Items().Count; i++)
                {
                    var FolderItem = Recycler.Items().Item(i);
                    string FileName = Recycler.GetDetailsOf(FolderItem, 0);

                    if (Path.GetExtension(FileName) == "")
                        FileName += Path.GetExtension(FolderItem.Path);

                    //Necessary for systems with hidden file extensions.
                    string FilePath = Recycler.GetDetailsOf(FolderItem, 1);
                    if (path == Path.Combine(FilePath, FileName))
                    {
                        DoVerb(FolderItem, "ESTORE");
                        return true;
                    }
                }
                return false;
            }
        }

        #endregion

        #region User

        public class User
        {
            [DllImport("shell32.dll", EntryPoint = "#261", CharSet = CharSet.Unicode, PreserveSig = false)]
            public static extern void GetUserTilePath(string username, UInt32 whatever, /*0x80000000*/ System.Text.StringBuilder picpath, int maxLength);

            public static string GetPicture(string username)
            {
                var sb = new System.Text.StringBuilder(1000);
                GetUserTilePath(username, 0x80000000, sb, sb.Capacity);
                return sb.ToString();
            }
        }

        #endregion

        #endregion

        #region Properties

        #endregion

        #region Methods

        public static string Description(string path)
        {
            var result = path.EndsWith(@":\") ? "Drive" : (Folder.Long.Exists(path) ? "Folder" : (!path.NullOrEmpty() ? File.Long.Description(path) : path));
            return result.NullOrEmpty() ? System.IO.Path.GetFileNameWithoutExtension(path) : result;
        }

        public static string FriendlyName(string path)
        {
            if (path == null)
                return string.Empty;

            if (path == Folder.Long.Root)
                return Folder.Long.RootName;

            if (path == Folder.Long.RecycleBin)
                return Folder.Long.RecycleBinName;

            if (path.EndsWith(@":\"))
            {
                var volumeLabel = Drives.FirstOrDefault(i => i.Name == path)?.VolumeLabel;
                return volumeLabel != null ? $"{volumeLabel} ({path.TrimEnd('\\')})" : path;
            }

            var result = Path.GetFileName(path);
            return result.NullOrEmpty() ? path : result;
        }

        public static ItemType GetType(string path)
        {
            if (path.EndsWith(@":") || path.EndsWith(@":\"))
            {
                return ItemType.Drive;
            }
            else if (path == Folder.Long.RecycleBin || Folder.Long.Exists(path))
            {
                return ItemType.Folder;
            }
            else if (File.Long.Exists(path))
            {
                return ItemType.File;
            }
            else return ItemType.File;
        }

        public static bool Hidden(string itemPath)
        {
            foreach (var i in Drives)
            {
                if (i.Name == itemPath)
                    return false;
            }
            if (File.Long.Exists(itemPath))
            {
                var file = new File(itemPath);
                file.Refresh();
                return file.IsHidden;
            }
            else if (Folder.Long.Exists(itemPath))
            {
                var folder = new Folder(itemPath);
                folder.Refresh();
                return folder.IsHidden;
            }
            return false;
        }

        //-----------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Copies the given items to the given target path.
        /// </summary>
        /// <param name="items">The items to copy.</param>
        /// <param name="targetPath">The target path (does not include the name of the copied item).</param>
        public static void Copy(IEnumerable<Item> items, string targetPath)
        {
            foreach (var i in items)
            {
                var destination = i.Path.Replace(Path.GetDirectoryName(i.Path), targetPath);
                if (i.Path == destination)
                {
                    destination = $@"{Path.GetDirectoryName(destination)}\{Path.GetFileNameWithoutExtension(destination)} (Copy){Path.GetExtension(destination)}";
                }

                try
                {
                    switch (i.Type)
                    {
                        case ItemType.Drive:
                            continue;

                        case ItemType.File:
                        case ItemType.Shortcut:
                            Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(i.Path, destination, UIOption.AllDialogs);
                            continue;

                        case ItemType.Folder:
                            Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(i.Path, destination, UIOption.AllDialogs);
                            continue;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public static void Copy(string i, string targetPath)
        {
            var destination = i.Replace(Path.GetDirectoryName(i), targetPath);
            if (i == destination)
            {
                if (File.Long.Exists(i))
                    destination = File.Long.ClonePath(i);

                if (Folder.Long.Exists(i))
                    destination = Folder.Long.ClonePath(i);
            }

            if (File.Long.Exists(i))
                Microsoft.VisualBasic.FileIO.FileSystem.CopyFile(i, destination, UIOption.AllDialogs);

            if (Folder.Long.Exists(i))
                Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(i, destination, UIOption.AllDialogs);
        }

        /// <summary>
        /// Moves the given items to the given target path.
        /// </summary>
        /// <param name="items">The items to move.</param>
        /// <param name="targetFolderPath">The target path (does not include the name of the moved item).</param>
        public static Result Move(Item i, string targetFolderPath, string targetFileName)
        {
            var destination 
                = targetFileName == null
                ? i.Path.Replace(Path.GetDirectoryName(i.Path), targetFolderPath)
                : $@"{targetFolderPath}\{targetFileName}";

            if (i.Path == destination)
                return new Error(new InvalidOperationException());

            try
            {
                switch (i.Type)
                {
                    case ItemType.Drive:
                        return new Error(new InvalidOperationException());

                    case ItemType.File:
                    case ItemType.Shortcut:
                        Microsoft.VisualBasic.FileIO.FileSystem.MoveFile(i.Path, destination, UIOption.AllDialogs);
                        break;

                    case ItemType.Folder:
                        Microsoft.VisualBasic.FileIO.FileSystem.MoveDirectory(i.Path, destination, UIOption.AllDialogs);
                        break;
                }
                return new Success();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new Error(e);
            }
        }

        public static void Move(IEnumerable<Item> items, string targetFolderPath)
        {
            foreach (var i in items)
            {
                Move(i, targetFolderPath, null);
            }
        }

        public static void Recycle(IEnumerable<string> paths, RecycleOption recycleOption = RecycleOption.SendToRecycleBin)
        {
            foreach (var i in paths)
            {
                Recycle(i, recycleOption);
            }
        }

        public static void Recycle(string path, RecycleOption recycleOption = RecycleOption.SendToRecycleBin)
        {
            try
            {
                if (System.IO.Directory.Exists(path))
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(path, UIOption.AllDialogs, recycleOption);
                }

                else if (System.IO.File.Exists(path))
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(path, UIOption.AllDialogs, recycleOption);
                }
            }
            catch { }
        }

        //-----------------------------------------------------------------------------------------------------------------------------

        public static Result OpenInWindowsExplorer(string path)
            => File.Long.Open("explorer.exe", path);

        public static Result ShowInWindowsExplorer(string path)
            => File.Long.Open("explorer.exe", @"/select, {0}".F(path));

        #endregion
    }
}