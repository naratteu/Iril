#include <stdint.h>

typedef void (*dispatch_t)(void*, void*, void*);

void pony_sendv(void* ctx, void* actor, void* message, void* next, uint8_t urgent)
{
  dispatch_t dispatch = *(dispatch_t*)(*(char**)actor + 32);
  dispatch(ctx, actor, message);
}

void pony_sendv_single(void* ctx, void* actor, void* message, void* next, uint8_t urgent)
{
  pony_sendv(ctx, actor, message, next, urgent);
}
