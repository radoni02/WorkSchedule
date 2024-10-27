using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkSchedule.Application.Interfaces;
public interface IRoles
{
    Task<IList<string>> GetAllRoles();
}
