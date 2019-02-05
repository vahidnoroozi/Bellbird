using Bellbird.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bellbird.Services
{
    public interface IBellbirdService
    {
        Task UpVoteAsync(int AlarmId);
        Task<Alarm> CreateAsync(Alarm newAlarm);
        Task<List<Alarm>> GetAlarms();
        Task PostAlarm(int AlarmId);
    }
}
