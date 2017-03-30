using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using Ionic.Zip;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnlineGameManager : MonoBehaviour
{
    public MessagePanel messagePanel;
    public ResearchPanel researchPanel;
    public PlanetReport planetReport;
    public FleetReport fleetReport;
    public GameGameObject game;
    public GameObject baseFleet;
    public Transform mapObject;
    public LayoutManager layoutManager;

    [Space]
    public GameObject blockerPanel;

    FleetController fc = new FleetControllerImpl();
    PlanetController pc;
    ShipDesigner sd = new ShipDesignerImpl();
    string lastUploadId;

    public bool load = false;

    // Use this for initialization

    private void Awake()
    {
        UploadCompleteMessage.Listener += GetUploadMessage;
        if (FindObjectOfType<StaticTechStore>() == null)
        {
            GameObject go = new GameObject("TechStore", typeof(StaticTechStore));
        }
        if (NewGameInit.instance != null)
        {
            load = NewGameInit.instance.load;
        }
    }

    void Start()
    {
        pc = new PlanetControllerImpl(fc, mapObject, game, baseFleet);
        if (!load)
        {
            UniverseGenerator gen = GetComponent<UniverseGenerator>();

            if (NewGameInit.instance != null)
            {
                game.getGame().setSize(NewGameInit.instance.size);
                game.getGame().setDensity(NewGameInit.instance.density);

                game.getGame().setName(NewGameInit.instance.gameName);

                game.getGame().getPlayers()[0].setName(NewGameInit.instance.playerNames[0]);
                game.getGame().getPlayers()[0].getUser().setName(NewGameInit.instance.playerNames[0]);
                game.getGame().getPlayers()[0].setRace(NewGameInit.instance.races[0]);
                game.getGame().getPlayers()[1].setRace(NewGameInit.instance.races[1]);
                game.getGame().getPlayers()[1].setName(NewGameInit.instance.playerNames[1]);
                game.getGame().getPlayers()[1].getUser().setName(NewGameInit.instance.playerNames[1]);

                game.getGame().getPlayers()[0].getUser().setAi(false);
                game.getGame().getPlayers()[1].getUser().setAi(false);

            }
            gen.setUniverseGenerator(game.getGame(), fc, pc, new ShipDesignerImpl(), StaticTechStore.getInstance());

            gen.generate();
        }
        else
        {
            LoadUniverseGenerator loadGen = GetComponent<LoadUniverseGenerator>();

            if (NewGameInit.instance != null)
            {
                game.getGame().setSize(NewGameInit.instance.size);
                game.getGame().setDensity(NewGameInit.instance.density);

                game.getGame().setName(NewGameInit.instance.gameName);

                game.getGame().getPlayers()[0].setName(NewGameInit.instance.playerNames[0]);
                game.getGame().getPlayers()[0].getUser().setName(NewGameInit.instance.playerNames[0]);
                game.getGame().getPlayers()[0].setRace(NewGameInit.instance.races[0]);
                game.getGame().getPlayers()[1].setRace(NewGameInit.instance.races[1]);
                game.getGame().getPlayers()[1].setName(NewGameInit.instance.playerNames[1]);
                game.getGame().getPlayers()[1].getUser().setName(NewGameInit.instance.playerNames[1]);

                game.getGame().getPlayers()[0].getUser().setAi(false);
                game.getGame().getPlayers()[1].getUser().setAi(false);

                loadGen.setUniverseGenerator(Application.persistentDataPath + "/Online/serverFile/", game.getGame(), fc, pc, new ShipDesignerImpl(), StaticTechStore.getInstance());
            }
            else
                loadGen.setUniverseGenerator(Application.persistentDataPath + "/Online/serverFile/", game.getGame(), fc, pc, new ShipDesignerImpl(), StaticTechStore.getInstance());

            loadGen.generate();

            new FileInfo(Application.persistentDataPath + "/Online/serverFile.zip").Delete();
            new DirectoryInfo(Application.persistentDataPath + "/Online/serverFile/").Delete(true);
        }

        messagePanel.playerIndex = researchPanel.playerIndex = planetReport.playerIndex = fleetReport.playerIndex = GameSparksManager.getInstance().getPlayerName() == game.getGame().getPlayers()[0].getName() ? 0 : 1;

        Settings.instance.playerID = GameSparksManager.getInstance().getPlayerName() == game.getGame().getPlayers()[0].getName() ? game.getGame().getPlayers()[0].getID() : game.getGame().getPlayers()[1].getID();

        layoutManager.LoadLayout();
    }

    public void processTurn()
    {
        blockerPanel.SetActive(true);
        TurnGenerator tg = new TurnGenerator(game.getGame(), fc, pc);
        // do turn processing
        tg.generate();
        //write the game files
        writeGameToJson();
        //save the game files to a zip
        saveGameToZip();
        //upload the game files
        uploadGameFiles();
        //takes the turn, passing a reference to the uploaded game file
        //takeTurn();

    }

    public void writeGameToJson()
    {

        if (!(new DirectoryInfo(Application.persistentDataPath + "/Online/").Exists))
            new DirectoryInfo(Application.persistentDataPath + "/Online/").Create();

        SaveGame();
        SaveShipDesigns();
        SavePlayerTechs();
        foreach (Planet planet in game.getGame().getPlanets())
            SaveGamePlanet(planet);
        foreach (Wormhole wormhole in game.getGame().getWormholes())
            SaveGameWormhole(wormhole);
        foreach (Fleet fleet in game.getGame().getFleets())
            SaveGameFleet(fleet);
    }

    public void saveGameToZip()
    {
        ZipFile zip = new ZipFile();
        zip.AddDirectory(Application.persistentDataPath + "/Online/" + game.getGame().getName() + "/");
        zip.Password = GameSparksManager.getInstance().getChallengeID();
        zip.SaveProgress += Zip_SaveProgress;
        zip.Save(Application.persistentDataPath + "/Online/" + game.getGame().getName() + ".zip");
    }

    private void Zip_SaveProgress(object sender, SaveProgressEventArgs e)
    {
        if (e.EventType == ZipProgressEventType.Saving_Completed)
        {
            Debug.Log("Saving Completed");
        }
    }
    
    public void uploadGameFiles()
    {
        new GetUploadUrlRequest().Send((response) =>
        {
            StartCoroutine(UploadAFile(response.Url));
        });
    }
    
    public IEnumerator UploadAFile(string uploadUrl)
    {
        // Create a Web Form, this will be our POST method's data
        var form = new WWWForm();
        form.AddField("somefield", "somedata");
        form.AddBinaryData("file", File.ReadAllBytes(Application.persistentDataPath + "/Online/" + game.getGame().getName() + ".zip"), game.getGame().getName() + ".zip", "application/zip");
        //POST the screenshot to GameSparks
        WWW w = new WWW(uploadUrl, form);
        yield return w;

        if (w.error != null)
        {
            Debug.Log(w.error);
        }
        else
        {
            Debug.Log(w.text);
        }
    }

    //This will be our message listener, this will be triggered when we successfully upload a file
    public void GetUploadMessage(GSMessage message)
    {
        new LogEventRequest_DeleteUploaded()
            .Set_Upload(GameSparksManager.getInstance().getGameDataURL())
            .Send((response) => {
                if (response.HasErrors)
                    Debug.Log(response.Errors.JSON);
                else
                    Debug.Log("Deleted " + GameSparksManager.getInstance().getGameDataURL());
            });
        new DismissMessageRequest()
            .SetMessageId(message.MessageId)
            .Send((response) => {
                if (response.HasErrors)
                    Debug.Log(response.Errors.JSON);
                else
                    Debug.Log("Deleted message" + message.MessageId);
            });

        //Every time we get a message
        Debug.Log(message.BaseData.GetString("uploadId"));
        //Save the last uploadId
        lastUploadId = message.BaseData.GetString("uploadId");

        new FileInfo(Application.persistentDataPath + "/Online/" + game.getGame().getName() + ".zip").Delete();
        new DirectoryInfo(Application.persistentDataPath + "/Online/" + game.getGame().getName()).Delete(true);

        takeTurn();
    }

    void takeTurn()
    {
        new LogChallengeEventRequest()
            .SetChallengeInstanceId(game.getGame().getName())
            .SetEventKey("TT")
            .SetEventAttribute("GD", lastUploadId)
            .Send((response) =>
            {
                if (response.HasErrors)
                {
                    Debug.Log("Failed");
                }
                else
                {
                    Debug.Log("Successful");
                    GameSparksManager.getInstance().setGameDataURL(lastUploadId);
                    SceneManager.LoadScene(0);
                }
            });
    }

    public void SaveGame()
    {
        Debug.Log(JsonUtility.ToJson(game.getGame(), true));

        foreach(Player player in game.getGame().getPlayers())
            player.setDesignIDs(player.getDesigns().Select(design => design.getID()).ToList());

        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Online/" + game.getGame().getName() + "/");
        if (!dirInf.Exists)
        {
            Debug.Log("Creating subdirectory");
            dirInf.Create();
        }
        else
        {
            dirInf.Delete(true);
            Debug.Log("Creating subdirectory");
            dirInf.Create();
        }

        File.WriteAllText(Application.persistentDataPath + "/Online/" + game.getGame().getName() + "/" + game.getGame().getName() + ".json", JsonUtility.ToJson(game.getGame(), true));
    }

    public void SaveShipDesigns()
    {
        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Online/" + game.getGame().getName() + "/ShipDesigns/");
        if (!dirInf.Exists)
        {
            Debug.Log("Creating subdirectory");
            dirInf.Create();
        }
        else
        {
            dirInf.Delete(true);
            Debug.Log("Creating subdirectory");
            dirInf.Create();
        }

        foreach (Player player in game.getGame().getPlayers())
        {
            foreach (ShipDesign design in player.getDesigns())
                File.WriteAllText(Application.persistentDataPath + "/Online/" + game.getGame().getName() + "/ShipDesigns/" + design.getID() + ".design", JsonUtility.ToJson(design, true));
        }
    }

    public void SavePlayerTechs()
    {
        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Online/" + game.getGame().getName() + "/Techs/");
        if (!dirInf.Exists)
        {
            Debug.Log("Creating subdirectory");
            dirInf.Create();
        }
        else
        {
            dirInf.Delete(true);
            Debug.Log("Creating subdirectory");
            dirInf.Create();
        }

        foreach (Player player in game.getGame().getPlayers())
                File.WriteAllText(Application.persistentDataPath + "/Online/" + game.getGame().getName() + "/Techs/" + player.getName() + ".techs", JsonUtility.ToJson(player.getTechs(), true));
    }

    public void SaveGamePlanet(Planet planet)
    {
        Debug.Log(JsonUtility.ToJson(planet, true));

        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Online/" + game.getGame().getName() + "/Planets/");
        if (!dirInf.Exists)
        {
            Debug.Log("Creating subdirectory");
            dirInf.Create();
        }

        File.WriteAllText(Application.persistentDataPath + "/Online/" + game.getGame().getName() + "/Planets/" + planet.getName() + ".planet", JsonUtility.ToJson(planet, true));
    }

    public void SaveGameWormhole(Wormhole wormhole)
    {
        Debug.Log(JsonUtility.ToJson(wormhole, true));

        DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Online/" + game.getGame().getName() + "/Wormholes/");
        if (!dirInf.Exists)
        {
            Debug.Log("Creating subdirectory");
            dirInf.Create();
        }

        File.WriteAllText(Application.persistentDataPath + "/Online/" + game.getGame().getName() + "/Wormholes/" + wormhole.getName() + ".wormhole", JsonUtility.ToJson(wormhole, true));
    }

    public void SaveGameFleet(Fleet fleet)
    {
        Debug.Log(JsonUtility.ToJson(fleet, true));

        if (fleet.getOrbiting() != null)
        {
            DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Online/" + game.getGame().getName() + "/Fleets/" + fleet.getOrbiting().getName());
            if (!dirInf.Exists)
            {
                Debug.Log("Creating subdirectory");
                dirInf.Create();
            }

            File.WriteAllText(Application.persistentDataPath + "/Online/" + game.getGame().getName() + "/Fleets/" + fleet.getOrbiting().getName() + "/" + fleet.getID() + ".fleet", JsonUtility.ToJson(fleet, true));
        }
        else
        {
            DirectoryInfo dirInf = new DirectoryInfo(Application.persistentDataPath + "/Online/" + game.getGame().getName() + "/Fleets/Empty Space");
            if (!dirInf.Exists)
            {
                Debug.Log("Creating subdirectory");
                dirInf.Create();
            }

            File.WriteAllText(Application.persistentDataPath + "/Online/" + game.getGame().getName() + "/Fleets/Empty Space/" + fleet.getID() + ".fleet", JsonUtility.ToJson(fleet, true));
        }
    }
}
