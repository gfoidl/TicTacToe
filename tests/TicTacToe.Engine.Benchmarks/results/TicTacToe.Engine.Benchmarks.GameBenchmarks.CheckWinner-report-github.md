``` ini

BenchmarkDotNet=v0.11.1, OS=Windows 10.0.17134.228 (1803/April2018Update/Redstone4)
Intel Core i7-7700HQ CPU 2.80GHz (Max: 2.81GHz) (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
Frequency=2742186 Hz, Resolution=364.6726 ns, Timer=TSC
.NET Core SDK=2.1.401
  [Host] : .NET Core 2.1.3-servicing-26724-03 (CoreCLR 4.6.26724.06, CoreFX 4.6.26724.03), 64bit RyuJIT
  Clr    : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3132.0
  Core   : .NET Core 2.1.3-servicing-26724-03 (CoreCLR 4.6.26724.06, CoreFX 4.6.26724.03), 64bit RyuJIT


```
|                             Method |  Job | Runtime | UserWins |      Mean |     Error |    StdDev |    Median | Scaled | ScaledSD |
|----------------------------------- |----- |-------- |--------- |----------:|----------:|----------:|----------:|-------:|---------:|
|                   **Array_sequential** |  **Clr** |     **Clr** |    **False** | **18.677 ns** | **0.3494 ns** | **0.3268 ns** | **18.611 ns** |   **1.00** |     **0.00** |
|                   Array_vectorized |  Clr |     Clr |    False |  6.903 ns | 0.1700 ns | 0.2024 ns |  6.868 ns |   0.37 |     0.01 |
| Array_vectorized_transform_inlined |  Clr |     Clr |    False |  5.561 ns | 0.1298 ns | 0.1151 ns |  5.510 ns |   0.30 |     0.01 |
|  Array_vectorized_different_locals |  Clr |     Clr |    False | 13.846 ns | 0.3406 ns | 0.3922 ns | 13.711 ns |   0.74 |     0.02 |
|   Array_vectorized_values_combined |  Clr |     Clr |    False | 18.841 ns | 0.2598 ns | 0.2430 ns | 18.889 ns |   1.01 |     0.02 |
|                                    |      |         |          |           |           |           |           |        |          |
|                   Array_sequential | Core |    Core |    False | 18.673 ns | 0.0947 ns | 0.0839 ns | 18.657 ns |   1.00 |     0.00 |
|                   Array_vectorized | Core |    Core |    False |  6.557 ns | 0.0690 ns | 0.0645 ns |  6.568 ns |   0.35 |     0.00 |
| Array_vectorized_transform_inlined | Core |    Core |    False |  5.282 ns | 0.0611 ns | 0.0572 ns |  5.290 ns |   0.28 |     0.00 |
|  Array_vectorized_different_locals | Core |    Core |    False | 13.237 ns | 0.1466 ns | 0.1371 ns | 13.273 ns |   0.71 |     0.01 |
|   Array_vectorized_values_combined | Core |    Core |    False | 19.291 ns | 0.4364 ns | 0.5025 ns | 19.173 ns |   1.03 |     0.03 |
|                                    |      |         |          |           |           |           |           |        |          |
|                   **Array_sequential** |  **Clr** |     **Clr** |     **True** | **18.893 ns** | **0.2868 ns** | **0.2683 ns** | **18.818 ns** |   **1.00** |     **0.00** |
|                   Array_vectorized |  Clr |     Clr |     True | 13.023 ns | 0.2000 ns | 0.1773 ns | 13.000 ns |   0.69 |     0.01 |
| Array_vectorized_transform_inlined |  Clr |     Clr |     True |  9.723 ns | 0.1378 ns | 0.1289 ns |  9.708 ns |   0.51 |     0.01 |
|  Array_vectorized_different_locals |  Clr |     Clr |     True | 14.290 ns | 0.3212 ns | 0.6265 ns | 14.005 ns |   0.76 |     0.03 |
|   Array_vectorized_values_combined |  Clr |     Clr |     True | 19.459 ns | 0.1340 ns | 0.1188 ns | 19.425 ns |   1.03 |     0.02 |
|                                    |      |         |          |           |           |           |           |        |          |
|                   Array_sequential | Core |    Core |     True | 18.999 ns | 0.3483 ns | 0.3258 ns | 18.944 ns |   1.00 |     0.00 |
|                   Array_vectorized | Core |    Core |     True | 14.544 ns | 0.3263 ns | 0.5080 ns | 14.396 ns |   0.77 |     0.03 |
| Array_vectorized_transform_inlined | Core |    Core |     True |  9.100 ns | 0.0952 ns | 0.0890 ns |  9.094 ns |   0.48 |     0.01 |
|  Array_vectorized_different_locals | Core |    Core |     True | 13.618 ns | 0.1455 ns | 0.1290 ns | 13.613 ns |   0.72 |     0.01 |
|   Array_vectorized_values_combined | Core |    Core |     True | 19.328 ns | 0.2524 ns | 0.2361 ns | 19.328 ns |   1.02 |     0.02 |
