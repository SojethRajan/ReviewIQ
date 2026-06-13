namespace ReviewIQ.AI.Interfaces;

public interface IDiffChunker
{
    List<string> Chunk(string diff);
}