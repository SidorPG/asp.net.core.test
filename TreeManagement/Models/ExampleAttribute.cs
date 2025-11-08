namespace Api.Models;

[AttributeUsage(AttributeTargets.Property)]
public class ExampleAttribute : Attribute
{
    public string Text { get; }
    public ExampleAttribute(string text)
    {
        Text = text;
    }
}