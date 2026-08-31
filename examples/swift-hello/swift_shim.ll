; Mock libswiftCore -- minimal, consistent ABI so print() actually outputs.
; Compiled ALONGSIDE the Swift IR so multi-value returns share Iril's
; generated anonymous struct types (no ABI mismatch).
target datalayout = "e-m:o-i64:64-n32:64-S128"

declare i64 @write(i32, ptr, i64)
declare ptr @malloc(i64)

; String literal -> { countAndFlags, object }. Mock encoding: { length, bytesPtr }.
define { i64, ptr } @"$sSS21_builtinStringLiteral17utf8CodeUnitCount7isASCIISSBp_BwBi1_tcfC"(ptr %bytes, i64 %len, i1 %ascii) #0 {
entry:
  %r0 = insertvalue { i64, ptr } undef, i64 %len, 0
  %r1 = insertvalue { i64, ptr } %r0, ptr %bytes, 1
  ret { i64, ptr } %r1
}

; allocateUninitializedArray(count, elemType) -> { storage, elements }.
; storage must hold a 32-byte header + count*32-byte element slots.
define { ptr, ptr } @"$ss27_allocateUninitializedArrayySayxG_BptBwlF"(i64 %count, ptr %elemType) #0 {
entry:
  %sz0 = mul i64 %count, 32
  %sz = add i64 %sz0, 64
  %buf = call ptr @malloc(i64 %sz)
  %elem = getelementptr i8, ptr %buf, i64 32
  %r0 = insertvalue { ptr, ptr } undef, ptr %buf, 0
  %r1 = insertvalue { ptr, ptr } %r0, ptr %elem, 1
  ret { ptr, ptr } %r1
}

; $sSaMa(size, elemType) -> array type metadata response { ptr, i64 }. Mock: zero.
define { ptr, i64 } @"$sSaMa"(i64 %0, ptr %1) #0 {
entry:
  ret { ptr, i64 } zeroinitializer
}

; swift_bridgeObjectRelease -> no-op (mock has no ref counting).
define void @swift_bridgeObjectRelease(ptr %0) #0 {
entry:
  ret void
}

; print(array, sepCount, sepObj, termCount, termObj): write element0's String then the terminator.
define void @"$ss5print_9separator10terminatoryypd_S2StF"(ptr %array, i64 %sepCount, ptr %sepObj, i64 %termCount, ptr %termObj) #0 {
entry:
  %elem = getelementptr i8, ptr %array, i64 32
  %len = load i64, ptr %elem, align 8
  %bytesPtrAddr = getelementptr i8, ptr %elem, i64 8
  %bytes = load ptr, ptr %bytesPtrAddr, align 8
  %w0 = call i64 @write(i32 1, ptr %bytes, i64 %len)
  %w1 = call i64 @write(i32 1, ptr %termObj, i64 %termCount)
  ret void
}

attributes #0 = { }
