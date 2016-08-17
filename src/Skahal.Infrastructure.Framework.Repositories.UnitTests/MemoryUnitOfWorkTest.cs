using System.Threading.Tasks;
using NUnit.Framework;
using Rhino.Mocks;

namespace Skahal.Infrastructure.Framework.Repositories.UnitTests
{
    [TestFixture]
    public class MemoryUnitOfWorkTest
    {
        [Test]
        public async Task RegisterAdded_Commit_Added()
        {
            var target = new MemoryUnitOfWork();
            var user1 = new EntityUserStub("1");
            var repository = MockRepository.GenerateStrictMock<IUnitOfWorkRepository>();
            repository.Expect(r => r.PersistNewItemAsync(user1)).Return(Task.Delay(0));

            target.RegisterAdded(user1, repository);

            await target.CommitAsync();

            repository.VerifyAllExpectations();
        }

        [Test]
        public async Task RegisterChanged_Commit_Updated()
        {
            var target = new MemoryUnitOfWork();
            var user1 = new EntityUserStub("1");
            var repository = MockRepository.GenerateStrictMock<IUnitOfWorkRepository>();
            repository.Expect(r => r.PersistUpdatedItemAsync(user1)).Return(Task.Delay(0));

            target.RegisterChanged(user1, repository);

            await target.CommitAsync();

            repository.VerifyAllExpectations();
        }

        [Test]
        public async Task RegisterRemoved_Commit_Deleted()
        {
            var target = new MemoryUnitOfWork();
            var user1 = new EntityUserStub("1");
            var repository = MockRepository.GenerateStrictMock<IUnitOfWorkRepository>();
            repository.Expect(r => r.PersistDeletedItemAsync(user1)).Return(Task.Delay(0));

            target.RegisterRemoved(user1, repository);

            await target.CommitAsync();

            repository.VerifyAllExpectations();
        }

        [Test]
        public async Task Commit_EntitiesToAddChangeAndDelete_RightCommit()
        {
            var target = new MemoryUnitOfWork();

            var userToDelete = new EntityUserStub("1");
            var userToAdd = new EntityUserStub("2");
            var userToUpdate = new EntityUserStub("3");

            var repository = MockRepository.GenerateStrictMock<IUnitOfWorkRepository>();
            repository.Expect(r => r.PersistDeletedItemAsync(userToDelete)).Return(Task.Delay(0));
            repository.Expect(r => r.PersistNewItemAsync(userToAdd)).Return(Task.Delay(0));
            repository.Expect(r => r.PersistUpdatedItemAsync(userToUpdate)).Return(Task.Delay(0));

            target.RegisterRemoved(userToDelete, repository);
            target.RegisterAdded(userToAdd, repository);
            target.RegisterChanged(userToUpdate, repository);

            await target.CommitAsync();

            repository.VerifyAllExpectations();
        }

        [Test]
        public async Task Rollback_EntitiesToAddChangeAndDelete_Undo()
        {
            var target = new MemoryUnitOfWork();

            var userToDelete = new EntityUserStub("1");
            var userToAdd = new EntityUserStub("2");
            var userToUpdate = new EntityUserStub("3");

            var repository = MockRepository.GenerateStrictMock<IUnitOfWorkRepository>();

            target.RegisterRemoved(userToDelete, repository);
            target.RegisterAdded(userToAdd, repository);
            target.RegisterChanged(userToUpdate, repository);

            target.Rollback();
            await target.CommitAsync();

            repository.VerifyAllExpectations();
        }
    }
}
