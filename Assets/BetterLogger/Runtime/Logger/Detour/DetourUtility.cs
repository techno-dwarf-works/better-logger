using System;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace Better.Logger.Runtime
{
    public static class DetourUtility
    {
        // this is based on an interesting technique from the RimWorld ComunityCoreLibrary project, originally credited to RawCode:
        // https://github.com/RimWorldCCLTeam/CommunityCoreLibrary/blob/master/DLL_Project/Classes/Static/Detours.cs
        // licensed under The Unlicense:
        // https://github.com/RimWorldCCLTeam/CommunityCoreLibrary/blob/master/LICENSE
        public static unsafe void TryDetourFromTo(MethodInfo src, MethodInfo dst)
        {
            try
            {
                if (IntPtr.Size == sizeof(long))
                {
                    // 64-bit systems use 64-bit absolute address and jumps
                    // 12 byte destructive

                    // Get function pointers
                    var srcBase = src.MethodHandle.GetFunctionPointer().ToInt64();
                    var dstBase = dst.MethodHandle.GetFunctionPointer().ToInt64();

                    // Native source address
                    var pointerRawSource = (byte*)srcBase;

                    // Pointer to insert jump address into native code
                    var pointerRawAddress = (long*)(pointerRawSource + 2);

                    // Insert 64-bit absolute jump into native code (address in rax)
                    // mov rax, immediate64
                    // jmp [rax]
                    *(pointerRawSource + 0) = 72;
                    *(pointerRawSource + 1) = 184;
                    *pointerRawAddress = dstBase; // ( pointerRawSource + 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09 )
                    *(pointerRawSource + 10) = 255;
                    *(pointerRawSource + 11) = 224;
                }
                else
                {
                    // 32-bit systems use 32-bit relative offset and jump
                    // 5 byte destructive

                    // Get function pointers
                    var srcBase = src.MethodHandle.GetFunctionPointer().ToInt32();
                    var dstBase = dst.MethodHandle.GetFunctionPointer().ToInt32();

                    // Native source address
                    var pointerRawSource = (byte*)srcBase;

                    // Pointer to insert jump address into native code
                    var pointerRawAddress = (int*)(pointerRawSource + 1);

                    // Jump offset (less instruction size)
                    var offset = dstBase - srcBase - 5;

                    // Insert 32-bit relative jump into native code
                    *pointerRawSource = 233;
                    *pointerRawAddress = offset;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unable to detour: {src?.Name ?? "null src"} -> {dst?.Name ?? "null dst"}\n{ex}");
                throw;
            }
        }
    }
}