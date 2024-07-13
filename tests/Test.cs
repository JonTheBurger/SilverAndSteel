namespace Tests;

using GdUnit4;
using static GdUnit4.Assertions;

[TestSuite]
public class ExampleTest
{
    [TestCase]
    public void Success()
    {
        AssertBool(true).IsTrue();
    }
}