using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.OrmLite;

namespace TwitterStreamClient
{
    public interface IDasUserRepository
    {
        IEnumerable<DasUser> GetAll();

        int? InsertOrUpate(DasUser user);
    }

    public class DasUserRepository : IDasUserRepository
    {
        public IEnumerable<DasUser> GetAll()
        {
            IEnumerable<DasUser> result;
            using (var _connection = Utilities.GetOpenConnection())
            {
                result = _connection.Select<DasUser>();
                _connection.Close();
            }

            return result;
        }

        public int? InsertOrUpate(DasUser user)
        {
            int? ret;
            using (var _connection = Utilities.GetOpenConnection())
            {
                var result = _connection.Where<DasUser>(new { TwitterId = user.TwitterId });

                var updateUser = result.SingleOrDefault();
                if (updateUser != null)
                {
                    updateUser.Name = user.Name;
                    updateUser.Status = user.Status;
                    updateUser.Message = user.Message;
                    updateUser.Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Utilities.koreaTZI);
                    ret = _connection.Update(updateUser);
                }
                else
                {
                    user.Date = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Utilities.koreaTZI);
                    ret = (int)_connection.Insert<DasUser>(user);
                }
                _connection.Close();
            }

            return ret;
        }
    }
}
