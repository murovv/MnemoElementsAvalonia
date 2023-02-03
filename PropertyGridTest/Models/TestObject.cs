using AvAp2;

namespace PropertyGridTest.Models;

public class TestObject
{
    private string _stringProp = "hi";
    [PropertyGridFilter]
    public string StringProp
    {
        get => _stringProp;
        set { _stringProp = value; }
    } 
    [PropertyGridFilter]
    public int IntProp { get; set; }
}