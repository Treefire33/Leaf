using System.Text;
using Newtonsoft.Json;
using Raylib_cs;

namespace Cattail.UI.Theming;

public struct UIElementTheme
{
	[JsonProperty("prototype", Required = Required.Default)]
	public string? Prototype;

	[JsonProperty("colour", Required = Required.Default)]
	public Dictionary<string, string>? Colour;

	[JsonProperty("font", Required = Required.Default)]
	public Dictionary<string, string>? Font;

	[JsonProperty("misc", Required = Required.Default)]
	public Dictionary<string, dynamic>? Misc;
	
	public override string ToString()
	{
		StringBuilder representation = new();
		if (Prototype != null) { representation.Append($"Prototype: {Prototype}\n\t"); }
		if (Colour != null) 
		{
			foreach (var item in Colour)
			{ 
				representation.Append($"{item.Key}: {item.Value}\n\t"); 
			}
		}
		if (Font != null) 
		{
			foreach (var item in Font)
			{
				representation.Append($"{item.Key}: {item.Value}\n\t");
			}
		}
		if (Misc != null) 
		{
			foreach (var item in Misc)
			{
				representation.Append($"{item.Key}: {item.Value}\n\t");
			}
		}
		return representation.ToString();
	}
}
