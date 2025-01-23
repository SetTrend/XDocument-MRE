using System.Xml;
using System.Xml.Linq;

namespace XDocument_MRE;

internal class Program
{
	private static readonly ConsoleColor _fgColorBU = Console.ForegroundColor;
	private static readonly ConsoleColor _bgColorBU = Console.BackgroundColor;

	private const string _new = "i";
	private const string source = @"<body>
	<p>
		<span>
			<span>This</span> <span>is a</span>
			<b><span>test</span></b><span>.</span>
		</span>
	</p>
</body>";

	internal static void Main()
	{
		// ------ Not preserving Whitespace ------------------------------------------------------

		XDocument doc = XDocument.Parse(source);

		string source1a = doc.ToString();
		string source1b = doc.Root!.Value;


		while (doc.DescendantNodes().Where(d => d.NodeType == XmlNodeType.Text && d.Parent?.Name.LocalName != _new).FirstOrDefault() is XText text)
		{
			XElement element = text.Parent!;
			XElement newElement = new XElement(element.GetDefaultNamespace() + _new, element.Nodes());

			element.RemoveAll();
			element.Add(newElement);
		}

		string result1a = doc.ToString();
		string result1b = doc.ToString(SaveOptions.DisableFormatting);
		string result1c = doc.Root!.Value;


		// ------ Preserving Whitespace ------------------------------------------------------

		doc = XDocument.Parse(source, LoadOptions.PreserveWhitespace);

		string source2a = doc.ToString();
		string source2b = doc.Root!.Value;

		while
			(doc.DescendantNodes()
			.Where(d => d.NodeType == XmlNodeType.Text && d.Parent?.Name.LocalName != _new)
			.FirstOrDefault() is XText text
			)
		{
			XElement element = text.Parent!;
			XElement newElement = new XElement(element.GetDefaultNamespace() + _new, element.Nodes());

			element.RemoveAll();
			element.Add(newElement);
		}

		string result2a = doc.ToString();
		string result2b = doc.ToString(SaveOptions.DisableFormatting);
		string result2c = doc.Root!.Value;


		// ------ Output ------------------------------------------------------

		WriteLine(ConsoleColor.Green, "Source XML:", source);

		WriteLine(ConsoleColor.DarkYellow, "XDocument.Parse(source)", source1a);
		WriteLine(ConsoleColor.Yellow, "doc.Value before", source1b);
		WriteLine(ConsoleColor.DarkYellow, "doc.ToString()", result1a);
		WriteLine(ConsoleColor.DarkYellow, "doc.ToString(SaveOptions.DisableFormatting)", result1b);
		WriteLine(ConsoleColor.DarkYellow, "doc.Value after", result1c);

		WriteLine(ConsoleColor.DarkCyan, "XDocument.Parse(source, LoadOptions.PreserveWhitespace)", source2a);
		WriteLine(ConsoleColor.Cyan, "doc.Value before", source2b);
		WriteLine(ConsoleColor.DarkCyan, "doc.ToString()", result2a);
		WriteLine(ConsoleColor.DarkCyan, "doc.ToString(SaveOptions.DisableFormatting)", result2b);
		WriteLine(ConsoleColor.DarkCyan, "doc.Value after", result2c);

		Console.ReadKey();
	}

	private static void WriteLine(ConsoleColor color, string title, string text)
	{
		Console.ForegroundColor = _fgColorBU;
		Console.WriteLine();

		Console.BackgroundColor = ConsoleColor.DarkBlue;
		Console.WriteLine(title);

		Console.ForegroundColor = color;
		Console.BackgroundColor = _bgColorBU;
		Console.WriteLine(text);

		Console.ForegroundColor = _fgColorBU;
	}
}
