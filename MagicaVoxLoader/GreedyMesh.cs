using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicaVoxLoader
{
    public static class GreedyMesh
    {

        // C# port adapted from https://www.giawa.com/journal-entry-6-greedy-mesh-optimization/
        // I rewrote some parts for readability and so I got a better understanding of the code
        // Functional changes:
        // - backfaces and frontfaces are handled seperately so we can set the right normals and have backface culling
        // - faces are compared to check if they can be merged (e.g. if they have a different material they can't be merged)

        public static void Generate<T>(byte[][][] grid, Func<int, int, int, int, int, FaceData, IEnumerable<T>> createFace, 
            out T[] vertices, out int[] indices) where T : IVertexType
        {
            // This greedy algorithm is converted from JavaScript to C# from this article:
            // http://0fps.wordpress.com/2012/06/30/meshing-in-a-minecraft-game/
            //
            // The original source code can be found here:
            // https://github.com/mikolalysenko/mikolalysenko.github.com/blob/gh-pages/MinecraftMeshes/js/greedy.js
            var verts = new List<T>();
            var inds = new List<int>();

            int[] size = {grid.Length, grid[0].Length, grid[0][0].Length};
            // loop over the three directions; x -> 0, y -> 1, z -> 2
            for (var direction = 0; direction < 3; direction++)
            {
                // u and v are the other two directions
                var u = (direction + 1)%3;
                var v = (direction + 2)%3;

                // pos holds the current position in the grid
                var pos = new int[3];
                // 1 for the current direction and 0 for others, used below to check if face is occluded by a voxel
                var negDir = new int[3];
                // contains the rendering data for each face in the current layers, a layer being a slice of the grid; Note that this is linearized
                var backfaces = new FaceData[grid.GetSize(u)*grid.GetSize(v)];
                var frontfaces = new FaceData[grid.GetSize(u)*grid.GetSize(v)];

                negDir[direction] = 1;
                // we start at -1 because we check for faces *between* blocks and have to get the outer faces too
                pos[direction] = -1;

                // outer loop goes through all layers
                for (pos[direction] = -1; pos[direction] < grid.GetSize(direction); pos[direction]++)
                {
                    // Get all faces that need to be rendered in the current layer (front and back seperately)
                    for (pos[v] = 0; pos[v] < size[v]; pos[v]++)
                    {
                        for (pos[u] = 0; pos[u] < size[u]; pos[u]++)
                        {
                            // if this block is visible and the one behind it is not we need to render the backface of the current block
                            // if this one is not visible but the one behind it is, we need to render the frontface of the 'behind' block
                            var index = pos[v]*size[v] + pos[u];
                            var current = pos[direction] < 0 ||
                                          grid[pos[0]][pos[1]][pos[2]].IsEmpty();
                            var behind = pos[direction] > grid.GetSize(direction) ||
                                         grid[pos[0] + negDir[0]][pos[1] + negDir[1]][pos[2] + negDir[2]].IsEmpty();

                            if (current && !behind)
                                backfaces[index] = new FaceData(grid[pos[0]][pos[1]][pos[2]]);
                            else if (!current && behind)
                                frontfaces[index] = new FaceData(grid[pos[0] + negDir[0]][pos[1] + negDir[1]][pos[2] + negDir[2]]);
                        }
                    }

                    // Then process both layers to build quads
                    ProcessLayer(backfaces, createFace, verts, inds, pos, direction, size);
                    ProcessLayer(frontfaces, createFace, verts, inds, pos, direction, size);
                }
            }

            vertices = verts.ToArray();
            indices = inds.ToArray();
        }

        private static void ProcessLayer<T>(
            FaceData[] faces, Func<int, int, int, int, int, FaceData, IEnumerable<T>> createFace, 
            List<T> vertices, List<int> indices,
            int[] pos, int dir, int[] size) where T : IVertexType
        {
            if (!faces.Any()) return;

            var u = (dir + 1)%3;
            var v = (dir + 2)%3;


            // Generate mesh for mask using lexicographic ordering
            for (var v0 = 0; v0 < size[v]; v0++)
            {
                for (var u0 = 0; u0 < size[u];)
                {
                    var n = v0*size[u] + u0;
                    if (!faces[n].IsEmpty)
                    {
                        var currentFace = faces[n];
                        // Compute width
                        var u1 = 1;
                        while (u0 + u1 < size[u] && currentFace == faces[n + u1]) u1++;

                        // Compute height
                        int v1;
                        for (v1 = 1; v0 + v1 < size[v]; v1++)
                        {
                            for (var k = 0; k < u1; k++)
                            {
                                if (faces[n + k + v1*size[u]] != currentFace)
                                    goto EndLoop;
                            }
                        }

                        EndLoop:

                        // Add quad
                        pos[u] = u0;
                        pos[v] = v0;

                        // x, y, z, widht, height, data
                        indices.AddRange(CreateQuadIndices(vertices.Count));
                        vertices.AddRange(createFace(u0, v0, dir, u1, v1, currentFace));

                        // todo make faces 2D?
                        // Zero-out mask
                        for (var l = 0; l < v1; ++l)
                            for (var k = 0; k < u1; ++k)
                                faces[n + k + l*32] = FaceData.Empty;

                        // Increment counters and continue
                        u0 += u1;
                    }
                    else
                    {
                        ++u0;
                    }
                }
            }
        }

        private static int[] CreateQuadIndices(int start)
        {
            return new []
            {
                start, start + 1, start + 2,
                start + 2, start + 3, start
            };
        }

        #region Methods

        // Use extension methods so this can easily be refactored later in case blocks become more complex
        private static bool IsEmpty(this byte b)
        {
            return b == 0;
        }

        private static int GetSize<T>(this T[][][] grid, int dimension)
        {
            switch (dimension)
            {
                // X
                case 0:
                    return grid.Length;
                // Y
                case 1:
                    return grid[0].Length;
                // Z
                case 2:
                    return grid[0][0].Length;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dimension));
            }
        }

        #endregion

        public struct FaceData
        {
            public readonly byte Index;

            public bool IsEmpty => Index == 0;

            public FaceData(byte index)
            {
                Index = index;
            }

            public static FaceData Empty => new FaceData(0);

            public bool Equals(FaceData other)
            {
                return Index == other.Index;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is FaceData && Equals((FaceData) obj);
            }

            public override int GetHashCode()
            {
                return Index.GetHashCode();
            }

            public static bool operator ==(FaceData left, FaceData right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(FaceData left, FaceData right)
            {
                return !left.Equals(right);
            }
        }
    }

}