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

        public Task<List<Favorite>> GetFavoritosAsync(int? userId)
        {
            // Using a LINQ query to filter favorites by the provided user ID
            return _connection.Table<Favorite>().Where(f => f.UsuarioId == userId).ToListAsync();
        }


        public Task<int> SaveFavoritoAsync(Favorite favorito)
        {
           
            return _connection.InsertAsync(favorito);
        }


        public async Task<Favorite> GetFavoritoByProductoId(int productoId, int? usuarioId)
        {
            return await _connection.Table<Favorite>().Where(f => f.ProductoId == productoId && f.UsuarioId == usuarioId).FirstOrDefaultAsync();
        }

        public async Task DeleteFavoritoAsync(int productoId, int? usuarioId)
        {
            var favorite = await _connection.Table<Favorite>().Where(f => f.ProductoId == productoId && f.UsuarioId == usuarioId).FirstOrDefaultAsync();
            if (favorite != null)
            {
                await _connection.DeleteAsync(favorite);
            }
        }
        public Task<int> DeleteAllFavoritesAsync()
        {
            return _connection.DeleteAllAsync<Favorite>();
        }
    }
}
