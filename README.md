## Lightweight C# bindings for Databento’s DBN Rust crate


##### Layout
```text
dbn-dotnet/
├─ external/dbn
│   └─ rust/dbn-ffi/           < new crate in databento/dbn fork
├─ src/
│   └─ Dbn.Net.csproj          < C# P/Invoke bindings
```
 
##### Clone
```
git clone --recurse-submodules https://github.com/s-brez/dbn-dotnet.git
```

##### Build
```
cd dbn-dotnet/external/dbn
cargo build -p dbn-ffi --release
```
