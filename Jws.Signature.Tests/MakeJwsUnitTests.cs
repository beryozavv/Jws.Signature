using Jws.Signature.Jws;
using Jws.Signature.Signing;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Jws.Signature.Tests;

public class MakeJwsUnitTests
{
    private readonly ITestOutputHelper _outputHelper;

    public MakeJwsUnitTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    private ISignDataService GetSignDataServiceMock()
    {
        var mock = new Mock<ISignDataService>();
        mock.Setup(sds => sds.SignData(It.IsAny<string>())).Returns("testSign001");

        return mock.Object;
    }

    private IVerifySignService GetVerifySignServiceMock()
    {
        var mock = new Mock<IVerifySignService>();
        mock.Setup(vfs => vfs.VerifySign(It.IsAny<string>(), "testSign001")).Returns(true);

        return mock.Object;
    }

    [Fact]
    public void MakeJwsTest()
    {
        var testEntity = new TestEntity { Id = 1, Name = "test entity 1", Description = "first test" };

        var makeJwsService = new MakeJwsService(GetSignDataServiceMock());

        var jws = makeJwsService.MakeJws(testEntity, "RS256");

        _outputHelper.WriteLine(jws);

        var parseJwsService = new ParseJwsService(GetVerifySignServiceMock());

        var parsedEntity = parseJwsService.ParseJws<TestEntity>(jws);

        Assert.Equal(testEntity.Id, parsedEntity.Id);
        Assert.Equal(testEntity.Name, parsedEntity.Name);
        Assert.Equal(testEntity.Description, parsedEntity.Description);

        _outputHelper.WriteLine(parsedEntity.ToString());
    }
}