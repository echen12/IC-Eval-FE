using Microsoft.AspNetCore.Components.Forms;

public class FormField
{
    public string Type { get; set; }
    public string Label { get; set; }
    public bool Required { get; set; }
    public IEnumerable<string> Values { get; set; } 
    public int Min { get; set; }
    public int Max { get; set; }
}

public class FormModel
{
    public string Title { get; set; }
    public List<FormField> Fields { get; set; } = new List<FormField>();
}