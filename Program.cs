using System.Xml;
using System.Xml.Linq;

namespace XDocument_MRE;

internal class Program
{
	private const string _new = "i";
	private const string source = @"<body>
	<p>
		<span>
			<span>This</span> <span>is a</span>
			<b><span>test</span></b><span>.</span>
		</span>
	</p>
</body>";

	static void Main()
	{
		// ------ Not preserving Whitespace ------------------------------------------------------

		XDocument doc = XDocument.Parse(source);

		string source1 = doc.ToString();

		while (doc.DescendantNodes().Where(d => d.NodeType == XmlNodeType.Text && d.Parent?.Name.LocalName != _new).FirstOrDefault() is XText text)
		{
			XElement element = text.Parent!;
			XElement newElement = new XElement(element.GetDefaultNamespace() + _new, element.Nodes());

			element.RemoveAll();
			element.Add(newElement);
		}

		string result1 = doc.ToString();


		// ------ Preserving Whitespace ------------------------------------------------------

		doc = XDocument.Parse(source, LoadOptions.PreserveWhitespace);

		string source2 = doc.ToString();

		while (doc.DescendantNodes().Where(d => d.NodeType == XmlNodeType.Text && d.Parent?.Name.LocalName != _new).FirstOrDefault() is XText text)
		{
			XElement element = text.Parent!;
			XElement newElement = new XElement(element.GetDefaultNamespace() + _new, element.Nodes());

			element.RemoveAll();
			element.Add(newElement);
		}

		string result2 = doc.ToString();


		// ------ Output ------------------------------------------------------

		ConsoleColor backup = Console.ForegroundColor;

		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine(source);

		Console.ForegroundColor = ConsoleColor.DarkYellow;
		Console.WriteLine(source1);

		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine(result1);

		Console.ForegroundColor = ConsoleColor.DarkCyan;
		Console.WriteLine(source2);

		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine(result2);

		Console.ForegroundColor = backup;
	}
}
