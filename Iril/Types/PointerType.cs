using System;
using Mono.Cecil;

namespace Iril.Types
{
    public class PointerType : AggregateType
    {
        public readonly LType ElementType;
        public readonly int AddressSpace;

        public static readonly PointerType I8Pointer = new PointerType (IntegerType.I8, 0);
        public static readonly PointerType I32Pointer = new PointerType (IntegerType.I32, 0);
        public static readonly PointerType VoidPointer = new PointerType (VoidType.Void, 0);

        /// <summary>
        /// LLVM 15+ opaque pointer (<c>ptr</c>). It carries no element type of its own;
        /// every typed operation (load/store/getelementptr/call) supplies the type explicitly.
        /// For CLR emission it behaves like a generic <c>i8*</c> byte pointer.
        /// </summary>
        public static readonly PointerType OpaquePointer = new PointerType (IntegerType.I8, 0, isOpaque: true);

        /// <summary>True for the LLVM 15+ opaque <c>ptr</c> type.</summary>
        public readonly bool IsOpaque;

        public PointerType (LType elementType, int addressSpace, bool isOpaque = false)
        {
            ElementType = elementType;
            AddressSpace = addressSpace;
            IsOpaque = isOpaque;
        }

        public override string ToString () => IsOpaque ? "ptr" : $"{ElementType}*";

        public override long GetByteSize (Module module) => module.PointerByteSize;

        public override int GetAlignment (Module module) => module.PointerByteSize;

        public override bool StructurallyEquals (LType other) =>
            other is PointerType a
            && ElementType.StructurallyEquals (a.ElementType);

        public override int GetStructuralHashCode () =>
            789
            + ElementType.GetStructuralHashCode ();
    }
}
