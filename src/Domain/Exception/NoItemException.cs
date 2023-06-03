using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary_BorrowingService.Domain.Exception
{
    public class NoItemException : System.Exception
    {
        public NoItemException(string message) : base(message)
        {
        }
    }
}
