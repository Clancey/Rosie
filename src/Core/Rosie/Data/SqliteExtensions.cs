using System;
using System.Threading.Tasks;
using System.Collections;
namespace SQLite
{
	public static class SqliteExtensions
	{
		public static Task<int> InsertAsync(this SQLiteAsyncConnection connection, object obj, Type objType)
 		{
 			return Task.Factory.StartNew(() => {
				var conn = connection.GetConnection();
 				using (conn.Lock ()) {
 					return conn.Insert(obj, objType);
 				}
 			});
 		}

		public static Task<int> InsertAllAsync(this SQLiteAsyncConnection connection, IEnumerable items, string extra)
		{
			return Task.Factory.StartNew(() => {
				var conn = connection.GetConnection();
				using (conn.Lock())
				{
					return conn.InsertAll(items, extra);
				}
			});
		}

		public static Task<int> InsertOrReplaceAllAsync(this SQLiteAsyncConnection connection, IEnumerable items)
		{
			return  connection.InsertAllAsync(items, "OR REPLACE");
		}
	}
}
