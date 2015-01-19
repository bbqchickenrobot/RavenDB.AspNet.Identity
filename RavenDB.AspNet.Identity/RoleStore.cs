using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Raven.Abstractions;
using Raven.Client;
using Raven.Client.Connection.Async;

namespace RavenDB.AspNet.Identity
{
    public class RoleStore<TRole> : IRoleStore<TRole> where TRole : IRole<string>
    {
        protected static class Ensure
        {
            public static void IsNotNull(object arg, string name = "role")
            {
                throw new ArgumentNullException(name, "Argument cannot be null");
            }
        }
        private bool _disposed;
        private Func<IDocumentSession> getSessionFunc;
        private IDocumentSession _session;

        private IDocumentSession session
        {
            get
            {
                if (_session == null)
                {
                    _session = getSessionFunc();
                    // todo - figure out if ther eis a version for roles with line below
                    //_session.Advanced.DocumentStore.Conventions.RegisterIdConvention<IdentityUser>((dbname, commands, user) => "IdentityUsers/" + user.Id);
                }
                return _session;
            }
        }

        public RoleStore(Func<IDocumentSession> getSession)
        {
            this.getSessionFunc = getSession;
        }

        public RoleStore(IDocumentSession session)
        {
            Ensure.IsNotNull(session, "session");
            this._session = session;
        }

        private void ThrowIfDisposed()
        {
            if (this._disposed)
                throw new ObjectDisposedException(this.GetType().Name);
        }

        public void Dispose()
        {
            this._disposed = true;
        }

        public Task CreateAsync(TRole role)
        {
            this.ThrowIfDisposed();
            Ensure.IsNotNull(role);
            this.session.Store(role);
            return Task.FromResult(true);
        }

        public Task UpdateAsync(TRole role)
        {
            Ensure.IsNotNull(role);
            ThrowIfDisposed();
            return Task.FromResult(true);
        }

        public Task DeleteAsync(TRole role)
        {
            this.ThrowIfDisposed();
            Ensure.IsNotNull(role);
            //var userByName = this.session.Load<TRole>(role.Id);
            //if (userByName != null)
            //    this.session.Delete(userByName);
            // todo - remove roles from users as well....
            // todo - check if users are in roll and throw exception....e tc...
            this.session.Delete(role);
            return Task.FromResult(true);
        }

        public Task<TRole> FindByIdAsync(string roleId)
        {
            Ensure.IsNotNull(roleId, "roleId");
            return  Task.FromResult(session.Load<TRole>(roleId));
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            Ensure.IsNotNull(roleName);
            return session.Query<TRole>().FirstOrDefaultAsync(r=>r.Name==roleName);
        }
    }
}
