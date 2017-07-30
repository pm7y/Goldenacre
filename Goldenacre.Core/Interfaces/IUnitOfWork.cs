// ReSharper disable CheckNamespace

namespace Goldenacre.Core
{
    using System;

    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}