interface FileAddOn
{
    string Name { get; }
    int getSize();
}

class SomeFile : FileAddOn
{
    public string Name { get; }
    public int size = 10;

    public int getSize()
    {
        Console.WriteLine("나는 이만큼 커!" + size);
        return size;
    }
}

class SomeFolder : FileAddOn
{
    public string Name { get; }
    public int size { get; private set; }

    public List<FileAddOn> someFiles = new List<FileAddOn>();

    public int getSize()
    {
        int total = 0;

        foreach(FileAddOn f in someFiles)
        {
            total += f.getSize();
        }

        Console.WriteLine("나는 이만큼 커!" + total);
        return total;
    }
}


class Program
{
    static void Main()
    {
        SomeFolder f1 = new SomeFolder();
        SomeFile f2 = new SomeFile();
        SomeFile f3 = new SomeFile();
        SomeFolder f4 = new SomeFolder();
        SomeFile f5 = new SomeFile();
        SomeFile f6 = new SomeFile();

        f1.someFiles.Add(f2);
        f1.someFiles.Add(f3);
        f1.someFiles.Add(f4);

        f4.someFiles.Add(f5);
        f4.someFiles.Add(f6);

        f1.getSize();
    }
}