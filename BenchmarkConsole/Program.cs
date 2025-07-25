using BenchmarkConsole;
using BenchmarkDotNet.Running;
using ClassLibrary1;
using Microsoft.Diagnostics.Tracing.Parsers.Clr;

// User.SetName();

BenchmarkRunner.Run<StaticVsInstanceBenchmark>();

Console.ReadLine();
// BenchmarkRunner.Run<MyBenchmarks>();
// BenchmarkRunner.Run<MyBenchmarks2>();
