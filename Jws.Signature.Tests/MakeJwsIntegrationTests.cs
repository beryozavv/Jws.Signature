using Jws.Signature.Jws;
using Jws.Signature.KeyExtractor;
using Jws.Signature.Signing;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Jws.Signature.Tests;

public class MakeJwsIntegrationTests
{
    private readonly ITestOutputHelper _outputHelper;

    public MakeJwsIntegrationTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    private ISignDataService GetSignDataService()
    {
        var signDataRsaService = new SignDataRsaService(new DefaultPrivateKeyExtractor());
        return signDataRsaService;
    }

    private IVerifySignService GetVerifySignService()
    {
        var verifySignService = new VerifySignService(new DefaultPublicKeyExtractor());
        return verifySignService;
    }

    [Fact]
    public void MakeJwsTest()
    {
        var testEntity = new TestEntity { Id = 2, Name = "test entity 22", Description = "second test" };

        var makeJwsService = new MakeJwsService(GetSignDataService());

        var jws = makeJwsService.MakeJws(testEntity, "RS256");

        _outputHelper.WriteLine(jws);

        var parseJwsService = new ParseJwsService(GetVerifySignService());

        var parsedEntity = parseJwsService.ParseJws<TestEntity>(jws);

        Assert.Equal(testEntity.Id, parsedEntity.Id);
        Assert.Equal(testEntity.Name, parsedEntity.Name);
        Assert.Equal(testEntity.Description, parsedEntity.Description);

        _outputHelper.WriteLine(parsedEntity.ToString());
    }
}