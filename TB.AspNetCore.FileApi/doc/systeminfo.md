## 一.前言
.NET Core 内置了一些API供我们获取操作系统、运行时、框架等信息。
这些API不是很常用，所有有些小伙伴可能还不知道，这里做一些可能用到的获取操作系统的API介绍

## 二.判断操作系统
判断操作系统是否为 Linux OSX Windows，主要使用 System.Runtime.InteropServices.IsOSPlatform()这个方法，使用如下：

Console.WriteLine("判断是否为Windows Linux OSX");
Console.WriteLine($"Linux:{RuntimeInformation.IsOSPlatform(OSPlatform.Linux)}");
Console.WriteLine($"OSX:{RuntimeInformation.IsOSPlatform(OSPlatform.OSX)}");
Console.WriteLine($"Windows:{RuntimeInformation.IsOSPlatform(OSPlatform.Windows)}");
执行结果：

1526923666756

## 三.获取操作系统架构、名称
Console.WriteLine($"系统架构：{RuntimeInformation.OSArchitecture}");
Console.WriteLine($"系统名称：{RuntimeInformation.OSDescription}");
Console.WriteLine($"进程架构：{RuntimeInformation.ProcessArchitecture}");
Console.WriteLine($"是否64位操作系统：{Environment.Is64BitOperatingSystem}");
执行结果：

1526923737607

四.写在最后
获取这些信息的类主要都在 System.Runtime.InteropServices名称空间下。
相关类名都带 Runtime 或者 Environment，如果还有其他需求，请大家去这里查找。