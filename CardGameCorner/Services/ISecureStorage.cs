//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CardGameCorner.Services
//{
//    public interface ISecureStorage
//    {
//        Task SetAsync(string key, string value);
//        Task<string> GetAsync(string key);
//        Task RemoveAsync(string key);
//    }
//}


using System.Threading.Tasks;

namespace CardGameCorner.Services
{
    public interface ISecureStorage
    {
        Task<string> GetAsync(string key);
        Task SetAsync(string key, string value);
        Task RemoveAsync(string key);
    }

   
}