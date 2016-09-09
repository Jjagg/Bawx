namespace Bawx
{
    public class VoxelScene
    {
        public int SizeX;
        public int SizeY;
        public int SizeZ;

        public VoxelData[] Voxels;

        public int TotalSize => SizeX*SizeY*SizeZ;
    }
}