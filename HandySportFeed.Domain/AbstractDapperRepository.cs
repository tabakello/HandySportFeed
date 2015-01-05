using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using DapperExtensions;
using HandySportFeed.Domain.Model;

namespace HandySportFeed.Domain {
    public abstract class AbstractDapperRepository<T> : IRepository<T> where T : EntityBase {
        private readonly string tableName;
        private readonly string connectionStringName;

        protected AbstractDapperRepository(string tableName, string connectionStringName) {
            this.tableName = tableName;
            this.connectionStringName = connectionStringName;
        }

        internal IDbConnection Connection {
            get { return new SqlConnection(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString); }
        }

        public virtual object Add(T item) {
            using (var cn = Connection) {
                cn.Open();
                var t = cn.Insert(item);
                cn.Close();
                return t;
            }
        }

        public virtual void Update(T item) {
            using (var cn = Connection) {
                cn.Open();
                cn.Update(item);
                cn.Close();
            }
        }

        public virtual void Remove(T item) {
            using (var cn = Connection) {
                cn.Open();
                cn.Execute("DELETE FROM " + tableName + " WHERE ID=@ID", new { ID = item.Id });
                cn.Close();
            }
        }

        public virtual T FindById(int id) {
            T item;

            using (var cn = Connection) {
                cn.Open();
                item = cn.Get<T>(id);
                cn.Close();
            }

            return item;
        }

        IEnumerable<T> IRepository<T>.FindAll() {
            return FindAll();
        }

        internal virtual dynamic Mapping(T item) {
            return item;
        }

        public virtual IEnumerable<T> FindAll() {
            IEnumerable<T> items;

            using (var cn = Connection) {
                cn.Open();
                items = cn.Query<T>("SELECT * FROM " + tableName);
                cn.Close();
            }

            return items;
        }
    }
}