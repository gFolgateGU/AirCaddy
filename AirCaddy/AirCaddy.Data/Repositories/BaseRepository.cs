using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirCaddy.Data.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly AirCaddyDataEntities _dataEntities;

        protected BaseRepository()
        {
            _dataEntities = new AirCaddyDataEntities();
        }
    }
}
