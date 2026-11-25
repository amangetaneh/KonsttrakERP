using ERP.Models;
using ERP.Services.AssetNumberServices;

namespace ERP.Tests
{
    public class AssetNumberTests
    {
        [Fact]
        public async Task GeneratesUniqueNumbersAndContinuesSavedSequence()
        {
            using var context = TestContext.Create();
            context.Items.Add(new Item { ItemId = 1, Name = "Drill 42" });
            context.Miscellaneouses.Add(new Miscellaneous { Key = MICELLANEOUSKEYS.COMPANYPREFIX, Value = "DEMO" });
            await context.SaveChangesAsync();
            var service = new AssetNumberService(context);
            var first = await service.GenerateAssetNumbers(1, 2);
            var second = await service.GenerateAssetNumbers(1, 1);
            Assert.Equal(new[] { "DEMODR0000001", "DEMODR0000002" }, first);
            Assert.Equal("DEMODR0000003", Assert.Single(second));
            Assert.Equal(3, context.AssetNumberIds.Single().LastId);
        }

        [Fact]
        public async Task FallsBackToUnusedPrefixForAnotherItem()
        {
            using var context = TestContext.Create();
            context.Items.AddRange(new Item { ItemId = 1, Name = "Demo" }, new Item { ItemId = 2, Name = "Demo" });
            await context.SaveChangesAsync();
            var service = new AssetNumberService(context);
            var first = await service.GenerateAssetNumbers(1, 1);
            var second = await service.GenerateAssetNumbers(2, 1);
            Assert.NotEqual(first[0], second[0]);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task RejectsNonPositiveCountsWithoutSaving(int count)
        {
            using var context = TestContext.Create();
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => new AssetNumberService(context).GenerateAssetNumbers(1, count));
            Assert.Empty(context.AssetNumberIds);
        }

        [Fact]
        public async Task RejectsMissingOrUnusableItems()
        {
            using var context = TestContext.Create();
            var service = new AssetNumberService(context);
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GenerateAssetNumbers(1, 1));
            context.Items.Add(new Item { ItemId = 1, Name = "123 !!!" });
            await context.SaveChangesAsync();
            await Assert.ThrowsAsync<ArgumentException>(() => service.GenerateAssetNumbers(1, 1));
        }

        [Fact]
        public void FallbackAlphabetHasAllLettersExactlyOnce()
        {
            Assert.Equal("ABCDEFGHIJKLMNOPQRSTUVWXYZ", string.Concat(PrefixGenerator.GetCombinations(1)));
        }
    }
}
