namespace Demonomania.Scripts.MazeGen.Mask {
    public static class MaskCreator {
        public static MazeMask CornerEastNorth(int width, int height) {
            var mask = new MazeMask();

            if (width > 1 && height > 1) {
                for (var i = 0; i < width / 2; ++i) {
                    for (var j = 0; j < height / 2; ++j) {
                        mask.Disabled.Add((i, j));
                    }
                }
            }

            return mask;
        }

        public static MazeMask Corners(int width, int height) {
            var mask = new MazeMask();

            if (width < 3 || height < 3) {
                return mask;
            }

            mask.Disabled.Add((0, 0));
            mask.Disabled.Add((0, height - 1));
            mask.Disabled.Add((width - 1, 0));
            mask.Disabled.Add((width - 1, height - 1));

            return mask;
        }

        public static MazeMask Empty() => new MazeMask();
    }
}
