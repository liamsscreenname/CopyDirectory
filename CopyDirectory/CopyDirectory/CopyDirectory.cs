using System;
using System.IO;

public class CopyDirectory
{
    private string source;
    private DirectoryInfo destination;
    public bool copying = true;
    public string current;

    public CopyDirectory(string s, string d)
    {
        source = s;
        destination = new DirectoryInfo(d);
    }

    //Copy a file or folder
    public int Copy()
    {
        //If the source path points to a file, copy that file to destination directory
        if (File.Exists(source))
        {
            CopyFile(new FileInfo(source), destination);
            copying = false;
            return 1;
        }
        //Otherwise copy the entire folder to a new sub-directory in the destination folder
        CopyFolder(new DirectoryInfo(source), destination);
        copying = false;
        return 1;
    }

    private void CopyFile(FileInfo src, DirectoryInfo dst)
    {
        try
        {
            Console.WriteLine("Copying..." + src.FullName);
            current = src.FullName;
            src.CopyTo(Path.Combine(dst.FullName, src.Name));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    private void CopyFolder(DirectoryInfo src, DirectoryInfo dst)
    {
        //Create new sub-directory
        DirectoryInfo newSubDir;
        try
        {
            newSubDir = dst.CreateSubdirectory(src.Name);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }

        //Copy all files in source folder to new destination folder
        foreach (FileInfo file in src.GetFiles())
            CopyFile(file, newSubDir);

        //Copy all sub-folders to new destination folder
        foreach (DirectoryInfo subFolder in src.GetDirectories())
            CopyFolder(subFolder, newSubDir);
    }
}
