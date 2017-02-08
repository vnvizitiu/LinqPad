<Query Kind="Program">
  <Namespace>System.Security.AccessControl</Namespace>
</Query>

public class FileStream
{	
	public FileStream (
		string path,
		FileMode mode,
		FileAccess access = FileAccess.ReadWrite,
		FileShare share = FileShare.Read,
		int bufferSize = 0x1000,
		FileOptions options = FileOptions.None)
	{
		// Actual code
		new { path, mode, access, share, bufferSize, options }.Dump();
	}
}	

void Main()
{
	new FileStream ("temp.txt", FileMode.Create, options: FileOptions.Asynchronous);
}