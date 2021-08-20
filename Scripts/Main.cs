using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Serilog;

public class Main : Spatial {
    private const int DEFAULT_ROOM_COUNT = 1;
    private const float STEP = 2.5f;

    private readonly List<Spatial> _rooms = new List<Spatial>();
    private Spatial[,] _testSquares;
    private int _testSquaresSqrtCount = 0;

    public Vector3 UpPosition => _testSquaresSqrtCount == 0
        ? Vector3.Zero
        : _testSquares[
            0,
            0
            ].GlobalTransform.origin;

    public Vector3 DownPosition => _testSquaresSqrtCount == 0
        ? Vector3.Zero
        : _testSquares[
            _testSquaresSqrtCount - 1,
            _testSquaresSqrtCount - 1
            ].GlobalTransform.origin;

    public Vector3 LeftPosition => _testSquaresSqrtCount == 0
        ? Vector3.Zero
        : _testSquares[
            0,
            _testSquaresSqrtCount - 1
            ].GlobalTransform.origin;

    public Vector3 RightPosition => _testSquaresSqrtCount == 0
        ? Vector3.Zero
        : _testSquares[
            _testSquaresSqrtCount - 1,
            0
            ].GlobalTransform.origin;

    private PackedScene _roomScene;
    private Vector2 _begin;

    public override void _Ready() {
        SetupLogger();

        _roomScene = GD.Load<PackedScene>("Scenes/Room.tscn");

        OS.WindowMaximized = true;
    }

    public override void _UnhandledInput(InputEvent @event) {
        if (@event.IsActionPressed("debug_exit")) {
            GetTree().Quit(0);
        }
    }

    public void CreateRooms(int number = -1) {
        ClearRooms();

        var count = number < 0 ? DEFAULT_ROOM_COUNT : number;
        _begin = new Vector2(
            -count / 2f - (count / 2 - 1) * STEP,
            -count / 2f - (count / 2 - 1) * STEP
        );
        _testSquaresSqrtCount = count;

        var rand = new Random();
        _testSquares = new Spatial[count, count];

        var coordinates = new Vector2[count];
        for (var i = 0; i < count; ++i) {
            coordinates[i].x = rand.Next(0, count);
            coordinates[i].y = rand.Next(0, count);
        }

        for (var i = 0; i < count; ++i) {
            for (var j = 0; j < count; ++j) {
                // CreateTestSquare(i, j, rand);

                if (coordinates.Contains(new Vector2(i, j))) {
                    CreateRoom(i, j, rand);
                }
            }
        }
    }

    private void CreateRoom(int i, int j, Random rand) {
        var room = _roomScene.Instance() as Spatial;
        Debug.Assert(room != null, nameof(room) + " != null");
        room.Scale = new Vector3(
            rand.Next(1, 5),
            room.Scale.y,
            rand.Next(1, 5)
        );
        _rooms.Add(room);

        AddChild(room);

        var tmp = room.GetChild<KinematicBody>(1);
        tmp.MoveLockY = true;
        tmp.MoveAndSlide(Vector3.Forward);

        room.GetChild<MeshInstance>(0).SetSurfaceMaterial(
            0,
            new SpatialMaterial {
                AlbedoColor = new Color(
                    (float)rand.NextDouble(),
                    (float)rand.NextDouble(),
                    (float)rand.NextDouble()
                ),
            }
        );
    }

    private void CreateTestSquare(int i, int j, Random rand) {
        var room = _roomScene.Instance() as Spatial;
        Debug.Assert(room != null, nameof(room) + " != null");
        _testSquares[i, j] = room;
        AddChild(room);
        var transform = room.GlobalTransform;
        transform.origin = new Vector3(
            _begin.x + STEP * i,
            0,
            _begin.y + STEP * j
        );
        room.GlobalTransform = transform;
        room.Visible         = false;

        if (i != 0 || j != 0 || i != _testSquaresSqrtCount - 1 || j != _testSquaresSqrtCount) {
            room.GetChild<MeshInstance>(0).SetSurfaceMaterial(
                0,
                new SpatialMaterial {
                    AlbedoColor = new Color(
                        (float)rand.NextDouble(),
                        (float)rand.NextDouble(),
                        (float)rand.NextDouble()
                    ),
                }
            );
        }

    }

    private void ClearRooms() {
        if (_testSquares != null) {
            foreach (var room in _testSquares) {
                RemoveChild(room);
                room?.Free();
            }
        }

        foreach (var room in _rooms) {
            RemoveChild(room);
            room.Free();
        }
    }

    private static void SetupLogger() {
        var config = new LoggerConfiguration()
                .WriteTo.Console()
#if DEBUG
                .MinimumLevel.Debug()
#else
                .MinimumLevel.Warning()
#endif
            ;

        Log.Logger = config.CreateLogger();
    }
}
