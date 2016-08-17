using System;
using NUnit.Framework;
using Rhino.Mocks;

namespace Skahal.Infrastructure.Framework.Repositories.UnitTests
{
    [TestFixture]
    public class RepositoryBaseTest
    {
        [Test]
        public void PersistDeletedItem_Null_Exception()
        {
            var target = MockRepository.GeneratePartialMock<RepositoryBase<EntityUserStub>>();

            Assert.ThrowsAsync<ArgumentNullException>(async () => await target.PersistDeletedItemAsync(null));
        }

        [Test]
        public void PersistNewItem_Null_Exception()
        {
            var target = MockRepository.GeneratePartialMock<RepositoryBase<EntityUserStub>>();

            Assert.ThrowsAsync<ArgumentNullException>(async () => await target.PersistNewItemAsync(null));
        }

        [Test]
        public void PersistUpdatedItem_Null_Exception()
        {
            var target = MockRepository.GeneratePartialMock<RepositoryBase<EntityUserStub>>();

            Assert.ThrowsAsync<ArgumentNullException>(async () => await target.PersistUpdatedItemAsync(null));
        }
    }
}