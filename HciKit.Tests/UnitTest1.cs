namespace HciKit.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Class1 c1 = new Class1();

        Assert.Equal(3, c1.Add(1, 2));
    }
}
