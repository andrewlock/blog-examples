using System.Diagnostics;
using JavaScriptEngineSwitcher.Jurassic;

var sw = new Stopwatch();
sw.Start();
Console.WriteLine("Loading JurassicEngine");

var engine = new JurassicJsEngine();

Console.WriteLine("Executing prism.js");
engine.ExecuteResource("JsInDotnet.prism.js", typeof(Program).Assembly);

Console.WriteLine("Highlighting code:");
Console.WriteLine();
var code = @"
using System;

public class Test : ITest
{
    public int ID { get; set; }
    public string Name { get; set; }
}";
Console.WriteLine(code);

engine.SetVariableValue("input", code);
engine.SetVariableValue("lang", "csharp");


engine.Execute($"highlighted = Prism.highlight(input, Prism.languages.csharp, lang)");
string result = engine.Evaluate<string>("highlighted");

Console.WriteLine("Highlighed version:");
Console.WriteLine(result);


Console.WriteLine("Finished at " + sw.Elapsed.ToString());
