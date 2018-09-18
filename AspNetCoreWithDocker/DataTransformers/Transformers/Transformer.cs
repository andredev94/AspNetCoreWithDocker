using System.Collections.Generic;

namespace AspNetCoreWithDocker.DataTransformers.Transformers
{
    //S(source) is the source object(value object) to populate an D(destination) entity
    public interface Transformer<S, D>
    {
        D Transform(S source);
        List<D> TransformList(List<S> source);
    }
}
