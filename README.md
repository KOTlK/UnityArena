# UnityArena
Arena allocator for Unity

## Installation

Make sure you have standalone git installed.

- `Unity->Edit->Project Settings->Player->Allow 'unsafe' Code` should be turned on 
- `Unity->Package Manager ->Add package from git URL`.
- Paste: `https://github.com/KOTlK/UnityArena.git`.

## Usage

- Create Allocator instance (`new ArenaAllocator(long initialSizeInBytes)`).
- Allocate data using `T *Alloc<T>(uint count)` method.
- - `count` Parameter means amount of items you want to have.
- - `Alloc<int>(10)` means that you will get pointer to 10 integers, allocating totally `10 * 
    4 = 40`bytes.
- Free all data using `void FreeAll()` method.
- - This method doesn't freeing any memory from the heap.
- Use `Dispose` method to free the memory, when you don't need the allocator anymore.
