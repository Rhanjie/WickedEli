using System.Threading.Tasks;

namespace Map.Noises
{
    public interface INoise
    {
        public Task<float[,]> Generate(uint size);
    }
}