public sealed class WavPcmDecoder
{
    public int Channels { get; private set; }
    public int SampleRate { get; private set; }
    public int BitsPerSample { get; private set; }
    public float Duration => PcmData.LongLength / (SampleRate * Channels * (BitsPerSample / 8.0f));
    public byte[] PcmData { get; private set; }

    public void Load(Stream stream)
    {
        using (var br = new BinaryReader(stream, Encoding.Unicode, true))
        {
            // --- RIFF チェック ---
            if (Encoding.ASCII.GetString(br.ReadBytes(4)) != "RIFF")
                throw new Exception("Invalid WAV: Missing RIFF");

            br.ReadInt32(); // file size 忘れてよい
            if (Encoding.ASCII.GetString(br.ReadBytes(4)) != "WAVE")
                throw new Exception("Invalid WAV: Missing WAVE");

            // --- Chunk 読み込みループ ---
            bool fmtFound = false;
            bool dataFound = false;

            while (stream.Position < stream.Length)
            {
                string chunkId = Encoding.ASCII.GetString(br.ReadBytes(4));
                int chunkSize = br.ReadInt32();

                switch (chunkId)
                {
                    case "fmt ":
                        // PCM フォーマット読み込み
                        int audioFormat = br.ReadInt16();
                        if (audioFormat != 1)
                            throw new Exception("Only PCM WAV is supported.");

                        Channels = br.ReadInt16();
                        SampleRate = br.ReadInt32();

                        br.ReadInt32(); // byte rate
                        br.ReadInt16(); // block align

                        BitsPerSample = br.ReadInt16();

                        // fmt chunk が16以上の場合、追加スキップ
                        if (chunkSize > 16)
                            br.ReadBytes(chunkSize - 16);
                        
                        fmtFound = true;
                        break;

                    case "data":
                        // PCM データをそのまま取り出す
                        PcmData = br.ReadBytes(chunkSize);
                        dataFound = true;
                        break;

                    default:
                        // その他のチャンクはスキップ
                        br.ReadBytes(chunkSize);
                        break;
                }

                if (fmtFound && dataFound) break;
            }

            if (!fmtFound) throw new Exception("WAV fmt chunk missing.");
            if (!dataFound) throw new Exception("WAV data chunk missing.");
        }
    }
}
