using System.Collections.Generic;
using System.Linq;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Tests;
using NUnit.Framework;

namespace Nop.Services.Tests.Catalog
{
    [TestFixture]
    public class AdvancedPriceExtensionTests : ServiceTest
    {
        [SetUp]
        public new void SetUp()
        {

        }

        [Test]
        public void Can_remove_duplicatedQuantities()
        {
            var advancedPrices = new List<AdvancedPrice>();
            advancedPrices.Add(new AdvancedPrice
            {
                //will be removed
                Id = 1,
                Price = 150,
                Quantity = 1
            });
            advancedPrices.Add(new AdvancedPrice
            {
                //will stay
                Id = 2,
                Price = 100,
                Quantity = 1
            });
            advancedPrices.Add(new AdvancedPrice
            {
                //will stay
                Id = 3,
                Price = 200,
                Quantity = 3
            });
            advancedPrices.Add(new AdvancedPrice
            {
                //will stay
                Id = 4,
                Price = 250,
                Quantity = 4
            });
            advancedPrices.Add(new AdvancedPrice
            {
                //will be removed
                Id = 5,
                Price = 300,
                Quantity = 4
            });
            advancedPrices.Add(new AdvancedPrice
            {
                //will stay
                Id = 6,
                Price = 350,
                Quantity = 5
            });

            advancedPrices.RemoveDuplicatedQuantities();

            advancedPrices.FirstOrDefault(x => x.Id == 1).ShouldBeNull();
            advancedPrices.FirstOrDefault(x => x.Id == 2).ShouldNotBeNull();
            advancedPrices.FirstOrDefault(x => x.Id == 3).ShouldNotBeNull();
            advancedPrices.FirstOrDefault(x => x.Id == 4).ShouldNotBeNull();
            advancedPrices.FirstOrDefault(x => x.Id == 5).ShouldBeNull();
            advancedPrices.FirstOrDefault(x => x.Id == 6).ShouldNotBeNull();
        }
    }
}
