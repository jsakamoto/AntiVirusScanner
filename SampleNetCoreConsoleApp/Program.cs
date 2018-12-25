using System;

class Program
{
    static void Main(string[] args)
    {
        var scanner = new AntiVirus.Scanner();
        var result = scanner.ScanAndClean(@"c:\work\test.com");
        Console.WriteLine(result);
    }
}
