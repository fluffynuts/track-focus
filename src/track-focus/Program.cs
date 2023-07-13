// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using PeanutButter.EasyArgs;
using PeanutButter.Utils;
using track_focus;

var opts = args.ParseTo<IOptions>();
var lastProcess = null as Process;
Console.WriteLine($"Polling foreground process every {opts.Interval}ms");
while (true)
{
    var proc = FindForegroundWindow.GetForegroundProcess();
    if (proc is null)
    {
        Console.WriteLine("- can't determine foreground process");
    }
    else
    {
        if (proc.Id != lastProcess?.Id)
        {
            lastProcess = proc;
            var ancestryLine = "";
            if (opts.ShowAncestry)
            {
                var ancestry = new List<Process>();
                var parent = proc.ParentProcessId();
                while (parent != -1)
                {
                    var parentProcess = TryFindProcessById(parent);
                    ancestry.Add(parentProcess);
                    parent = parentProcess?.ParentProcessId() ?? -1;
                }

                ancestryLine = GenerateAncestryLineFor(ancestry);
            }

            Console.WriteLine(
                $"{GenerateProcessInfo(proc)} {ancestryLine}"
            );
        }
    }

    Thread.Sleep(opts.Interval);
}

string GenerateProcessInfo(Process proc)
{
    return proc is null
        ? "(unknown)"
        : $"[{proc.Id}] {proc.ProcessName}";
}

string GenerateAncestryLineFor(List<Process> ancestry)
{
    if (ancestry.Count == 0 || ancestry.All(a => a == null))
    {
        return "";
    }

    while (ancestry.Last() is null)
    {
        ancestry.RemoveAt(ancestry.Count - 1);
    }

    var parts = new List<string>();
    foreach (var p in ancestry)
    {
        parts.Push(GenerateProcessInfo(p));
    }

    return $" < {parts.JoinWith(" < ")}";
}

Process TryFindProcessById(int processId)
{
    try
    {
        return Process.GetProcessById(processId);
    }
    catch
    {
        return null;
    }
}