using System.Text;
using ReviewIQ.AI.Interfaces;

namespace ReviewIQ.AI.Services;

public class DiffChunker : IDiffChunker
{
    private const int MaxChunkSize = 50000;

    public List<string> Chunk(string diff)
    {

        var chunks = new List<string>();

        if(string.IsNullOrWhiteSpace(diff))
        {
            return chunks;
        }

        if(diff.Length <= MaxChunkSize)
        {
            chunks.Add(diff);
            return chunks;
        }

        var lines = diff.Split('\n');
        var currentChunk = new StringBuilder();

        foreach (var line in lines)
        {
            if (currentChunk.Length + line.Length + 1 > MaxChunkSize)
            {
                chunks.Add(currentChunk.ToString());
                currentChunk.Clear();
            }
            currentChunk.AppendLine(line);
        }

        if (currentChunk.Length > 0)
        {
            chunks.Add(currentChunk.ToString());
        }

        return chunks;
    }
}