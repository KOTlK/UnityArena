using System;
using UnityEngine;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using static Unity.Collections.LowLevel.Unsafe.UnsafeUtility;

public unsafe interface IAllocator : IDisposable {
        T *Alloc<T>(uint count) where T : unmanaged;
        void Free<T>(T *ptr) where T : unmanaged;
}

public unsafe class ArenaAllocator : IAllocator {
    public byte *Data;

    private ulong occupied;
    private ulong totalSize;

    public ArenaAllocator(long size) {
#if UNTIY_EDITOR
        Data = (byte*)MallocTracked(size, AlignOf<byte>(), Allocator.Persistent, 0);
#endif
#if !UNTIY_EDITOR
        Data = (byte*)Malloc(size, AlignOf<byte>(), Allocator.Persistent);
#endif
        totalSize = (ulong)size;
    }

    public void Dispose() {
#if UNTIY_EDITOR
        FreeTracked((void*)Data, Allocator.Persistent);
#endif
#if !UNTIY_EDITOR
        UnsafeUtility.Free((void*)Data, Allocator.Persistent);
#endif
    }

    public T *Alloc<T>(uint count) 
    where T : unmanaged {
        ulong size = (ulong)sizeof(T) * count;

        if(size + occupied > totalSize) {
            Resize((long)((size + occupied) << 1));
        }

        var ptr = (T*)(Data + occupied);

        occupied += size;

        return ptr;
    }

    public void Free<T>(T *ptr) 
    where T : unmanaged{
        throw new Exception("You can't free individual parts of memory with the Arena Allocator");
    }
    
    public void FreeAll() {
        occupied = 0;
    }

    public void Resize(long newSize) {
        byte *newData;
#if UNTIY_EDITOR
        newData = (byte*)MallocTracked(newSize, AlignOf<byte>(), Allocator.Persistent, 0);
#endif
#if !UNTIY_EDITOR
        newData = (byte*)Malloc(newSize, AlignOf<byte>(), Allocator.Persistent);
#endif

        for(ulong i = 0; i < totalSize; ++i) {
            newData[i] = Data[i];
        }
        
#if UNTIY_EDITOR
        FreeTracked((void*)Data, Allocator.Persistent);
#endif
#if !UNTIY_EDITOR
        UnsafeUtility.Free((void*)Data, Allocator.Persistent);
#endif
        totalSize = (ulong)newSize;
        Data = newData;
    }
}