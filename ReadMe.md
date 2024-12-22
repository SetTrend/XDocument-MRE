# .NET Runtime System.Xml.Linq.XDocument Issue MRE

This is a minimum reproducible example demonstrating that it is not possible to surround [`XText`](https://learn.microsoft.com/en-us/dotnet/api/system.xml.linq.xtext) nodes by newly created elements without breaking the original formatting.

1. Using [`XDocument.Parse(String)`](https://learn.microsoft.com/en-us/dotnet/api/system.xml.linq.xdocument.parse#system-xml-linq-xdocument-parse(system-string)),

   … XText nodes can correctly be identified, but whitespace formatting is ***broken***, i.e., whitespace is unsolicitedly added.

1. Using [`XDocument.Parse(String, LoadOptions.PreserveWhitespace)`](https://learn.microsoft.com/en-us/dotnet/api/system.xml.linq.xdocument.parse#system-xml-linq-xdocument-parse(system-string-system-xml-linq-loadoptions)),

   … significant `XText` nodes can ***not*** correctly be identified and segregated from insignificant whitespace `XText` nodes.

## Results From The MRE

Source XML:

```xml
<body>
	<p>
		<span>
			<span>This</span> <span>is a</span>
			<b><span>test</span></b><span>.</span>
		</span>
	</p>
</body>
```

---

Running `XDocument.Parse(String)`:

```xml
<body>
  <p>
    <span>
      <span>This</span>
      <span>is a</span>
      <b>
        <span>test</span>
      </b>
      <span>.</span>
    </span>
  </p>
</body>
```

… and trying to surround the `XText` nodes by an `<i>` element:

```xml
<body>
  <p>
    <span>
      <span>
        <i>This</i>
      </span>
      <span>
        <i>is a</i>
      </span>
      <b>
        <span>
          <i>test</i>
        </span>
      </b>
      <span>
        <i>.</i>
      </span>
    </span>
  </p>
</body>
```

---

Running `XDocument.Parse(String, LoadOptions)`:

```xml
<body>
	<p>
		<span>
			<span>This</span> <span>is a</span>
			<b><span>test</span></b><span>.</span>
		</span>
	</p>
</body>
```

… and trying to surround the `XText` nodes by an `<i>` element:

```xml
<body>
  <i>
	<p><i>
		<span><i>
			<span><i>This</i></span> <span><i>is a</i></span>
			<b><span><i>test</i></span></b><span><i>.</i></span>
		</i></span>
	</i></p>
</i>
</body>
```