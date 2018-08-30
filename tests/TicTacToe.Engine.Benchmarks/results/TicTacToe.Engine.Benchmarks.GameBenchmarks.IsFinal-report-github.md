``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.17134.228 (1803/April2018Update/Redstone4)
Intel Core i7-7700HQ CPU 2.80GHz (Max: 2.81GHz) (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
Frequency=2742186 Hz, Resolution=364.6726 ns, Timer=TSC
.NET Core SDK=2.1.401
  [Host] : .NET Core 2.1.3-servicing-26724-03 (CoreCLR 4.6.26724.06, CoreFX 4.6.26724.03), 64bit RyuJIT
  Clr    : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3132.0
  Core   : .NET Core 2.1.3-servicing-26724-03 (CoreCLR 4.6.26724.06, CoreFX 4.6.26724.03), 64bit RyuJIT


```
|     Method |  Job | Runtime |     Mean |     Error |    StdDev | Scaled |
|----------- |----- |-------- |---------:|----------:|----------:|-------:|
| Sequential |  Clr |     Clr | 3.940 ns | 0.0387 ns | 0.0362 ns |   1.00 |
| Vectorized |  Clr |     Clr | 1.981 ns | 0.0298 ns | 0.0279 ns |   0.50 |
|            |      |         |          |           |           |        |
| Sequential | Core |    Core | 6.288 ns | 0.1611 ns | 0.1790 ns |   1.00 |
| Vectorized | Core |    Core | 2.015 ns | 0.0302 ns | 0.0283 ns |   0.32 |
