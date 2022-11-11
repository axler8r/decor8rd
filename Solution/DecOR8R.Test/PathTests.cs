using DecOR8R.Component.Path;

namespace DecOR8R.Test;

public class PathTests
{
    [Fact]
    public void PathDecorationTest()
    {
        const string EXPECTED_ = "";
        const string INPUT_ = "";
        Assert.Equal(EXPECTED_, Implementation.Instance.Decorate(INPUT_));
    }
}
