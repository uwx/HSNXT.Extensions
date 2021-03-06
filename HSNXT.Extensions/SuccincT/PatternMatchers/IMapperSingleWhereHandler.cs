﻿using System;

namespace HSNXT.SuccincT.PatternMatchers
{
    public interface IMapperSingleWhereHandler<T, TResult>
    {
        IMapperMatcher<T, TResult> Do(TResult doValue);
        IMapperMatcher<T, TResult> Do(Func<T, TResult> doFunc);
    }
}