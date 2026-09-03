; Opaque-pointer alloca + store/load through `ptr`. compute() == 42.
target datalayout = "e-m:o-i64:64-n32:64-S128"

define i32 @compute() #0 {
entry:
  %p = alloca i32, align 4
  store i32 42, ptr %p, align 4
  %v = load i32, ptr %p, align 4
  ret i32 %v
}

attributes #0 = { noinline nounwind }
