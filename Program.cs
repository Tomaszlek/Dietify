using Spectre.Console;

class Program
{
    static void Main(string[] args)
    {
        var table = new Table();
        table.AddColumn("Column 1");
        table.AddColumn("Column 2");

        table.AddRow("Cell 1", "Cell 2");
        table.AddRow("Cell 3", "Cell 4");

        AnsiConsole.Write(table);
    }
}