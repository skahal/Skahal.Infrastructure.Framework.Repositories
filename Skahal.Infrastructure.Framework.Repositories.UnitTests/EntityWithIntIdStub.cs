using System;
using Skahal.Infrastructure.Framework.Domain;

namespace Skahal.Infrastructure.Framework.Repositories.UnitTests
{
    public class EntityWithIntIdStub : EntityWithIdBase<int>, IAggregateRoot
    {
    }
}

