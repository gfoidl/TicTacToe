``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.17134.228 (1803/April2018Update/Redstone4)
Intel Core i7-7700HQ CPU 2.80GHz (Max: 2.81GHz) (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
Frequency=2742186 Hz, Resolution=364.6726 ns, Timer=TSC
.NET Core SDK=2.1.401
  [Host]     : .NET Core 2.1.3-servicing-26724-03 (CoreCLR 4.6.26724.06, CoreFX 4.6.26724.03), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.3-servicing-26724-03 (CoreCLR 4.6.26724.06, CoreFX 4.6.26724.03), 64bit RyuJIT


```
|       Method |      Mean |     Error |    StdDev | Scaled | ScaledSD |
|------------- |----------:|----------:|----------:|-------:|---------:|
| BitOperation | 0.0887 ns | 0.0169 ns | 0.0158 ns |   1.00 |     0.00 |
|   Intrinsics | 0.0885 ns | 0.0135 ns | 0.0126 ns |   1.03 |     0.22 |
