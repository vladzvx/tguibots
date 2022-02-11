using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TGUI.CoreLib.Interfaces
{
    public interface IDataLogger
    {
        public Task Log<TData>(TData data, Expression<Func<TData, bool>> filter = null);
        public Task<List<TData>> GetData<TData>(Expression<Func<TData, bool>> filter);
    }
}
