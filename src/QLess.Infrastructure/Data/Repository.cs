using Dapper.Contrib.Extensions;
using Microsoft.Extensions.Configuration;
using QLess.Core.Interface;
using System.Data;
using System.Data.SqlClient;

namespace QLess.Infrastructure.Data
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		private readonly string _connectionString;
		internal IDbConnection dbConnection;

		public Repository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("QLessDbConnection");
			dbConnection = new SqlConnection(_connectionString);
		}

		public async Task<bool> CreateAsync(TEntity entity)
		{
			if (entity == null)
				return false;

			dbConnection.Open();

			try
			{
				var inserted = await dbConnection.InsertAsync<TEntity>(entity);
				return inserted > 0;
			}
			finally
			{
				dbConnection.Close();
			}
		}

		public bool Create(TEntity entity, out long id)
		{
			id = 0;

			if (entity == null)
			{
				id = 0;
				return false;
			}

			dbConnection.Open();

			try
			{
				id = SqlMapperExtensions.Insert<TEntity>(dbConnection, entity);
				return id > 0;
			}
			finally
			{
				dbConnection.Close();
			}
		}

		public async Task<bool> DeleteAsync(long id)
		{
			dbConnection.Open();

			try
			{
				var entity = await dbConnection.GetAsync<TEntity>(id);

				if (entity == null)
					return false;

				return await dbConnection.DeleteAsync(entity);
			}
			finally
			{
				dbConnection.Close();
			}
		}

		public async Task<IQueryable<TEntity>> FindAllAsync()
		{
			dbConnection.Open();

			try
			{
				var results = await dbConnection.GetAllAsync<TEntity>();
				return results.AsQueryable();
			}
			finally
			{
				dbConnection.Close();
			}
		}

		public async Task<TEntity> FindByIdAsync(long id)
		{
			dbConnection.Open();

			try
			{
				return await dbConnection.GetAsync<TEntity>(id);
			}
			finally
			{
				dbConnection.Close();
			}
		}

		public async Task<bool> UpdateAsync(TEntity entity)
		{
			if (entity == null)
				return false;

			dbConnection.Open();

			try
			{
				return await dbConnection.UpdateAsync<TEntity>(entity);
			}
			finally
			{
				dbConnection.Close();
			}
		}
	}
}
