using Bellbird.DAL;
using Bellbird.DAL.Models;
using BIApiGateway.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Bellbird.Services
{
    public class BellbirdService : IBellbirdService
    {
        private readonly ApiEndPoints _apiEndPoints;
        public readonly BellbirdDbContext _dbcntxt;

        public BellbirdService(BellbirdDbContext sdbx, IOptions<ApiEndPoints> apiEndPoints)
        {
            _dbcntxt = sdbx;
            _apiEndPoints = apiEndPoints.Value;
        }

        public async Task<Alarm> CreateAsync(Alarm newAlarm)
        {
            Alarm al=null;
            await Task.Run(() =>
            {
                al = (from a in _dbcntxt.Alarms where a.Name.Equals(newAlarm.Name) select a).FirstOrDefault<Alarm>();
                if (al != null)
                {
                    throw new Exception("Alarm already exists");
                }
                _dbcntxt.Alarms.Add(newAlarm);
               
            });
            await _dbcntxt.SaveChangesAsync();
            await Task.Run(() =>
            {
                al = (from a in _dbcntxt.Alarms where a.Name.Equals(newAlarm.Name) select a).FirstOrDefault<Alarm>();
                return al;
            });
            return al;
        }

        public async Task<List<Alarm>> GetAlarms()
        {
            return await Task.Run(() =>
            {
                var result = (from a in _dbcntxt.Alarms select a).ToList<Alarm>();
                return result;

            });
        }

        public async Task PostAlarm(int AlarmId)
        {
            var anonobj = new { alarm_id = AlarmId };
            var alarmIdContent = JsonConvert.SerializeObject(anonobj);
            var alarmIdBuffer = System.Text.Encoding.UTF8.GetBytes(alarmIdContent);
            var alarmIdByteContent = new ByteArrayContent(alarmIdBuffer);
            alarmIdByteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var uri = _apiEndPoints.HandShake;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = await client.PostAsync(uri, alarmIdByteContent);
                var responseString = await response.Content.ReadAsStringAsync();

               // return responseString;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task UpVoteAsync(int AlarmId)
        {
            Alarm al = null;
            await Task.Run(() =>
            {
                al = (from a in _dbcntxt.Alarms where a.Id.Equals(AlarmId) select a).FirstOrDefault<Alarm>();
                if (al != null)
                {
                    al.Upvotes += 1;
                }
                _dbcntxt.Alarms.Update(al);                
            });
            await _dbcntxt.SaveChangesAsync();           

        }
    }
}
