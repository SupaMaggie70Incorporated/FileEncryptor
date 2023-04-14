using System.Security.Cryptography;
using System.Text;

Console.Write("If you would like to decrypt type \"y\": ");

if(Console.ReadLine() == "y") // Decrypt
{
    Console.Write("Input file name: ");
    string inputFile = Console.ReadLine();
    Console.Write("Output file name: ");
    string outputFile = Console.ReadLine();
    Console.Write("Password: ");

    string password = Console.ReadLine();
    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

    byte[] buffer = SHA512.Create().ComputeHash(passwordBytes);

    byte[] hash = new byte[32];
    Buffer.BlockCopy(buffer, 0, hash, 0, 32);

    byte[] shuffle = new byte[32];
    Buffer.BlockCopy(buffer, 16, shuffle, 0, 32); // Part of it intersects with hash

    FileStream input = File.OpenRead(inputFile);

    for (int i = 0; i < 16; i++)
    {
        if (input.ReadByte() != buffer[i + 48])
        {
            Console.WriteLine("Incorrect password");
            Environment.Exit(0);
        }
    }

    FileStream output = File.OpenWrite(outputFile);

    int amountRead;
    buffer = new byte[32];
    Random random = new Random();
    int[] arr = Enumerable.Range(0, 32).OrderBy(x=>random.Next());
    while ((amountRead = input.Read(buffer, 0, 32)) != 0 && input.CanRead)
    {
        // We need to work on this...
        
        for(int i = 0;i < amountRead;i++)
        {
            byte x = buffer[i];

        }

        output.Flush();
    }
    input.Close();
    output.Close();
    Console.WriteLine("Finished");

}
else
{
    Console.Write("Input file name: ");
    string inputFile = Console.ReadLine();
    Console.Write("Output file name: ");
    string outputFile = Console.ReadLine();
    Console.Write("Password: ");
    string password = Console.ReadLine();
    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

    byte[] buffer = SHA512.Create().ComputeHash(passwordBytes); ;

    byte[] hash = new byte[32];
    Buffer.BlockCopy(buffer, 0, hash, 0, 32);

    byte[] shuffle = new byte[32];
    Buffer.BlockCopy(buffer, 16, shuffle, 0, 32); // Part of it intersects with hash


    FileStream input = File.OpenRead(inputFile);
    FileStream output = File.OpenWrite(outputFile);

    output.Write(buffer, 48, 16); // Write out first checker for password
    // First checker is only 248 bits, one byte is used for shuffle

    int amountRead;
    buffer = new byte[32];
    while ((amountRead = input.Read(buffer, 0, 32)) != 0 && input.CanRead)
    {
        output.Flush();
    }
    input.Close();
    output.Close();
    Console.WriteLine("Finished");
}