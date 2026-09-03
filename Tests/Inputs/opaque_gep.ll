; Opaque-pointer getelementptr using the instruction's explicit source
; element type (the `ptr` itself carries none). compute() == 15.
target datalayout = "e-m:o-i64:64-n32:64-S128"

define i32 @compute() #0 {
entry:
  %a = alloca [4 x i32], align 16
  %p0 = getelementptr inbounds [4 x i32], ptr %a, i64 0, i64 0
  store i32 10, ptr %p0, align 4
  %p3 = getelementptr inbounds [4 x i32], ptr %a, i64 0, i64 3
  store i32 5, ptr %p3, align 4
  %v0 = load i32, ptr %p0, align 4
  %v3 = load i32, ptr %p3, align 4
  %s = add i32 %v0, %v3
  ret i32 %s
}

attributes #0 = { noinline nounwind }
