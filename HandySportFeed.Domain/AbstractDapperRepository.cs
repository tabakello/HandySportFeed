using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using DapperExtensions;
using HandySportFeed.Domain.Model;
using System.Configuration;

namespace HandySportFeed.Domain
{
    public abstract class AbstractDapperRepository<T> : IRepository<T> where T : EntityBase
    {
        protected readonly string TableName;
        private readonly string _connectionStringName;

        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString);
            }
        }

        protected AbstractDapperRepository(string tableName, string connectionStringName)
        {
            TableName = tableName;
            _connectionStringName = connectionStringName;
        }

        internal virtual dynamic Mapping(T item)
        {
            return item;
        }

        public virtual object Add(T item)
        {
            using (var cn = Connection)
            {
                cn.Open();
                var t = cn.Insert(item);
                cn.Close();
                return t;
            }
        }

        public virtual void Update(T item)
        {
            using (var cn = Connection)
            {
                cn.Open();
                cn.Update(item);
                cn.Close();
            }
        }

        public virtual void Remove(T item)
        {
            using (var cn = Connection)
            {
                cn.Open();
                cn.Execute("DELETE FROM " + TableName + " WHERE ID=@ID", new { ID = item.Id });
                cn.Close();
            }
        }

        public virtual T FindById(int id)
        {
            T item;

            using (var cn = Connection)
            {
                cn.Open();
                item = cn.Get<T>(id);
                cn.Close();
            }

            return item;
        }

        IEnumerable<T> IRepository<T>.FindAll()
        {
            return FindAll();
        }

        public virtual IEnumerable<T> FindAll()
        {
            IEnumerable<T> items;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                items = cn.Query<T>("SELECT * FROM " + TableName);
                cn.Close();
            }

            return items;
        }
    }
}
