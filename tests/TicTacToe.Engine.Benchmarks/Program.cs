using BenchmarkDotNet.Running;

namespace TicTacToe.Engine.Benchmarks
{
    static class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
