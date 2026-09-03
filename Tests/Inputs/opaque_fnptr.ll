; Indirect call through an opaque function `ptr`: the callee signature is
; recovered from the call site, not the pointer type. compute() == 42.
target datalayout = "e-m:o-i64:64-n32:64-S128"

define i32 @add2(i32 %a, i32 %b) #0 {
entry:
  %s = add i32 %a, %b
  ret i32 %s
}

define i32 @compute() #0 {
entry:
  %f = alloca ptr, align 8
  store ptr @add2, ptr %f, align 8
  %fp = load ptr, ptr %f, align 8
  %r = call i32 %fp(i32 30, i32 12)
  ret i32 %r
}

attributes #0 = { noinline nounwind }
