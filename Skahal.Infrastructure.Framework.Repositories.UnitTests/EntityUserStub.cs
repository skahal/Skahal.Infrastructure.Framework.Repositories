using System;
using Skahal.Infrastructure.Framework.Domain;

namespace Skahal.Infrastructure.Framework.Repositories.UnitTests
{
    public class EntityUserStub : EntityBase, IAggregateRoot
    {
        public string Name
        {
            get;
            set;
        }

        public EntityUserStub(object key) : base(key)
        {
        }

        public EntityUserStub()
        {
        }

    }
}

