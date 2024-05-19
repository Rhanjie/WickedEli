namespace Map.Noises
{
    public interface INoise
    {
        public float[,] Generate(uint size);
    }
}