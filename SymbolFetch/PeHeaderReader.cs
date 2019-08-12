using System;
using System.Runtime.InteropServices;
using System.IO;

namespace SymbolFetch
{

    public class PeHeaderReader
    {
        #region File Header Structures

        public struct IMAGE_DOS_HEADER
        {      
            public ushort e_magic;              // Magic number
            public ushort e_cblp;               // Bytes on last page of file
            public ushort e_cp;                 // Pages in file
            public ushort e_crlc;               // Relocations
            public ushort e_cparhdr;            // Size of header in paragraphs
            public ushort e_minalloc;           // Minimum extra paragraphs needed
            public ushort e_maxalloc;           // Maximum extra paragraphs needed
            public ushort e_ss;                 // Initial (relative) SS value
            public ushort e_sp;                 // Initial SP value
            public ushort e_csum;               // Checksum
            public ushort e_ip;                 // Initial IP value
            public ushort e_cs;                 // Initial (relative) CS value
            public ushort e_lfarlc;             // File address of relocation table
            public ushort e_ovno;               // Overlay number
            public ushort e_res_0;              // Reserved words
            public ushort e_res_1;              // Reserved words
            public ushort e_res_2;              // Reserved words
            public ushort e_res_3;              // Reserved words
            public ushort e_oemid;              // OEM identifier (for e_oeminfo)
            public ushort e_oeminfo;            // OEM information; e_oemid specific
            public ushort e_res2_0;             // Reserved words
            public ushort e_res2_1;             // Reserved words
            public ushort e_res2_2;             // Reserved words
            public ushort e_res2_3;             // Reserved words
            public ushort e_res2_4;             // Reserved words
            public ushort e_res2_5;             // Reserved words
            public ushort e_res2_6;             // Reserved words
            public ushort e_res2_7;             // Reserved words
            public ushort e_res2_8;             // Reserved words
            public ushort e_res2_9;             // Reserved words
            public uint e_lfanew;             // File address of new exe header
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct IMAGE_DATA_DIRECTORY
        {
            public uint VirtualAddress;
            public uint Size;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_OPTIONAL_HEADER32
        {
            public ushort Magic;
            public byte MajorLinkerVersion;
            public byte MinorLinkerVersion;
            public uint SizeOfCode;
            public uint SizeOfInitializedData;
            public uint SizeOfUninitializedData;
            public uint AddressOfEntryPoint;
            public uint BaseOfCode;
            public uint BaseOfData;
            public uint ImageBase;
            public uint SectionAlignment;
            public uint FileAlignment;
            public ushort MajorOperatingSystemVersion;
            public ushort MinorOperatingSystemVersion;
            public ushort MajorImageVersion;
            public ushort MinorImageVersion;
            public ushort MajorSubsystemVersion;
            public ushort MinorSubsystemVersion;
            public uint Win32VersionValue;
            public uint SizeOfImage;
            public uint SizeOfHeaders;
            public uint CheckSum;
            public ushort Subsystem;
            public ushort DllCharacteristics;
            public uint SizeOfStackReserve;
            public uint SizeOfStackCommit;
            public uint SizeOfHeapReserve;
            public uint SizeOfHeapCommit;
            public uint LoaderFlags;
            public uint NumberOfRvaAndSizes;

            public IMAGE_DATA_DIRECTORY ExportTable;
            public IMAGE_DATA_DIRECTORY ImportTable;
            public IMAGE_DATA_DIRECTORY ResourceTable;
            public IMAGE_DATA_DIRECTORY ExceptionTable;
            public IMAGE_DATA_DIRECTORY CertificateTable;
            public IMAGE_DATA_DIRECTORY BaseRelocationTable;
            public IMAGE_DATA_DIRECTORY Debug;
            public IMAGE_DATA_DIRECTORY Architecture;
            public IMAGE_DATA_DIRECTORY GlobalPtr;
            public IMAGE_DATA_DIRECTORY TLSTable;
            public IMAGE_DATA_DIRECTORY LoadConfigTable;
            public IMAGE_DATA_DIRECTORY BoundImport;
            public IMAGE_DATA_DIRECTORY IAT;
            public IMAGE_DATA_DIRECTORY DelayImportDescriptor;
            public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;
            public IMAGE_DATA_DIRECTORY Reserved;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_OPTIONAL_HEADER64
        {
            public ushort Magic;
            public byte MajorLinkerVersion;
            public byte MinorLinkerVersion;
            public uint SizeOfCode;
            public uint SizeOfInitializedData;
            public uint SizeOfUninitializedData;
            public uint AddressOfEntryPoint;
            public uint BaseOfCode;
            public ulong ImageBase;
            public uint SectionAlignment;
            public uint FileAlignment;
            public ushort MajorOperatingSystemVersion;
            public ushort MinorOperatingSystemVersion;
            public ushort MajorImageVersion;
            public ushort MinorImageVersion;
            public ushort MajorSubsystemVersion;
            public ushort MinorSubsystemVersion;
            public uint Win32VersionValue;
            public uint SizeOfImage;
            public uint SizeOfHeaders;
            public uint CheckSum;
            public ushort Subsystem;
            public ushort DllCharacteristics;
            public ulong SizeOfStackReserve;
            public ulong SizeOfStackCommit;
            public ulong SizeOfHeapReserve;
            public ulong SizeOfHeapCommit;
            public uint LoaderFlags;
            public uint NumberOfRvaAndSizes;

            public IMAGE_DATA_DIRECTORY ExportTable;
            public IMAGE_DATA_DIRECTORY ImportTable;
            public IMAGE_DATA_DIRECTORY ResourceTable;
            public IMAGE_DATA_DIRECTORY ExceptionTable;
            public IMAGE_DATA_DIRECTORY CertificateTable;
            public IMAGE_DATA_DIRECTORY BaseRelocationTable;
            public IMAGE_DATA_DIRECTORY Debug;
            public IMAGE_DATA_DIRECTORY Architecture;
            public IMAGE_DATA_DIRECTORY GlobalPtr;
            public IMAGE_DATA_DIRECTORY TLSTable;
            public IMAGE_DATA_DIRECTORY LoadConfigTable;
            public IMAGE_DATA_DIRECTORY BoundImport;
            public IMAGE_DATA_DIRECTORY IAT;
            public IMAGE_DATA_DIRECTORY DelayImportDescriptor;
            public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;
            public IMAGE_DATA_DIRECTORY Reserved;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_FILE_HEADER
        {
            public ushort Machine;
            public ushort NumberOfSections;
            public uint TimeDateStamp;
            public uint PointerToSymbolTable;
            public uint NumberOfSymbols;
            public ushort SizeOfOptionalHeader;
            public ushort Characteristics;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct IMAGE_SECTION_HEADER
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] Name;
            [FieldOffset(8)]
            public uint VirtualSize;
            [FieldOffset(12)]
            public uint VirtualAddress;
            [FieldOffset(16)]
            public uint SizeOfRawData;
            [FieldOffset(20)]
            public uint PointerToRawData;
            [FieldOffset(24)]
            public uint PointerToRelocations;
            [FieldOffset(28)]
            public uint PointerToLinenumbers;
            [FieldOffset(32)]
            public ushort NumberOfRelocations;
            [FieldOffset(34)]
            public ushort NumberOfLinenumbers;
            [FieldOffset(36)]
            public DataSectionFlags Characteristics;

            public string Section
            {
                get { return new string(Name); }
            }
        }

        [Flags]
        public enum DataSectionFlags : uint
        {
            
            TypeReg = 0x00000000,
            TypeDsect = 0x00000001,
            TypeNoLoad = 0x00000002,
            TypeGroup = 0x00000004,
            TypeNoPadded = 0x00000008,
            TypeCopy = 0x00000010,
            ContentCode = 0x00000020,
            ContentInitializedData = 0x00000040,
            ContentUninitializedData = 0x00000080,
            LinkOther = 0x00000100,
            LinkInfo = 0x00000200,
            TypeOver = 0x00000400,
            LinkRemove = 0x00000800,
            LinkComDat = 0x00001000,
            NoDeferSpecExceptions = 0x00004000,
            RelativeGP = 0x00008000,
            MemPurgeable = 0x00020000,
            Memory16Bit = 0x00020000,
            MemoryLocked = 0x00040000,
            MemoryPreload = 0x00080000,
            Align1Bytes = 0x00100000,
            Align2Bytes = 0x00200000,
            Align4Bytes = 0x00300000,
            Align8Bytes = 0x00400000,
            Align16Bytes = 0x00500000,
            Align32Bytes = 0x00600000,
            Align64Bytes = 0x00700000,
            Align128Bytes = 0x00800000,
            Align256Bytes = 0x00900000,
            Align512Bytes = 0x00A00000,
            Align1024Bytes = 0x00B00000,
            Align2048Bytes = 0x00C00000,
            Align4096Bytes = 0x00D00000,
            Align8192Bytes = 0x00E00000,
            LinkExtendedRelocationOverflow = 0x01000000,
            MemoryDiscardable = 0x02000000,
            MemoryNotCached = 0x04000000,
            MemoryNotPaged = 0x08000000,
            MemoryShared = 0x10000000,
            MemoryExecute = 0x20000000,
            MemoryRead = 0x40000000,
            MemoryWrite = 0x80000000
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_DEBUG_DIRECTORY
        {
            public uint Characteristics;
            public uint TimeDateStamp;
            public ushort MajorVersion;
            public ushort MinorVersion;
            public uint Type;
            public uint SizeOfData;
            public uint AddressOfRawData;
            public uint PointerToRawData;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IMAGE_DEBUG_DIRECTORY_RAW
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] format;
            public Guid guid;
            public uint age;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
            public char[] name;
        }

        #endregion File Header Structures

        #region Private Fields
        
        private IMAGE_DOS_HEADER dosHeader;
        private IMAGE_FILE_HEADER fileHeader;
        private IMAGE_OPTIONAL_HEADER32 optionalHeader32;
        private IMAGE_OPTIONAL_HEADER64 optionalHeader64;
        private IMAGE_SECTION_HEADER[] imageSectionHeaders;

        private IMAGE_DEBUG_DIRECTORY imageDebugDirectory;
        private IMAGE_DEBUG_DIRECTORY_RAW DebugInfo;

        private string _pdbName = "";
        private string _pdbage = "";
        private Guid _debugGUID;

        #endregion Private Fields

        #region Public Methods

        public PeHeaderReader(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                BinaryReader reader = new BinaryReader(stream);
                dosHeader = FromBinaryReader<IMAGE_DOS_HEADER>(reader);

                // Add 4 bytes to the offset
                stream.Seek(dosHeader.e_lfanew, SeekOrigin.Begin);

                uint ntHeadersSignature = reader.ReadUInt32();
                fileHeader = FromBinaryReader<IMAGE_FILE_HEADER>(reader);
                if (this.Is32BitHeader)
                {
                    optionalHeader32 = FromBinaryReader<IMAGE_OPTIONAL_HEADER32>(reader);
                }
                else
                {
                    optionalHeader64 = FromBinaryReader<IMAGE_OPTIONAL_HEADER64>(reader);
                }

                uint offDebug = 0;
                uint cbDebug = 0;
                long cbFromHeader = 0;
                int loopexit = 0;

                if (this.Is32BitHeader)
                    cbDebug = optionalHeader32.Debug.Size;
                else
                    cbDebug = optionalHeader64.Debug.Size;

                imageSectionHeaders = new IMAGE_SECTION_HEADER[fileHeader.NumberOfSections];
                for (int headerNo = 0; headerNo < imageSectionHeaders.Length; ++headerNo)
                {
                    imageSectionHeaders[headerNo] = FromBinaryReader<IMAGE_SECTION_HEADER>(reader);

                    if ((imageSectionHeaders[headerNo].PointerToRawData != 0) &&
                            (imageSectionHeaders[headerNo].SizeOfRawData != 0) &&
                                (cbFromHeader < (long)
                                    (imageSectionHeaders[headerNo].PointerToRawData + imageSectionHeaders[headerNo].SizeOfRawData)))
                    {
                        cbFromHeader = (long)
                            (imageSectionHeaders[headerNo].PointerToRawData + imageSectionHeaders[headerNo].SizeOfRawData);
                    }

                    if (cbDebug != 0)
                    {
                        if (this.Is32BitHeader)
                        {
                            if (imageSectionHeaders[headerNo].VirtualAddress <= optionalHeader32.Debug.VirtualAddress &&
                                    ((imageSectionHeaders[headerNo].VirtualAddress + imageSectionHeaders[headerNo].SizeOfRawData) > optionalHeader32.Debug.VirtualAddress))
                            {
                                offDebug = optionalHeader32.Debug.VirtualAddress - imageSectionHeaders[headerNo].VirtualAddress + imageSectionHeaders[headerNo].PointerToRawData;
                            }
                        }
                        else
                        {
                            if (imageSectionHeaders[headerNo].VirtualAddress <= optionalHeader64.Debug.VirtualAddress &&
                                ((imageSectionHeaders[headerNo].VirtualAddress + imageSectionHeaders[headerNo].SizeOfRawData) > optionalHeader64.Debug.VirtualAddress))
                            {
                                offDebug = optionalHeader64.Debug.VirtualAddress - imageSectionHeaders[headerNo].VirtualAddress + imageSectionHeaders[headerNo].PointerToRawData;
                            }
                        }
                    }
                }

                stream.Seek(offDebug, SeekOrigin.Begin);

                while (cbDebug >= Marshal.SizeOf(typeof(IMAGE_DEBUG_DIRECTORY)))
                {
                    if (loopexit == 0)
                    {
                        imageDebugDirectory = FromBinaryReader<IMAGE_DEBUG_DIRECTORY>(reader);
                        long seekPosition = stream.Position;

                        if (imageDebugDirectory.Type == 0x2)
                        {
                            stream.Seek(imageDebugDirectory.PointerToRawData, SeekOrigin.Begin);
                            DebugInfo = FromBinaryReader<IMAGE_DEBUG_DIRECTORY_RAW>(reader);
                            loopexit = 1;

                            //Downloading logic for .NET native images
                            if (new string(DebugInfo.name).Contains(".ni."))
                            {
                                stream.Seek(seekPosition, SeekOrigin.Begin);
                                loopexit = 0;
                            }
                        }

                        if ((imageDebugDirectory.PointerToRawData != 0) &&
                                (imageDebugDirectory.SizeOfData != 0) &&
                                (cbFromHeader < (long)
                                    (imageDebugDirectory.PointerToRawData + imageDebugDirectory.SizeOfData)))
                        {
                            cbFromHeader = (long)
                                (imageDebugDirectory.PointerToRawData + imageDebugDirectory.SizeOfData);
                        }
                    }

                    cbDebug -= (uint)Marshal.SizeOf(typeof(IMAGE_DEBUG_DIRECTORY));
                }

                if (loopexit != 0)
                {
                    _pdbName = new string(DebugInfo.name);
                    _pdbName = _pdbName.Remove(_pdbName.IndexOf("\0"));

                    _pdbage = DebugInfo.age.ToString("X");
                    _debugGUID = DebugInfo.guid;
                }

            }
        }
        
        public static T FromBinaryReader<T>(BinaryReader reader)
        {
            byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(T)));

            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();

            return theStructure;
        }

        #endregion Public Methods

        #region Properties

        public string pdbName { get { return _pdbName; } }
        public string pdbage { get { return _pdbage; } }
        public Guid debugGUID { get { return _debugGUID; } }
        
        public bool Is32BitHeader
        {
            get
            {
                //UInt16 IMAGE_FILE_32BIT_MACHINE = 0x0100;
                //return (IMAGE_FILE_32BIT_MACHINE & FileHeader.Characteristics) == IMAGE_FILE_32BIT_MACHINE;
                return (FileHeader.Machine == 332) ? true : false; //14C = X86
            }
        }
        
        public IMAGE_FILE_HEADER FileHeader
        {
            get
            {
                return fileHeader;
            }
        }
        
        public IMAGE_OPTIONAL_HEADER32 OptionalHeader32
        {
            get
            {
                return optionalHeader32;
            }
        }

        public IMAGE_OPTIONAL_HEADER64 OptionalHeader64
        {
            get
            {
                return optionalHeader64;
            }
        }

        public IMAGE_SECTION_HEADER[] ImageSectionHeaders
        {
            get
            {
                return imageSectionHeaders;
            }
        }

        #endregion Properties
    }
}
