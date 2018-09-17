using System;

class MainFunction
{
    static void Main(string[] args)
    {
        string srcIn, dstIn;
        Console.WriteLine("Enter source path: ");
        srcIn = Console.ReadLine().ToLower();
        Console.WriteLine("Enter destination path: ");
        dstIn = Console.ReadLine().ToLower();

        CopyDirectory cDir = new CopyDirectory(srcIn, dstIn);
        cDir.Copy();
    }
}
