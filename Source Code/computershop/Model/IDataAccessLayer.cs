using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace computershop.Model
{
    public interface IDataAccessLayer
    {
        void EncryptPassword(SecureString password, string userName);
    }
    public class DataAccessLayer : IDataAccessLayer
    {
        public void EncryptPassword(SecureString password, string userName)
        {
            // Triển khai phương thức EncryptPassword ở đây
            // Sử dụng password và userName để thực hiện mã hóa mật khẩu
        }
    }
}
