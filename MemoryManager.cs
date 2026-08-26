using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace LiveSplit.CatQuest3
{
    public class MemoryManager
    {
        // ============================================================
        // WINDOWS MEMORY FUNCTIONS
        // ============================================================

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            int dwSize,
            out IntPtr lpNumberOfBytesRead
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr VirtualQueryEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            out MEMORY_BASIC_INFORMATION lpBuffer,
            IntPtr dwLength
        );

        [StructLayout(LayoutKind.Sequential)]
        private struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public IntPtr RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }

        private const uint MEM_COMMIT = 0x1000;

        private const uint PAGE_NOACCESS = 0x01;
        private const uint PAGE_GUARD = 0x100;

        private const uint PAGE_EXECUTE = 0x10;
        private const uint PAGE_EXECUTE_READ = 0x20;
        private const uint PAGE_EXECUTE_READWRITE = 0x40;
        private const uint PAGE_EXECUTE_WRITECOPY = 0x80;

        private const int SIGNATURE_CHUNK_SIZE =
            1024 * 1024;

        // ============================================================
        // GAME PROCESS
        // ============================================================

        public Process Game { get; private set; }

        public bool IsAttached
        {
            get
            {
                return
                    Game != null &&
                    !Game.HasExited;
            }
        }

        public bool TryAttach()
        {
            if (IsAttached)
            {
                return true;
            }

            Process[] processes =
                Process.GetProcessesByName(
                    "Cat Quest III"
                );

            if (processes.Length == 0)
            {
                Game = null;
                return false;
            }

            Game = processes[0];

            return true;
        }

        // ============================================================
        // READ MEMORY
        // ============================================================

        public byte[] ReadBytes(
            IntPtr address,
            int count)
        {
            if (!IsAttached)
            {
                return null;
            }

            byte[] buffer =
                new byte[count];

            bool success =
                ReadProcessMemory(
                    Game.Handle,
                    address,
                    buffer,
                    count,
                    out IntPtr bytesRead
                );

            if (
                !success ||
                bytesRead.ToInt64() != count
            )
            {
                return null;
            }

            return buffer;
        }

        public uint ReadUInt32(
            IntPtr address)
        {
            byte[] bytes =
                ReadBytes(
                    address,
                    4
                );

            if (bytes == null)
            {
                return 0;
            }

            return BitConverter.ToUInt32(
                bytes,
                0
            );
        }

        public uint ReadPointer(
            uint address)
        {
            if (address == 0)
            {
                return 0;
            }

            return ReadUInt32(
                new IntPtr(address)
            );
        }

        public string ReadMonoString(
            uint address)
        {
            if (address == 0)
            {
                return null;
            }

            int length =
                (int)ReadUInt32(
                    new IntPtr(
                        address + 0x08
                    )
                );

            if (
                length <= 0 ||
                length > 1000
            )
            {
                return null;
            }

            byte[] bytes =
                ReadBytes(
                    new IntPtr(
                        address + 0x0C
                    ),
                    length * 2
                );

            if (bytes == null)
            {
                return null;
            }

            return
                System.Text.Encoding.Unicode
                    .GetString(bytes);
        }

        // ============================================================
        // GAME DATA
        // ============================================================

        public bool HasShipKey(
            uint obtainedKeys)
        {
            if (obtainedKeys == 0)
            {
                return false;
            }

            // HashSet._slots
            uint slots =
                ReadPointer(
                    obtainedKeys + 0x0C
                );

            // HashSet._lastIndex
            int lastIndex =
                (int)ReadUInt32(
                    new IntPtr(
                        obtainedKeys + 0x1C
                    )
                );

            if (
                slots == 0 ||
                lastIndex <= 0 ||
                lastIndex > 10000
            )
            {
                return false;
            }

            const string shipKeyGuid =
                "5d16ca25d9411a744b61d54265287cad";

            for (
                int i = 0;
                i < lastIndex;
                i++
            )
            {
                // Mono array data begins at +0x10.
                //
                // HashSet Slot size = 0x0C.
                // Slot.value        = +0x08.
                uint keyData =
                    ReadPointer(
                        slots
                        + 0x10u
                        + (uint)(i * 0x0C)
                        + 0x08u
                    );

                if (keyData == 0)
                {
                    continue;
                }

                // KeyData +0x0C -> GUID string.
                uint guidPointer =
                    ReadPointer(
                        keyData + 0x0C
                    );

                string guid =
                    ReadMonoString(
                        guidPointer
                    );

                if (guid == shipKeyGuid)
                {
                    return true;
                }
            }

            return false;
        }

        // ============================================================
        // CONTEXTS
        // ============================================================

        public uint FindContextsStaticStorage()
        {
            Trace.WriteLine(
                "CONTEXT SCAN: starting"
            );

            // This is the final-build JIT shape of:
            //
            // Contexts:get_sharedInstance
            //
            // Several Mono-generated singleton getters look similar,
            // so we cannot simply accept the first match.
            //
            // Instead, every candidate is validated by following its
            // static field and checking that the resulting object has
            // plausible FrameworkContext and GUIContext objects.

            const string pattern =
                "55 8B EC 83 EC 28 " +
                "8B 05 ?? ?? ?? ?? " +
                "85 C0 " +
                "0F 85 37 00 00 00 " +
                "C7 04 24 ?? ?? ?? ?? " +
                "E8 ?? ?? ?? ?? " +
                "89 45 F8 " +
                "89 04 24 " +
                "90 " +
                "E8 ?? ?? ?? ?? " +
                "8B 4D F8 " +
                "B8 ?? ?? ?? ?? " +
                "89 08";

            Trace.WriteLine(
                "CONTEXT SCAN: pattern ready"
            );

            IntPtr searchStart =
                IntPtr.Zero;

            while (true)
            {
                IntPtr getter =
                    FindExecutableSignatureChunked(
                        pattern,
                        searchStart
                    );

                Trace.WriteLine(
                    "CONTEXT SCAN: candidate getter = 0x" +
                    getter.ToInt64().ToString("X")
                );

                if (getter == IntPtr.Zero)
                {
                    Trace.WriteLine(
                        "CONTEXT SCAN: no more candidates"
                    );

                    return 0;
                }

                // At getter +0x06:
                //
                // 8B 05 XX XX XX XX
                //
                // The absolute static-field address begins
                // at getter +0x08.
                uint staticStorage =
                    ReadUInt32(
                        IntPtr.Add(
                            getter,
                            0x08
                        )
                    );

                Trace.WriteLine(
                    "CONTEXT SCAN: candidate static storage = 0x" +
                    staticStorage.ToString("X")
                );

                if (
                    IsValidContextsStaticStorage(
                        staticStorage
                    )
                )
                {
                    Trace.WriteLine(
                        "CONTEXT SCAN: VALID Contexts getter"
                    );

                    return staticStorage;
                }

                Trace.WriteLine(
                    "CONTEXT SCAN: rejected candidate"
                );

                // Continue searching one byte after this getter.
                searchStart =
                    IntPtr.Add(
                        getter,
                        1
                    );
            }
        }

        private bool IsValidContextsStaticStorage(
            uint staticStorage)
        {
            if (staticStorage == 0)
            {
                return false;
            }

            // Static storage contains Contexts._sharedInstance.
            uint contexts =
                ReadPointer(
                    staticStorage
                );

            if (contexts == 0)
            {
                return false;
            }

            // Confirmed Contexts field offsets:
            //
            // +0x18 framework
            // +0x24 gUI
            uint frameworkContext =
                ReadPointer(
                    contexts + 0x18
                );

            uint guiContext =
                ReadPointer(
                    contexts + 0x24
                );

            Trace.WriteLine(
                "CONTEXT VALIDATE: contexts=0x" +
                contexts.ToString("X") +
                " framework=0x" +
                frameworkContext.ToString("X") +
                " gui=0x" +
                guiContext.ToString("X")
            );

            bool frameworkValid =
                IsValidEntitasContext(
                    frameworkContext
                );

            bool guiValid =
                IsValidEntitasContext(
                    guiContext
                );

            Trace.WriteLine(
                "CONTEXT VALIDATE: frameworkValid=" +
                frameworkValid +
                " guiValid=" +
                guiValid
            );

            return
                frameworkValid &&
                guiValid;
        }

        private bool IsValidEntitasContext(
            uint context)
        {
            if (context == 0)
            {
                return false;
            }

            // Entitas.Context<TEntity>._entities = +0x28.
            uint entities =
                ReadPointer(
                    context + 0x28
                );

            if (entities == 0)
            {
                return false;
            }

            // HashSet<TEntity>:
            //
            // _slots     = +0x0C
            // _lastIndex = +0x1C
            uint slots =
                ReadPointer(
                    entities + 0x0C
                );

            int lastIndex =
                (int)ReadUInt32(
                    new IntPtr(
                        entities + 0x1C
                    )
                );

            return
                slots != 0 &&
                lastIndex >= 0 &&
                lastIndex < 10000;
        }

        // ============================================================
        // GENERAL SIGNATURE SCANNING
        // ============================================================

        public IntPtr FindSignature(
            string pattern)
        {
            if (!IsAttached)
            {
                return IntPtr.Zero;
            }

            byte?[] signature =
                ParseSignature(
                    pattern
                );

            IntPtr currentAddress =
                IntPtr.Zero;

            int structureSize =
                Marshal.SizeOf(
                    typeof(
                        MEMORY_BASIC_INFORMATION
                    )
                );

            while (true)
            {
                IntPtr queryResult =
                    VirtualQueryEx(
                        Game.Handle,
                        currentAddress,
                        out MEMORY_BASIC_INFORMATION memoryInfo,
                        new IntPtr(
                            structureSize
                        )
                    );

                if (queryResult == IntPtr.Zero)
                {
                    break;
                }

                long regionBase =
                    memoryInfo.BaseAddress
                        .ToInt64();

                long regionSize =
                    memoryInfo.RegionSize
                        .ToInt64();

                bool committed =
                    memoryInfo.State ==
                    MEM_COMMIT;

                bool accessible =
                    (
                        memoryInfo.Protect &
                        PAGE_NOACCESS
                    ) == 0
                    &&
                    (
                        memoryInfo.Protect &
                        PAGE_GUARD
                    ) == 0;

                if (
                    committed &&
                    accessible &&
                    regionSize > 0 &&
                    regionSize <= int.MaxValue
                )
                {
                    byte[] memory =
                        ReadBytes(
                            memoryInfo.BaseAddress,
                            (int)regionSize
                        );

                    if (memory != null)
                    {
                        int offset =
                            FindPattern(
                                memory,
                                signature,
                                0
                            );

                        if (offset >= 0)
                        {
                            return IntPtr.Add(
                                memoryInfo.BaseAddress,
                                offset
                            );
                        }
                    }
                }

                long nextAddress =
                    regionBase +
                    regionSize;

                if (
                    nextAddress <= regionBase ||
                    nextAddress > uint.MaxValue
                )
                {
                    break;
                }

                currentAddress =
                    new IntPtr(
                        nextAddress
                    );
            }

            return IntPtr.Zero;
        }

        // ============================================================
        // EXECUTABLE SIGNATURE SCANNING
        // ============================================================

        public IntPtr FindExecutableSignatureChunked(
            string pattern,
            IntPtr searchStart)
        {
            if (!IsAttached)
            {
                return IntPtr.Zero;
            }

            byte?[] signature =
                ParseSignature(
                    pattern
                );

            long minimumAddress =
                searchStart.ToInt64();

            IntPtr currentAddress =
                searchStart;

            int structureSize =
                Marshal.SizeOf(
                    typeof(
                        MEMORY_BASIC_INFORMATION
                    )
                );

            Trace.WriteLine(
                "EXEC SCAN: starting at 0x" +
                minimumAddress.ToString("X")
            );

            while (true)
            {
                IntPtr queryResult =
                    VirtualQueryEx(
                        Game.Handle,
                        currentAddress,
                        out MEMORY_BASIC_INFORMATION memoryInfo,
                        new IntPtr(
                            structureSize
                        )
                    );

                if (queryResult == IntPtr.Zero)
                {
                    break;
                }

                long regionBase =
                    memoryInfo.BaseAddress
                        .ToInt64();

                long regionSize =
                    memoryInfo.RegionSize
                        .ToInt64();

                long regionEnd =
                    regionBase +
                    regionSize;

                bool committed =
                    memoryInfo.State ==
                    MEM_COMMIT;

                bool accessible =
                    (
                        memoryInfo.Protect &
                        PAGE_NOACCESS
                    ) == 0
                    &&
                    (
                        memoryInfo.Protect &
                        PAGE_GUARD
                    ) == 0;

                bool executable =
                    IsExecutable(
                        memoryInfo.Protect
                    );

                if (
                    committed &&
                    accessible &&
                    executable &&
                    regionSize > 0
                )
                {
                    // If searchStart lies in the middle of this
                    // region, don't start scanning again from the
                    // beginning of the region. Otherwise we would
                    // rediscover the same rejected candidate forever.
                    long scanStart =
                        Math.Max(
                            regionBase,
                            minimumAddress
                        );

                    if (scanStart < regionEnd)
                    {
                        Trace.WriteLine(
                            "EXEC SCAN: region 0x" +
                            regionBase.ToString("X") +
                            " size=0x" +
                            regionSize.ToString("X") +
                            " scanStart=0x" +
                            scanStart.ToString("X")
                        );

                        IntPtr result =
                            ScanExecutableRangeInChunks(
                                scanStart,
                                regionEnd,
                                signature
                            );

                        if (result != IntPtr.Zero)
                        {
                            return result;
                        }
                    }
                }

                if (
                    regionEnd <= regionBase ||
                    regionEnd > uint.MaxValue
                )
                {
                    break;
                }

                currentAddress =
                    new IntPtr(
                        regionEnd
                    );

                minimumAddress =
                    regionEnd;
            }

            return IntPtr.Zero;
        }

        private IntPtr ScanExecutableRangeInChunks(
            long scanStart,
            long scanEnd,
            byte?[] signature)
        {
            int overlap =
                Math.Max(
                    signature.Length - 1,
                    0
                );

            long address =
                scanStart;

            while (address < scanEnd)
            {
                long remaining =
                    scanEnd - address;

                int bytesToRead =
                    (int)Math.Min(
                        SIGNATURE_CHUNK_SIZE,
                        remaining
                    );

                if (
                    remaining > bytesToRead &&
                    overlap > 0
                )
                {
                    bytesToRead =
                        (int)Math.Min(
                            (long)bytesToRead +
                            overlap,
                            remaining
                        );
                }

                IntPtr chunkAddress =
                    new IntPtr(
                        address
                    );

                byte[] memory =
                    ReadBytes(
                        chunkAddress,
                        bytesToRead
                    );

                if (memory != null)
                {
                    int offset =
                        FindPattern(
                            memory,
                            signature,
                            0
                        );

                    if (offset >= 0)
                    {
                        return IntPtr.Add(
                            chunkAddress,
                            offset
                        );
                    }
                }

                address +=
                    SIGNATURE_CHUNK_SIZE;
            }

            return IntPtr.Zero;
        }

        // ============================================================
        // SIGNATURE HELPERS
        // ============================================================

        private bool IsExecutable(
            uint protection)
        {
            uint baseProtection =
                protection & 0xFF;

            return
                baseProtection ==
                    PAGE_EXECUTE ||
                baseProtection ==
                    PAGE_EXECUTE_READ ||
                baseProtection ==
                    PAGE_EXECUTE_READWRITE ||
                baseProtection ==
                    PAGE_EXECUTE_WRITECOPY;
        }

        private byte?[] ParseSignature(
            string pattern)
        {
            string[] parts =
                pattern.Split(
                    new[] { ' ' },
                    StringSplitOptions
                        .RemoveEmptyEntries
                );

            byte?[] signature =
                new byte?[parts.Length];

            for (
                int i = 0;
                i < parts.Length;
                i++
            )
            {
                if (parts[i] == "??")
                {
                    signature[i] =
                        null;
                }
                else
                {
                    signature[i] =
                        byte.Parse(
                            parts[i],
                            NumberStyles.HexNumber
                        );
                }
            }

            return signature;
        }

        private int FindPattern(
            byte[] memory,
            byte?[] signature,
            int startIndex)
        {
            for (
                int i = startIndex;
                i <=
                memory.Length -
                signature.Length;
                i++
            )
            {
                bool match =
                    true;

                for (
                    int j = 0;
                    j < signature.Length;
                    j++
                )
                {
                    if (
                        signature[j].HasValue &&
                        memory[i + j] !=
                        signature[j].Value
                    )
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}