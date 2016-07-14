using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using TestSharp;

namespace Skahal.Infrastructure.Framework.Repositories.UnitTests
{
    [TestFixture()]
    public class MemoryRepositoryTest
    {
        #region Fields
        private MemoryUnitOfWork m_unitOfWork;
        private MemoryRepository<EntityUserStub> m_target;
        #endregion

        #region Initialize
        [SetUp]
        public void InitializeTest()
        {
            m_unitOfWork = new MemoryUnitOfWork();
            m_target = new MemoryRepository<EntityUserStub>(m_unitOfWork, (u) => { return Guid.NewGuid().ToString(); });
        }
        #endregion

        #region Tests
        [Test()]
        public async Task FindAll_Filter_EntitiesFiltered()
        {
            m_target.Add(new EntityUserStub() { });

            var user = new EntityUserStub();
            m_target.Add(user);
            m_target.Add(new EntityUserStub() { });
            m_target.Add(new EntityUserStub() { });
            await m_unitOfWork.Commit();

            var result = await m_target.FindAll(f => f.Key == user.Key);
            var actual = result.ToList();
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(user.Key, actual[0].Key);
        }

        [Test()]
        public void FindAllAscending_NullOrder_ArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await m_target.FindAllAscendingAsync<int>(0, 1, (o) => o.Name == null, null));
        }

        [Test()]
        public async Task FindAllAscending_FilterAndOrder_EntitiesFilteredAndOrdered()
        {
            m_target.Add(new EntityUserStub() { Name = "B" });
            m_target.Add(new EntityUserStub() { Name = "C" });
            m_target.Add(new EntityUserStub() { Name = "A" });
            await m_unitOfWork.Commit();


            var result = await m_target.FindAllAscendingAsync(0, 3, (f) => true, (o) => o.Name);
            var actual = result.ToList();
            Assert.AreEqual(3, actual.Count);
            Assert.AreEqual("A", actual[0].Name);
            Assert.AreEqual("B", actual[1].Name);
            Assert.AreEqual("C", actual[2].Name);

            result = await m_target.FindAllAscendingAsync(1, 3, (f) => true, (o) => o.Name);
            actual = result.ToList();
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("B", actual[0].Name);
            Assert.AreEqual("C", actual[1].Name);

            result = await m_target.FindAllAscendingAsync(0, 2, (f) => true, (o) => o.Name);
            actual = result.ToList();
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("A", actual[0].Name);
            Assert.AreEqual("B", actual[1].Name);
        }

        [Test()]
        public void FindAllDescending_NullOrder_ArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await m_target.FindAllDescendingAsync<int>(0, 1, (o) => o.Name == null, null));
        }

        [Test()]
        public async Task FindAllDescending_FilterAndOrder_EntitiesFilteredAndOrdered()
        {
            m_target.Add(new EntityUserStub() { Name = "B" });
            m_target.Add(new EntityUserStub() { Name = "C" });
            m_target.Add(new EntityUserStub() { Name = "A" });
            await m_unitOfWork.Commit();

            var result = await m_target.FindAllDescendingAsync(0, 3, (f) => true, (o) => o.Name);
            var actual = result.ToList();

            Assert.AreEqual(3, actual.Count);
            Assert.AreEqual("C", actual[0].Name);
            Assert.AreEqual("B", actual[1].Name);
            Assert.AreEqual("A", actual[2].Name);

            result = await m_target.FindAllDescendingAsync(1, 3, (f) => true, (o) => o.Name);
            actual = result.ToList();

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("B", actual[0].Name);
            Assert.AreEqual("A", actual[1].Name);

            result = await m_target.FindAllDescendingAsync(0, 2, (f) => true, (o) => o.Name);
            actual = result.ToList();

            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual("C", actual[0].Name);
            Assert.AreEqual("B", actual[1].Name);
        }

        [Test()]
        public async Task CountAll_Filter_EntitiesFiltered()
        {
            m_target.Add(new EntityUserStub() { });
            m_target.Add(new EntityUserStub() { });
            m_target.Add(new EntityUserStub() { });
            m_target.Add(new EntityUserStub() { });
            await m_unitOfWork.Commit();

            var actual = await m_target.CountAllAsync(f => true);
            Assert.AreEqual(4, actual);
        }

        [Test()]
        public void Add_NullEntity_ArgumentNullException()
        {
            ExceptionAssert.IsThrowing(new ArgumentNullException("item"), () =>
            {
                m_target.Add(null);
            });
        }

        [Test()]
        public async Task Add_EntityAlreadyAdded_Exception()
        {
            var user = new EntityUserStub("1");
            m_target.Add(user);
            m_target.Add(user);

            Assert.ThrowsAsync<InvalidOperationException>(async () => await m_unitOfWork.Commit());

            var result = await m_target.FindAll(f => true);
            var actual = result.ToList();

            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual("1", actual[0].Key);
        }

        [Test()]
        public async Task Add_Entity_Added()
        {
            m_target.Add(new EntityUserStub());
            await m_unitOfWork.Commit();

            var result = await m_target.FindAll(f => true);
            var actual = result.ToList();

            Assert.AreEqual(1, actual.Count);
            Assert.IsFalse(String.IsNullOrWhiteSpace((string)actual[0].Key));

            m_target.Add(new EntityUserStub());
            await m_unitOfWork.Commit();

            var result2 = await m_target.FindAll(f => true);
            var actual2 = result2.ToList();
            Assert.AreEqual(2, actual2.Count);
        }

        [Test()]
        public async Task Add_EntityWithIntId_AddedWithNewId()
        {
            var target = new MemoryRepository<EntityWithIntIdStub>((e) => 1);
            target.SetUnitOfWork(m_unitOfWork);
            target.Add(new EntityWithIntIdStub());
            await m_unitOfWork.Commit();

            var result = await target.FindAll(f => true);
            var actual = result.ToList();

            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(1, actual[0].Id);
        }

        [Test()]
        public void Delete_NullEntity_ArgumentNullException()
        {
            ExceptionAssert.IsThrowing(new ArgumentNullException("item"), () =>
            {
                m_target.Remove(null);
            });
        }

        [Test()]
        public void Delete_EntityWithIdDoesNotExists_ArgumentNullException()
        {
            var user = new EntityUserStub() { };
            m_target.Remove(user);

            Assert.ThrowsAsync<InvalidOperationException>(async () => await m_unitOfWork.Commit());
        }

        [Test()]
        public async Task Delete_Entity_EntityDeleted()
        {
            var user = new EntityUserStub() { };
            m_target.Add(user);
            await m_unitOfWork.Commit();

            m_target.Remove(user);
            await m_unitOfWork.Commit();

            var result = await m_target.FindAll(f => true);
            var actual = result.ToList();
            Assert.AreEqual(0, actual.Count);
        }

        [Test()]
        public async Task Modify_EntityWithIdDoesNotExist_Added()
        {
            var user = new EntityUserStub();
            Assert.IsNull(user.Key);

            await m_target.Attach(user);

            await m_unitOfWork.Commit();

            Assert.IsNotNull(user.Key);

            var result = await m_target.FindAll(f => true);
            var actual = result.ToList();
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(user.Key, actual[0].Key);
        }

        [Test()]
        public async Task Modify_Entity_EntityModified()
        {
            var user = new EntityUserStub();
            m_target.Add(user);
            user = new EntityUserStub();
            m_target.Add(user);
            await m_unitOfWork.Commit();

            var modifiedUser = new EntityUserStub(user.Key) { Name = "new name" };
            await m_target.Attach(modifiedUser);

            await m_unitOfWork.Commit();

            var result = await m_target.FindAll(f => f == user);
            var actual = result.FirstOrDefault();
            Assert.AreEqual(user.Key, actual.Key);
            Assert.AreEqual("new name", actual.Name);
        }
        #endregion
    }
}