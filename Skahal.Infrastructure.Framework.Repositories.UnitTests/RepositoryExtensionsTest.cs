using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Skahal.Infrastructure.Framework.Repositories.UnitTests
{
    [TestFixture()]
    public class RepositoryExtensionsTest
    {
        #region Fields
        private MemoryRepository<EntityUserStub> m_userRepository;
        private IUnitOfWork m_unitOfWork = new MemoryUnitOfWork();
        #endregion

        [SetUp]
        public void InitializeFixture()
        {
            m_userRepository = new MemoryRepository<EntityUserStub>(m_unitOfWork, (u) => { return Guid.NewGuid().ToString(); });
        }

        #region FindLast
        [Test()]
        public async Task FindLast_NoEntities_Null()
        {
            Assert.IsNull(await m_userRepository.FindLast());
        }

        [Test()]
        public async Task FindLast_OnlyOneEntity_TheOneEntity()
        {
            var entity = new EntityUserStub() { Name = "1" };
            m_userRepository.Add(entity);
            await m_unitOfWork.Commit();

            Assert.AreEqual("1", (await m_userRepository.FindLast()).Name);
        }

        [Test()]
        public async Task FindLast_ManyEntities_LastOne()
        {
            var entity = new EntityUserStub() { Key = "1" };
            m_userRepository.Add(entity);

            entity = new EntityUserStub() { Key = "2" };
            m_userRepository.Add(entity);

            entity = new EntityUserStub() { Key = "3" };
            m_userRepository.Add(entity);

            await m_unitOfWork.Commit();

            Assert.AreEqual("3", (await m_userRepository.FindLast()).Key);
        }
        #endregion

        #region FindFirst
        [Test()]
        public async Task FindFirst_NoMatch_Null()
        {
            var entity = new EntityUserStub() { Name = "1" };
            m_userRepository.Add(entity);
            await m_unitOfWork.Commit();

            Assert.IsNull(await m_userRepository.FindFirst((u) => u.Name.Equals("2")));
        }

        [Test()]
        public async Task FindFirst_Match_Entity()
        {
            m_userRepository.Add(new EntityUserStub() { Name = "1" });
            m_userRepository.Add(new EntityUserStub() { Name = "2" });
            m_userRepository.Add(new EntityUserStub() { Name = "3" });
            await m_unitOfWork.Commit();

            Assert.AreEqual("2", (await m_userRepository.FindFirst((u) => u.Name.Equals("2"))).Name);
        }

        [Test()]
        public async Task FindFirstAscending_Match_EntityOrdered()
        {
            m_userRepository.Add(new EntityUserStub() { Name = "3" });
            m_userRepository.Add(new EntityUserStub() { Name = "1" });
            m_userRepository.Add(new EntityUserStub() { Name = "2" });
            await m_unitOfWork.Commit();

            Assert.AreEqual("1", (await m_userRepository.FindFirstAscending((f) => true, (o) => o.Name)).Name);
            Assert.AreEqual("2", (await m_userRepository.FindFirstAscending((f) => f.Name == "2", (o) => o.Name)).Name);
        }

        [Test()]
        public async Task FindFirstDescending_Match_EntityOrdered()
        {
            m_userRepository.Add(new EntityUserStub() { Name = "1" });
            m_userRepository.Add(new EntityUserStub() { Name = "3" });
            m_userRepository.Add(new EntityUserStub() { Name = "2" });
            await m_unitOfWork.Commit();

            Assert.AreEqual("3", (await m_userRepository.FindFirstDescending((f) => true, (o) => o.Name)).Name);
            Assert.AreEqual("2", (await m_userRepository.FindFirstDescending((f) => f.Name == "2", (o) => o.Name)).Name);
        }
        #endregion

        #region FindAllAscending
        [Test()]
        public async Task FindAllAscending_Match_OrderedEntities()
        {
            m_userRepository.Add(new EntityUserStub() { Name = "3" });
            m_userRepository.Add(new EntityUserStub() { Name = "1" });
            m_userRepository.Add(new EntityUserStub() { Name = "2" });
            await m_unitOfWork.Commit();

            var actual = (await m_userRepository.FindAllAscending((o) => o.Name)).ToList();
            Assert.AreEqual(3, actual.Count);
            Assert.AreEqual("1", actual[0].Name);
            Assert.AreEqual("2", actual[1].Name);
            Assert.AreEqual("3", actual[2].Name);

            actual = (await m_userRepository.FindAllAscending((f) => f.Name == "2", (o) => o.Name)).ToList();
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("2", actual[0].Name);

            actual = (await m_userRepository.FindAllAscending(1, 2, (o) => o.Name)).ToList();
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("2", actual[0].Name);
            Assert.AreEqual("3", actual[1].Name);

            actual = (await m_userRepository.FindAllAscendingAsync(0, 2, (f) => f.Name == "2", (o) => o.Name)).ToList();
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("2", actual[0].Name);
        }
        #endregion

        #region FindAllDescending
        [Test()]
        public async Task FindAllDescending_Match_OrderedEntities()
        {
            m_userRepository.Add(new EntityUserStub() { Name = "3" });
            m_userRepository.Add(new EntityUserStub() { Name = "1" });
            m_userRepository.Add(new EntityUserStub() { Name = "2" });
            await m_unitOfWork.Commit();

            var actual = (await m_userRepository.FindAllDescending((o) => o.Name)).ToList();
            Assert.AreEqual(3, actual.Count);
            Assert.AreEqual("3", actual[0].Name);
            Assert.AreEqual("2", actual[1].Name);
            Assert.AreEqual("1", actual[2].Name);

            actual = (await m_userRepository.FindAllDescending((f) => f.Name == "2", (o) => o.Name)).ToList();
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("2", actual[0].Name);

            actual = (await m_userRepository.FindAllDescending(1, 2, (o) => o.Name)).ToList();
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("2", actual[0].Name);
            Assert.AreEqual("1", actual[1].Name);

            actual = (await m_userRepository.FindAllDescendingAsync(0, 2, (f) => f.Name == "2", (o) => o.Name)).ToList();
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("2", actual[0].Name);
        }
        #endregion
    }
}
