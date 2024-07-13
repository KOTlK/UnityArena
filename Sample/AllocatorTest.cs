using System.Text;
using UnityEngine;

public unsafe class AllocatorTest : MonoBehaviour {
    public ArenaAllocator Allocator;
    
    private StringBuilder sb1 = new();
    private StringBuilder sb2 = new();

    private void Start() {
        Allocator = new ArenaAllocator(1024);

        var ints = Allocator.Alloc<int>(128);

        for(var i = 0; i < 128; ++i) {
            ints[i] = i;
        }

        var longs = Allocator.Alloc<long>(64);

        for(long i = 0; i < 64; ++i) {
            longs[i] = i;
        }

        var someShit = Allocator.Alloc<char>(300);

        sb1.Append("Ints: ");

        for(var i = 0; i < 128; ++i) {
            sb1.Append($"{ints[i]}, ");
        }

        sb1.Append("\n");
        sb1.Append("Longs: ");

        for(long i = 0; i < 64; ++i) {
            sb1.Append($"{longs[i]}, ");
        }

        Debug.Log(sb1.ToString());

        Allocator.FreeAll();

        longs = Allocator.Alloc<long>(64);

        for(long i = 0; i < 64; ++i) {
            longs[i] = i;
        }

        ints = Allocator.Alloc<int>(128);

        for(var i = 0; i < 128; ++i) {
            ints[i] = i;
        }

        sb2.Append("Ints: ");

        for(var i = 0; i < 128; ++i) {
            sb2.Append($"{ints[i]}, ");
        }

        sb2.Append("\n");
        sb2.Append("Longs: ");

        for(long i = 0; i < 64; ++i) {
            sb2.Append($"{longs[i]}, ");
        }

        Debug.Log(sb2.ToString());

        Debug.Log(sb1.ToString() == sb2.ToString());
    }

    private void OnDestroy() {
        Allocator.Dispose();
    }
}