using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ServiceFabric.RandomGenerator
{
    public class RandomGenerator : IRandomGenerator
    {
        private readonly Action<string> _logFactory;
        private readonly List<PropertyInfo> propertyInfos;
        private readonly Random randomProp;
        private readonly Random randomModel;
        private readonly int propertyCount;
        private readonly ConcurrentDictionary<int, RandomEntry> randomEntries;
        private List<RandomEntry> randomEntriesList;

        public RandomGenerator(Action<string> logFactory)
        {
            _logFactory = logFactory;

            propertyInfos = typeof(RandomEntry).GetProperties().ToList();

            randomProp = new Random();

            randomModel = new Random();

            propertyCount = propertyInfos.Count;

            randomEntries = new ConcurrentDictionary<int, RandomEntry>();

            randomEntriesList = new List<RandomEntry>();
        }


        public async Task Fill(int count)
        {
            int toAdd = randomEntries.Count == 0 ? count : randomEntries.Count + count;
            // TODO: what if it is not possible to fill count? implement some duration of this task, if not completed in n time cancel and use what you have collected
            while (toAdd != 0)
            {
                List<Task> tasks = new List<Task>();

                for (int i = 0; i < toAdd; i++)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            using (var httpClientHandler = new HttpClientHandler())
                            {
                                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                                using (var client = new HttpClient(httpClientHandler))
                                {
                                    var content = await client.GetStringAsync("https://api.namefake.com/");

                                    if (!content.StartsWith("{"))
                                    {
                                        int index = content.IndexOf("{", StringComparison.OrdinalIgnoreCase);

                                        content = content.Substring(index, content.Length - index);
                                    }

                                    var model = JsonConvert.DeserializeObject<RandomEntry>(content);

                                    if (!randomEntries.ContainsKey(model.GetHashCode()))
                                    {
                                        randomEntries.AddOrUpdate(model.GetHashCode(), model, (s, entry) => model);
                                        Interlocked.Decrement(ref toAdd);
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            _logFactory?.Invoke("Error requesting random entry:" + e.Message);
                        }
                        finally
                        {
                            _logFactory?.Invoke("Random messages count:" + randomEntries.Count);
                        }

                    }));
                }

                await Task.WhenAll(tasks);
            }

            randomEntriesList = randomEntries.Values.ToList();
        }

        public async Task<string> Generate()
        {
            if (randomEntries.Count == 0)
                return await Task.FromResult("There are none random entries");

            try
            {
                var model = randomEntriesList[randomModel.Next(0, randomEntries.Count)];

                var prop = propertyInfos[randomProp.Next(0, propertyCount)];

                var result = string.Concat("Random ", prop.Name, " with value ", prop.GetValue(model));

                return await Task.FromResult(result);

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }

    public interface IRandomGenerator
    {
        Task Fill(int count);

        Task<string> Generate();
    }
}
