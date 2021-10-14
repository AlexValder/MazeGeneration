using System;
using System.Collections.Generic;
using Godot;
using Serilog;

namespace Demonomania.Scripts {
    public class Main : Spatial {
        private List<Spatial> _rooms = new List<Spatial>();
        private PackedScene _roomScene;
        private readonly Random _random = new Random();

        public override void _Ready() {
            SetupLogger();

            _roomScene = GD.Load<PackedScene>("Scenes/Room.tscn");

            OS.WindowMaximized = Settings.WindowMaximized;
        }

        public override void _UnhandledInput(InputEvent @event) {
            if (@event.IsActionPressed("debug_exit")) {
#if DEBUG
                GetTree().Quit(0);
#endif
            }
        }

        public void CreateRooms(int count) {
            ClearRooms();
            lock (_rooms) {
                var camera = GetViewport().GetCamera();
                // assuming that each "room" is 1x1 cube + 0.25 as a space between rooms
                for (var i = 0; i < count; ++i) {
                    for (var j = 0; j < count; ++j) {
                        var node = _roomScene.Instance<Spatial>();
                        AddChild(node);
                        node.GlobalTranslate(
                            new Vector3(
                                x: 2 * j,
                                y: .5f,
                                z: 2 * i
                            )
                        );
                        var mesh = node.GetChild<MeshInstance>(0);
                        if (mesh.Mesh.SurfaceGetMaterial(0).Duplicate() is SpatialMaterial material) {
                            material.AlbedoColor = new Color(
                                r: _random.Next(0, 255) / 255f,
                                g: _random.Next(0, 255) / 255f,
                                b: _random.Next(0, 255) / 255f,
                                a: 1f
                            );
                            mesh.MaterialOverride = material;
                        }

                        var label = node.GetChild<Label>(1);
                        label.Text = (i * count + j + 1).ToString();
                        label.SetPosition(
                            camera.UnprojectPosition(node.GlobalTransform.origin + Vector3.Up / 2)
                            - label.RectSize / 2
                        );
                        _rooms.Add(node);
                    }
                }

                if (_rooms.Count > 0) {
                    _rooms[0].Scale = new Vector3(1, 2, 1);
                    _rooms[0].Translate(new Vector3(0, 0.5f, 0));
                }
            }
        }

        public void ClearRooms() {
            lock (_rooms) {
                if (_rooms.Count < 1) {
                    return;
                }

                foreach (var room in _rooms) {
                    RemoveChild(room);
                    room.Free();
                }
                _rooms.Clear();
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
}
