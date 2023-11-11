using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ComparaYa.localBD
{
   public class Db
    {

        private readonly SQLiteAsyncConnection _connection;  
        
        public Db(string dbPath)
        {
            _connection = new SQLiteAsyncConnection(dbPath);
            _connection.CreateTableAsync<Favorite>().Wait();
        }

        public  Task<List<Favorite>> GetFavoritosAsync()
        {
            return _connection.Table<Favorite>().ToListAsync();
        }

        public Task<int> SaveFavoritoAsync(Favorite favorito)
        {
            return _connection.InsertAsync(favorito);
        }

        public Task<int> DeleteFavoritoAsync(int favoritoId)
        {
            return _connection.DeleteAsync<Favorite>(favoritoId);
        }

        public Task<int> DeleteAllFavoritesAsync()
        {
            return _connection.DeleteAllAsync<Favorite>();
        }
    }
}
