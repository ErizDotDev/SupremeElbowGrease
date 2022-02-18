using DbUp;
using DbUp.Helpers;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace QLess.DbScriptRunner
{
	public class Migrations
	{
		private string _connectionString;

		public Migrations(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("QLessDbConnection");
		}

		public bool RunScripts()
		{
			try
			{
				var upgrader = DeployChanges.To
					.SqlDatabase(_connectionString)
					.WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(),
						(string s) => s.StartsWith("QLess.DbScriptRunner.Scripts.Script"))
					.JournalToSqlTable("dbo", "_script_migration_")
					.LogToConsole()
					.Build();

				var result = upgrader.PerformUpgrade();

				return result.Successful;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool RunReset()
		{
			try
			{
				var resetScriptRunner = DeployChanges.To
					.SqlDatabase(_connectionString)
					.WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(),
						(string s) => s.StartsWith("QLess.DbScriptRunner.Scripts.Reset"))
					.JournalTo(new NullJournal())
					.LogToConsole()
					.Build();

				var result = resetScriptRunner.PerformUpgrade();

				return result.Successful;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
