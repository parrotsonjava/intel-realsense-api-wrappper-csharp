using System.IO;
using System.Media;

namespace IntelRealSenseStart.Code.RealSense.Native
{
    public class VoiceOut
    {
        private readonly MemoryStream fileStream;
        private readonly BinaryWriter binaryWriter;

        public VoiceOut(PXCMAudio.AudioInfo ainfo)
        {
            fileStream = new MemoryStream();
            binaryWriter = new BinaryWriter(fileStream);

            binaryWriter.Write((int)0x46464952);  // chunkIdRiff:'FFIR'
            binaryWriter.Write((int)0);           // chunkDataSizeRiff
            binaryWriter.Write((int)0x45564157);  // riffType:'EVAW'
            binaryWriter.Write((int)0x20746d66);  // chunkIdFmt:' tmf'
            binaryWriter.Write((int)0x12);        // chunkDataSizeFmt
            binaryWriter.Write((short)1);         // compressionCode
            binaryWriter.Write((short)ainfo.nchannels);  // numberOfChannels
            binaryWriter.Write((int)ainfo.sampleRate);   // sampleRate
            binaryWriter.Write((int)(ainfo.sampleRate * 2 * ainfo.nchannels));        // averageBytesPerSecond
            binaryWriter.Write((short)(ainfo.nchannels * 2));   // blockAlign
            binaryWriter.Write((short)16);        // significantBitsPerSample
            binaryWriter.Write((short)0);         // extraFormatSize
            binaryWriter.Write((int)0x61746164);  // chunkIdData:'atad'
            binaryWriter.Write((int)0);           // chunkIdSizeData
        }

        public bool RenderAudio(PXCMAudio audio)
        {
            PXCMAudio.AudioData adata;
            pxcmStatus sts = audio.AcquireAccess(PXCMAudio.Access.ACCESS_READ, PXCMAudio.AudioFormat.AUDIO_FORMAT_PCM, out adata);
            if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR) return false;
            binaryWriter.Write(adata.ToByteArray());
            audio.ReleaseAccess(adata);
            return true;
        }

        public void Close()
        {
            long position = binaryWriter.Seek(0, SeekOrigin.Current);
            binaryWriter.Seek(0x2a, SeekOrigin.Begin); // chunkDataSizeData
            binaryWriter.Write((int)(position - 46));
            binaryWriter.Seek(0x04, SeekOrigin.Begin); // chunkDataSizeRiff
            binaryWriter.Write((int)(position - 8));

            binaryWriter.Seek(0, SeekOrigin.Begin);
            var soundPlayer = new SoundPlayer(fileStream);
            soundPlayer.PlaySync();
            soundPlayer.Dispose();

            binaryWriter.Close();
            fileStream.Close();
        }
    }
}