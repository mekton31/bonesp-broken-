using bonesp;
using Swed64;
using System.Numerics;

//init swed
 
 Swed swed = new Swed("cs2");

IntPtr client = swed.GetModuleBase("client.dll");

//memort render
Reader reader = new Reader(swed);


//imgui stuff
 
Renderer renderer = new Renderer();
renderer.Start().Wait();

//entitu handle

List<Entity> entities = new List<Entity>(); // all entityes
Entity localPlayer = new Entity(); // mein chancter
Vector2 Screen = new Vector2(1920, 1080); // game scrren

renderer.overlaySize = Screen; // write over defauly

//bone esp loop
while (true)
{
    // remove old enitits
    entities.Clear();
    Console.Clear();

    //get enetity list

    IntPtr entityList = swed.ReadPointer(client, Offsets.dwEntityList); // we need offsets

    //first entry
    IntPtr listEntry = swed.ReadPointer(entityList, 0x10);

    // update localpalyer
    localPlayer.pawnAddress = swed.ReadPointer(client, Offsets.dwLocalPlayerPawn);
    localPlayer.team = swed.ReadInt(localPlayer.pawnAddress, Offsets.m_iTeamNum);
    localPlayer.origin = swed.ReadVec(localPlayer.pawnAddress, Offsets.m_vOldOrigin);

    //loop throuhh entity list

    for (int i = 0; i < 64; i++) // 64 xontrolles
    {
        if (listEntry == IntPtr.Zero)
            continue;

        // get current controller
        IntPtr currentController = swed.ReadPointer(listEntry, i * 0x78);

        if (currentController == IntPtr.Zero)
            continue;

        //get current paWN
        int pawnHandle = swed.ReadInt(currentController, Offsets.m_hPlayerPawn);

        if (pawnHandle == 0)
            continue;


        //secont entry
        IntPtr listEntry2 = swed.ReadPointer(entityList, 0x8 * ((pawnHandle & 0x7FFF) >> 9) + 0x10);

        //get pwan
        IntPtr currentPawn = swed.ReadPointer(listEntry2, 0x78 * (pawnHandle & 0x1FF));

        if (currentPawn == localPlayer.pawnAddress) // if they are us
            continue;

        IntPtr sceneNode = swed.ReadPointer(currentPawn, Offsets.m_pGameSceneNode);
        IntPtr boneMatrix = swed.ReadPointer(sceneNode, Offsets.m_modelState + 0x80);

        //get viewmatrix
        ViewMatrix viewMatrix = reader.readMatrix(client + Offsets.dwViewMatrix);

        //get pawn atrbutes
        int team = swed.ReadInt(currentPawn, Offsets.m_iTeamNum);
        uint lifeState = swed.ReadUInt(currentPawn, Offsets.m_lifeState);

        //check if alive
        if (lifeState != 256)
            continue;

        Entity entity = new Entity();

        entity.pawnAddress = currentPawn;
        entity.controllerAddress = currentController;
        entity.team = team;
        entity.lifeState = lifeState;
        entity.origin = swed.ReadVec(currentPawn, Offsets.m_vOldOrigin);
        entity.distance = Vector3.Distance(entity.origin, localPlayer.origin);
        entity.bones = reader.ReadBones(boneMatrix);
        entity.bones2d = reader.ReadBones2d(entity.bones, viewMatrix, Screen);

        entities.Add(entity);

        //draw console
        Console.ForegroundColor = ConsoleColor.Green; 
        
        if (team != localPlayer.team)
        {
            Console.ForegroundColor= ConsoleColor.Red; // opposite team
        }
       // Console.WriteLine($"{entity.team}hp, pos: {(entity.bones[0].Z)}");

        Console.ResetColor();
    }
    //fetch over to renderer
    renderer.entitiesCopy = entities;
    renderer.localPlayerCopy = localPlayer;
    Thread.Sleep(3);
 }