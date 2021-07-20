using Xunit;
using Xunit.Abstractions;
using YQTrack.Backend.Standard.LanguageHelper;

namespace XUnitTest
{
    public class UnitTestDefault
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public UnitTestDefault(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Test()
        {
            var text = LanguageManage.GetText("zh-cn", LanguageType.GlobalWDCountry, "9802", "_name");
            _testOutputHelper.WriteLine(text);
        }
    }
}
