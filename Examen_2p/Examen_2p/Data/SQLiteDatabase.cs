using Examen_2p.Extensions;
using Examen_2p.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Examen_2p.Data
{
    public class SQLiteDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() => {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Connection => lazyInitializer.Value;

        static bool IsInitialized = false;

        async Task InitializeAsync()
        {
            if (!IsInitialized)
            {
                if (!Connection.TableMappings.Any(m => m.MappedType.Name == typeof(GasModel).Name))
                {
                    await Connection.CreateTablesAsync(CreateFlags.None, typeof(GasModel)).ConfigureAwait(false);
                    IsInitialized = true;
                }

            }
        }

        public SQLiteDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        private void OnInitializeError(Exception ex)
        {
            throw new NotImplementedException();
        }

        public Task<List<GasModel>> GetAllGasAsync()
        {
            return Connection.Table<GasModel>().ToListAsync();
        }


        public Task<GasModel> GetGasAsync(int id)
        {
            return Connection.Table<GasModel>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveGasAsync(GasModel gas)
        {
            if (gas.Id != 0)
            {
                return Connection.UpdateAsync(gas);
            }
            else
            {
                return Connection.InsertAsync(gas);
            }
        }

        public Task<int> DeleteGasAsync(GasModel gas)
        {
            return Connection.DeleteAsync(gas);
        }

    }
}
