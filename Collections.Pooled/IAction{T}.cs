﻿namespace ZBase.Collections.Pooled
{
    public interface IAction<in T>
    {
        void Action(T value);
    }
}
