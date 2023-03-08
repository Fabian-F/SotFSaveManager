using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SotFSaveManager.MVVM.Model
{
    class Manager
    {
        public static void ReviveKelvin(string savePath)
        {
            // SaveData.json -> Data > (VailWorldSim) > Actors[TypeId=9] > State = 2
            // SaveData.json -> Data > (VailWorldSim) > KillStatsList[TypeId=9] > State = 2
            // GameStateSaveData.json -> Data > (GameState) > IsRobbyDead = false

            // Modify SaveData.json
            JObject json = GetJsonObject(savePath, "SaveData.json");
            JObject data = (JObject)json["Data"];
            JObject vailWorldSim = JObject.Parse(data["VailWorldSim"].Value<string>());

            JArray actors = (JArray)vailWorldSim["Actors"];
            JObject kelvin = (JObject)actors.Single(element => ((JObject)element).GetValue("TypeId").Value<int>() == 9);
            kelvin["State"] = 2;
            kelvin["Stats"]["Health"] = 100.0;

            JArray killStats = (JArray)vailWorldSim["KillStatsList"];
            JObject kelvinKillStat = (JObject)killStats.Single(element => ((JObject)element).GetValue("TypeId").Value<int>() == 9);
            kelvinKillStat["PlayerKilled"] = 0;

            string vailWorldSimSerialized = vailWorldSim.ToString(Formatting.None);
            data["VailWorldSim"] = vailWorldSimSerialized;

            File.WriteAllText(GetPath(savePath, "SaveData.json"), json.ToString(Formatting.None));

            // Modify GameStateSaveData.json
            JObject gameStateJson = GetJsonObject(savePath, "GameStateSaveData.json");
            JObject gameState = JObject.Parse(gameStateJson["Data"]["GameState"].Value<string>());
            gameState["IsRobbyDead"] = false;

            // Overwrite File
            gameStateJson["Data"]["GameState"] = gameState.ToString(Formatting.None);
            File.WriteAllText(GetPath(savePath, "GameStateSaveData.json"), gameStateJson.ToString(Formatting.None));
        }

        public static void ReviveVirginia(string savePath)
        {
            // SaveData.json -> Data > (VailWorldSim) > Actors[TypeId=10] > State = 2
            // SaveData.json -> Data > (VailWorldSim) > KillStatsList[TypeId=10] > State = 2
            // GameStateSaveData.json -> Data > (GameState) > IsVirginiaDead = false

            // Modify SaveData.json
            JObject json = GetJsonObject(savePath, "SaveData.json");
            JObject data = (JObject)json["Data"];
            JObject vailWorldSim = JObject.Parse(data["VailWorldSim"].Value<string>());

            JArray actors = (JArray)vailWorldSim["Actors"];
            JObject virginia = (JObject)actors.Single(element => ((JObject)element).GetValue("TypeId").Value<int>() == 10);
            virginia["State"] = 2;
            virginia["Stats"]["Health"] = 120.0;
            virginia["Stats"]["Fear"] = 0.0;
            virginia["Stats"]["Affection"] = 0.0;

            JArray killStats = (JArray)vailWorldSim["KillStatsList"];
            JObject virginiaKillStat = (JObject)killStats.Single(element => ((JObject)element).GetValue("TypeId").Value<int>() == 10);
            virginiaKillStat["PlayerKilled"] = 0;

            string vailWorldSimSerialized = vailWorldSim.ToString(Formatting.None);
            data["VailWorldSim"] = vailWorldSimSerialized;

            File.WriteAllText(GetPath(savePath, "SaveData.json"), json.ToString(Formatting.None));

            // Modify GameStateSaveData.json
            JObject gameStateJson = GetJsonObject(savePath, "GameStateSaveData.json");
            JObject gameState = JObject.Parse(gameStateJson["Data"]["GameState"].Value<string>());
            gameState["IsVirginiaDead"] = false;

            // Overwrite File
            gameStateJson["Data"]["GameState"] = gameState.ToString(Formatting.None);
            File.WriteAllText(GetPath(savePath, "GameStateSaveData.json"), gameStateJson.ToString(Formatting.None));
        }

        public static void TeleportKelvin(string savePath)
        {
            // PlayerStateSaveData.json -> Data > (PlayerState) > _entries[Name=player.position] > x = FloatArrayValue
            // Transform x[x,y,z] to ActorPosition{"x":x,"y":y,"z":z}
            // SaveData.json -> Data > (VailWorldSim) > Actors[TypeId=9] > Position = x

            // Get PlayerState
            JObject playerStateJson = GetJsonObject(savePath, "PlayerStateSaveData.json");
            JArray entries = (JArray)JObject.Parse(playerStateJson["Data"]["PlayerState"].Value<string>())["_entries"];
            JArray playerPosition = (JArray)entries.Single(element => ((JObject)element).GetValue("Name").Value<string>() == "player.position")["FloatArrayValue"];

            // Transform JArray to JObject
            JObject actorPosition = new JObject();
            actorPosition.Add("x", playerPosition[0]);
            actorPosition.Add("y", playerPosition[1]);
            actorPosition.Add("z", playerPosition[2]);

            // Insert PlayerPosition into Actor
            JObject saveDataJson = GetJsonObject(savePath, "SaveData.json");
            JObject vailWorldSim = JObject.Parse(saveDataJson["Data"]["VailWorldSim"].Value<string>());
            JArray actors = (JArray)vailWorldSim["Actors"];
            JObject kelvin = (JObject)actors.Single(element => ((JObject)element).GetValue("TypeId").Value<int>() == 9);
            kelvin["Position"] = actorPosition;

            // Overwrite File
            saveDataJson["Data"]["VailWorldSim"] = vailWorldSim.ToString(Formatting.None);
            File.WriteAllText(GetPath(savePath, "SaveData.json"), saveDataJson.ToString(Formatting.None));
        }

        public static void TeleportVirginia(string savePath)
        {
            // PlayerStateSaveData.json -> Data > (PlayerState) > _entries[Name=player.position] > x = FloatArrayValue
            // Transform x[x,y,z] to ActorPosition{"x":x,"y":y,"z":z}
            // SaveData.json -> Data > (VailWorldSim) > Actors[TypeId=10] > Position = x

            // Get PlayerState
            JObject playerStateJson = GetJsonObject(savePath, "PlayerStateSaveData.json");
            JArray entries = (JArray)JObject.Parse(playerStateJson["Data"]["PlayerState"].Value<string>())["_entries"];
            JArray playerPosition = (JArray)entries.Single(element => ((JObject)element).GetValue("Name").Value<string>() == "player.position")["FloatArrayValue"];

            // Transform JArray to JObject
            JObject actorPosition = new JObject();
            actorPosition.Add("x", playerPosition[0]);
            actorPosition.Add("y", playerPosition[1]);
            actorPosition.Add("z", playerPosition[2]);

            // Insert PlayerPosition into Actor
            JObject saveDataJson = GetJsonObject(savePath, "SaveData.json");
            JObject vailWorldSim = JObject.Parse(saveDataJson["Data"]["VailWorldSim"].Value<string>());
            JArray actors = (JArray)vailWorldSim["Actors"];
            JObject virginia = (JObject)actors.Single(element => ((JObject)element).GetValue("TypeId").Value<int>() == 10);
            virginia["Position"] = actorPosition;

            // Overwrite File
            saveDataJson["Data"]["VailWorldSim"] = vailWorldSim.ToString(Formatting.None);
            File.WriteAllText(GetPath(savePath, "SaveData.json"), saveDataJson.ToString(Formatting.None));
        }

        public static void RegrowStumps(string savePath)
        {
            // WorldObjectLocatorManagerSaveDat.json -> Data > (WorldObjectLocatorManager) > SerializedStates[Value=3] > Value = 2
            JObject worldObjectJson = GetJsonObject(savePath, "WorldObjectLocatorManagerSaveData.json");
            JObject locatorManagerJson = JObject.Parse(worldObjectJson["Data"]["WorldObjectLocatorManager"].Value<string>());
            JArray serializedStates = (JArray)locatorManagerJson["SerializedStates"];
            foreach(JObject obj in serializedStates)
            {
                if(obj["Value"].Value<int>() == 3)
                {
                    obj["Value"] = 2;
                }
            }

            // Overwrite File
            worldObjectJson["Data"]["WorldObjectLocatorManager"] = locatorManagerJson.ToString(Formatting.None);
            File.WriteAllText(GetPath(savePath, "WorldObjectLocatorManagerSaveData.json"), worldObjectJson.ToString(Formatting.None));
        }
        public static void RegrowAllTrees(string savePath)
        {
            // WorldObjectLocatorManagerSaveDat.json -> Data > (WorldObjectLocatorManager) > SerializedStates = []
            JObject worldObjectJson = GetJsonObject(savePath, "WorldObjectLocatorManagerSaveData.json");
            JObject locatorManagerJson = JObject.Parse(worldObjectJson["Data"]["WorldObjectLocatorManager"].Value<string>());
            JArray serializedStates = (JArray)locatorManagerJson["SerializedStates"];
            serializedStates.Clear();

            // Overwrite File
            worldObjectJson["Data"]["WorldObjectLocatorManager"] = locatorManagerJson.ToString(Formatting.None);
            File.WriteAllText(GetPath(savePath, "WorldObjectLocatorManagerSaveData.json"), worldObjectJson.ToString(Formatting.None));
        }

        public static JObject GetJsonObject(string savePath, string fileName)
        {
            return JObject.Parse(File.ReadAllText(GetPath(savePath, fileName)));
        }

        public static string GetPath(string savePath, string fileName)
        {
            return savePath + Path.DirectorySeparatorChar + fileName;
        }
    }
}
