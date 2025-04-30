using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientWPF.VeiwModel.Base
{
    public interface INavigationService
    {
        void NavigateToTask(string token);
        void NavigateToUser(string token);
    }

}
