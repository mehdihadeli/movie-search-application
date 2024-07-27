using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BuildingBlocks.Test.Fixtures;

//https://stackoverflow.com/questions/43927955/should-getenvironmentvariable-work-in-xunit-test/43951218
//https://stackoverflow.com/questions/57359147/how-to-use-environment-variables-in-unit-tests-net-core
public class LaunchSettingsFixture : IDisposable
{
    public LaunchSettingsFixture()
    {
        using (var file = File.OpenText("launchSettings.json"))
        {
            var reader = new JsonTextReader(file);
            var jObject = JObject.Load(reader);

            var variables = jObject
                .GetValue("profiles")
                //select a proper profile here
                .SelectMany(profiles => profiles.Children())
                .SelectMany(profile => profile.Children<JProperty>())
                .Where(prop => prop.Name == "environmentVariables")
                .SelectMany(prop => prop.Value.Children<JProperty>())
                .ToList();

            foreach (var variable in variables)
                Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
        }
    }

    public void Dispose()
    {
        // ... clean up
    }
}
