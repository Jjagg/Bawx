using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace MagicaVoxLoader
{
    [ContentImporter(".vox", DisplayName = "VOX Importer", DefaultProcessor = "VoxelSceneProcessor")]
    public class VoxImporter : ContentImporter<VoxContent>
    {
        private ContentImporterContext _context;
        private string _fileName;
        private bool _customPalette;

        private const string ExpectedMagicNumber = "VOX ";
        private const string MainChunkHeader = "MAIN";
        private const string SizeChunkHeader = "SIZE";
        private const string VoxelChunkHeader = "XYZI";
        private const string PaletteChunkHeader = "RGBA";

        private static readonly uint[] DefaultColorValues = {  0x00000000, 0xffffffff, 0xffccffff, 0xff99ffff, 0xff66ffff, 0xff33ffff, 0xff00ffff, 0xffffccff, 0xffccccff, 0xff99ccff, 0xff66ccff, 0xff33ccff, 0xff00ccff, 0xffff99ff, 0xffcc99ff, 0xff9999ff,
    0xff6699ff, 0xff3399ff, 0xff0099ff, 0xffff66ff, 0xffcc66ff, 0xff9966ff, 0xff6666ff, 0xff3366ff, 0xff0066ff, 0xffff33ff, 0xffcc33ff, 0xff9933ff, 0xff6633ff, 0xff3333ff, 0xff0033ff, 0xffff00ff,
    0xffcc00ff, 0xff9900ff, 0xff6600ff, 0xff3300ff, 0xff0000ff, 0xffffffcc, 0xffccffcc, 0xff99ffcc, 0xff66ffcc, 0xff33ffcc, 0xff00ffcc, 0xffffcccc, 0xffcccccc, 0xff99cccc, 0xff66cccc, 0xff33cccc,
    0xff00cccc, 0xffff99cc, 0xffcc99cc, 0xff9999cc, 0xff6699cc, 0xff3399cc, 0xff0099cc, 0xffff66cc, 0xffcc66cc, 0xff9966cc, 0xff6666cc, 0xff3366cc, 0xff0066cc, 0xffff33cc, 0xffcc33cc, 0xff9933cc,
    0xff6633cc, 0xff3333cc, 0xff0033cc, 0xffff00cc, 0xffcc00cc, 0xff9900cc, 0xff6600cc, 0xff3300cc, 0xff0000cc, 0xffffff99, 0xffccff99, 0xff99ff99, 0xff66ff99, 0xff33ff99, 0xff00ff99, 0xffffcc99,
    0xffcccc99, 0xff99cc99, 0xff66cc99, 0xff33cc99, 0xff00cc99, 0xffff9999, 0xffcc9999, 0xff999999, 0xff669999, 0xff339999, 0xff009999, 0xffff6699, 0xffcc6699, 0xff996699, 0xff666699, 0xff336699,
    0xff006699, 0xffff3399, 0xffcc3399, 0xff993399, 0xff663399, 0xff333399, 0xff003399, 0xffff0099, 0xffcc0099, 0xff990099, 0xff660099, 0xff330099, 0xff000099, 0xffffff66, 0xffccff66, 0xff99ff66,
    0xff66ff66, 0xff33ff66, 0xff00ff66, 0xffffcc66, 0xffcccc66, 0xff99cc66, 0xff66cc66, 0xff33cc66, 0xff00cc66, 0xffff9966, 0xffcc9966, 0xff999966, 0xff669966, 0xff339966, 0xff009966, 0xffff6666,
    0xffcc6666, 0xff996666, 0xff666666, 0xff336666, 0xff006666, 0xffff3366, 0xffcc3366, 0xff993366, 0xff663366, 0xff333366, 0xff003366, 0xffff0066, 0xffcc0066, 0xff990066, 0xff660066, 0xff330066,
    0xff000066, 0xffffff33, 0xffccff33, 0xff99ff33, 0xff66ff33, 0xff33ff33, 0xff00ff33, 0xffffcc33, 0xffcccc33, 0xff99cc33, 0xff66cc33, 0xff33cc33, 0xff00cc33, 0xffff9933, 0xffcc9933, 0xff999933,
    0xff669933, 0xff339933, 0xff009933, 0xffff6633, 0xffcc6633, 0xff996633, 0xff666633, 0xff336633, 0xff006633, 0xffff3333, 0xffcc3333, 0xff993333, 0xff663333, 0xff333333, 0xff003333, 0xffff0033,
    0xffcc0033, 0xff990033, 0xff660033, 0xff330033, 0xff000033, 0xffffff00, 0xffccff00, 0xff99ff00, 0xff66ff00, 0xff33ff00, 0xff00ff00, 0xffffcc00, 0xffcccc00, 0xff99cc00, 0xff66cc00, 0xff33cc00,
    0xff00cc00, 0xffff9900, 0xffcc9900, 0xff999900, 0xff669900, 0xff339900, 0xff009900, 0xffff6600, 0xffcc6600, 0xff996600, 0xff666600, 0xff336600, 0xff006600, 0xffff3300, 0xffcc3300, 0xff993300,
    0xff663300, 0xff333300, 0xff003300, 0xffff0000, 0xffcc0000, 0xff990000, 0xff660000, 0xff330000, 0xff0000ee, 0xff0000dd, 0xff0000bb, 0xff0000aa, 0xff000088, 0xff000077, 0xff000055, 0xff000044,
    0xff000022, 0xff000011, 0xff00ee00, 0xff00dd00, 0xff00bb00, 0xff00aa00, 0xff008800, 0xff007700, 0xff005500, 0xff004400, 0xff002200, 0xff001100, 0xffee0000, 0xffdd0000, 0xffbb0000, 0xffaa0000,
    0xff880000, 0xff770000, 0xff550000, 0xff440000, 0xff220000, 0xff110000, 0xffeeeeee, 0xffdddddd, 0xffbbbbbb, 0xffaaaaaa, 0xff888888, 0xff777777, 0xff555555, 0xff444444, 0xff222222, 0xff111111  };

        private static Color[] _defaultPalette;

        private static Color[] DefaultPalette
        {
            get
            {
                if (_defaultPalette == null)
                {
                    _defaultPalette = new Color[255];
                    for (var i = 0; i < 255; i++)
                    {
                        var v = DefaultColorValues[i];
                        // format is ABGR
                        _defaultPalette[i] = new Color(v & 0x000000ff, v & 0x0000ff00, v & 0x00ff0000, v & 0xff000000);
                        // ARGB?
                        //_defaultPalette[i] = new Color(v & 0x00ff0000, v & 0x0000ff00, v & 0x000000ff, v & 0xff000000);
                        // RGBA?
                        //_defaultPalette[i] = new Color(v & 0x0000ff00, v & 0x000000ff, v & 0xff000000, v & 0x00ff0000);
                    }
                }

                return _defaultPalette;
            }
        }

        public override VoxContent Import(string fileName, ContentImporterContext context)
        {
            _context = context;
            _fileName = fileName;

            using (var fileStream = File.OpenRead(fileName))
            using (var binaryReader = new BinaryReader(fileStream))
            {
                return ReadVoxFile(binaryReader);
            }
        }

        private VoxContent ReadVoxFile(BinaryReader reader)
        {
            var result = new VoxContent();
            result.Palette = new Color[255];

            var magicNumber = new string(reader.ReadChars(4));
            if (magicNumber != ExpectedMagicNumber)
            {
                throw new InvalidContentException(
                    $"File {_fileName} is not a valid .vox file, it does not start with '{ExpectedMagicNumber}'.",
                    new ContentIdentity(_fileName));
            }

            var version = reader.ReadInt32();
            _context.Logger.LogMessage($"Reading Voxel file. Magica Voxel version {version}.");

            var mainId = new string(reader.ReadChars(4));
            var mainSize = reader.ReadInt32();
            var mainChildrenSize = reader.ReadInt32();

            if (mainId != MainChunkHeader)
                throw new InvalidContentException(
                    $"File {_fileName} is not a valid .vox file, it does not start with the MAIN chunk.",
                    new ContentIdentity(_fileName));

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                // each chunk has an ID, size and child chunks
                var chunkId = new string(reader.ReadChars(4));
                var chunkSize = reader.ReadInt32();
                var childrenSize = reader.ReadInt32();

                switch (chunkId)
                {
                    case SizeChunkHeader:
                        // SIZE chunk
                        ReadSize(result, reader);
                        break;
                    case VoxelChunkHeader:
                        // VOXEL chunk
                        ReadVoxel(result, reader);
                        break;
                    case PaletteChunkHeader:
                        // PALETTE chunk (optional)
                        ReadPalette(result, reader);
                        break;
                    default:
                        _context.Logger.LogMessage($"Skipping deprecated chunk {chunkId}");
                        reader.BaseStream.Seek(chunkSize + childrenSize, SeekOrigin.Current);
                        break;
                }
            }

            if (result.Voxels == null) 
                throw new ContentLoadException("No voxels were loaded.");

            if (!_customPalette)
                result.Palette = DefaultPalette;

            return result;
        }

        private void ReadSize(VoxContent voxContent, BinaryReader reader)
        {
            _context.Logger.LogMessage($"Reading SIZE chunk...");
            voxContent.SizeX = reader.ReadInt32();
            // Z-axis points up for MV but backwards (towards us) for MG. 
            // So MG axes are rotated a quarter over the positive x-axis relative to MV
            voxContent.SizeZ = reader.ReadInt32();
            voxContent.SizeY = reader.ReadInt32();
        }

        private void ReadVoxel(VoxContent voxContent, BinaryReader reader)
        {
            _context.Logger.LogMessage($"Reading VOXEL chunk...");
            voxContent.VoxelCount = reader.ReadInt32();

            var voxels = new MagicaVoxel[voxContent.VoxelCount];
            for (var i = 0; i < voxContent.VoxelCount; i++)
            {
                voxels[i] = new MagicaVoxel();
                voxels[i].X = reader.ReadByte();
                // Z-axis points up for MV but backwards (towards us) for MG. 
                // So MG axes are rotated a quarter over the positive x-axis relative to MV
                voxels[i].Z = (byte) (voxContent.SizeZ-reader.ReadByte());
                voxels[i].Y = reader.ReadByte();
                voxels[i].ColorIndex = reader.ReadByte();
            }

            voxContent.Voxels = voxels;
        }

        private void ReadPalette(VoxContent voxContent, BinaryReader reader)
        {
            _context.Logger.LogMessage($"Reading PALETTE chunk...");

            // - last color is not used ( only the first 255 colors are used ). 
            // - the first color ( at position 0 ) is corresponding to color index 1.
            //   -> we need to subtract 1 to get the correct index!

            for (var i = 0; i < 255; i++)
            {
                var r = reader.ReadByte();
                var g = reader.ReadByte();
                var b = reader.ReadByte();
                var a = reader.ReadByte();

                voxContent.Palette[i] = new Color(r, g, b, a);
            }

            // skip the last color
            reader.BaseStream.Seek(4, SeekOrigin.Current);

            _customPalette = true;
        }
    }

}
