# Pony Hello World

Requires `ponyc`, `dotnet`, and `clang`.

```sh
git clone --branch pony-llvm22-preprocess https://github.com/naratteu/Iril.git
cd Iril/examples/pony-hello
./run.sh
```

This is a synchronous runtime slice for one actor startup and stdout message.
It is not the Pony scheduler or garbage collector.
